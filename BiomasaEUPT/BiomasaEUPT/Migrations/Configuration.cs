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
                    Descripcion = "Este es el TipoProveedor 1",
                },
                new TipoProveedor()
                {
                    Nombre = "TipoProveedor 2",
                    Descripcion = "Este es el TipoProveedor 2",
                });
            context.SaveChanges();

            context.GruposClientes.AddOrUpdate(
               gc => gc.Nombre,
               new GrupoCliente()
               {
                   Nombre = "GrupoCliente 1",
                   Descripcion = "Este es el GrupoCliente 1",
               },
               new GrupoCliente()
               {
                   Nombre = "GrupoCliente 2",
                   Descripcion = "Este es el GrupoCliente 2",
               });
            context.SaveChanges();

            context.TiposClientes.AddOrUpdate(
               tc => tc.Nombre,
               new TipoCliente()
               {
                   Nombre = "TipoCliente 1",
                   Descripcion = "Este es el TipoCliente 1",
                   GrupoId = context.GruposClientes.Local.Single(gc => gc.Nombre == "GrupoCliente 1").GrupoClienteId,
               },
               new TipoCliente()
               {
                   Nombre = "TipoCliente 2",
                   Descripcion = "Este es el TipoCliente 2",
                   GrupoId = context.GruposClientes.Local.Single(gc => gc.Nombre == "GrupoCliente 1").GrupoClienteId,
               });
            context.SaveChanges();

            context.Paises.AddOrUpdate(
                  p => p.Codigo,
                  new Pais()
                  {
                      Codigo = "ES",
                      Nombre = "España",
                  },
                  new Pais()
                  {
                      Codigo = "FR",
                      Nombre = "Francia",
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
                    ComunidadId = context.Comunidades.Local.Single(c => c.Codigo == "ES-VC").ComunidadId,
                },
                new Provincia()
                {
                    Codigo = "FR-75",
                    Nombre = "París",
                    ComunidadId = context.Comunidades.Local.Single(c => c.Codigo == "FR-11").ComunidadId,
                },
                new Provincia()
                {
                    Codigo = "ES-TE",
                    Nombre = "Teruel",
                    ComunidadId = context.Comunidades.Local.Single(c => c.Codigo == "ES-AR").ComunidadId,
                },
                new Provincia()
                {
                    Codigo = "ES-V",
                    Nombre = "Valencia",
                    ComunidadId = context.Comunidades.Local.Single(c => c.Codigo == "ES-VC").ComunidadId,
                },
                new Provincia()
                {
                    Codigo = "ES-Z",
                    Nombre = "Zaragoza",
                    ComunidadId = context.Comunidades.Local.Single(c => c.Codigo == "ES-AR").ComunidadId,
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
                     MunicipioId = context.Municipios.Local.Single(gc => gc.CodigoPostal == "44003").MunicipioId,
                     Calle = "Calle 1"
                 },
                 new Cliente()
                 {
                     RazonSocial = "Cliente 2",
                     Nif = "11111111-B",
                     Email = "cliente2@biomasaeupt.es",
                     TipoId = context.TiposClientes.Local.Single(tc => tc.Nombre == "TipoCliente 1").TipoClienteId,
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
                     //HuecoRecepcionId = 1,
                     Nombre = "A01",
                     UnidadesTotales = 30,
                     VolumenTotal = 20,
                     SitioId = context.SitiosRecepciones.Local.Single(sr => sr.Nombre == "Sitio A").SitioRecepcionId,
                 },
                  new HuecoRecepcion()
                  {
                      //HuecoRecepcionId = 2,
                      Nombre = "A02",
                      UnidadesTotales = 25,
                      VolumenTotal = 15,
                      SitioId = context.SitiosRecepciones.Local.Single(sr => sr.Nombre == "Sitio A").SitioRecepcionId,
                  },
                   new HuecoRecepcion()
                   {
                       //HuecoRecepcionId = 3,
                       Nombre = "A03",
                       UnidadesTotales = 60,
                       VolumenTotal = 50,
                       SitioId = context.SitiosRecepciones.Local.Single(sr => sr.Nombre == "Sitio A").SitioRecepcionId,
                   },
                    new HuecoRecepcion()
                    {
                        //HuecoRecepcionId = 4,
                        Nombre = "B01",
                        UnidadesTotales = 10,
                        VolumenTotal = 5,
                        SitioId = context.SitiosRecepciones.Local.Single(sr => sr.Nombre == "Sitio A").SitioRecepcionId,
                    },
                    new HuecoRecepcion()
                    {
                        //HuecoRecepcionId = 5,
                        Nombre = "B02",
                        UnidadesTotales = 60,
                        VolumenTotal = 50,
                        SitioId = context.SitiosRecepciones.Local.Single(sr => sr.Nombre == "Sitio B").SitioRecepcionId,
                    },
                    new HuecoRecepcion()
                    {
                        //HuecoRecepcionId = 6,
                        Nombre = "B03",
                        UnidadesTotales = 50,
                        VolumenTotal = 40,
                        SitioId = context.SitiosRecepciones.Local.Single(sr => sr.Nombre == "Sitio B").SitioRecepcionId,
                    });
            context.SaveChanges();

            context.Procedencias.AddOrUpdate(
                tc => tc.Nombre,
                new Procedencia()
                {
                    Nombre = "Procedencia 1",
                    Descripcion = "Esta es la Procedencia 1",
                },
                new Procedencia()
                {
                    Nombre = "Procedencia 2",
                    Descripcion = "Esta es la Procedencia 2",
                });
            context.SaveChanges();

            context.Recepciones.AddOrUpdate(
                rc => rc.NumeroAlbaran,
                new Recepcion()
                {
                    NumeroAlbaran = "A-0100B",
                    FechaRecepcion = new DateTime(2017, 02, 10),
                    ProveedorId = context.Proveedores.Local.Single(p => p.ProveedorId == 1).ProveedorId,
                    EstadoId = context.EstadosRecepciones.Local.Single(e => e.EstadoRecepcionId == 1).EstadoRecepcionId,
                },
                new Recepcion()
                {
                    NumeroAlbaran = "A-010VB",
                    FechaRecepcion = new DateTime(2017, 01, 20),
                    ProveedorId = context.Proveedores.Local.Single(p => p.ProveedorId == 2).ProveedorId,
                    EstadoId = context.EstadosRecepciones.Local.Single(e => e.EstadoRecepcionId == 2).EstadoRecepcionId,
                });
            context.SaveChanges();

            context.GruposMateriasPrimas.AddOrUpdate(
                gmp => gmp.Nombre,
                new GrupoMateriaPrima()
                {
                    Nombre = "GrupoMateriaPrima 1",
                    Descripcion = "Descripción para Grupo 1",
                },
                new GrupoMateriaPrima()
                {
                    Nombre = "GrupoMateriaPrima 2",
                    Descripcion = "Descripción para Grupo 2",
                });
            context.SaveChanges();


            context.TiposMateriasPrimas.AddOrUpdate(
                tmp => tmp.Nombre,
                new TipoMateriaPrima()
                {
                    Nombre = "Tipo 1",
                    Descripcion = "Descripción para Tipo 1",
                    MedidoEnUnidades = true,
                    GrupoId = context.GruposMateriasPrimas.Local.Single(gc => gc.Nombre == "GrupoMateriaPrima 1").GrupoMateriaPrimaId,
                },
                new TipoMateriaPrima()
                {
                    Nombre = "Tipo 2",
                    Descripcion = "Descripción para Tipo 2",
                    MedidoEnVolumen = true,
                    GrupoId = context.GruposMateriasPrimas.Local.Single(gc => gc.Nombre == "GrupoMateriaPrima 2").GrupoMateriaPrimaId,
                });
            context.SaveChanges();


            context.MateriasPrimas.AddOrUpdate(
                mp => mp.Codigo,
                new MateriaPrima()
                {
                    TipoId= context.TiposMateriasPrimas.Local.Single(tmp => tmp.Nombre == "Tipo 1").TipoMateriaPrimaId,
                    Unidades = 35,
                    Observaciones = "Es muy rico",
                    RecepcionId = context.Recepciones.Local.Single(r => r.RecepcionId == 1).RecepcionId,
                    ProcedenciaId = context.Procedencias.Local.Single(p => p.Nombre == "Procedencia 1").ProcedenciaId,
                    Codigo="1000000000"
                },
                new MateriaPrima()
                {
                    TipoId = context.TiposMateriasPrimas.Local.Single(tmp => tmp.Nombre == "Tipo 2").TipoMateriaPrimaId,
                    Volumen = 50,
                    Observaciones = "Falta de respeto",
                    RecepcionId = context.Recepciones.Local.Single(r => r.RecepcionId == 2).RecepcionId,
                    ProcedenciaId = context.Procedencias.Local.Single(p => p.Nombre == "Procedencia 2").ProcedenciaId,
                    Codigo = "1000000001"
                });
            context.SaveChanges();

            context.HuecosMateriasPrimas.AddOrUpdate(
                hmp => hmp.HuecoMateriaPrimaId,
                new HuecoMateriaPrima()
                {
                    Unidades = 30,
                    UnidadesRestantes= 30,
                    MateriaPrimaId = context.MateriasPrimas.Local.Single(mp => mp.Codigo == "1000000000").MateriaPrimaId,
                    HuecoRecepcionId = context.HuecosRecepciones.Local.Single(hr => hr.Nombre =="A01" ).HuecoRecepcionId,
                },
                new HuecoMateriaPrima()
                {
                    Volumen = 5,
                    VolumenRestantes = 5,
                    MateriaPrimaId = context.MateriasPrimas.Local.Single(mp => mp.Codigo == "1000000000").MateriaPrimaId,
                    HuecoRecepcionId = context.HuecosRecepciones.Local.Single(hr => hr.Nombre == "B01").HuecoRecepcionId,
                },
                
                new HuecoMateriaPrima()
                {
                    Volumen = 50,
                    VolumenRestantes = 50,
                    MateriaPrimaId = context.MateriasPrimas.Local.Single(mp => mp.Codigo == "1000000001").MateriaPrimaId,
                    HuecoRecepcionId = context.HuecosRecepciones.Local.Single(hr => hr.Nombre == "B02").HuecoRecepcionId,
                });
            context.SaveChanges();

            context.EstadosElaboraciones.AddOrUpdate(
                 ee => ee.Nombre,
                 new EstadoElaboracion()
                 {
                     Nombre = "Nuevo",
                     Descripcion = "La orden de elaboracion es nueva"
                 },
                 new EstadoElaboracion()
                 {
                     Nombre = "Procesado",
                     Descripcion = "La orden de elaboración se está procesando"
                 },
                  new EstadoElaboracion()
                  {
                      Nombre = "Finalizado",
                      Descripcion = "La orden de elaboración ha finalizado"
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
