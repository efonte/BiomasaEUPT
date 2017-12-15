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


EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_Usuarios_U]
    ON [dbo].[Usuarios]
    AFTER UPDATE
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    IF UPDATE(Baneado)
    BEGIN
        UPDATE u
        SET    FechaBaja = CASE WHEN i.Baneado = 1 THEN GETDATE() ELSE NULL END
        FROM   Usuarios u        
        JOIN   inserted i ON u.UsuarioId = i.UsuarioId;
    END
    IF UPDATE(Contrasena)
    BEGIN
        UPDATE u
        SET    FechaContrasena = CASE WHEN i.Contrasena IS NOT NULL THEN GETDATE() ELSE NULL END
        FROM   Usuarios u
        JOIN   inserted i ON u.UsuarioId = i.UsuarioId;
    END
END
'


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
    WHERE  HuecosRecepciones.HuecoRecepcionId = i.HuecoRecepcionId;

    UPDATE HistorialHuecosRecepciones
    SET    VolumenRestante = i.Volumen,
           UnidadesRestantes = i.Unidades
    FROM   inserted i
    WHERE  HistorialHuecosRecepciones.HistorialHuecoRecepcionId = i.HistorialHuecoRecepcionId
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
    WHERE  HuecosAlmacenajes.HuecoAlmacenajeId = i.HuecoAlmacenajeId;

    UPDATE HistorialHuecosAlmacenajes
    SET    VolumenRestante = i.Volumen,
           UnidadesRestantes = i.Unidades
    FROM   inserted i
    WHERE  HistorialHuecosAlmacenajes.HistorialHuecoAlmacenajeId = i.HistorialHuecoAlmacenajeId
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

EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_ProductosEnvasados_I]
    ON [dbo].[ProductosEnvasados]
    AFTER INSERT
AS 
BEGIN
    SET NOCOUNT ON;

    UPDATE pe
    SET    Codigo = i.ProductoEnvasadoId + 3000000000
    FROM   ProductosEnvasados pe
    JOIN   inserted i ON pe.ProductoEnvasadoId = i.ProductoEnvasadoId
END
'


EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_ProductosTerminadosComposiciones_I]
    ON [dbo].[ProductosTerminadosComposiciones]
    AFTER INSERT
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    UPDATE HistorialHuecosRecepciones
    SET    VolumenRestante = CASE WHEN i.Volumen IS NOT NULL THEN (VolumenRestante - i.Volumen) ELSE VolumenRestante END,
           UnidadesRestantes = CASE WHEN i.Unidades IS NOT NULL THEN (UnidadesRestantes - i.Unidades) ELSE UnidadesRestantes END
    FROM   inserted i
    WHERE  HistorialHuecosRecepciones.HistorialHuecoRecepcionId = i.HistorialHuecoId

END
'

EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_ProductosTerminadosComposiciones_D]
    ON [dbo].[ProductosTerminadosComposiciones]
    AFTER DELETE
AS 
BEGIN
    SET NOCOUNT ON;

    UPDATE HistorialHuecosRecepciones
    SET    VolumenRestante = CASE WHEN i.Volumen IS NOT NULL THEN (VolumenRestante + i.Volumen) ELSE VolumenRestante END,
           UnidadesRestantes = CASE WHEN i.Unidades IS NOT NULL THEN (UnidadesRestantes + i.Unidades) ELSE UnidadesRestantes END
    FROM   inserted i
    WHERE  HistorialHuecosRecepciones.HistorialHuecoRecepcionId = i.HistorialHuecoId
END
'

EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_ProductosEnvasadosComposiciones_I]
    ON [dbo].[ProductosEnvasadosComposiciones]
    AFTER INSERT
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    UPDATE HistorialHuecosAlmacenajes
    SET    VolumenRestante = CASE WHEN i.Volumen IS NOT NULL THEN (VolumenRestante - i.Volumen) ELSE VolumenRestante END,
           UnidadesRestantes = CASE WHEN i.Unidades IS NOT NULL THEN (UnidadesRestantes - i.Unidades) ELSE UnidadesRestantes END
    FROM   inserted i
    WHERE  HistorialHuecosAlmacenajes.HistorialHuecoAlmacenajeId = i.HistorialHuecoId
END
'

EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_ProductosEnvasadosComposiciones_D]
    ON [dbo].[ProductosEnvasadosComposiciones]
    AFTER DELETE
AS 
BEGIN
    SET NOCOUNT ON;

    UPDATE HistorialHuecosAlmacenajes
    SET    VolumenRestante = CASE WHEN i.Volumen IS NOT NULL THEN (VolumenRestante + i.Volumen) ELSE VolumenRestante END,
           UnidadesRestantes = CASE WHEN i.Unidades IS NOT NULL THEN (UnidadesRestantes + i.Unidades) ELSE UnidadesRestantes END
    FROM   inserted i
    WHERE  HistorialHuecosAlmacenajes.HistorialHuecoAlmacenajeId = i.HistorialHuecoId
END
'


EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_Picking_I]
    ON [dbo].[Picking]
    AFTER INSERT
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON; 

    UPDATE Picking
    SET    VolumenRestante = i.VolumenTotal,
           UnidadesRestantes = i.UnidadesTotales
    FROM   inserted i
    WHERE  Picking.PickingId = i.PickingId
END
'

EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_ProductosEnvasados_Cantidades_I]
    ON [dbo].[ProductosEnvasados]
    AFTER INSERT
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    UPDATE Picking
    SET    VolumenRestante = CASE WHEN i.Volumen IS NOT NULL THEN (VolumenRestante - i.Volumen) ELSE VolumenRestante END,
           UnidadesRestantes = CASE WHEN i.Unidades IS NOT NULL THEN (UnidadesRestantes - i.Unidades) ELSE UnidadesRestantes END
    FROM   inserted i
    WHERE  Picking.PickingId = i.PickingId
END
'

EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_ProductosEnvasados_Cantidades_D]
    ON [dbo].[ProductosEnvasados]
    AFTER DELETE
AS 
BEGIN
    SET NOCOUNT ON;

    UPDATE Picking
    SET    VolumenRestante = CASE WHEN i.Volumen IS NOT NULL THEN (VolumenRestante + i.Volumen) ELSE VolumenRestante END,
           UnidadesRestantes = CASE WHEN i.Unidades IS NOT NULL THEN (UnidadesRestantes + i.Unidades) ELSE UnidadesRestantes END
    FROM   inserted i
    WHERE  Picking.PickingId = i.PickingId
END
'


EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_PedidoDetalle_I]
    ON [dbo].[PedidosDetalles]
    AFTER INSERT
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    UPDATE PedidosLineas
    SET    VolumenPreparado = CASE WHEN i.Volumen IS NOT NULL THEN (VolumenPreparado + i.Volumen) ELSE VolumenPreparado END,
           UnidadesPreparadas = CASE WHEN i.Unidades IS NOT NULL THEN (UnidadesPreparadas + i.Unidades) ELSE UnidadesPreparadas END
    FROM   inserted i
    WHERE  PedidosLineas.PedidoLineaId = i.PedidoLineaId
END
'

EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_PedidoDetalle_D]
    ON [dbo].[PedidosDetalles]
    AFTER INSERT
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    UPDATE PedidosLineas
    SET    VolumenPreparado = CASE WHEN i.Volumen IS NOT NULL THEN (VolumenPreparado - i.Volumen) ELSE VolumenPreparado END,
           UnidadesPreparadas = CASE WHEN i.Unidades IS NOT NULL THEN (UnidadesPreparadas - i.Unidades) ELSE UnidadesPreparadas END
    FROM   inserted i
    WHERE  PedidosLineas.PedidoLineaId = i.PedidoLineaId
END
'

EXEC dbo.sp_executesql @statement = N'
CREATE TRIGGER [dbo].[TR_PedidoDetalleSetCantidad_I]
    ON [dbo].[PedidosDetalles]
    AFTER INSERT
AS 
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    UPDATE PedidosLineas
    SET    VolumenPreparado = CASE WHEN i.Volumen IS NOT NULL THEN (VolumenPreparado + i.Volumen) ELSE VolumenPreparado END,
           UnidadesPreparadas = CASE WHEN i.Unidades IS NOT NULL THEN (UnidadesPreparadas + i.Unidades) ELSE UnidadesPreparadas END
    FROM   inserted i
    WHERE  PedidosLineas.PedidoLineaId = i.PedidoLineaId
END
'
"
                );
            base.Seed(context);
        }
    }
}