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


SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[TR_HuecosMateriasPrimas_I]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_HuecosMateriasPrimas_I]
    ON [dbo].[HuecosMateriasPrimas]
    AFTER INSERT
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    UPDATE HuecosRecepciones
    SET    Ocupado = 1
    FROM   inserted i
    WHERE  HuecosRecepciones.HuecoRecepcionId = i.HuecoRecepcionId
END
' 

ALTER TABLE [dbo].[HuecosMateriasPrimas] ENABLE TRIGGER [TR_HuecosMateriasPrimas_I];


"
                );
            base.Seed(context);
        }
    }
}