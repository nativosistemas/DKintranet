USE [db_Kellerhoff]
GO
/****** Object:  StoredProcedure [Clientes].[spRecuperarTodosClientesBySucursal]    Script Date: 09/26/2019 15:21:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Batch submitted through debugger: SQLQuery4.sql|4|0|C:\Users\Pablo\AppData\Local\Temp\3\~vsB652.sql
ALTER PROCEDURE [Clientes].[spRecuperarTodosClientesBySucursal]
@cli_sucursal nvarchar(2)
AS
SELECT [cli_codigo]
      ,[cli_nombre]
      ,[cli_nomweb]
      ,[cli_estado]
      ,[cli_TitularPuntoVenta]
      ,[cli_fecnac]
      ,[cli_codprov]
      ,[cli_localidad]
      ,[cli_cpa]
      ,[cli_dirección]
      ,[cli_Telefono]
      ,[cli_email]
      ,[cli_login]
      ,[cli_password]
      ,[cli_paginaweb]
      ,[cli_recibenotificaciones]
      ,[cli_es24hs]
      ,[cli_mostrarweb]
      ,[cli_mostraremail]
      ,[cli_mostrartelefono]
      ,[cli_mostrardireccion]
      ,[cli_contactotecnico]
      ,[cli_codtpoenv]
      ,[cli_codrep]
      ,[cli_codsuc]
      ,[cli_cobracadeteria]
      ,[cli_pordesespmed]
      ,[cli_pordesbetmed]
      ,[cli_pordesnetos]
      ,[cli_pordesfinperfcyo]
      ,[cli_pordescomperfcyo]
      ,[cli_deswebespmed]
      ,[cli_deswebnetmed]
      ,[cli_deswebnetacc]
      ,[cli_deswebnetperpropio]
      ,[cli_deswebnetpercyo]
      ,cli_destransfer
      ,cli_isGLN
      ,cli_tomaOfertas 
      ,cli_tomaPerfumeria 
      ,cli_tomaTransfers
      ,cli_tipo
	  ,cli_IdSucursalAlternativa
	  ,cli_AceptaPsicotropicos
	  ,[cli_promotor]
  FROM [dbo].[tbl_Clientes]
  WHERE [cli_codsuc] = @cli_sucursal
  ORDER BY cli_nombre