alter table  [dbo].[tbl_Productos]
add pro_PrecioBase [decimal](9, 2) NULL,
pro_PorcARestarDelDtoDeCliente [decimal](6, 2) NULL
GO
---
alter table  [dbo].tbl_Clientes
add cli_PorcentajeDescuentoDeEspecialidadesMedicinalesDirecto [decimal](6, 2) NULL
GO
---
alter table  [dbo].tbl_Transfers_Detalle
add tde_PrecioConDescuentoDirecto [decimal](9, 2) NULL,
	tde_PorcARestarDelDtoDeCliente [decimal](6, 2) NULL	
GO
---
ALTER procedure [CAR].[spRecuperarCarritoTransferPorIdCliente]
@lrc_codCliente int,
@tipo [nvarchar](100)
 AS
--BEGIN TRANSACTION
--BEGIN TRY

IF OBJECT_ID ( 'tempdb..#tempTblCarritoTransfer' ) IS NOT NULL 
BEGIN
   DROP TABLE #tempTblCarritoTransfer
END

SELECT car_Fecha as 'ctr_Fecha'
      ,[car_codSucursal] as 'ctr_codSucursal'
      ,[car_codCliente] as 'ctr_codCliente'
	  ,[cat_codTransfer] as 'ctr_codTransfer'
	  ,[cat_id] as 'ctr_id'
	  ,car_id
 INTO #tempTblCarritoTransfer 
  FROM [CAR].[CarritosTransfer] with (NOLOCK)
  inner join [CAR].[Carritos] with (NOLOCK) on [cat_codCarrito] = [car_id]
  WHERE car_codCliente = @lrc_codCliente AND car_tipo = @tipo


IF OBJECT_ID ( 'tempdb..#tempTablaProductosCarritoTransfer' ) IS NOT NULL 
BEGIN
   DROP TABLE #tempTablaProductosCarritoTransfer
END

SELECT cad_id as 'ctd_id'
      ,cad_codCarritosTransfer as 'ctd_idCarritoTransfers'
      ,cad_nameProducto as 'ctd_codProducto'
      ,cad_cantidad as 'ctd_Cantidad'
      ,cad_codUsuario as 'ctd_codUsuario'
      ,cad_Fecha as 'ctd_Fecha'
      ,pro.*
      ,tde.*
	  ,stk_stock
 INTO #tempTablaProductosCarritoTransfer      
  FROM [CAR].[CarritosDetalles] with (NOLOCK)--LogRegistro.CarritoTransfersDetalles
  INNER JOIN tbl_Productos as pro on [cad_nameProducto] = pro_nombre
  INNER JOIN #tempTblCarritoTransfer on ctr_id = [cad_codCarritosTransfer]
  INNER JOIN tbl_Transfers_Detalle as tde on tde_codtfr = ctr_codTransfer AND tde_codpro = [cad_nameProducto]
  INNER JOIN tbl_Productos_Stocks with (NOLOCK) on (stk_codpro = pro_codigo AND stk_codsuc = ctr_codSucursal)
  WHERE [cad_codCarritosTransfer] in (SELECT ctr_id
  FROM #tempTblCarritoTransfer) AND [cad_cantidad] > 0

--SELECT ctr_id
--  FROM #tempTblCarritoTransfer
--WHERE ctr_codCliente = @lrc_codCliente

SELECT *
FROM #tempTablaProductosCarritoTransfer

SELECT ctr_id ,tfr_codigo,tfr_nombre,tfr_deshab,tfr_pordesadi,tfr_tipo,ctr_codSucursal,car_id
  FROM #tempTblCarritoTransfer
  INNER JOIN tbl_Transfers on tfr_codigo = ctr_codTransfer
--WHERE ctr_codCliente = @lrc_codCliente

SELECT tde.*,tfr.*
from tbl_Transfers_Detalle as tde
INNER JOIN  (SELECT * FROM tbl_Transfers WHERE tfr_facturaciondirecta = 1) as tfr on  [tde_codtfr] = tfr.tfr_codigo
WHERE tde_codpro in (SELECT ctd_codProducto   FROM #tempTablaProductosCarritoTransfer)

DROP TABLE #tempTablaProductosCarritoTransfer 
   
--COMMIT TRANSACTION 
--END TRY
--BEGIN CATCH
--ROLLBACK TRANSACTION 
--EXEC LogRegistro.spLogError @mensaje = N'';
--END CATCH
GO
--------------
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
	  ,cli_promotor
	  ,cli_GrupoCliente
	  ,cli_PorcentajeDescuentoDeEspecialidadesMedicinalesDirecto)
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
	  ,GrupoCliente
	  ,PorcentajeDescuentoDeEspecialidadesMedicinalesDirecto
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
	 cli_promotor = Promotor,
	 cli_GrupoCliente = GrupoCliente,
	 cli_PorcentajeDescuentoDeEspecialidadesMedicinalesDirecto = PorcentajeDescuentoDeEspecialidadesMedicinalesDirecto
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
	  ,cli_promotor
	  ,cli_GrupoCliente
	  ,cli_PorcentajeDescuentoDeEspecialidadesMedicinalesDirecto)
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
	  ,GrupoCliente
	  ,PorcentajeDescuentoDeEspecialidadesMedicinalesDirecto
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
ALTER PROCEDURE [Productos].[spSincronizarPrecios]
AS 
BEGIN TRANSACTION
BEGIN TRY

UPDATE tbl_Productos
SET pro_precio = PrecioPublico,
	pro_preciofarmacia = PrecioFarmacia,
	pro_PrecioBase = PrecioBase
FROM tbl_Productos 
INNER JOIN tmp_Productos_Precios ON pro_codigo = NumeroProducto

DELETE  tmp_Productos_Precios
WHERE NumeroProducto in (select pro_codigo from tbl_Productos)

EXEC LogRegistro.spHistorialProcesos @descripcion = N'', @nombreProcedimiento=N'Productos.spSincronizarPrecios';

COMMIT TRANSACTION 
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION 
EXEC LogRegistro.spLogError @mensaje = N'';
END CATCH
GO
ALTER PROCEDURE [Productos].[spSincronizarProductos]
AS 
BEGIN TRANSACTION
BEGIN TRY


--DELETE FROM [dbo].[tbl_Productos]

