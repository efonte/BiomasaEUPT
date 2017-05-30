namespace BiomasaEUPT.Migrations
{
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


        }
    }
}
