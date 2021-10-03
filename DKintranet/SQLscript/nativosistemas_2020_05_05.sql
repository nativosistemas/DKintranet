
CREATE TABLE [LogRegistro].[tbMensajesNew](
	[tmn_codigo] [int] IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[tmn_asunto] [nvarchar](300) NOT NULL DEFAULT (''),
	[tmn_mensaje] [nvarchar](max) NOT NULL DEFAULT (''),
	[tmn_fecha] [datetime] NULL,
	[tmn_fechaDesde] [datetime] NULL,
	[tmn_fechaHasta] [datetime] NULL,
	[tmn_importante] [bit] NULL DEFAULT ((0)),
	[tmn_todosSucursales] [nvarchar](500)) 

GO
--- Actualizar y insertar mensajes
IF OBJECT_ID ( 'LogRegistro.spActualizarInsertarMensajeNew', 'P' ) IS NOT NULL 
    DROP PROCEDURE LogRegistro.spActualizarInsertarMensajeNew;
GO
CREATE procedure LogRegistro.spActualizarInsertarMensajeNew
@codigo int,
@asunto nvarchar (300),
@mensaje nvarchar (max),
@fechaDesde datetime, 
@fechaHasta datetime,
@importante bit,
@todosSucursales nvarchar (500)
 AS
BEGIN TRANSACTION
BEGIN TRY

IF @codigo = 0
BEGIN

INSERT INTO LogRegistro.tbMensajesNew
(tmn_asunto ,tmn_mensaje,tmn_fecha,tmn_fechaDesde,tmn_fechaHasta,tmn_importante,tmn_todosSucursales)
VALUES(@asunto,@mensaje, GETDATE(),@fechaDesde,@fechaHasta,@importante,@todosSucursales)

SELECT SCOPE_IDENTITY()	 as tmn_codigo

END
ELSE
BEGIN

UPDATE LogRegistro.tbMensajesNew
SET   tmn_asunto = @asunto,
	  tmn_mensaje = @mensaje,
	  tmn_fecha = GETDATE(),
	  tmn_fechaDesde = @fechaDesde,
	  tmn_fechaHasta = @fechaHasta,
	  tmn_importante = @importante,
	  tmn_todosSucursales =  @todosSucursales
WHERE tmn_codigo = @codigo

SELECT @codigo as tmn_codigo
END

COMMIT TRANSACTION 
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION 
EXEC LogRegistro.spLogError @mensaje = N'';
END CATCH
GO
--- Elimimar mensaje
IF OBJECT_ID ( 'LogRegistro.spElimimarMensajeNewPorId', 'P' ) IS NOT NULL 
    DROP PROCEDURE LogRegistro.spElimimarMensajeNewPorId;
GO
CREATE procedure LogRegistro.spElimimarMensajeNewPorId
@codigo int
 AS
BEGIN TRANSACTION
BEGIN TRY

DELETE FROM LogRegistro.tbMensajesNew
WHERE tmn_codigo = @codigo

COMMIT TRANSACTION 
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION 
EXEC LogRegistro.spLogError @mensaje = N'';
END CATCH
GO
--- Recuperar todos mensajes 
IF OBJECT_ID ( 'LogRegistro.spRecuperartTodosMensajesNew', 'P' ) IS NOT NULL 
    DROP PROCEDURE LogRegistro.spRecuperartTodosMensajesNew;
GO
CREATE procedure LogRegistro.spRecuperartTodosMensajesNew
 AS

SELECT *
  FROM LogRegistro.tbMensajesNew

  --ORDER BY [tmn_fecha] DESC

GO
--- recuperar mensaje por id
IF OBJECT_ID ( 'LogRegistro.spRecuperarMensajeNewPorId', 'P' ) IS NOT NULL 
    DROP PROCEDURE LogRegistro.spRecuperarMensajeNewPorId;
GO
CREATE procedure LogRegistro.spRecuperarMensajeNewPorId
@codigo int
 AS

SELECT *
  FROM LogRegistro.tbMensajesNew
WHERE tmn_codigo = @codigo

GO
---
IF OBJECT_ID ( 'LogRegistro.spRecuperartTodosMensajeNewPorSucursal', 'P' ) IS NOT NULL 
    DROP PROCEDURE LogRegistro.spRecuperartTodosMensajeNewPorSucursal;
GO
CREATE procedure [LogRegistro].[spRecuperartTodosMensajeNewPorSucursal]
@codSucursal nvarchar(2)
 AS
 declare @fechaHoy datetime
 set @fechaHoy = GETDATE()
SELECT *
  FROM [LogRegistro].[tbMensajesNew]
WHERE  (tmn_todosSucursales like '%' + @codSucursal + '%' or tmn_todosSucursales is null) 
		and ((tmn_fechaDesde is null and tmn_fechaHasta is null) or 
		tmn_fechaDesde <= @fechaHoy and tmn_fechaHasta >= @fechaHoy)

GO
----