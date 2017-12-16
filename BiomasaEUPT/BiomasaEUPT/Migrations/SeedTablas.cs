using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Migrations
{
    public class SeedTablas
    {
        private BiomasaEUPTContext context;
        private Assembly assembly;
        private string NOMBRE_CSV = "BiomasaEUPT.Migrations.DatosSeed.{0}.csv";

        private sealed class UsuarioMap : CsvClassMap<Usuario>
        {
            public UsuarioMap()
            {
                Map(m => m.UsuarioId);
                Map(m => m.Nombre);
                Map(m => m.Contrasena);
                Map(m => m.Email);
                Map(m => m.Baneado);
                Map(m => m.TipoId);
                Map(m => m.FechaAlta);
                Map(m => m.FechaBaja);
                Map(m => m.FechaContrasena);
            }
        }

        private sealed class ComunidadMap : CsvClassMap<Comunidad>
        {
            public ComunidadMap()
            {
                Map(m => m.ComunidadId);
                Map(m => m.Codigo);
                Map(m => m.Nombre);
                Map(m => m.PaisId);
            }
        }

        private sealed class ProvinciaMap : CsvClassMap<Provincia>
        {
            public ProvinciaMap()
            {
                Map(m => m.ProvinciaId);
                Map(m => m.Codigo);
                Map(m => m.Nombre);
                Map(m => m.ComunidadId);
            }
        }

        private sealed class MunicipioMap : CsvClassMap<Municipio>
        {
            public MunicipioMap()
            {
                Map(m => m.MunicipioId);
                Map(m => m.CodigoPostal);
                Map(m => m.Latitud);
                Map(m => m.Longitud);
                Map(m => m.Nombre);
                Map(m => m.ProvinciaId);
            }
        }

        private sealed class ClienteMap : CsvClassMap<Cliente>
        {
            public ClienteMap()
            {
                Map(m => m.ClienteId);
                Map(m => m.Nif);
                Map(m => m.Email);
                Map(m => m.RazonSocial);
                Map(m => m.Calle);
                Map(m => m.Observaciones);
                Map(m => m.TipoId);
                Map(m => m.GrupoId);
                Map(m => m.MunicipioId);
            }
        }

        private sealed class ProveedorMap : CsvClassMap<Proveedor>
        {
            public ProveedorMap()
            {
                Map(m => m.ProveedorId);
                Map(m => m.Nif);
                Map(m => m.Email);
                Map(m => m.RazonSocial);
                Map(m => m.Calle);
                Map(m => m.Observaciones);
                Map(m => m.TipoId);
                Map(m => m.MunicipioId);
            }
        }

        private sealed class TipoMateriaPrimaMap : CsvClassMap<TipoMateriaPrima>
        {
            public TipoMateriaPrimaMap()
            {
                Map(tmp => tmp.GrupoId);
                Map(tmp => tmp.Nombre);
                Map(tmp => tmp.MedidoEnUnidades);
                Map(tmp => tmp.MedidoEnVolumen);
                Map(m => m.Descripcion);
            }
        }

        public SeedTablas(BiomasaEUPTContext context)
        {
            this.context = context;
            assembly = Assembly.GetExecutingAssembly();

            CsvConfiguration csvConfig = new CsvConfiguration()
            {
                WillThrowOnMissingField = false,
                Delimiter = ";"
            };

            string resourceName = String.Format(NOMBRE_CSV, "TiposUsuarios");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = csvReader.GetRecords<TipoUsuario>().ToArray();
                    context.TiposUsuarios.AddOrUpdate(d => d.Nombre, datos);
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "Permisos");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<Permiso>();
                    while (csvReader.Read())
                    {
                        // Alternativa a PermisoMap
                        datos.Add(new Permiso()
                        {
                            PermisoId = csvReader.GetField<int>("PermisoId"),
                            Tab = (Tab)Enum.Parse(typeof(Tab), csvReader.GetField<string>("Tab")),
                            TipoId = csvReader.GetField<int>("TipoId")
                        });
                    }
                    context.Permisos.AddOrUpdate(d => d.PermisoId, datos.ToArray());
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "Usuarios");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    csvReader.Configuration.RegisterClassMap<UsuarioMap>();
                    var datos = csvReader.GetRecords<Usuario>().ToArray();
                    foreach (var d in datos)
                    {
                        if (d.Contrasena == null)
                            d.Contrasena = ContrasenaHashing.ObtenerHashSHA256(d.Nombre);
                    }
                    context.Usuarios.AddOrUpdate(d => d.Nombre, datos);
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "TiposProveedores");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = csvReader.GetRecords<TipoProveedor>().ToArray();
                    context.TiposProveedores.AddOrUpdate(d => d.Nombre, datos);
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "TiposClientes");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = csvReader.GetRecords<TipoCliente>().ToArray();
                    context.TiposClientes.AddOrUpdate(d => d.Nombre, datos);
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "GruposClientes");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = csvReader.GetRecords<GrupoCliente>().ToArray();
                    context.GruposClientes.AddOrUpdate(d => d.Nombre, datos);
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "Paises");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = csvReader.GetRecords<Pais>().ToArray();
                    context.Paises.AddOrUpdate(d => d.Codigo, datos);
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "Comunidades");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    // Hay que mapear cada columna ya que la columna Codigo
                    // piensa que es la de la tabla Pais
                    csvReader.Configuration.RegisterClassMap<ComunidadMap>();
                    var datos = csvReader.GetRecords<Comunidad>().ToArray();
                    foreach (var d in datos)
                    {
                        if (d.PaisId == null)
                            d.PaisId = context.Paises.Single(c => c.Codigo == d.Codigo.Substring(0, 2)).PaisId;
                    }
                    context.Comunidades.AddOrUpdate(d => d.Codigo, datos);
                }
            }
            context.SaveChanges();

            resourceName = String.Format(NOMBRE_CSV, "Provincias");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    csvReader.Configuration.RegisterClassMap<ProvinciaMap>();
                    while (csvReader.Read())
                    {
                        var provincia = csvReader.GetRecord<Provincia>();
                        var codigoComunidad = csvReader.GetField<string>("CodigoComunidad");
                        provincia.ComunidadId = context.Comunidades.Single(c => c.Codigo == codigoComunidad).ComunidadId;
                        context.Provincias.AddOrUpdate(p => p.Codigo, provincia);
                    }
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "Municipios");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    csvReader.Configuration.RegisterClassMap<MunicipioMap>();
                    while (csvReader.Read())
                    {
                        var municipio = csvReader.GetRecord<Municipio>();
                        var codigoProvincia = csvReader.GetField<string>("CodigoProvincia");
                        municipio.ProvinciaId = context.Provincias.Single(c => c.Codigo == codigoProvincia).ProvinciaId;
                        // Si se ejecuta dos veces el seed puede machacar municipios por tener el mismo código postal
                        context.Municipios.AddOrUpdate(m => m.CodigoPostal, municipio);
                    }
                }
            }
            context.SaveChanges();

            resourceName = String.Format(NOMBRE_CSV, "Clientes");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    csvReader.Configuration.RegisterClassMap<ClienteMap>();
                    var datos = csvReader.GetRecords<Cliente>().ToArray();
                    foreach (var d in datos)
                    {
                        if (d.Observaciones == string.Empty)
                            d.Observaciones = null;
                    }
                    context.Clientes.AddOrUpdate(p => p.ClienteId, datos);
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "Proveedores");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    csvReader.Configuration.RegisterClassMap<ProveedorMap>();
                    var datos = csvReader.GetRecords<Proveedor>().ToArray();
                    foreach (var d in datos)
                    {
                        if (d.Observaciones == string.Empty)
                            d.Observaciones = null;
                    }
                    context.Proveedores.AddOrUpdate(p => p.ProveedorId, datos);
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "EstadosRecepciones");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = csvReader.GetRecords<EstadoRecepcion>().ToArray();
                    context.EstadosRecepciones.AddOrUpdate(d => d.Nombre, datos);
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "SitiosRecepciones");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = csvReader.GetRecords<SitioRecepcion>().ToArray();
                    context.SitiosRecepciones.AddOrUpdate(d => d.Nombre, datos);
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "HuecosRecepciones");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<HuecoRecepcion>();
                    while (csvReader.Read())
                    {
                        // Alternativa a HuecoRecepcionMap
                        datos.Add(new HuecoRecepcion()
                        {
                            HuecoRecepcionId = csvReader.GetField<int>("HuecoRecepcionId"),
                            Nombre = csvReader.GetField<string>("Nombre"),
                            UnidadesTotales = csvReader.GetField<int>("UnidadesTotales"),
                            VolumenTotal = csvReader.GetField<double>("VolumenTotal"),
                            SitioId = csvReader.GetField<int>("SitioId")
                        });
                    }
                    context.HuecosRecepciones.AddOrUpdate(d => d.HuecoRecepcionId, datos.ToArray());
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "Procedencias");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = csvReader.GetRecords<Procedencia>().ToArray();
                    context.Procedencias.AddOrUpdate(d => d.ProcedenciaId, datos);
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "Recepciones");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<Recepcion>();
                    while (csvReader.Read())
                    {
                        // Alternativa a RecepcionMap
                        datos.Add(new Recepcion()
                        {
                            NumeroAlbaran = csvReader.GetField<string>("NumeroAlbaran"),
                            FechaRecepcion = csvReader.GetField<DateTime>("FechaRecepcion"),
                            ProveedorId = csvReader.GetField<int>("ProveedorId"),
                            EstadoId = csvReader.GetField<int>("EstadoId")
                        });
                    }
                    context.Recepciones.AddOrUpdate(d => d.NumeroAlbaran, datos.ToArray());
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "GruposMateriasPrimas");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<GrupoMateriaPrima>();
                    while (csvReader.Read())
                    {
                        // Alternativa a GrupoMateriaPrimaMap
                        datos.Add(new GrupoMateriaPrima()
                        {
                            GrupoMateriaPrimaId = csvReader.GetField<int>("GrupoMateriaPrimaId"),
                            Nombre = csvReader.GetField<string>("Nombre"),
                            Descripcion = csvReader.GetField<string>("Descripcion")
                        });
                    }
                    context.GruposMateriasPrimas.AddOrUpdate(d => d.GrupoMateriaPrimaId, datos.ToArray());
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "TiposMateriasPrimas");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<TipoMateriaPrima>();
                    while (csvReader.Read())
                    {
                        // Alternativa a TipoMateriaPrimaMap
                        datos.Add(new TipoMateriaPrima()
                        {
                            TipoMateriaPrimaId = csvReader.GetField<int>("TipoMateriaPrimaId"),
                            Nombre = csvReader.GetField<string>("Nombre"),
                            Descripcion = csvReader.GetField<string>("Descripcion"),
                            MedidoEnUnidades = csvReader.GetField<bool>("MedidoEnUnidades"),
                            MedidoEnVolumen = csvReader.GetField<bool>("MedidoEnVolumen"),
                            GrupoId = csvReader.GetField<int>("GrupoId")
                        });
                    }
                    context.TiposMateriasPrimas.AddOrUpdate(d => d.TipoMateriaPrimaId, datos.ToArray());
                }
            }
            context.SaveChanges();

            resourceName = String.Format(NOMBRE_CSV, "MateriasPrimas");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<MateriaPrima>();
                    while (csvReader.Read())
                    {
                        // Campos opcionales
                        csvReader.TryGetField<string>("Observaciones", out var observaciones);
                        observaciones = (observaciones == "") ? null : observaciones;

                        datos.Add(new MateriaPrima()
                        {
                            MateriaPrimaId = csvReader.GetField<int>("MateriaPrimaId"),
                            TipoId = csvReader.GetField<int>("TipoId"),
                            Volumen = csvReader.GetField<double?>("Volumen"),
                            Unidades = csvReader.GetField<int?>("Unidades"),
                            RecepcionId = csvReader.GetField<int>("RecepcionId"),
                            ProcedenciaId = csvReader.GetField<int>("ProcedenciaId"),
                            FechaBaja = csvReader.GetField<DateTime?>("FechaBaja"),
                            Observaciones = observaciones
                        });
                    }
                    context.MateriasPrimas.AddOrUpdate(d => d.MateriaPrimaId, datos.ToArray());
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "HistorialHuecosRecepciones");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<HistorialHuecoRecepcion>();
                    while (csvReader.Read())
                    {
                        // Alternativa a HistorialHuecoRecepcionMap
                        datos.Add(new HistorialHuecoRecepcion()
                        {
                            HistorialHuecoRecepcionId = csvReader.GetField<int>("HistorialHuecoRecepcionId"),
                            Volumen = csvReader.GetField<double?>("Volumen"),
                            Unidades = csvReader.GetField<int?>("Unidades"),
                            MateriaPrimaId = csvReader.GetField<int>("MateriaPrimaId"),
                            HuecoRecepcionId = csvReader.GetField<int>("HuecoRecepcionId")
                        });
                    }
                    context.HistorialHuecosRecepciones.AddOrUpdate(d => d.HistorialHuecoRecepcionId, datos.ToArray());
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "EstadosElaboraciones");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = csvReader.GetRecords<EstadoElaboracion>().ToArray();
                    context.EstadosElaboraciones.AddOrUpdate(d => d.Nombre, datos);
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "GruposProductosTerminados");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<GrupoProductoTerminado>();
                    while (csvReader.Read())
                    {
                        // Alternativa a GrupoProductoTerminadoMap
                        datos.Add(new GrupoProductoTerminado()
                        {
                            GrupoProductoTerminadoId = csvReader.GetField<int>("GrupoProductoTerminadoId"),
                            Nombre = csvReader.GetField<string>("Nombre"),
                            Descripcion = csvReader.GetField<string>("Descripcion")
                        });
                    }
                    context.GruposProductosTerminados.AddOrUpdate(d => d.GrupoProductoTerminadoId, datos.ToArray());
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "TiposProductosTerminados");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<TipoProductoTerminado>();
                    while (csvReader.Read())
                    {
                        // Alternativa a TipoProductoTerminadoMap
                        datos.Add(new TipoProductoTerminado()
                        {
                            TipoProductoTerminadoId = csvReader.GetField<int>("TipoProductoTerminadoId"),
                            Nombre = csvReader.GetField<string>("Nombre"),
                            Tamano = csvReader.GetField<string>("Tamano"),
                            Humedad = csvReader.GetField<double>("Humedad"),
                            MedidoEnUnidades = csvReader.GetField<bool>("MedidoEnUnidades"),
                            MedidoEnVolumen = csvReader.GetField<bool>("MedidoEnVolumen"),
                            GrupoId = csvReader.GetField<int>("GrupoId")
                        });
                    }
                    context.TiposProductosTerminados.AddOrUpdate(d => d.TipoProductoTerminadoId, datos.ToArray());
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "OrdenesElaboraciones");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<OrdenElaboracion>();
                    while (csvReader.Read())
                    {
                        // Alternativa a OrdenElaboracionMap
                        datos.Add(new OrdenElaboracion()
                        {
                            OrdenElaboracionId = csvReader.GetField<int>("OrdenElaboracionId"),
                            EstadoElaboracionId = csvReader.GetField<int>("EstadoElaboracionId"),
                            Descripcion = csvReader.GetField<string>("Descripcion")
                        });
                    }
                    context.OrdenesElaboraciones.AddOrUpdate(d => d.OrdenElaboracionId, datos.ToArray());
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "SitiosAlmacenajes");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = csvReader.GetRecords<SitioAlmacenaje>().ToArray();
                    context.SitiosAlmacenajes.AddOrUpdate(d => d.Nombre, datos);
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "HuecosAlmacenajes");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<HuecoAlmacenaje>();
                    while (csvReader.Read())
                    {
                        // Alternativa a HuecoAlmacenajeMap
                        datos.Add(new HuecoAlmacenaje()
                        {
                            HuecoAlmacenajeId = csvReader.GetField<int>("HuecoAlmacenajeId"),
                            Nombre = csvReader.GetField<string>("Nombre"),
                            UnidadesTotales = csvReader.GetField<int>("UnidadesTotales"),
                            VolumenTotal = csvReader.GetField<double>("VolumenTotal"),
                            SitioId = csvReader.GetField<int>("SitioId")
                        });
                    }
                    context.HuecosAlmacenajes.AddOrUpdate(d => d.HuecoAlmacenajeId, datos.ToArray());
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "ProductosTerminados");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<ProductoTerminado>();
                    while (csvReader.Read())
                    {
                        // Campos opcionales
                        csvReader.TryGetField<string>("Observaciones", out var observaciones);
                        observaciones = (observaciones == "") ? null : observaciones;

                        datos.Add(new ProductoTerminado()
                        {
                            ProductoTerminadoId = csvReader.GetField<int>("ProductoTerminadoId"),
                            TipoId = csvReader.GetField<int>("TipoId"),
                            Volumen = csvReader.GetField<double?>("Volumen"),
                            Unidades = csvReader.GetField<int?>("Unidades"),
                            OrdenId = csvReader.GetField<int>("OrdenId"),
                            FechaBaja = csvReader.GetField<DateTime?>("FechaBaja"),
                            Observaciones = observaciones
                        });
                    }
                    context.ProductosTerminados.AddOrUpdate(d => d.ProductoTerminadoId, datos.ToArray());
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "HistorialHuecosAlmacenajes");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<HistorialHuecoAlmacenaje>();
                    while (csvReader.Read())
                    {
                        // Alternativa a HistorialHuecoAlmacenajeMap
                        datos.Add(new HistorialHuecoAlmacenaje()
                        {
                            HistorialHuecoAlmacenajeId = csvReader.GetField<int>("HistorialHuecoAlmacenajeId"),
                            Volumen = csvReader.GetField<double?>("Volumen"),
                            Unidades = csvReader.GetField<int?>("Unidades"),
                            ProductoTerminadoId = csvReader.GetField<int>("ProductoTerminadoId"),
                            HuecoAlmacenajeId = csvReader.GetField<int>("HuecoAlmacenajeId")
                        });
                    }
                    context.HistorialHuecosAlmacenajes.AddOrUpdate(d => d.HistorialHuecoAlmacenajeId, datos.ToArray());
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "ProductosTerminadosComposiciones");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<ProductoTerminadoComposicion>();
                    while (csvReader.Read())
                    {
                        // Alternativa a ProductoTerminadoComposicionMap
                        datos.Add(new ProductoTerminadoComposicion()
                        {
                            ProductoTerminadoComposicionId = csvReader.GetField<int>("ProductoTerminadoComposicionId"),
                            Volumen = csvReader.GetField<int?>("Volumen"),
                            Unidades = csvReader.GetField<int?>("Unidades"),
                            HistorialHuecoId = csvReader.GetField<int>("HistorialHuecoId"),
                            ProductoId = csvReader.GetField<int>("ProductoId")
                        });
                    }
                    context.ProductosTerminadosComposiciones.AddOrUpdate(d => d.ProductoTerminadoComposicionId, datos.ToArray());
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "EstadosPedidos");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = csvReader.GetRecords<EstadoPedido>().ToArray();
                    context.EstadosPedidos.AddOrUpdate(d => d.Nombre, datos);
                }
            }
            context.SaveChanges();

            resourceName = String.Format(NOMBRE_CSV, "EstadosEnvasados");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = csvReader.GetRecords<EstadoEnvasado>().ToArray();
                    context.EstadosEnvasados.AddOrUpdate(d => d.Nombre, datos);
                }
            }
            context.SaveChanges();

            resourceName = String.Format(NOMBRE_CSV, "GruposProductosEnvasados");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<GrupoProductoEnvasado>();
                    while (csvReader.Read())
                    {
                        // Alternativa a GrupoProductoEnvasadoMap
                        datos.Add(new GrupoProductoEnvasado()
                        {
                            GrupoProductoEnvasadoId = csvReader.GetField<int>("GrupoProductoEnvasadoId"),
                            Nombre = csvReader.GetField<string>("Nombre"),
                            Descripcion = csvReader.GetField<string>("Descripcion")
                        });
                    }
                    context.GruposProductosEnvasados.AddOrUpdate(d => d.GrupoProductoEnvasadoId, datos.ToArray());
                }
            }
            context.SaveChanges();
            

            resourceName = String.Format(NOMBRE_CSV, "TiposProductosEnvasados");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<TipoProductoEnvasado>();
                    while (csvReader.Read())
                    {
                        datos.Add(new TipoProductoEnvasado()
                        {
                            TipoProductoEnvasadoId = csvReader.GetField<int>("TipoProductoEnvasadoId"),
                            Nombre = csvReader.GetField<string>("Nombre"),
                            Descripcion = csvReader.GetField<string>("Descripcion"),
                            MedidoEnVolumen = csvReader.GetField<bool>("MedidoEnVolumen"),
                            MedidoEnUnidades = csvReader.GetField<bool>("MedidoEnUnidades"),
                            GrupoId = csvReader.GetField<int>("GrupoId")
                        });
                    }
                    context.TiposProductosEnvasados.AddOrUpdate(d => d.TipoProductoEnvasadoId, datos.ToArray());
                }
            }
            context.SaveChanges();

          
            resourceName = String.Format(NOMBRE_CSV, "Picking");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<Picking>();
                    while (csvReader.Read())
                    {
                        datos.Add(new Picking()
                        {
                            PickingId = csvReader.GetField<int>("PickingId"),
                            Nombre = csvReader.GetField<string>("Nombre"),
                            VolumenTotal = csvReader.GetField<double>("VolumenTotal"),
                            UnidadesTotales = csvReader.GetField<int>("UnidadesTotales")
                        });

                    }
                    context.Picking.AddOrUpdate(d => d.PickingId, datos.ToArray());
                }
            }
            context.SaveChanges();


            resourceName = String.Format(NOMBRE_CSV, "OrdenesEnvasados");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<OrdenEnvasado>();
                    while (csvReader.Read())
                    {
                        // Alternativa a OrdenEnvasadoMap
                        datos.Add(new OrdenEnvasado()
                        {
                            OrdenEnvasadoId = csvReader.GetField<int>("OrdenEnvasadoId"),
                            EstadoEnvasadoId = csvReader.GetField<int>("EstadoEnvasadoId"),
                            Descripcion = csvReader.GetField<string>("Descripcion")
                        });
                    }
                    context.OrdenesEnvasados.AddOrUpdate(d => d.OrdenEnvasadoId, datos.ToArray());
                }
            }
            context.SaveChanges();

            resourceName = String.Format(NOMBRE_CSV, "ProductosEnvasados");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<ProductoEnvasado>();
                    while (csvReader.Read())
                    {
                        // Campos opcionales
                        csvReader.TryGetField<string>("Observaciones", out var observaciones);
                        observaciones = (observaciones == "") ? null : observaciones;

                        datos.Add(new ProductoEnvasado()
                        {
                            ProductoEnvasadoId = csvReader.GetField<int>("ProductoEnvasadoId"),
                            Volumen = csvReader.GetField<double?>("Volumen"),
                            Unidades = csvReader.GetField<int?>("Unidades"),
                            TipoProductoEnvasadoId = csvReader.GetField<int>("TipoProductoEnvasadoId"),
                            OrdenId = csvReader.GetField<int>("OrdenId"),
                            PickingId = csvReader.GetField<int>("PickingId"),
                            Observaciones = observaciones
                        });
                    }
                    context.ProductosEnvasados.AddOrUpdate(d => d.ProductoEnvasadoId, datos.ToArray());
                }
            }
            context.SaveChanges();

            resourceName = String.Format(NOMBRE_CSV, "ProductosEnvasadosComposiciones");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<ProductoEnvasadoComposicion>();
                    while (csvReader.Read())
                    {
                        // Alternativa a ProductoTerminadoComposicionMap
                        datos.Add(new ProductoEnvasadoComposicion()
                        {
                            ProductoEnvasadoComposicionId = csvReader.GetField<int>("ProductoEnvasadoComposicionId"),
                            Volumen = csvReader.GetField<int?>("Volumen"),
                            Unidades = csvReader.GetField<int?>("Unidades"),
                            HistorialHuecoId = csvReader.GetField<int>("HistorialHuecoId"),
                            ProductoId = csvReader.GetField<int>("ProductoId")
                        });
                    }
                    context.ProductosEnvasadosComposiciones.AddOrUpdate(d => d.ProductoEnvasadoComposicionId, datos.ToArray());
                }
            }
            context.SaveChanges();

            resourceName = String.Format(NOMBRE_CSV, "PedidosCabeceras");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<PedidoCabecera>();
                    while (csvReader.Read())
                    {
                        datos.Add(new PedidoCabecera()
                        {
                            PedidoCabeceraId = csvReader.GetField<int>("PedidoCabeceraId"),
                            FechaPedido = csvReader.GetField<DateTime>("FechaPedido"),
                            EstadoId = csvReader.GetField<int>("EstadoId"),
                            ClienteId = csvReader.GetField<int>("ClienteId")
                        });
                    }
                    context.PedidosCabeceras.AddOrUpdate(d => d.PedidoCabeceraId, datos.ToArray());
                }
            }
            context.SaveChanges();

            resourceName = String.Format(NOMBRE_CSV, "PedidosLineas");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<PedidoLinea>();
                    while (csvReader.Read())
                    {
                        datos.Add(new PedidoLinea()
                        {
                            PedidoLineaId = csvReader.GetField<int>("PedidoLineaId"),
                            Volumen = csvReader.GetField<double?>("Volumen"),
                            Unidades = csvReader.GetField<int?>("Unidades"),
                            VolumenPreparado = csvReader.GetField<double?>("VolumenPreparado"),
                            UnidadesPreparadas = csvReader.GetField<int?>("UnidadesPreparadas"),
                            PedidoCabeceraId = csvReader.GetField<int>("PedidoCabeceraId"),
                            TipoProductoEnvasadoId = csvReader.GetField<int>("TipoProductoEnvasadoId")
                        });
                    }
                    context.PedidosLineas.AddOrUpdate(d => d.PedidoLineaId, datos.ToArray());
                }
            }
            context.SaveChanges();

            resourceName = String.Format(NOMBRE_CSV, "PedidosDetalles");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var datos = new List<PedidoDetalle>();
                    while (csvReader.Read())
                    {
                        datos.Add(new PedidoDetalle()
                        {
                            PedidoDetalleId = csvReader.GetField<int>("PedidoDetalleId"),
                            Volumen = csvReader.GetField<double?>("Volumen"),
                            Unidades = csvReader.GetField<int?>("Unidades"),
                            PedidoLineaId = csvReader.GetField<int>("PedidoLineaId"),
                            ProductoEnvasadoId = csvReader.GetField<int>("ProductoEnvasadoId")
                        });
                    }
                    context.PedidosDetalles.AddOrUpdate(d => d.PedidoDetalleId, datos.ToArray());
                }
            }
            context.SaveChanges();

        }
    }
}