INSERT [dbo].[tbl_Productos]
	 ([pro_codigo]
      ,[pro_nombre]
      ,[pro_laboratorio]
      ,[pro_accionterapeutica]
      ,[pro_monodroga]
      ,[pro_codtpovta]
      ,[pro_precio]
      ,[pro_preciofarmacia]
      ,[pro_neto]
      ,[pro_isCadenaFrio]
      ,[pro_ofeporcentaje]
      ,[pro_ofeunidades]
      ,[pro_codtpopro]
      ,[pro_troquel]
      ,[pro_codigobarra]
      ,[pro_codigoalfabeta]
      ,[pro_entransfer]
      ,[pro_canmaxima]
      ,[pro_vtasolotransfer]
      ,[pro_descuentoweb]
      ,[pro_isTrazable]
      ,pro_acuerdo
	  ,pro_NoTransfersEnClientesPerf
	  ,pro_Familia
	  ,pro_PackDeVenta
	  ,pro_AceptaVencidos
	  ,pro_PrecioBase
	  ,pro_PorcARestarDelDtoDeCliente)
SELECT [Numero]
      ,[Nombre]
      ,[Laboratorio]
      ,[AccionTerapeutica]
      ,[MonoDroga]
      ,[TipoDeVenta]
      ,[Precio]
      ,[PrecioFarmacia]
      ,[Neto]
      ,[CadenaFrio]
      ,[OfertaMensualPorcentajeDescuento]
      ,[OfertaMensualUnidadesMinimas]
      ,[TipoProducto]
      ,[Troquel]
      ,[CodigoBarra]
      ,[CodigoAlfaBeta]
      ,[EnTransfer]
      ,[CantidadMaximaVentaPorPedido]
      ,[SeVendeSoloPorTransfer]
      ,[PorcentajeDescuentoWeb]
      ,[Trazable]
      ,Acuerdo
	  ,NoTransfersEnClientesPerf
	  ,Familia
	  ,PackDeVenta
	  ,AceptaVencidos
	  ,PrecioBase
	  ,PorcARestarDelDtoDeCliente
  FROM [dbo].[tmp_Productos]
  WHERE Accion = 'I'


UPDATE tbl_Productos
SET [pro_codigo] =  [Numero]
      ,[pro_nombre] =[Nombre]
      ,[pro_laboratorio] = [Laboratorio]
      ,[pro_accionterapeutica] = [AccionTerapeutica]
      ,[pro_monodroga] = [MonoDroga]
      ,[pro_codtpovta] = [TipoDeVenta]
      ,[pro_precio] = [Precio]
      ,[pro_preciofarmacia] = [PrecioFarmacia]
      ,[pro_neto] = [Neto]
      ,[pro_isCadenaFrio] = [CadenaFrio]
      ,[pro_ofeporcentaje] = [OfertaMensualPorcentajeDescuento]
      ,[pro_ofeunidades] = [OfertaMensualUnidadesMinimas]
      ,[pro_codtpopro] = [TipoProducto]
      ,[pro_troquel] = [Troquel]
      ,[pro_codigobarra] = [CodigoBarra]
      ,[pro_codigoalfabeta] = [CodigoAlfaBeta]
      ,[pro_entransfer] = [EnTransfer]
      ,[pro_canmaxima] = [CantidadMaximaVentaPorPedido]
      ,[pro_vtasolotransfer] = [SeVendeSoloPorTransfer]
      ,[pro_descuentoweb] = [PorcentajeDescuentoWeb]
      ,[pro_isTrazable] = [Trazable]
      ,pro_acuerdo = Acuerdo
	  ,pro_NoTransfersEnClientesPerf = NoTransfersEnClientesPerf
	  ,pro_Familia = Familia
	  ,pro_PackDeVenta = PackDeVenta
	  ,pro_AceptaVencidos = AceptaVencidos
	  ,pro_PrecioBase = PrecioBase
	  ,pro_PorcARestarDelDtoDeCliente = PorcARestarDelDtoDeCliente
FROM tbl_Productos 
INNER JOIN tmp_Productos ON Numero = pro_codigo
WHERE  Accion = 'U'

  DELETE FROM [dbo].[tmp_Productos]
  WHERE Accion = 'I' OR Accion = 'U'
  
EXEC LogRegistro.spHistorialProcesos @descripcion = N'', @nombreProcedimiento=N'Productos.spSincronizarProductos';

COMMIT TRANSACTION 
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION 
EXEC LogRegistro.spLogError @mensaje = N'';
END CATCH
GO
ALTER PROCEDURE [Productos].[spSincronizarProductos_Todos]
AS 
BEGIN TRANSACTION
BEGIN TRY

DECLARE @CANT INT

SELECT  @CANT = COUNT(*) FROM [dbo].[tmp_Productos_Todos]

IF @CANT <> 0
BEGIN

DELETE FROM [dbo].[tbl_Productos]

INSERT [dbo].[tbl_Productos]
	 ([pro_codigo]
      ,[pro_nombre]
      ,[pro_laboratorio]
      ,[pro_accionterapeutica]
      ,[pro_monodroga]
      ,[pro_codtpovta]
      ,[pro_precio]
      ,[pro_preciofarmacia]
      ,[pro_neto]
      ,[pro_isCadenaFrio]
      ,[pro_ofeporcentaje]
      ,[pro_ofeunidades]
      ,[pro_codtpopro]
      ,[pro_troquel]
      ,[pro_codigobarra]
      ,[pro_codigoalfabeta]
      ,[pro_entransfer]
      ,[pro_canmaxima]
      ,[pro_vtasolotransfer]
      ,[pro_descuentoweb]
      ,[pro_isTrazable]
      ,pro_acuerdo
	  ,pro_NoTransfersEnClientesPerf
	  ,pro_Familia
	  ,pro_PackDeVenta
	  ,pro_AceptaVencidos
	  ,pro_PrecioBase
	  ,pro_PorcARestarDelDtoDeCliente)
SELECT [Numero]
      ,[Nombre]
      ,[Laboratorio]
      ,[AccionTerapeutica]
      ,[MonoDroga]
      ,[TipoDeVenta]
      ,[Precio]
      ,[PrecioFarmacia]
      ,[Neto]
      ,[CadenaFrio]
      ,[OfertaMensualPorcentajeDescuento]
      ,[OfertaMensualUnidadesMinimas]
      ,[TipoProducto]
      ,[Troquel]
      ,[CodigoBarra]
      ,[CodigoAlfaBeta]
      ,[EnTransfer]
      ,[CantidadMaximaVentaPorPedido]
      ,[SeVendeSoloPorTransfer]
      ,[PorcentajeDescuentoWeb]
      ,[Trazable]
      ,Acuerdo
	  ,NoTransfersEnClientesPerf
	  ,Familia
	  ,PackDeVenta
	  ,AceptaVencidos
	  ,PrecioBase
	  ,PorcARestarDelDtoDeCliente
  FROM [dbo].[tmp_Productos_Todos]

  DELETE FROM [dbo].[tmp_Productos_Todos]
  
