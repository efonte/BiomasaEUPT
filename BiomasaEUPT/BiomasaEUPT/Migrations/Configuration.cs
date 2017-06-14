namespace BiomasaEUPT.Migrations
{
    using BiomasaEUPT.Domain;
    using BiomasaEUPT.Modelos.Tablas;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Modelos.BiomasaEUPTContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "BiomasaEUPT.Modelo.BiomasaEUPTContext";
        }

        protected override void Seed(Modelos.BiomasaEUPTContext context)
        {
            context.TiposUsuarios.AddOrUpdate(
                tu => tu.Nombre,
                new TipoUsuario()
                {
                    Nombre = "Administrador",
                    Descripcion = "Usuario con máximos privilegios"
                },
                new TipoUsuario()
                {
                    Nombre = "Técnico A",
                    Descripcion = "Este es un técnico A"
                },
                new TipoUsuario()
                {
                    Nombre = "Técnico B",
                    Descripcion = "Este es un técnico B"
                });
            context.SaveChanges();

            context.Usuarios.AddOrUpdate(
                   u => u.Nombre,
                   new Usuario()
                   {
                       Nombre = "admin",
                       Contrasena = ContrasenaHashing.obtenerHashSHA256("admin"),
                       Email = "admin@biomasaeupt.es",
                       TipoId = context.TiposUsuarios.Local.Single(u => u.Nombre == "Administrador").TipoUsuarioId
                   },
                   new Usuario()
                   {
                       Nombre = "tecnico1",
                       Contrasena = ContrasenaHashing.obtenerHashSHA256("tecnico1"),
                       Email = "tecnico1@biomasaeupt.es",
                       TipoId = context.TiposUsuarios.Local.Single(u => u.Nombre == "Técnico A").TipoUsuarioId
                   });
            context.SaveChanges();

            context.TiposProveedores.AddOrUpdate(
                tp => tp.Nombre,
                new TipoProveedor()
                {
                    Nombre = "TipoProveedor 1",
                    Descripcion = "Este es el TipoProveedor 1"
                },
                new TipoProveedor()
                {
                    Nombre = "TipoProveedor 2",
                    Descripcion = "Este es el TipoProveedor 2"
                });
            context.SaveChanges();

            context.GruposClientes.AddOrUpdate(
               gc => gc.Nombre,
               new GrupoCliente()
               {
                   Nombre = "GrupoCliente 1",
                   Descripcion = "Este es el GrupoCliente 1"
               },
               new GrupoCliente()
               {
                   Nombre = "GrupoCliente 2",
                   Descripcion = "Este es el GrupoCliente 2"
               });
            context.SaveChanges();

            context.TiposClientes.AddOrUpdate(
               tc => tc.Nombre,
               new TipoCliente()
               {
                   Nombre = "TipoCliente 1",
                   Descripcion = "Este es el TipoCliente 1"
               },
               new TipoCliente()
               {
                   Nombre = "TipoCliente 2",
                   Descripcion = "Este es el TipoCliente 2"
               });
            context.SaveChanges();

            context.Paises.AddOrUpdate(
                  p => p.Codigo,
                  new Pais()
                  {
                      Codigo = "ES",
                      Nombre = "España"
                  },
                  new Pais()
                  {
                      Codigo = "FR",
                      Nombre = "Francia"
                  });
            context.SaveChanges();

            context.Comunidades.AddOrUpdate(
                c => c.Codigo,
                new Comunidad()
                {
                    Codigo = "ES-AR",
                    Nombre = "Aragón",
                    PaisId = context.Paises.Local.Single(p => p.Codigo == "ES").PaisId
                },
                new Comunidad()
                {
                    Codigo = "ES-VC",
                    Nombre = "Comunidad Valenciana",
                    PaisId = context.Paises.Local.Single(p => p.Codigo == "ES").PaisId
                },
                new Comunidad()
                {
                    Codigo = "FR-11",
                    Nombre = "Île-de-France",
                    PaisId = context.Paises.Local.Single(p => p.Codigo == "FR").PaisId
                });
            context.SaveChanges();

            context.Provincias.AddOrUpdate(
                p => p.Codigo,
                new Provincia()
                {
                    Codigo = "ES-CS",
                    Nombre = "Castellón",
                    ComunidadId = context.Comunidades.Local.Single(c => c.Codigo == "ES-VC").ComunidadId
                },
                new Provincia()
                {
                    Codigo = "FR-75",
                    Nombre = "París",
                    ComunidadId = context.Comunidades.Local.Single(c => c.Codigo == "FR-11").ComunidadId
                },
                new Provincia()
                {
                    Codigo = "ES-TE",
                    Nombre = "Teruel",
                    ComunidadId = context.Comunidades.Local.Single(c => c.Codigo == "ES-AR").ComunidadId
                },
                new Provincia()
                {
                    Codigo = "ES-V",
                    Nombre = "Valencia",
                    ComunidadId = context.Comunidades.Local.Single(c => c.Codigo == "ES-VC").ComunidadId
                },
                new Provincia()
                {
                    Codigo = "ES-Z",
                    Nombre = "Zaragoza",
                    ComunidadId = context.Comunidades.Local.Single(c => c.Codigo == "ES-AR").ComunidadId
                });
            context.SaveChanges();

            context.Municipios.AddOrUpdate(
                m => m.CodigoPostal,
                new Municipio()
                {
                    CodigoPostal = "44500",
                    Nombre = "Andorra",
                    Latitud = "40.9766",
                    Longitud = "-0.4472",
                    ProvinciaId = context.Provincias.Local.Single(p => p.Codigo == "ES-TE").ProvinciaId
                },
                new Municipio()
                {
                    CodigoPostal = "44600",
                    Nombre = "Alcañiz",
                    Latitud = "41.05",
                    Longitud = "-0.1333",
                    ProvinciaId = context.Provincias.Local.Single(p => p.Codigo == "ES-TE").ProvinciaId
                },
                new Municipio()
                {
                    CodigoPostal = "12449",
                    Nombre = "Benafer",
                    Latitud = "39.9333",
                    Longitud = "-0.5667",
                    ProvinciaId = context.Provincias.Local.Single(p => p.Codigo == "ES-CS").ProvinciaId
                },
                new Municipio()
                {
                    CodigoPostal = "46100",
                    Nombre = "Burjassot",
                    Latitud = "39.5167",
                    Longitud = "-0.4167",
                    ProvinciaId = context.Provincias.Local.Single(p => p.Codigo == "ES-V").ProvinciaId
                },
                new Municipio()
                {
                    CodigoPostal = "46176",
                    Nombre = "Chelva",
                    Latitud = "39.7493",
                    Longitud = "-0.9968",
                    ProvinciaId = context.Provincias.Local.Single(p => p.Codigo == "ES-V").ProvinciaId
                },
                new Municipio()
                {
                    CodigoPostal = "50376",
                    Nombre = "Cubel",
                    Latitud = "41.096",
                    Longitud = "-1.6373",
                    ProvinciaId = context.Provincias.Local.Single(p => p.Codigo == "ES-Z").ProvinciaId
                },
                new Municipio()
                {
                    CodigoPostal = "12122",
                    Nombre = "Figueroles",
                    Latitud = "40.1167",
                    Longitud = "-0.2333",
                    ProvinciaId = context.Provincias.Local.Single(p => p.Codigo == "ES-CS").ProvinciaId
                },
                new Municipio()
                {
                    CodigoPostal = "75000",
                    Nombre = "Paris",
                    Latitud = "48.8534",
                    Longitud = "2.3488",
                    ProvinciaId = context.Provincias.Local.Single(p => p.Codigo == "FR-75").ProvinciaId
                },
                new Municipio()
                {
                    CodigoPostal = "75020",
                    Nombre = "Paris",
                    Latitud = "48.8534",
                    Longitud = "2.3488",
                    ProvinciaId = context.Provincias.Local.Single(p => p.Codigo == "FR-75").ProvinciaId
                },
                new Municipio()
                {
                    CodigoPostal = "44003",
                    Nombre = "Teruel",
                    Latitud = "40.3456",
                    Longitud = "-1.1065",
                    ProvinciaId = context.Provincias.Local.Single(p => p.Codigo == "ES-TE").ProvinciaId
                },
                new Municipio()
                {
                    CodigoPostal = "50580",
                    Nombre = "Vera De Moncayo",
                    Latitud = "41.824",
                    Longitud = "-1.688",
                    ProvinciaId = context.Provincias.Local.Single(p => p.Codigo == "ES-Z").ProvinciaId
                });
            context.SaveChanges();

            context.Clientes.AddOrUpdate(
                 c => c.RazonSocial,
                 new Cliente()
                 {
                     RazonSocial = "Cliente 1",
                     Nif = "A-11111111",
                     Email = "cliente1@biomasaeupt.es",
                     TipoId = context.TiposClientes.Local.Single(tc => tc.Nombre == "TipoCliente 1").TipoClienteId,
                     GrupoId = context.GruposClientes.Local.Single(gc => gc.Nombre == "GrupoCliente 1").GrupoClienteId,
                     MunicipioId = context.Municipios.Local.Single(gc => gc.CodigoPostal == "44003").MunicipioId,
                     Calle = "Calle 1"
                 },
                 new Cliente()
                 {
                     RazonSocial = "Cliente 2",
                     Nif = "11111111-B",
                     Email = "cliente2@biomasaeupt.es",
                     TipoId = context.TiposClientes.Local.Single(tc => tc.Nombre == "TipoCliente 1").TipoClienteId,
                     GrupoId = context.GruposClientes.Local.Single(gc => gc.Nombre == "GrupoCliente 2").GrupoClienteId,
                     MunicipioId = context.Municipios.Local.Single(gc => gc.CodigoPostal == "50580").MunicipioId,
                     Calle = "Calle 2"
                 });
            context.SaveChanges();

            context.Proveedores.AddOrUpdate(
                 p => p.RazonSocial,
                 new Proveedor()
                 {
                     RazonSocial = "Proveedor 1",
                     Nif = "C-11111111",
                     Email = "proveedor1@biomasaeupt.es",
                     TipoId = context.TiposProveedores.Local.Single(tp => tp.Nombre == "TipoProveedor 1").TipoProveedorId,
                     MunicipioId = context.Municipios.Local.Single(gc => gc.CodigoPostal == "44600").MunicipioId,
                     Calle = "Calle 3"
                 },
                 new Proveedor()
                 {
                     RazonSocial = "Proveedor 2",
                     Nif = "D-11111111",
                     Email = "proveedor2@biomasaeupt.es",
                     TipoId = context.TiposProveedores.Local.Single(tp => tp.Nombre == "TipoProveedor 2").TipoProveedorId,
                     MunicipioId = context.Municipios.Local.Single(gc => gc.CodigoPostal == "75020").MunicipioId,
                     Calle = "Calle 4"
                 });
            context.SaveChanges();

            context.EstadosRecepciones.AddOrUpdate(
                 sr => sr.Nombre,
                 new EstadoRecepcion()
                 {
                     Nombre = "Disponible",
                     Descripcion = "Las materias primas aún no se ha descargado"
                 },
                 new EstadoRecepcion()
                 {
                     Nombre = "Aceptada",
                     Descripcion = "Las materias primas se han descargado"
                 });
            context.SaveChanges();

            context.SitiosRecepciones.AddOrUpdate(
                 sr => sr.Nombre,
                 new SitioRecepcion()
                 {
                     Nombre = "Sitio A",
                     Descripcion = "Este es el Sitio A"
                 },
                  new SitioRecepcion()
                  {
                      Nombre = "Sitio B",
                      Descripcion = "Este es el Sitio A"
                  });
            context.SaveChanges();

            context.HuecosRecepciones.AddOrUpdate(
                 hr => hr.Nombre,
                 new HuecoRecepcion()
                 {
                     Nombre = "A01",
                     UnidadesTotales = 30,
                     VolumenTotal = 20,
                     SitioId = context.SitiosRecepciones.Local.Single(sr => sr.Nombre == "Sitio A").SitioRecepcionId
                 },
                  new HuecoRecepcion()
                  {
                      Nombre = "A02",
                      UnidadesTotales = 25,
                      VolumenTotal = 15,
                      SitioId = context.SitiosRecepciones.Local.Single(sr => sr.Nombre == "Sitio A").SitioRecepcionId
                  },
                   new HuecoRecepcion()
                   {
                       Nombre = "A03",
                       UnidadesTotales = 60,
                       VolumenTotal = 50,
                       SitioId = context.SitiosRecepciones.Local.Single(sr => sr.Nombre == "Sitio A").SitioRecepcionId
                   },
                    new HuecoRecepcion()
                    {
                        Nombre = "B01",
                        UnidadesTotales = 10,
                        VolumenTotal = 5,
                        SitioId = context.SitiosRecepciones.Local.Single(sr => sr.Nombre == "Sitio B").SitioRecepcionId
                    },
                    new HuecoRecepcion()
                    {
                        Nombre = "B02",
                        UnidadesTotales = 60,
                        VolumenTotal = 50,
                        SitioId = context.SitiosRecepciones.Local.Single(sr => sr.Nombre == "Sitio B").SitioRecepcionId
                    },
                    new HuecoRecepcion()
                    {
                        Nombre = "B03",
                        UnidadesTotales = 50,
                        VolumenTotal = 40,
                        SitioId = context.SitiosRecepciones.Local.Single(sr => sr.Nombre == "Sitio B").SitioRecepcionId
                    });
            context.SaveChanges();

            context.Procedencias.AddOrUpdate(
                tc => tc.Nombre,
                new Procedencia()
                {
                    Nombre = "Procedencia 1",
                    Descripcion = "Esta es la Procedencia 1"
                },
                new Procedencia()
                {
                    Nombre = "Procedencia 2",
                    Descripcion = "Esta es la Procedencia 2"
                });
            context.SaveChanges();

            context.Recepciones.AddOrUpdate(
                rc => rc.NumeroAlbaran,
                new Recepcion()
                {
                    NumeroAlbaran = "A-0100B",
                    FechaRecepcion = new DateTime(2017, 02, 10),
                    ProveedorId = context.Proveedores.Local.Single(p => p.ProveedorId == 1).ProveedorId,
                    EstadoId = context.EstadosRecepciones.Local.Single(e => e.EstadoRecepcionId == 1).EstadoRecepcionId
                },
                new Recepcion()
                {
                    NumeroAlbaran = "A-010VB",
                    FechaRecepcion = new DateTime(2017, 01, 20),
                    ProveedorId = context.Proveedores.Local.Single(p => p.ProveedorId == 2).ProveedorId,
                    EstadoId = context.EstadosRecepciones.Local.Single(e => e.EstadoRecepcionId == 2).EstadoRecepcionId
                });
            context.SaveChanges();

            context.GruposMateriasPrimas.AddOrUpdate(
                gmp => gmp.Nombre,
                new GrupoMateriaPrima()
                {
                    Nombre = "GrupoMateriaPrima 1",
                    Descripcion = "Descripción para Grupo 1"
                },
                new GrupoMateriaPrima()
                {
                    Nombre = "GrupoMateriaPrima 2",
                    Descripcion = "Descripción para Grupo 2"
                });
            context.SaveChanges();


            context.TiposMateriasPrimas.AddOrUpdate(
                tmp => tmp.Nombre,
                new TipoMateriaPrima()
                {
                    Nombre = "TipoMateriaPrima 1",
                    Descripcion = "Descripción para Tipo 1",
                    MedidoEnUnidades = true,
                    GrupoId = context.GruposMateriasPrimas.Local.Single(gc => gc.Nombre == "GrupoMateriaPrima 1").GrupoMateriaPrimaId
                },
                new TipoMateriaPrima()
                {
                    Nombre = "TipoMateriaPrima 2",
                    Descripcion = "Descripción para Tipo 2",
                    MedidoEnVolumen = true,
                    GrupoId = context.GruposMateriasPrimas.Local.Single(gc => gc.Nombre == "GrupoMateriaPrima 2").GrupoMateriaPrimaId
                });
            context.SaveChanges();


            context.MateriasPrimas.AddOrUpdate(
                mp => mp.Codigo,
                new MateriaPrima()
                {
                    TipoId = context.TiposMateriasPrimas.Local.Single(tmp => tmp.Nombre == "TipoMateriaPrima 1").TipoMateriaPrimaId,
                    Unidades = 35,
                    Observaciones = "Es muy rico",
                    RecepcionId = context.Recepciones.Local.Single(r => r.NumeroAlbaran == "A-0100B").RecepcionId,
                    ProcedenciaId = context.Procedencias.Local.Single(p => p.Nombre == "Procedencia 1").ProcedenciaId,
                    Codigo = "1000000000"
                },
                new MateriaPrima()
                {
                    TipoId = context.TiposMateriasPrimas.Local.Single(tmp => tmp.Nombre == "TipoMateriaPrima 2").TipoMateriaPrimaId,
                    Volumen = 50,
                    Observaciones = "Falta de respeto",
                    RecepcionId = context.Recepciones.Local.Single(r => r.NumeroAlbaran == "A-010VB").RecepcionId,
                    ProcedenciaId = context.Procedencias.Local.Single(p => p.Nombre == "Procedencia 2").ProcedenciaId,
                    Codigo = "1000000001"
                });
            context.SaveChanges();

            context.HistorialHuecosRecepciones.AddOrUpdate(
                hhr => hhr.HistorialHuecoRecepcionId,
                new HistorialHuecoRecepcion()
                {
                    HistorialHuecoRecepcionId = 1,
                    Unidades = 30,
                    UnidadesRestantes = 30,
                    MateriaPrimaId = context.MateriasPrimas.Local.Single(mp => mp.Codigo == "1000000000").MateriaPrimaId,
                    HuecoRecepcionId = context.HuecosRecepciones.Local.Single(hr => hr.Nombre == "A01").HuecoRecepcionId
                },
                new HistorialHuecoRecepcion()
                {
                    HistorialHuecoRecepcionId = 2,
                    Volumen = 5,
                    VolumenRestante = 5,
                    MateriaPrimaId = context.MateriasPrimas.Local.Single(mp => mp.Codigo == "1000000000").MateriaPrimaId,
                    HuecoRecepcionId = context.HuecosRecepciones.Local.Single(hr => hr.Nombre == "B01").HuecoRecepcionId
                },

                new HistorialHuecoRecepcion()
                {
                    HistorialHuecoRecepcionId = 3,
                    Volumen = 50,
                    VolumenRestante = 50,
                    MateriaPrimaId = context.MateriasPrimas.Local.Single(mp => mp.Codigo == "1000000001").MateriaPrimaId,
                    HuecoRecepcionId = context.HuecosRecepciones.Local.Single(hr => hr.Nombre == "B02").HuecoRecepcionId
                });
            context.SaveChanges();

            context.EstadosElaboraciones.AddOrUpdate(
                 ee => ee.Nombre,
                 new EstadoElaboracion()
                 {
                     Nombre = "Procesando",
                     Descripcion = "La orden de elaboración se está procesando"
                 },
                  new EstadoElaboracion()
                  {
                      Nombre = "Finalizado",
                      Descripcion = "La orden de elaboración ha finalizado"
                  });
            context.SaveChanges();

            context.GruposProductosTerminados.AddOrUpdate(
                gpt => gpt.Nombre,
                new GrupoProductoTerminado()
                {
                    Nombre = "GrupoProductoTerminado 1",
                    Descripcion = "Descripción para GPT 1"
                },
                new GrupoProductoTerminado()
                {
                    Nombre = "GrupoProductoTerminado 2",
                    Descripcion = "Descripción para GPT 2"
                });
            context.SaveChanges();

            context.TiposProductosTerminados.AddOrUpdate(
                tpt => tpt.Nombre,
                new TipoProductoTerminado()
                {
                    Nombre = "TipoProductoTerminado 1",
                    Tamano = "0.75",
                    Humedad = 75,
                    MedidoEnVolumen = true,
                    GrupoId = context.GruposProductosTerminados.Local.Single(gi => gi.Nombre == "GrupoProductoTerminado 1").GrupoProductoTerminadoId

                },
                new TipoProductoTerminado()
                {
                    Nombre = "TipoProductoTerminado 2",
                    Tamano = "0.80",
                    Humedad = 85,
                    MedidoEnUnidades = true,
                    GrupoId = context.GruposProductosTerminados.Local.Single(gi => gi.Nombre == "GrupoProductoTerminado 2").GrupoProductoTerminadoId
                });
            context.SaveChanges();

            context.OrdenesElaboraciones.AddOrUpdate(
                oe => oe.OrdenElaboracionId,
                new OrdenElaboracion()
                {
                    OrdenElaboracionId = 1,
                    Descripcion = "Descrición 1",
                    EstadoElaboracionId = context.EstadosElaboraciones.Local.Single(ee => ee.Nombre == "Procesando").EstadoElaboracionId
                },
                new OrdenElaboracion()
                {
                    OrdenElaboracionId = 2,
                    Descripcion = "Descrición 2",
                    EstadoElaboracionId = context.EstadosElaboraciones.Local.Single(ee => ee.Nombre == "Finalizado").EstadoElaboracionId
                });
            context.SaveChanges();

            

            context.ProductosTerminados.AddOrUpdate(
                pt => pt.ProductoTerminadoId,
                new ProductoTerminado()
                {
                    Unidades = 40,
                    TipoId = context.TiposProductosTerminados.Local.Single(ti => ti.Nombre == "TipoProductoTerminado 1").TipoProductoTerminadoId,
                    OrdenId = context.OrdenesElaboraciones.Local.Single(oe => oe.OrdenElaboracionId == 1).OrdenElaboracionId,
                    Codigo = "2000000000"
                }/*,
                new ProductoTerminado()
                {
                    Volumen = 50,
                    Observaciones = "Observaciones .......",
                    TipoId = context.TiposProductosTerminados.Local.Single(ti => ti.Nombre == "TipoProductoTerminado 2").TipoProductoTerminadoId,
                    OrdenId = context.OrdenesElaboraciones.Local.Single(oe => oe.OrdenElaboracionId == 2).OrdenElaboracionId,
                    Codigo = "2000000001"
                }*/);
            context.SaveChanges();

            context.ProductosTerminadosComposiciones.AddOrUpdate(
                ptc => ptc.ProductoTerminadoComposicionId,
                new ProductoTerminadoComposicion()
                {
                    ProductoTerminadoComposicionId = 1,
                    Unidades = 30,
                    HistorialHuecoId = context.HistorialHuecosRecepciones.Local.Single(hh => hh.HistorialHuecoRecepcionId == 1).HistorialHuecoRecepcionId,
                    ProductoId = context.ProductosTerminados.Local.Single(p => p.Codigo == "2000000000").ProductoTerminadoId
                },
                new ProductoTerminadoComposicion()
                {
                    ProductoTerminadoComposicionId = 2,
                    Unidades = 2,
                    HistorialHuecoId = context.HistorialHuecosRecepciones.Local.Single(hh => hh.HistorialHuecoRecepcionId == 2).HistorialHuecoRecepcionId,
                    ProductoId = context.ProductosTerminados.Local.Single(p => p.Codigo == "2000000000").ProductoTerminadoId
                },
                new ProductoTerminadoComposicion()
                {
                    ProductoTerminadoComposicionId = 3,
                    Volumen = 2,
                    HistorialHuecoId = context.HistorialHuecosRecepciones.Local.Single(hh => hh.HistorialHuecoRecepcionId == 3).HistorialHuecoRecepcionId,
                    ProductoId = context.ProductosTerminados.Local.Single(p => p.Codigo == "2000000000").ProductoTerminadoId
                });
            context.SaveChanges();

            context.SitiosAlmacenajes.AddOrUpdate(
                 sa => sa.Nombre,
                 new SitioAlmacenaje()
                 {
                     Nombre = "Sitio A",
                     Descripcion = "Este es el Sitio A"
                 },
                  new SitioAlmacenaje()
                  {
                      Nombre = "Sitio B",
                      Descripcion = "Este es el Sitio A"
                  });
            context.SaveChanges();

            context.HuecoAlmacenajes.AddOrUpdate(
                 ha => ha.Nombre,
                 new HuecoAlmacenaje()
                 {
                     Nombre = "A01",
                     UnidadesTotales = 30,
                     VolumenTotal = 20,
                     SitioId = context.SitiosAlmacenajes.Local.Single(sa => sa.Nombre == "Sitio A").SitioAlmacenajeId
                 },
                  new HuecoAlmacenaje()
                  {
                      Nombre = "A02",
                      UnidadesTotales = 25,
                      VolumenTotal = 15,
                      SitioId = context.SitiosAlmacenajes.Local.Single(sa => sa.Nombre == "Sitio A").SitioAlmacenajeId
                  },
                   new HuecoAlmacenaje()
                   {
                       Nombre = "A03",
                       UnidadesTotales = 60,
                       VolumenTotal = 50,
                       SitioId = context.SitiosAlmacenajes.Local.Single(sa => sa.Nombre == "Sitio A").SitioAlmacenajeId
                   },
                    new HuecoAlmacenaje()
                    {
                        Nombre = "B01",
                        UnidadesTotales = 10,
                        VolumenTotal = 5,
                        SitioId = context.SitiosAlmacenajes.Local.Single(sa => sa.Nombre == "Sitio B").SitioAlmacenajeId
                    },
                    new HuecoAlmacenaje()
                    {
                        Nombre = "B02",
                        UnidadesTotales = 60,
                        VolumenTotal = 50,
                        SitioId = context.SitiosAlmacenajes.Local.Single(sa => sa.Nombre == "Sitio B").SitioAlmacenajeId
                    },
                    new HuecoAlmacenaje()
                    {
                        Nombre = "B03",
                        UnidadesTotales = 50,
                        VolumenTotal = 40,
                        SitioId = context.SitiosAlmacenajes.Local.Single(sa => sa.Nombre == "Sitio B").SitioAlmacenajeId
                    });
            context.SaveChanges();

            context.HistorialHuecosAlmacenajes.AddOrUpdate(
                hha => hha.HistorialHuecoAlmacenajeId,
                new HistorialHuecoAlmacenaje()
                {
                    HistorialHuecoAlmacenajeId = 1,
                    Unidades = 30,
                    UnidadesRestantes = 30,
                    ProductoTerminadoId = context.ProductosTerminados.Local.Single(pt => pt.Codigo == "2000000000").ProductoTerminadoId,
                    HuecoAlmacenajeId = context.HuecoAlmacenajes.Local.Single(ha => ha.Nombre == "A01").HuecoAlmacenajeId
                },
                 new HistorialHuecoAlmacenaje()
                 {
                     HistorialHuecoAlmacenajeId = 2,
                     Unidades = 10,
                     UnidadesRestantes = 10,
                     ProductoTerminadoId = context.ProductosTerminados.Local.Single(pt => pt.Codigo == "2000000000").ProductoTerminadoId,
                     HuecoAlmacenajeId = context.HuecoAlmacenajes.Local.Single(ha => ha.Nombre == "A02").HuecoAlmacenajeId
                 });
            context.SaveChanges();

            /*   var usuarios = Builder<Usuario>.CreateListOfSize(100)
                  .All()
                      .With(c => c.Nombre = Faker.Internet.UserName())
                      .With(c => c.Email = Faker.Internet.Email())
                      .With(c => c.Contrasena = "1111111111111111111111111111111111111111111111111111111111111111")
                      .With(c => c.FechaAlta = DateTime.Now.AddDays(-new RandomGenerator().Next(1, 100)))
                      .With(c => c.TipoUsuario = Pick<TipoUsuario>.RandomItemFrom(context.TiposUsuarios.ToList()))
                  //.With(c => c.Baneado = new RandomGenerator().Boolean())
                  //.With(c => c.Baneado == false ? c.FechaBaja = DateTime.Now : c.FechaBaja = null)
                  .Random(30)
                      .With(c => c.Baneado = true)
                      .With(c => c.FechaBaja = DateTime.Now)
                   .Random(10)
                      .With(c => c.FechaContrasena = DateTime.Now)
                  .Build();

               context.Usuarios.AddOrUpdate(c => c.UsuarioId, usuarios.ToArray());*/

                //new SeedCodigosPostales(context); 

                // new SeedTablas(context);

        }
    }
}
