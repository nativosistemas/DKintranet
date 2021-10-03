
CREATE TABLE [TiposEnvios].[tbl_SucursalDependienteTipoEnvioCliente_TipoEnvios_Excepciones](
	[tdr_id] [int] IDENTITY(1,1) NOT NULL   primary key,
	[tdr_idSucursalDependienteTipoEnvioCliente] [int] NULL REFERENCES TiposEnvios.tbl_SucursalDependienteTipoEnvioCliente(tsd_id),
	[tdr_codReparto] nvarchar(2) NULL,
	[tdr_idTipoEnvio] [int] NULL REFERENCES TiposEnvios.tbl_TiposEnvios(env_id)
	) 
GO
--------------------
--------------------------
IF OBJECT_ID ( 'TiposEnvios.spInsertarEliminarSucursalDependienteTipoEnvioCliente_TipoEnvios_Excepciones', 'P' ) IS NOT NULL 
    DROP PROCEDURE TiposEnvios.spInsertarEliminarSucursalDependienteTipoEnvioCliente_TipoEnvios_Excepciones;
GO
CREATE procedure TiposEnvios.spInsertarEliminarSucursalDependienteTipoEnvioCliente_TipoEnvios_Excepciones
@tdr_id int null,
@tdr_idSucursalDependienteTipoEnvioCliente int null,
@tdr_idTipoEnvio  int null,
@tdr_codReparto  nvarchar(2) null 
 AS
BEGIN TRANSACTION
BEGIN TRY



IF @tdr_id IS NULL
BEGIN

DECLARE @varId INT

SELECT @varId = tdr_id 
FROM TiposEnvios.tbl_SucursalDependienteTipoEnvioCliente_TipoEnvios_Excepciones
WHERE tdr_idSucursalDependienteTipoEnvioCliente = @tdr_idSucursalDependienteTipoEnvioCliente AND tdr_idTipoEnvio = @tdr_idTipoEnvio and tdr_codReparto = @tdr_codReparto
IF @tdr_id IS NULL
BEGIN
INSERT INTO TiposEnvios.tbl_SucursalDependienteTipoEnvioCliente_TipoEnvios_Excepciones
(tdr_idSucursalDependienteTipoEnvioCliente,tdr_idTipoEnvio,tdr_codReparto)
VALUES(@tdr_idSucursalDependienteTipoEnvioCliente,@tdr_idTipoEnvio,@tdr_codReparto)

SET @tdr_id = SCOPE_IDENTITY() 
END


END
ELSE
BEGIN

DELETE FROM TiposEnvios.tbl_SucursalDependienteTipoEnvioCliente_TipoEnvios_Excepciones
WHERE tdr_idSucursalDependienteTipoEnvioCliente = @tdr_idSucursalDependienteTipoEnvioCliente AND tdr_idTipoEnvio = @tdr_idTipoEnvio and tdr_codReparto = @tdr_codReparto

END

SELECT @tdr_id as 'tdr_id'

COMMIT TRANSACTION 
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION 
EXEC LogRegistro.spLogError @mensaje = N'';
END CATCH
GO
---
-------------------
IF OBJECT_ID ( 'TiposEnvios.spRecuperarSucursalDependienteTipoEnvioCliente_TipoEnvios_Excepciones', 'P' ) IS NOT NULL 
    DROP PROCEDURE TiposEnvios.spRecuperarSucursalDependienteTipoEnvioCliente_TipoEnvios_Excepciones;
GO
CREATE procedure TiposEnvios.spRecuperarSucursalDependienteTipoEnvioCliente_TipoEnvios_Excepciones
@tdr_idSucursalDependienteTipoEnvioCliente int,
@tdr_codReparto nvarchar(2)  
 AS
SELECT [tdr_id]
      ,[tdr_idSucursalDependienteTipoEnvioCliente]
      ,[tdr_codReparto]
      ,[tdr_idTipoEnvio]
	  ,env_id
	  ,env_codigo
	  ,[env_nombre]
  FROM [TiposEnvios].[tbl_SucursalDependienteTipoEnvioCliente_TipoEnvios_Excepciones]
  inner join [TiposEnvios].[tbl_TiposEnvios] on [env_id] = [tdr_idTipoEnvio]
  WHERE tdr_idSucursalDependienteTipoEnvioCliente = @tdr_idSucursalDependienteTipoEnvioCliente and tdr_codReparto = @tdr_codReparto

  GO


   --TiposEnvios.spRecuperarSucursalDependienteTipoEnvioCliente_TipoEnvios_Excepciones 12,'A1'