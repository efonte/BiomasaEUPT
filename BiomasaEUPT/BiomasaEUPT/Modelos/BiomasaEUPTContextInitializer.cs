using System.Data.Entity;

namespace BiomasaEUPT.Modelos
{
    //public class BiomasaEUPTContextInitializer : CreateDatabaseIfNotExists<BiomasaEUPTContext>
    public class BiomasaEUPTContextInitializer : DropCreateDatabaseAlways<BiomasaEUPTContext>
    {
        protected override void Seed(BiomasaEUPTContext context)
        {
            // http://www.drowningintechnicaldebt.com/ShawnWeisfeld/archive/2011/07/15/entity-framework-code-first-executing-sql-files-on-database-creation.aspx

            context.Database.ExecuteSqlCommand(
@"
ALTER TABLE Usuarios ADD CONSTRAINT DF_UsuariosFechaAlta DEFAULT GETDATE() FOR FechaAlta;
ALTER TABLE Usuarios ADD CONSTRAINT DF_UsuariosBaneado DEFAULT 0 FOR Baneado;
ALTER TABLE ProductosTerminados ADD CONSTRAINT DF_ProductosTerminadosFechaElaboracion DEFAULT GETDATE() FOR FechaElaboracion;
"
                );
            base.Seed(context);
        }
    }
}