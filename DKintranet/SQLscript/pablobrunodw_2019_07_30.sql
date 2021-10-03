ALTER TABLE [dbo].[tbl_Clientes]
ADD [cli_promotor] [nvarchar](75)
GO
-----------
ALTER PROCEDURE [Clientes].[spRecuperarClientePorId]
@cli_codigo int
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
	  ,cli_promotor
  FROM tbl_Clientes  
  WHERE cli_codigo = @cli_codigo 
    GO
  ----------------

  ALTER PROCEDURE [Clientes].[spRecuperarTodosClientes]
AS 
--BEGIN TRANSACTION
--BEGIN TRY
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
      ,cli_promotor
  FROM tbl_Clientes   
  order by cli_nombre
  GO
  ---------------------
  CREATE PROCEDURE [Clientes].[spRecuperarTodosClientesByPromotor] 
@cli_promotor nvarchar(75)
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
  WHERE [cli_promotor] = @cli_promotor
  ORDER BY cli_nombre
    GO
--------------------
ALTER PROCEDURE [Clientes].[spSincronizarClientes]
AS 
BEGIN TRANSACTION
BEGIN TRY
--- Agregar clientes nuevos
IF OBJECT_ID ( 'tempdb..#ClientesNuevos' ) IS NOT NULL 
begin
   DROP TABLE #ClientesNuevos
end

SELECT [NumeroCliente]      
  INTO #ClientesNuevos
  FROM [dbo].[tmp_Clientes_Nuevos]
  WHERE  NumeroCliente  not in (select cli_codigo  from tbl_Clientes)
  
