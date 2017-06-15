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
ALTER TABLE HuecosRecepciones ADD CONSTRAINT DF_HuecosRecepcionesOcupado DEFAULT 0 FOR Ocupado;
ALTER TABLE HuecosAlmacenajes ADD CONSTRAINT DF_HuecosAlmacenajesOcupado DEFAULT 0 FOR Ocupado;


SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
-- IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[TR_HistorialHuecosRecepciones_I]'))
EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_HistorialHuecosRecepciones_I]
    ON [dbo].[HistorialHuecosRecepciones]
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
-- ALTER TABLE [dbo].[HistorialHuecosRecepciones] ENABLE TRIGGER [TR_HistorialHuecosRecepciones_I];


EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_HistorialHuecosRecepciones_D]
    ON [dbo].[HistorialHuecosRecepciones]
    AFTER DELETE
AS 
BEGIN
    SET NOCOUNT ON;

    UPDATE HuecosRecepciones
    SET    Ocupado = 0
    FROM   deleted d
    WHERE  HuecosRecepciones.HuecoRecepcionId = d.HuecoRecepcionId
END
'

EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_HistorialHuecosRecepciones_U]
    ON [dbo].[HistorialHuecosRecepciones]
    AFTER UPDATE
AS 
BEGIN
    SET NOCOUNT ON;

    UPDATE HuecosRecepciones
    SET    Ocupado =  CASE WHEN (i.VolumenRestante = 0 OR i.UnidadesRestantes = 0) THEN 0
                           ELSE 1 END
    FROM   inserted i
    WHERE  HuecosRecepciones.HuecoRecepcionId = i.HuecoRecepcionId
END
'

EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_HistorialHuecosAlmacenajes_I]
    ON [dbo].[HistorialHuecosAlmacenajes]
    AFTER INSERT
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    UPDATE HuecosAlmacenajes
    SET    Ocupado = 1
    FROM   inserted i
    WHERE  HuecosAlmacenajes.HuecoAlmacenajeId = i.HuecoAlmacenajeId
END
'


EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_HistorialHuecosAlmacenajes_D]
    ON [dbo].[HistorialHuecosAlmacenajes]
    AFTER DELETE
AS 
BEGIN
    SET NOCOUNT ON;

    UPDATE HuecosAlmacenajes
    SET    Ocupado = 0
    FROM   deleted d
    WHERE  HuecosAlmacenajes.HuecoAlmacenajeId = d.HuecoAlmacenajeId
END
'

EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_HistorialHuecosAlmacenajes_U]
    ON [dbo].[HistorialHuecosAlmacenajes]
    AFTER UPDATE
AS 
BEGIN
    SET NOCOUNT ON;

    UPDATE HuecosAlmacenajes
    SET    Ocupado =  CASE WHEN (i.VolumenRestante = 0 OR i.UnidadesRestantes = 0) THEN 0
                           ELSE 1 END
    FROM   inserted i
    WHERE  HuecosAlmacenajes.HuecoAlmacenajeId = i.HuecoAlmacenajeId
END
'

EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_MateriasPrimas_I]
    ON [dbo].[MateriasPrimas]
    AFTER INSERT
AS 
BEGIN
    SET NOCOUNT ON;

    UPDATE mp
    SET    Codigo = i.MateriaPrimaId + 1000000000
    FROM   MateriasPrimas mp
    JOIN   inserted i ON mp.MateriaPrimaId = i.MateriaPrimaId
END
'

EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_ProductosTerminados_I]
    ON [dbo].[ProductosTerminados]
    AFTER INSERT
AS 
BEGIN
    SET NOCOUNT ON;

    UPDATE pt
    SET    Codigo = i.ProductoTerminadoId + 2000000000
    FROM   ProductosTerminados pt
    JOIN   inserted i ON pt.ProductoTerminadoId = i.ProductoTerminadoId
END
'

"
                );
            base.Seed(context);
        }
    }
}