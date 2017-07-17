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


            //context.TiposUsuarios.Load();
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


            //context.Paises.Load();
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

            //context.Comunidades.Load();
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
                    context.Clientes.AddOrUpdate(p => p.RazonSocial, datos);
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
                    context.Proveedores.AddOrUpdate(p => p.RazonSocial, datos);
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
                            Nombre = csvReader.GetField<string>("Nombre"),
                            UnidadesTotales = csvReader.GetField<int>("UnidadesTotales"),
                            VolumenTotal = csvReader.GetField<double>("VolumenTotal"),
                            SitioId = csvReader.GetField<int>("SitioId")
                        });
                    }
                    context.HuecosRecepciones.AddOrUpdate(d => d.Nombre, datos.ToArray());
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
                    context.Procedencias.AddOrUpdate(d => d.Nombre, datos);
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
                        // Alternativa a HuecoRecepcionMap
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








            /*
            resourceName = String.Format(NOMBRE_CSV, "TiposClientes");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var tiposClientes = csvReader.GetRecords<TipoCliente>().ToArray();
                    context.TiposClientes.AddOrUpdate(c => c.Nombre, tiposClientes);
                }
            }

            resourceName = String.Format(NOMBRE_CSV, "GruposClientes");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var gruposClientes = csvReader.GetRecords<GrupoCliente>().ToArray();
                    context.GruposClientes.AddOrUpdate(c => c.Nombre, gruposClientes);
                }
            }

            resourceName = String.Format(NOMBRE_CSV, "Clientes");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    while (csvReader.Read())
                    {
                        var cliente = csvReader.GetRecord<Cliente>();
                        var tipoCliente = csvReader.GetField<string>("TipoCliente");
                        cliente.TipoCliente = context.TiposClientes.Local.Single(c => c.Nombre == tipoCliente);
                        var grupoCliente = csvReader.GetField<string>("GrupoCliente");
                        cliente.GrupoCliente = context.GruposClientes.Local.Single(c => c.Nombre == grupoCliente);
                        var municipio = csvReader.GetField<string>("Municipio");
                        cliente.Municipio = context.Municipios.Local.Single(c => c.CodigoPostal == municipio);
                        context.Clientes.AddOrUpdate(p => p.RazonSocial, cliente);
                    }
                }
            }


            resourceName = String.Format(NOMBRE_CSV, "EstadosRecepciones");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var estadosRecepciones = csvReader.GetRecords<EstadoRecepcion>().ToArray();
                    context.EstadosRecepciones.AddOrUpdate(c => c.Nombre, estadosRecepciones);
                }
            }

            resourceName = String.Format(NOMBRE_CSV, "EstadosElaboraciones");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var estadosElaboraciones = csvReader.GetRecords<EstadoElaboracion>().ToArray();
                    context.EstadosElaboraciones.AddOrUpdate(c => c.Nombre, estadosElaboraciones);
                }
            }*/

        }
    }
}