INSERT INTO [dbo].[tbl_Clientes]
	  ([cli_codigo]
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
      ,[cli_mostrarweb]
      ,[cli_mostraremail]
      ,[cli_recibenotificaciones]
      ,[cli_es24hs]
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
      ,cli_isGLN
	  ,cli_tomaOfertas
	  ,cli_tomaPerfumeria
	  ,cli_tomaTransfers
	  , cli_tipo
	  ,cli_IdSucursalAlternativa
	  ,cli_AceptaPsicotropicos
	  ,cli_promotor)
 SELECT[NumeroCliente] 
      ,[Nombre]
      ,[NombreWeb]
      ,[Estado]
      ,[TitularPuntoVenta]
      ,[FechaNacimientoTitular]
      ,[Provincia]
      ,[Localidad]
      ,[CPA]
      ,[Direccion]
      ,[Telefono]
      ,[Email]
      ,[Login]
      ,[Password]
      ,[PaginaWEB]
      ,[PublicaPaginaWeb]
      ,[PublicaEmail]
      ,[RecibeBoletinesInformativos]
      ,[Es24Hs]
      ,[PublicaTelefonoWeb]
      ,[PublicaDireccionWeb]
      ,[ContactoTecnicoWeb]
      ,[TipoEnvio]
      ,[CodigoReparto]
      ,[IdSucursal]
      ,[SeLeCobraCadeteria]     
      ,[PorcentajeDescuentoDeEspecialidadesMedicinales]
      ,[PorcentajeDescuentoDeNetosMedicamentos]
      ,[PorcentajeDescuentoDeNetos]
      ,[PorcentajeDescuentoFinancieroPerfCyO]
      ,[PorcentajeComercialPerfCyO]   
      ,[TomaDescuentoWebEspecialidadesMedicinales]
      ,[TomaDescuentoWebNetosMedicamentos]
      ,[TomaDescuentoWebNetosAccesorios]
      ,[TomaDescuentoWebNetosPerfumeriaPropio]
      ,[TomaDescuentoWebNetosPerfumeriaCyO]
      ,[ClienteConGLN]
      ,[TomaOfertas]
      ,[TomaPerfumeria]
      ,[TomaTransfers]
      , Tipo
	  ,IdSucursalAlternativa
	  ,AceptaPsicotropicos
	  ,Promotor
  FROM [dbo].[tmp_Clientes_Nuevos]
  where NumeroCliente in (SELECT NumeroCliente from #ClientesNuevos)
  
  DELETE  FROM [dbo].[tmp_Clientes_Nuevos]
  where NumeroCliente in (SELECT NumeroCliente from #ClientesNuevos)
  
  DROP TABLE #ClientesNuevos
 
 --- Actualizar clientes
 IF OBJECT_ID ( 'tempdb..#ClientesActualizar' ) IS NOT NULL 
begin
   DROP TABLE #ClientesActualizar    
end

 select[NumeroCliente]
  INTO #ClientesActualizar    
  FROM [dbo].[tmp_Clientes_Cambios]
  WHERE [NumeroCliente] in (select cli_codigo from tbl_Clientes)
 
 update tbl_Clientes
 set cli_estado = [Estado],
     cli_pordesespmed = [PorcentajeDescuentoDeEspecialidadesMedicinales],
     cli_pordesbetmed = [PorcentajeDescuentoDeNetosMedicamentos],
     cli_pordesnetos = [PorcentajeDescuentoDeNetos],
     cli_pordesfinperfcyo = [PorcentajeDescuentoFinancieroPerfCyO],
     cli_pordescomperfcyo = [PorcentajeComercialPerfCyO],
     cli_deswebespmed = [TomaDescuentoWebEspecialidadesMedicinales],
     cli_deswebnetmed = [TomaDescuentoWebNetosMedicamentos],
     cli_deswebnetacc = [TomaDescuentoWebNetosAccesorios],
     cli_deswebnetperpropio = [TomaDescuentoWebNetosPerfumeriaPropio],
     cli_deswebnetpercyo = [TomaDescuentoWebNetosPerfumeriaCyO],
     cli_isGLN = [ClienteConGLN],
     cli_tomaOfertas = [TomaOfertas],
     cli_tomaPerfumeria = [TomaPerfumeria],
     cli_tomaTransfers = [TomaTransfers],
     cli_tipo = Tipo,
	 cli_IdSucursalAlternativa = IdSucursalAlternativa,
	 cli_AceptaPsicotropicos = AceptaPsicotropicos,
	 cli_promotor = Promotor
 FROM tbl_Clientes tb1 INNER JOIN [dbo].[tmp_Clientes_Cambios] tb2 ON tb1.cli_codigo = tb2.NumeroCliente     
 where cli_codigo in (select NumeroCliente from #ClientesActualizar)
 
  DELETE FROM [dbo].[tmp_Clientes_Cambios]
  WHERE [NumeroCliente] in (select [NumeroCliente] from #ClientesActualizar )
 
  DROP TABLE #ClientesActualizar    
    
 EXEC LogRegistro.spHistorialProcesos @descripcion = N'', @nombreProcedimiento=N'Clientes.spSincronizarClientes';  
    
COMMIT TRANSACTION 
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION 
EXEC LogRegistro.spLogError @mensaje = N'';
END CATCH
  GO
------------------
ALTER PROCEDURE [Clientes].[spSincronizarClientes_Todos]
AS 
BEGIN TRANSACTION
BEGIN TRY

DECLARE @CANT INT

SELECT @CANT = COUNT(*) FROM tmp_Clientes_Todos

IF  @CANT <> 0 
BEGIN

DELETE FROM tbl_Clientes

INSERT INTO [dbo].[tbl_Clientes]
	  ([cli_codigo]
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
      ,[cli_mostrarweb]
      ,[cli_mostraremail]
      ,[cli_recibenotificaciones]
      ,[cli_es24hs]
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
      ,cli_isGLN
	  ,cli_tomaOfertas
	  ,cli_tomaPerfumeria
	  ,cli_tomaTransfers 
	  ,cli_tipo
	  ,cli_IdSucursalAlternativa
	  ,cli_AceptaPsicotropicos
	  ,cli_promotor)
SELECT [NumeroCliente] 
      ,[Nombre]
      ,[NombreWeb]
      ,[Estado]
      ,[TitularPuntoVenta]
      ,[FechaNacimientoTitular]
      ,[Provincia]
      ,[Localidad]
      ,[CPA]
      ,[Direccion]
      ,[Telefono]
      ,[Email]
      ,[Login]
      ,[Password]
      ,[PaginaWEB]
      ,[PublicaPaginaWeb]
      ,[PublicaEmail]
      ,[RecibeBoletinesInformativos]
      ,[Es24Hs]
      ,[PublicaTelefonoWeb]
      ,[PublicaDireccionWeb]
      ,[ContactoTecnicoWeb]
      ,[TipoEnvio]
      ,[CodigoReparto]
      ,[IdSucursal]
      ,[SeLeCobraCadeteria]     
      ,[PorcentajeDescuentoDeEspecialidadesMedicinales]
      ,[PorcentajeDescuentoDeNetosMedicamentos]
      ,[PorcentajeDescuentoDeNetos]
      ,[PorcentajeDescuentoFinancieroPerfCyO]
      ,[PorcentajeComercialPerfCyO]   
      ,[TomaDescuentoWebEspecialidadesMedicinales]
      ,[TomaDescuentoWebNetosMedicamentos]
      ,[TomaDescuentoWebNetosAccesorios]
      ,[TomaDescuentoWebNetosPerfumeriaPropio]
      ,[TomaDescuentoWebNetosPerfumeriaCyO]
      ,[ClienteConGLN]
      ,[TomaOfertas]
      ,[TomaPerfumeria]
      ,[TomaTransfers]
      ,[Tipo]
	  ,IdSucursalAlternativa
	  ,AceptaPsicotropicos
	  ,Promotor
  FROM [dbo].[tmp_Clientes_Todos]
  
 DELETE  FROM tmp_Clientes_Todos
 
END 
  
 EXEC LogRegistro.spHistorialProcesos @descripcion = N'', @nombreProcedimiento=N'Clientes.spSincronizarClientes_Todos';  
    
COMMIT TRANSACTION 
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION 
EXEC LogRegistro.spLogError @mensaje = N'';
END CATCH
  GO
---------
ALTER PROCEDURE [Seguridad].[spInicioSession] 
    @login nvarchar(255), 
    @Password nvarchar(255), 
    @Ip nvarchar(255), 
    @Host nvarchar(255),
    @UserName nvarchar(255)
AS 
BEGIN
BEGIN TRANSACTION
declare @codigoUsuarioLog int
DECLARE @codigoUsuario int
 SET @codigoUsuario =  (SELECT usu_codigo  FROM Seguridad.tbl_usuarios
 WHERE usu_login = @login and PWDCOMPARE (@Password , usu_psw) = 1 )
IF @codigoUsuario is not null
BEGIN
INSERT INTO Seguridad.tblUsuarioLog
(ulg_codUsuario, ulg_FechaIngreso, ulg_Ip, ulg_Host, ulg_UserAgent)
values (@codigoUsuario, GETDATE(), @Ip, @Host, @UserName);
set  @codigoUsuarioLog = SCOPE_IDENTITY()	
END
select usu_codigo,usu_codRol, usu_nombre + ' ' + usu_apellido as NombreYapellido, usu_apellido + ', ' + usu_nombre as ApNombre,ulg_codUsuarioLog,usu_estado,usu_codCliente,usu_pswDesencriptado
from Seguridad.tbl_usuarios  inner join  Seguridad.tblUsuarioLog on  usu_codigo = ulg_codUsuario 
where ulg_codUsuarioLog = @codigoUsuarioLog 
COMMIT
END
GO