END -- IF @CANT <> 0

EXEC LogRegistro.spHistorialProcesos @descripcion = N'', @nombreProcedimiento=N'Productos.spSincronizarProductos_Todos';

COMMIT TRANSACTION 
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION 
EXEC LogRegistro.spLogError @mensaje = N'';
END CATCH
GO
ALTER PROCEDURE [Transfers].[spSincronizarTransfers]
AS 
BEGIN TRANSACTION
BEGIN TRY

DELETE FROM tbl_Transfers_Detalle
WHERE tde_codtfr in (SELECT Id FROM tmp_Transfers WHERE Accion = 'B')

DELETE FROM  tbl_Transfers
WHERE tfr_codigo in (SELECT Id FROM tmp_Transfers WHERE Accion = 'B')



IF OBJECT_ID ( 'tempdb..#tmbNoRepetidos' ) IS NOT NULL 
BEGIN
   DROP TABLE #tmbNoRepetidos
END

select Id 
into #tmbNoRepetidos
from tmp_Transfers
WHERE Accion = 'A'  and Id  not in (select tfr_codigo from tbl_Transfers)


INSERT INTO tbl_Transfers
    ( tfr_codigo
      ,tfr_accion
      ,tfr_nombre
      ,tfr_deshab
      ,tfr_pordesadi
      ,tfr_tipo
      ,tfr_mospap
      ,tfr_minrenglones
      ,tfr_minunidades
      ,tfr_maxunidades
      ,tfr_mulunidades
      ,tfr_fijunidades
      ,tfr_descripcion
      ,tfr_facturaciondirecta
	  ,tfr_provincia)
  SELECT Id
      ,Accion
      ,Nombre
      ,TomaDescuentoHabitual
      ,PorcentajeDescuentoAdicional
      ,Tipo
      ,MostrarProductoAProducto
      ,RenglonesMinimos
      ,UnidadesMinimas
      ,UnidadesMaximas
      ,UnidadesMultiplo
      ,UnidadesFijas
      ,Descripcion
      ,FacturacionDirecta
	  ,Provincia
  FROM tmp_Transfers
 WHERE Accion = 'A' and Id in (select Id from #tmbNoRepetidos)

INSERT INTO tbl_Transfers_Detalle
    ( tde_codtfr
      ,tde_codpro
      ,tde_descripcion
      ,tde_prepublico
      ,tde_predescuento
      ,tde_minuni
      ,tde_maxuni
      ,tde_muluni
      ,tde_fijuni
      ,tde_proobligatorio
      ,tde_unidadesbonificadas
      ,tde_unidadesbonificadasdescripcion
	  ,tde_DescripcionDeProducto
	  ,tde_MaximoUnidadesMensuales
	  ,tde_PrecioConDescuentoDirecto
	  ,tde_PorcARestarDelDtoDeCliente)
  SELECT IDTransfer
      ,NombreProducto
      ,Descripcion
      ,PrecioPublico
      ,PrecioConDescuento
      ,UnidadesMinimas
      ,UnidadesMaximas
      ,UnidadesMultiplo
      ,UnidadesFijas
      ,ProductoObligatorio
      ,UnidadesBonificadas
      ,UnidadesBonificadasDescripcion
	  ,DescripcionDeProducto
	  ,MaximoUnidadesMensuales
	  ,PrecioConDescuentoDirecto
	  ,PorcARestarDelDtoDeCliente
  FROM tmp_Transfers_Detalle
 WHERE IDTransfer in (SELECT Id FROM tmp_Transfers WHERE Accion = 'A' and  Id in (select Id from #tmbNoRepetidos))

  ------

DELETE FROM tbl_TransfersClientes
WHERE tcl_IdTransfer in (SELECT Id FROM tmp_Transfers WHERE Accion = 'B')


INSERT INTO [dbo].tbl_TransfersClientes
	  ([tcl_IdTransfer]
      ,[tcl_NumeroCliente])
SELECT IdTransfer
	  ,NumeroCliente
  FROM [dbo].[TMP_Transfers_Clientes]
  
 DELETE  FROM [TMP_Transfers_Clientes]
 
  -----

 DELETE FROM tmp_Transfers_Detalle -- WHERE IDTransfer in (SELECT Id FROM tmp_Transfers WHERE Accion in( 'A' , 'B'))
 DELETE FROM tmp_Transfers -- WHERE Accion in( 'A' , 'B')
  
 EXEC LogRegistro.spHistorialProcesos @descripcion = N'', @nombreProcedimiento=N'Transfers.spSincronizarTransfers';  

COMMIT TRANSACTION 
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION 
EXEC LogRegistro.spLogError @mensaje = N'';
END CATCH
GO
ALTER PROCEDURE [Transfers].[spSincronizarTransfers_Todos]
AS 
BEGIN TRANSACTION
BEGIN TRY

DECLARE @CANT_TRANSFER INT
DECLARE @CANT_TRANSFER_DETALLE INT

SELECT  @CANT_TRANSFER = COUNT(*) FROM  tmp_Transfers_Todos
SELECT   @CANT_TRANSFER_DETALLE = COUNT(*) FROM tmp_Transfers_Detalle_Todos

IF  @CANT_TRANSFER <> 0 AND  @CANT_TRANSFER_DETALLE <> 0 
BEGIN

DELETE FROM tbl_Transfers_Detalle
DELETE FROM  tbl_Transfers


INSERT INTO tbl_Transfers
    ( tfr_codigo
      ,tfr_accion
      ,tfr_nombre
      ,tfr_deshab
      ,tfr_pordesadi
      ,tfr_tipo
      ,tfr_mospap
      ,tfr_minrenglones
      ,tfr_minunidades
      ,tfr_maxunidades
      ,tfr_mulunidades
      ,tfr_fijunidades
      ,tfr_descripcion
      ,tfr_facturaciondirecta
	  ,tfr_provincia)
  SELECT Id
      ,Accion
      ,Nombre
      ,TomaDescuentoHabitual
      ,PorcentajeDescuentoAdicional
      ,Tipo
      ,MostrarProductoAProducto
      ,RenglonesMinimos
      ,UnidadesMinimas
      ,UnidadesMaximas
      ,UnidadesMultiplo
      ,UnidadesFijas
      ,Descripcion
      ,FacturacionDirecta
	  ,Provincia
  FROM tmp_Transfers_Todos


INSERT INTO tbl_Transfers_Detalle
    ( tde_codtfr
      ,tde_codpro
      ,tde_descripcion
      ,tde_prepublico
      ,tde_predescuento
      ,tde_minuni
      ,tde_maxuni
      ,tde_muluni
      ,tde_fijuni
      ,tde_proobligatorio
      ,tde_unidadesbonificadas
      ,tde_unidadesbonificadasdescripcion
	  ,tde_DescripcionDeProducto
	  ,tde_MaximoUnidadesMensuales
	  ,tde_PrecioConDescuentoDirecto
	  ,tde_PorcARestarDelDtoDeCliente)
  SELECT IDTransfer
      ,NombreProducto
      ,Descripcion
      ,PrecioPublico
      ,PrecioConDescuento
      ,UnidadesMinimas
      ,UnidadesMaximas
      ,UnidadesMultiplo
      ,UnidadesFijas
      ,ProductoObligatorio
      ,UnidadesBonificadas
      ,UnidadesBonificadasDescripcion
	  ,DescripcionDeProducto
	  ,MaximoUnidadesMensuales
	  ,PrecioConDescuentoDirecto
	  ,PorcARestarDelDtoDeCliente
  FROM tmp_Transfers_Detalle_Todos


 DELETE FROM tmp_Transfers_Detalle_Todos
 DELETE FROM tmp_Transfers_Todos
 
END

------------------
DECLARE @CANT_Transfers_Clientes INT

SELECT @CANT_Transfers_Clientes = COUNT(*) FROM [TMP_Transfers_Clientes_Todos]

IF  @CANT_Transfers_Clientes <> 0 
BEGIN

DELETE FROM [tbl_TransfersClientes]

INSERT INTO tbl_TransfersClientes
	  ([tcl_IdTransfer]
      ,[tcl_NumeroCliente])
SELECT IdTransfer
	  ,NumeroCliente
  FROM [dbo].[TMP_Transfers_Clientes_Todos]
  
 DELETE  FROM [TMP_Transfers_Clientes_Todos]
 
END 

------------------


  
 EXEC LogRegistro.spHistorialProcesos @descripcion = N'', @nombreProcedimiento=N'Transfers.spSincronizarTransfers_Todos';  

COMMIT TRANSACTION 
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION 
EXEC LogRegistro.spLogError @mensaje = N'';
END CATCH
GO
--------------------------------------
ALTER FUNCTION [guest].[fn_Transfers] (@codCliente int)  
RETURNS TABLE  
AS  
RETURN   
(  
   SELECT [tfr_codigo]
      ,[tfr_accion]
      ,[tfr_nombre]
      ,[tfr_deshab]
      ,[tfr_pordesadi]
      ,[tfr_tipo]
      ,[tfr_mospap]
      ,[tfr_minrenglones]
      ,[tfr_minunidades]
      ,[tfr_maxunidades]
      ,[tfr_mulunidades]
      ,[tfr_fijunidades]
      ,[tfr_descripcion]
      ,[tfr_facturaciondirecta]
      ,[tfr_provincia]
FROM [tbl_Transfers]
where [tfr_codigo] in (select distinct tcl_IdTransfer
FROM  tbl_TransfersClientes where tcl_NumeroCliente = @codCliente) or [tfr_codigo] not in (select distinct tcl_IdTransfer
FROM  tbl_TransfersClientes )
);
GO
ALTER procedure [LogRegistro].[spCargarCarritoProductoSucursalDesdeArchivoPedidosV5]
@lrc_codSucursal nvarchar(2),
@Sucursal nvarchar(2),
@lrc_codCliente int,
@lcp_codUsuario int,
@Tabla_Detalle ProductosArchivosPedidosTableType READONLY,
@TipoDeArchivo nvarchar(1),
@cli_codprov nvarchar(75),
@cli_isGLN bit
 AS
BEGIN TRANSACTION
BEGIN TRY

----
IF OBJECT_ID ( 'tempdb..#tbl_Transfers_Temporal' ) IS NOT NULL 
BEGIN
   DROP TABLE #tbl_Transfers_Temporal
END
SELECT *
INTO #tbl_Transfers_Temporal
FROM [guest].[fn_Transfers] ( @lrc_codCliente)
----

if @TipoDeArchivo = 'F'
BEGIN
       
SELECT pro_codigo as 'codigo', SUM(cantidad) as cantidad, MIN(nroordenamiento) as 'nroordenamiento'
INTO #tempTablaProductosPedidosF
FROM tbl_Productos
INNER JOIN @Tabla_Detalle ON pro_codigo = codProducto
GROUP BY pro_codigo

select codigo,
       p.*,
       tde_codpro as isTransfer,
CASE ISNULL(val_codprov,'0')
         WHEN '0' THEN 0
         ELSE 1
         END as 'RequiereVale'
          , cantidad
         , nroordenamiento  as 'nroordenamiento'
from #tempTablaProductosPedidosF 
LEFT JOIN tbl_Productos as p  ON pro_codigo = codigo
LEFT JOIN (select distinct tde_codpro from tbl_Transfers_Detalle  INNER JOIN  #tbl_Transfers_Temporal ON tde_codtfr = tfr_codigo WHERE tfr_facturaciondirecta  is null OR tfr_facturaciondirecta  = 0) as tb1 on tb1.tde_codpro = pro_nombre
LEFT JOIN (SELECT val_codprov,val_codpro FROM tbl_Productos_Vales WHERE val_codprov =  @cli_codprov) as tblProductosVales ON pro_codigo = val_codpro
ORDER BY nroordenamiento


select stk_codpro,stk_codsuc,stk_stock
from tbl_Productos_Stocks
where stk_codpro in (select codigo from #tempTablaProductosPedidosF)
AND stk_codsuc in (select sde_sucursalDependiente from Clientes.tbl_sucursalDependiente where sde_sucursal = @Sucursal)

SELECT codProducto  as 'nombreNoEncontrado', SUM(tb1.cantidad) as cantidad, MIN(tb1.nroordenamiento) as 'nroordenamiento'
FROM @Tabla_Detalle as tb1
LEFT JOIN  #tempTablaProductosPedidosF as tb2 ON  codigo = codProducto 
WHERE  ISNULL(codigo ,'-1') = '-1'
GROUP BY codProducto


SELECT tde.*,tfr.*,tcl_IdTransfer
from tbl_Transfers_Detalle as tde
INNER JOIN  (SELECT * FROM #tbl_Transfers_Temporal WHERE tfr_facturaciondirecta = 1) as tfr on  [tde_codtfr] = tfr.tfr_codigo
LEFT JOIN tbl_TransfersClientes on [tde_codtfr] = tcl_IdTransfer and tcl_NumeroCliente = @lrc_codCliente
WHERE tde_codpro in (select pro_nombre from tbl_Productos INNER JOIN #tempTablaProductosPedidosF ON codigo = pro_codigo)

DROP TABLE #tempTablaProductosPedidosF


END
ELSE
BEGIN
IF @TipoDeArchivo = 'S'
BEGIN


--SELECT pro_codigo as 'codigo', SUM(cantidad) as cantidad, MIN(nroordenamiento) as 'nroordenamiento'
--INTO #tempTablaProductosPedidosS
--FROM tbl_Productos
--INNER JOIN @Tabla_Detalle ON (pro_codigobarra = codigobarra OR pro_troquel = troquel OR pro_codigoalfabeta = codigoalfabeta)
--GROUP BY pro_codigo
---------------
SELECT pro_codigo as 'codigo', SUM(cantidad) as cantidad, MIN(nroordenamiento) as 'nroordenamiento'
INTO #tempTablaProductosPedidosS
FROM tbl_Productos
INNER JOIN @Tabla_Detalle ON (RTRIM (LTRIM(pro_codigobarra)) = RTRIM (LTRIM(codigobarra)))
GROUP BY pro_codigo

INSERT INTO #tempTablaProductosPedidosS(codigo,cantidad,nroordenamiento)
SELECT pro_codigo as 'codigo', SUM(cantidad) as cantidad, MIN(nroordenamiento) as 'nroordenamiento'
FROM tbl_Productos 
INNER JOIN @Tabla_Detalle ON (RTRIM (LTRIM(pro_troquel)) = RTRIM (LTRIM(troquel)))
where nroordenamiento not in (select nroordenamiento FROM #tempTablaProductosPedidosS)
GROUP BY pro_codigo

INSERT INTO #tempTablaProductosPedidosS(codigo,cantidad,nroordenamiento)
SELECT pro_codigo as 'codigo', SUM(cantidad) as cantidad, MIN(nroordenamiento) as 'nroordenamiento'
FROM tbl_Productos 
INNER JOIN @Tabla_Detalle ON ( RTRIM (LTRIM(pro_codigoalfabeta)) = RTRIM (LTRIM(codigoalfabeta)))
where nroordenamiento not in (select nroordenamiento FROM #tempTablaProductosPedidosS)
GROUP BY pro_codigo



----------------------------

select codigo,
       p.*,
       tde_codpro as isTransfer,
CASE ISNULL(val_codprov,'0')
         WHEN '0' THEN 0
         ELSE 1
         END as 'RequiereVale'
         , cantidad
         ,nroordenamiento  as 'nroordenamiento'
from #tempTablaProductosPedidosS
LEFT JOIN tbl_Productos as p ON pro_codigo = codigo
LEFT JOIN (select distinct tde_codpro from tbl_Transfers_Detalle  INNER JOIN  #tbl_Transfers_Temporal ON tde_codtfr = tfr_codigo WHERE tfr_facturaciondirecta  is null OR tfr_facturaciondirecta  = 0) as tb1 on tb1.tde_codpro = pro_nombre
LEFT JOIN (SELECT val_codprov,val_codpro FROM tbl_Productos_Vales WHERE val_codprov =  @cli_codprov) as tblProductosVales ON pro_codigo = val_codpro
ORDER BY nroordenamiento



select stk_codpro,stk_codsuc,stk_stock
from tbl_Productos_Stocks
where stk_codpro in (select codigo from #tempTablaProductosPedidosS)
AND stk_codsuc in (select sde_sucursalDependiente from Clientes.tbl_sucursalDependiente where sde_sucursal = @Sucursal)

--
SELECT codigobarra + ' ' + troquel + ' ' + codigoalfabeta  as 'nombreNoEncontrado' ,codigobarra , troquel ,codigoalfabeta,  SUM(cantidad) as cantidad, MIN(nroordenamiento) as 'nroordenamiento'
--INTO #tempTablaProductosPedidosS_AUX
FROM tbl_Productos
RIGHT JOIN @Tabla_Detalle ON (RTRIM (LTRIM(pro_codigobarra)) = RTRIM (LTRIM(codigobarra)) OR RTRIM (LTRIM(pro_troquel)) = RTRIM (LTRIM(troquel)) OR RTRIM (LTRIM(pro_codigoalfabeta)) = RTRIM (LTRIM(codigoalfabeta)))
WHERE ISNULL( pro_codigo,'-1') = '-1'
GROUP BY codigobarra , troquel ,codigoalfabeta

SELECT tde.*,tfr.*,tcl_IdTransfer
from tbl_Transfers_Detalle as tde
INNER JOIN  (SELECT * FROM #tbl_Transfers_Temporal WHERE tfr_facturaciondirecta = 1) as tfr on  [tde_codtfr] = tfr.tfr_codigo
LEFT JOIN tbl_TransfersClientes on [tde_codtfr] = tcl_IdTransfer and tcl_NumeroCliente = @lrc_codCliente
WHERE tde_codpro in (select pro_nombre from tbl_Productos INNER JOIN #tempTablaProductosPedidosS ON codigo = pro_codigo)


--
DROP TABLE #tempTablaProductosPedidosS


END -- IF @TipoDeArchivo = 'S'
END -- ELSE



COMMIT TRANSACTION 
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION 
EXEC LogRegistro.spLogError @mensaje = N'';
END CATCH
GO
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
	  ,cli_PorcentajeDescuentoDeEspecialidadesMedicinalesDirecto
  FROM tbl_Clientes  
  WHERE cli_codigo = @cli_codigo 

GO
---
ALTER procedure [Productos].[spRecuperarTodosProductosBuscadorEnOferta]
@Sucursal nvarchar(2),
@codCliente int,
@cli_codprov nvarchar(75)
 AS

  Declare @cli_tipo nvarchar(1)
 SELECT @cli_tipo = [cli_tipo]--'P'
  FROM [tbl_Clientes]
  where [cli_codigo] = @codCliente 
  
----
IF OBJECT_ID ( 'tempdb..#tbl_Transfers_Temporal' ) IS NOT NULL 
BEGIN
   DROP TABLE #tbl_Transfers_Temporal
END
SELECT *
INTO #tbl_Transfers_Temporal
FROM [guest].[fn_Transfers] (@codCliente)
----

  
IF OBJECT_ID ( 'tempdb..#tempTablaProductos' ) IS NOT NULL 
begin
   DROP TABLE #tempTablaProductos
end  

Select *, 0 as pro_Ranking
into #tempTablaProductos
from Productos.vistaProductosBusqueda 
where ISNULL(pro_ofeporcentaje,-1) <> -1


select *, tde_codpro as isTransfer,
CASE ISNULL(val_codprov,'0')
         WHEN '0' THEN 0
         ELSE 1
         END as 'RequiereVale'
from #tempTablaProductos
LEFT JOIN (select distinct tde_codpro from tbl_Transfers_Detalle INNER JOIN  #tbl_Transfers_Temporal ON tde_codtfr = tfr_codigo WHERE tfr_facturaciondirecta  is null OR tfr_facturaciondirecta  = 0) as tb1 
on tb1.tde_codpro = pro_nombre and not (ISNULL( pro_NoTransfersEnClientesPerf,0) = 1 and @cli_tipo = 'P')
LEFT JOIN (SELECT val_codprov,val_codpro FROM tbl_Productos_Vales WHERE val_codprov =  @cli_codprov) as tblProductosVales ON pro_codigo = val_codpro
ORDER BY pro_nombre

select stk_codpro,stk_codsuc,stk_stock
from tbl_Productos_Stocks
where stk_codpro in (select pro_codigo from #tempTablaProductos)
AND stk_codsuc in (select sde_sucursalDependiente from Clientes.tbl_sucursalDependiente where sde_sucursal = @Sucursal)

SELECT tde.*,tfr.*,tcl_IdTransfer
from tbl_Transfers_Detalle as tde
INNER JOIN  (SELECT * FROM #tbl_Transfers_Temporal WHERE (tfr_facturaciondirecta = 1 AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov))) as tfr on  [tde_codtfr] = tfr.tfr_codigo
LEFT JOIN tbl_TransfersClientes as tcl on [tde_codtfr] = tcl_IdTransfer and tcl_NumeroCliente = @codCliente
WHERE tde_codpro in (select pro_nombre from #tempTablaProductos)

 DROP TABLE #tempTablaProductos
GO
--------------
ALTER procedure [Productos].[spRecuperarTodosProductosBuscadorEnTransfer]
@Sucursal nvarchar(2),
@codCliente int,
@cli_codprov nvarchar(75)
 AS

  Declare @cli_tipo nvarchar(1)
 SELECT @cli_tipo = [cli_tipo]--'P'
  FROM [tbl_Clientes]
  where [cli_codigo] = @codCliente 
 
----
IF OBJECT_ID ( 'tempdb..#tbl_Transfers_Temporal' ) IS NOT NULL 
BEGIN
   DROP TABLE #tbl_Transfers_Temporal
END
SELECT *
INTO #tbl_Transfers_Temporal
FROM [guest].[fn_Transfers] (@codCliente)
----

IF OBJECT_ID ( 'tempdb..#tempTablaProductos' ) IS NOT NULL 
begin
   DROP TABLE #tempTablaProductos
end  

SELECT *, 0 as pro_Ranking
INTO #tempTablaProductos
FROM Productos.vistaProductosBusqueda 

select *, tde_codpro as isTransfer,
CASE ISNULL(val_codprov,'0')
         WHEN '0' THEN 0
         ELSE 1
         END as 'RequiereVale'
FROM #tempTablaProductos
LEFT JOIN (select distinct tde_codpro from tbl_Transfers_Detalle INNER JOIN  #tbl_Transfers_Temporal ON tde_codtfr = tfr_codigo WHERE (tfr_facturaciondirecta  is null OR tfr_facturaciondirecta  = 0) AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov)) as tb1 
on tb1.tde_codpro = pro_nombre and not (ISNULL( pro_NoTransfersEnClientesPerf,0) = 1 and @cli_tipo = 'P') 
LEFT JOIN (SELECT val_codprov,val_codpro FROM tbl_Productos_Vales WHERE val_codprov =  @cli_codprov) as tblProductosVales ON pro_codigo = val_codpro
WHERE pro_nombre in (select distinct tde_codpro  from tbl_Transfers_Detalle  INNER JOIN  #tbl_Transfers_Temporal ON tde_codtfr = tfr_codigo WHERE (tfr_provincia is null OR  tfr_provincia = @cli_codprov))
ORDER BY pro_nombre

select stk_codpro,stk_codsuc,stk_stock
from tbl_Productos_Stocks
where stk_codpro in (select pro_codigo from #tempTablaProductos)
AND stk_codsuc in (select sde_sucursalDependiente from Clientes.tbl_sucursalDependiente where sde_sucursal = @Sucursal)

SELECT tde.*,tfr.*,tcl_IdTransfer
from tbl_Transfers_Detalle as tde
INNER JOIN  (SELECT * FROM #tbl_Transfers_Temporal WHERE (tfr_facturaciondirecta = 1 AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov))) as tfr on  [tde_codtfr] = tfr.tfr_codigo
LEFT JOIN tbl_TransfersClientes on [tde_codtfr] = tcl_IdTransfer and tcl_NumeroCliente = @codCliente
WHERE tde_codpro in (select pro_nombre from #tempTablaProductos)

 DROP TABLE #tempTablaProductos

GO
ALTER procedure [Productos].[spRecuperarTodosProductosBuscadorOferta]
@ofe_idOferta int,
@Sucursal nvarchar(2),
@codCliente int,
@cli_codprov nvarchar(75)
 AS

  IF OBJECT_ID ( 'tempdb..#tempTablaProductos' ) IS NOT NULL 
BEGIN
   DROP TABLE #tempTablaProductos
END
Select  *, tde_codpro as isTransfer,
CASE ISNULL(val_codprov,'0')
         WHEN '0' THEN 0
         ELSE 1
         END as 'RequiereVale'
into #tempTablaProductos
from Productos.vistaProductosBusqueda
LEFT JOIN (select distinct tde_codpro from tbl_Transfers_Detalle INNER JOIN  tbl_Transfers ON tde_codtfr = tfr_codigo WHERE (tfr_facturaciondirecta  is null OR tfr_facturaciondirecta  = 0) AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov)) as tb1 on tb1.tde_codpro = pro_nombre
LEFT JOIN (SELECT val_codprov,val_codpro FROM tbl_Productos_Vales WHERE val_codprov =  @cli_codprov) as tblProductosVales ON pro_codigo = val_codpro
inner join [dbo].[tbl_OfertaDetalle] on [ofd_productoCodigo] = pro_codigo
inner join [dbo].[tbl_Oferta] on [ofe_idOferta] = [ofd_idOferta]
where [ofe_idOferta]  = @ofe_idOferta
--ORDER BY pro_nombre

SELECT distinct * 
FROM #tempTablaProductos
ORDER BY pro_nombre


SELECT stk_codpro,stk_codsuc,stk_stock
FROM tbl_Productos_Stocks
WHERE stk_codpro in (select pro_codigo from #tempTablaProductos)
AND stk_codsuc in (select sde_sucursalDependiente from Clientes.tbl_sucursalDependiente where sde_sucursal = @Sucursal)


/* Para tener compatibilidad con código anterior */
SELECT * 
FROM #tempTablaProductos
where 0 = 1


SELECT tde.*,tfr.*
from tbl_Transfers_Detalle as tde
INNER JOIN  (SELECT * FROM tbl_Transfers WHERE (tfr_facturaciondirecta = 1 AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov))) as tfr on  [tde_codtfr] = tfr.tfr_codigo
WHERE tde_codpro in (select pro_nombre from #tempTablaProductos)

GO
ALTER  procedure [Productos].[spRecuperarTodosProductosBuscadorV3]
@Where nvarchar (4000),
@WherePrimeraOrdenacion nvarchar (4000),
@Sucursal nvarchar(2),
@codCliente int,
@cli_codprov nvarchar(75)
 AS

 Declare @cli_tipo nvarchar(1)
 SELECT @cli_tipo = [cli_tipo]--'P'
  FROM [tbl_Clientes]
  where [cli_codigo] = @codCliente 

DECLARE @SQLStringPrimeraOrdenacion nvarchar (4000)
DECLARE @SQLString nvarchar (4000)

SET @SQLStringPrimeraOrdenacion = N'Select  pro_codigo,pro_nombre,pro_precio,pro_preciofarmacia,pro_ofeunidades,pro_ofeporcentaje ,pro_neto,pro_codtpopro,pro_descuentoweb,pro_laboratorio,pro_monodroga,pro_codtpovta,pro_codigobarra,pro_codigoalfabeta, 0 as pro_Ranking,pro_isTrazable,pro_isCadenaFrio,pro_entransfer,pro_canmaxima,pro_vtasolotransfer,pro_troquel,pro_acuerdo,pro_NoTransfersEnClientesPerf,pro_familia,pro_PackDeVenta,pro_PrecioBase, pro_PorcARestarDelDtoDeCliente
from Productos.vistaProductosBusqueda
where ' + @WherePrimeraOrdenacion + ' AND ' +  @Where + ' '

SET @SQLString = N'Select  pro_codigo,pro_nombre,pro_precio,pro_preciofarmacia,pro_ofeunidades,pro_ofeporcentaje ,pro_neto,pro_codtpopro,pro_descuentoweb,pro_laboratorio,pro_monodroga,pro_codtpovta,pro_codigobarra,pro_codigoalfabeta, 0 as pro_Ranking,pro_isTrazable,pro_isCadenaFrio,pro_entransfer,pro_canmaxima,pro_vtasolotransfer,pro_troquel,pro_acuerdo,pro_NoTransfersEnClientesPerf,pro_familia,pro_PackDeVenta,pro_PrecioBase, pro_PorcARestarDelDtoDeCliente
from Productos.vistaProductosBusqueda 
where ' + @Where 

 IF OBJECT_ID ( 'tempdb..#tempTablaProductos' ) IS NOT NULL 
BEGIN
   DROP TABLE #tempTablaProductos
END
create table #tempTablaProductos (pro_codigo  nvarchar(50) ,pro_nombre nvarchar(75) ,pro_precio decimal(9,2),pro_preciofarmacia decimal(9,2),pro_ofeunidades int,pro_ofeporcentaje decimal(6,2),pro_neto bit,pro_codtpopro nvarchar(1),pro_descuentoweb decimal(6,2),pro_laboratorio nvarchar(75),pro_monodroga nvarchar(75),pro_codtpovta  nvarchar(2),pro_codigobarra nvarchar(13),pro_codigoalfabeta nvarchar(10), pro_Ranking int, pro_isTrazable bit,pro_isCadenaFrio bit,pro_entransfer bit null,pro_canmaxima int null, pro_vtasolotransfer bit null,pro_troquel nvarchar(50),pro_acuerdo int,pro_NoTransfersEnClientesPerf bit null,pro_familia nvarchar(75) null,pro_PackDeVenta int null,pro_PrecioBase decimal(9, 2) NULL, pro_PorcARestarDelDtoDeCliente decimal(6, 2) NULL)

insert into #tempTablaProductos  EXEC sp_executesql @SQLString

 IF OBJECT_ID ( 'tempdb..#tempTablaProductosPrimeraOrdenacion' ) IS NOT NULL 
BEGIN
   DROP TABLE #tempTablaProductosPrimeraOrdenacion
END
create table #tempTablaProductosPrimeraOrdenacion (pro_codigo  nvarchar(50) ,pro_nombre nvarchar(75) ,pro_precio decimal(9,2),pro_preciofarmacia decimal(9,2),pro_ofeunidades int,pro_ofeporcentaje decimal(6,2),pro_neto bit,pro_codtpopro nvarchar(1),pro_descuentoweb decimal(6,2),pro_laboratorio nvarchar(75),pro_monodroga nvarchar(75),pro_codtpovta  nvarchar(2),pro_codigobarra nvarchar(13),pro_codigoalfabeta nvarchar(10), pro_Ranking int, pro_isTrazable bit,pro_isCadenaFrio bit,pro_entransfer bit null,pro_canmaxima int null, pro_vtasolotransfer bit null,pro_troquel nvarchar(50),pro_acuerdo int,pro_NoTransfersEnClientesPerf bit null,pro_familia nvarchar(75) null,pro_PackDeVenta int null,pro_PrecioBase decimal(9, 2) NULL, pro_PorcARestarDelDtoDeCliente decimal(6, 2) NULL)

insert into #tempTablaProductosPrimeraOrdenacion  EXEC sp_executesql @SQLStringPrimeraOrdenacion

----
IF OBJECT_ID ( 'tempdb..#tbl_Transfers_Temporal' ) IS NOT NULL 
BEGIN
   DROP TABLE #tbl_Transfers_Temporal
END
SELECT *
INTO #tbl_Transfers_Temporal
FROM [guest].[fn_Transfers] (@codCliente)
----

SELECT p.*, tde_codpro as isTransfer,
CASE ISNULL(val_codprov,'0')
         WHEN '0' THEN 0
         ELSE 1
         END as 'RequiereVale'
FROM #tempTablaProductos as p
LEFT JOIN (select distinct tde_codpro from tbl_Transfers_Detalle INNER JOIN  #tbl_Transfers_Temporal ON tde_codtfr = tfr_codigo WHERE (tfr_facturaciondirecta  is null OR tfr_facturaciondirecta  = 0) AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov)) as tb1 
on tb1.tde_codpro = pro_nombre and not (ISNULL( pro_NoTransfersEnClientesPerf,0) = 1 and @cli_tipo = 'P') --(ISNULL( pro_NoTransfersEnClientesPerf,0) = 0 and @cli_tipo = 'P')
LEFT JOIN (SELECT val_codprov,val_codpro FROM tbl_Productos_Vales WHERE val_codprov =  @cli_codprov) as tblProductosVales ON pro_codigo = val_codpro
WHERE pro_codigo NOT IN (SELECT pro_codigo FROM #tempTablaProductosPrimeraOrdenacion)
ORDER BY pro_nombre

SELECT stk_codpro,stk_codsuc,stk_stock
FROM tbl_Productos_Stocks
WHERE stk_codpro in (select pro_codigo from #tempTablaProductos)
AND stk_codsuc in (select sde_sucursalDependiente from Clientes.tbl_sucursalDependiente where sde_sucursal = @Sucursal)

SELECT p.*, tde_codpro as isTransfer,
CASE ISNULL(val_codprov,'0')
         WHEN '0' THEN 0
         ELSE 1
         END as 'RequiereVale'
from #tempTablaProductosPrimeraOrdenacion  as p
LEFT JOIN (select distinct tde_codpro from tbl_Transfers_Detalle INNER JOIN  #tbl_Transfers_Temporal ON tde_codtfr = tfr_codigo WHERE (tfr_facturaciondirecta  is null OR tfr_facturaciondirecta  = 0) AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov)) as tb1 
on tb1.tde_codpro = pro_nombre and not (ISNULL( pro_NoTransfersEnClientesPerf,0) = 1 and @cli_tipo = 'P') --(ISNULL( pro_NoTransfersEnClientesPerf,0) = 0 and @cli_tipo = 'P')
LEFT JOIN (SELECT val_codprov,val_codpro FROM tbl_Productos_Vales WHERE val_codprov =  @cli_codprov) as tblProductosVales ON pro_codigo = val_codpro
ORDER BY pro_nombre

SELECT tde.*,tfr.*,tcl_IdTransfer
from tbl_Transfers_Detalle as tde
INNER JOIN  (SELECT * FROM #tbl_Transfers_Temporal WHERE (tfr_facturaciondirecta = 1 AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov))) as tfr on  [tde_codtfr] = tfr.tfr_codigo
LEFT JOIN tbl_TransfersClientes on [tde_codtfr] = tcl_IdTransfer and tcl_NumeroCliente = @codCliente
WHERE tde_codpro in (select pro_nombre from #tempTablaProductos)

--select distinct tcl_IdTransfer
--FROM  tbl_TransfersClientes where tcl_NumeroCliente = @codCliente
GO
ALTER procedure [Transfers].[spRecuperarTodosTransferMasDetallePorIdProducto]
 @tde_codpro nvarchar (75) ,
 @sucursal nvarchar(2),
 @codCliente int
 AS

--BEGIN TRANSACTION
--BEGIN TRY
IF OBJECT_ID ( 'tempdb..#tbl_Transfers_Temporal' ) IS NOT NULL 
begin
   DROP TABLE #tbl_Transfers_Temporal
end
IF OBJECT_ID ( 'tempdb..#Tabla_IdTransfer' ) IS NOT NULL 
begin
   DROP TABLE #Tabla_IdTransfer
end
IF OBJECT_ID ( 'tempdb..#Tabla_IdProductosTransfer' ) IS NOT NULL 
begin
   DROP TABLE #Tabla_IdProductosTransfer
end
---
SELECT *
INTO #tbl_Transfers_Temporal
FROM [guest].[fn_Transfers] (@codCliente)
---

SELECT DISTINCT tde_codtfr
 INTO #Tabla_IdTransfer
  FROM tbl_Transfers_Detalle
  inner join #tbl_Transfers_Temporal on tde_codtfr = tfr_codigo
  where tde_codpro = @tde_codpro
  
  SELECT *
  FROM #tbl_Transfers_Temporal
  where tfr_codigo in (select  tde_codtfr from #Tabla_IdTransfer)

  SELECT tde.*
      ,pro.*
  INTO #Tabla_IdProductosTransfer      
  FROM dbo.tbl_Transfers_Detalle as tde
  INNER JOIN tbl_Productos as pro ON pro_nombre = tde_codpro
  where tde_codtfr in (select  tde_codtfr from #Tabla_IdTransfer)
  
  SELECT *
  FROM  #Tabla_IdProductosTransfer 
  order by tde_codpro
  
select stk_codpro,stk_codsuc,stk_stock
from tbl_Productos_Stocks
where stk_codpro in (select DISTINCT pro_codigo from #Tabla_IdProductosTransfer)
AND stk_codsuc in (select sde_sucursalDependiente from Clientes.tbl_sucursalDependiente where sde_sucursal = @Sucursal)
  
  DROP TABLE #Tabla_IdTransfer 
  DROP TABLE #Tabla_IdProductosTransfer
  DROP TABLE #tbl_Transfers_Temporal
--COMMIT TRANSACTION 
--END TRY
--BEGIN CATCH
--ROLLBACK TRANSACTION 
--EXEC LogRegistro.spLogError @mensaje = N'';
--END CATCH
GO
ALTER VIEW [Productos].[vistaProductosBusqueda]
WITH SCHEMABINDING
AS
SELECT pro_codigo,pro_nombre,pro_precio,pro_preciofarmacia,pro_ofeunidades,pro_ofeporcentaje ,pro_neto,pro_codtpopro,pro_descuentoweb,pro_laboratorio,pro_monodroga,pro_codtpovta,pro_codigobarra,pro_codigoalfabeta,
pro_codigo + ' ' + ISNULL(pro_nombre,'')  + ' ' + ISNULL(pro_laboratorio,'') + ' ' + ISNULL(pro_monodroga,'') + ' ' + ISNULL(pro_codigobarra,'') + ' ' + ISNULL(pro_codigoalfabeta,'') as pro_columnaWhere
,ISNULL(pro_nombre,'')  + ' ' +  ISNULL(pro_codigobarra,'')  + ' ' + ISNULL(pro_laboratorio,'') as pop_columnaWhereDefault,pro_isTrazable,pro_isCadenaFrio,pro_entransfer,pro_canmaxima,pro_vtasolotransfer,pro_troquel,pro_acuerdo
,pro_NoTransfersEnClientesPerf,pro_familia,pro_PackDeVenta,pro_PrecioBase,pro_PorcARestarDelDtoDeCliente
FROM dbo.tbl_Productos


GO

