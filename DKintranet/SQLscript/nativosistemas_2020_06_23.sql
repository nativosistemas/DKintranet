alter table  [dbo].[tbl_Sucursales]
 add suc_pedirCC_ok bit NOT NULL default 1,
  suc_pedirCC_sucursalReferencia nvarchar(2)  NULL,
  suc_pedirCC_tomaSoloPerfumeria bit NOT NULL default 0
  GO
  ---
  
create procedure [LogRegistro].[spRecuperarFaltasProblemasCrediticiosTodosEstadosV2]
@fpc_codCliente int,
@fpc_tipo int,
@cantidadDia int,
@Sucursal nvarchar(2)
 AS

 Declare @cli_codprov nvarchar(75)
 Declare @cli_tipo nvarchar(1)
 SELECT @cli_tipo = [cli_tipo],@cli_codprov = cli_codprov
  FROM [tbl_Clientes]
  where [cli_codigo] = @fpc_codCliente 


 IF OBJECT_ID ( 'tempdb..#tempTablaProductos' ) IS NOT NULL 
BEGIN
   DROP TABLE #tempTablaProductos
END


--- Recupera los registro 
Select pro_codigo,pro_nombre,pro_precio,pro_preciofarmacia,pro_ofeunidades,pro_ofeporcentaje ,pro_neto,pro_codtpopro,pro_descuentoweb,pro_laboratorio,pro_monodroga,pro_codtpovta,pro_codigobarra,pro_codigoalfabeta, 0 as pro_Ranking,pro_isTrazable,pro_isCadenaFrio,pro_entransfer,pro_canmaxima,pro_vtasolotransfer,pro_troquel,pro_acuerdo,pro_NoTransfersEnClientesPerf
      ,fpc_codSucursal
	  ,suc_nombre 	
      ,fpc_codCliente
      ,fpc_nombreProducto
      ,SUM(fpc_cantidad) as 'fpc_cantidad'
      ,fpc_tipo
      ,stk_stock,pro_AceptaVencidos,pro_ProductoRequiereLote
into #tempTablaProductos
  FROM LogRegistro.tbl_FaltasProblemasCrediticios
  INNER JOIN Productos.vistaProductosBusqueda on pro_nombre = fpc_nombreProducto
  LEFT JOIN tbl_Sucursales on suc_codigo = fpc_codSucursal
  INNER JOIN tbl_Productos_Stocks on (stk_codpro = pro_codigo AND stk_codsuc = fpc_codSucursal)
  WHERE  fpc_codCliente = @fpc_codCliente AND fpc_tipo = @fpc_tipo  AND  fpc_fecha > DATEADD(dd, -@cantidadDia, GETDATE()) AND pro_vtasolotransfer = 0
  GROUP BY fpc_codSucursal,fpc_codCliente,fpc_nombreProducto,fpc_tipo,stk_stock,pro_ofeunidades,pro_ofeporcentaje,suc_nombre
       --
  	  ,pro_codigo
	  ,pro_nombre 
	  ,pro_precio 
	  ,pro_preciofarmacia 
	  ,pro_neto 
	  ,pro_codtpopro 
	  ,pro_descuentoweb 
	  ,pro_laboratorio
	  ,pro_monodroga
	  ,pro_codtpovta
	  ,pro_codigobarra
	  ,pro_codigoalfabeta
	  ,pro_isTrazable
	  ,pro_isCadenaFrio
	  ,pro_entransfer
	  ,pro_canmaxima
	  ,pro_vtasolotransfer
      ,pro_troquel
      ,pro_acuerdo
	  ,pro_NoTransfersEnClientesPerf,pro_AceptaVencidos,pro_ProductoRequiereLote

----
IF OBJECT_ID ( 'tempdb..#tbl_Transfers_Temporal' ) IS NOT NULL 
BEGIN
   DROP TABLE #tbl_Transfers_Temporal
END
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
INTO #tbl_Transfers_Temporal
FROM [tbl_Transfers]
where [tfr_codigo] in (select distinct tcl_IdTransfer
FROM  tbl_TransfersClientes where tcl_NumeroCliente = @fpc_codCliente) or [tfr_codigo] not in (select distinct tcl_IdTransfer
FROM  tbl_TransfersClientes )
----


SELECT pro_codigo,pro_nombre ,pro_precio ,pro_preciofarmacia ,pro_ofeunidades ,pro_ofeporcentaje ,pro_neto ,pro_codtpopro ,pro_descuentoweb ,pro_laboratorio,pro_monodroga,pro_codtpovta,pro_codigobarra,pro_codigoalfabeta,pro_Ranking, tde_codpro as isTransfer,pro_isTrazable,pro_isCadenaFrio,pro_entransfer,pro_canmaxima,pro_vtasolotransfer,
CASE ISNULL(val_codprov,'0')
         WHEN '0' THEN 0
         ELSE 1
         END as 'RequiereVale'
         ,pro_troquel
         ,pro_acuerdo
		 ,pro_NoTransfersEnClientesPerf
		 --
		 ,fpc_codSucursal
	     ,suc_nombre 	
         ,fpc_codCliente
         ,fpc_nombreProducto
         ,fpc_cantidad
         ,fpc_tipo
         ,stk_stock,pro_AceptaVencidos,pro_ProductoRequiereLote
FROM #tempTablaProductos
LEFT JOIN (select distinct tde_codpro from tbl_Transfers_Detalle INNER JOIN  #tbl_Transfers_Temporal ON tde_codtfr = tfr_codigo WHERE (tfr_facturaciondirecta  is null OR tfr_facturaciondirecta  = 0) AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov)) as tb1 
on tb1.tde_codpro = pro_nombre and not (ISNULL( pro_NoTransfersEnClientesPerf,0) = 1 and @cli_tipo = 'P') --(ISNULL( pro_NoTransfersEnClientesPerf,0) = 0 and @cli_tipo = 'P')
LEFT JOIN (SELECT val_codprov,val_codpro FROM tbl_Productos_Vales WHERE val_codprov =  @cli_codprov) as tblProductosVales ON pro_codigo = val_codpro


SELECT [tde_codtfr],[tde_codpro],[tde_descripcion],[tde_prepublico],[tde_predescuento],[tde_minuni],[tde_maxuni],[tde_muluni],[tde_fijuni],[tde_proobligatorio],tde_unidadesbonificadas,tde_unidadesbonificadasdescripcion,[tfr_codigo],[tfr_accion],[tfr_nombre],[tfr_deshab],[tfr_pordesadi],[tfr_tipo],[tfr_mospap],[tfr_minrenglones],[tfr_minunidades],[tfr_maxunidades],[tfr_mulunidades],[tfr_fijunidades],[tfr_descripcion],[tfr_facturaciondirecta]
,tcl_IdTransfer,tde_DescripcionDeProducto
from tbl_Transfers_Detalle
INNER JOIN  (SELECT * FROM #tbl_Transfers_Temporal WHERE (tfr_facturaciondirecta = 1 AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov))) as tb1 on  [tde_codtfr] = tb1.tfr_codigo
LEFT JOIN tbl_TransfersClientes on [tde_codtfr] = tcl_IdTransfer and tcl_NumeroCliente = @fpc_codCliente
WHERE tde_codpro in (select pro_nombre from #tempTablaProductos)


--
SELECT stk_codpro,stk_codsuc,stk_stock
FROM tbl_Productos_Stocks
WHERE stk_codpro in (select pro_codigo from #tempTablaProductos)
AND stk_codsuc in (select sde_sucursalDependiente from Clientes.tbl_sucursalDependiente where sde_sucursal = @Sucursal)
GO
---
create procedure [LogRegistro].[spRecuperarFaltasProblemasCrediticiosV2]
@fpc_codCliente int,
@fpc_tipo int,
@cantidadDia int,
@Sucursal nvarchar(2)
 AS

Declare @cli_codprov nvarchar(75)
 Declare @cli_tipo nvarchar(1)
 SELECT @cli_tipo = [cli_tipo],@cli_codprov = cli_codprov
  FROM [tbl_Clientes]
  where [cli_codigo] = @fpc_codCliente 


 IF OBJECT_ID ( 'tempdb..#tempTablaProductos' ) IS NOT NULL 
BEGIN
   DROP TABLE #tempTablaProductos
END

Select pro_codigo,pro_nombre,pro_precio,pro_preciofarmacia,pro_ofeunidades,pro_ofeporcentaje ,pro_neto,pro_codtpopro,pro_descuentoweb,pro_laboratorio,pro_monodroga,pro_codtpovta,pro_codigobarra,pro_codigoalfabeta, 0 as pro_Ranking,pro_isTrazable,pro_isCadenaFrio,pro_entransfer,pro_canmaxima,pro_vtasolotransfer,pro_troquel,pro_acuerdo,pro_NoTransfersEnClientesPerf
      ,fpc_codSucursal
	  ,suc_nombre 	
      ,fpc_codCliente
      ,fpc_nombreProducto
      ,SUM(fpc_cantidad) as 'fpc_cantidad'
      ,fpc_tipo
      ,stk_stock,pro_AceptaVencidos,pro_ProductoRequiereLote
into #tempTablaProductos
from LogRegistro.tbl_FaltasProblemasCrediticios
  INNER JOIN  Productos.vistaProductosBusqueda on pro_nombre = fpc_nombreProducto
  LEFT JOIN tbl_Sucursales on suc_codigo = fpc_codSucursal
  INNER JOIN tbl_Productos_Stocks on (stk_codpro = pro_codigo AND stk_codsuc = fpc_codSucursal)
    WHERE  fpc_codCliente = @fpc_codCliente AND fpc_tipo = @fpc_tipo AND stk_stock <> 'N' AND  fpc_fecha > DATEADD(dd, -@cantidadDia, GETDATE()) AND pro_vtasolotransfer = 0
  GROUP BY fpc_codSucursal,fpc_codCliente,fpc_nombreProducto,fpc_tipo,stk_stock,pro_ofeunidades,pro_ofeporcentaje,suc_nombre
       --
  	  ,pro_codigo
	  ,pro_nombre 
	  ,pro_precio 
	  ,pro_preciofarmacia 
	  ,pro_neto 
	  ,pro_codtpopro 
	  ,pro_descuentoweb 
	  ,pro_laboratorio
	  ,pro_monodroga
	  ,pro_codtpovta
	  ,pro_codigobarra
	  ,pro_codigoalfabeta
	  ,pro_isTrazable
	  ,pro_isCadenaFrio
	  ,pro_entransfer
	  ,pro_canmaxima
	  ,pro_vtasolotransfer
      ,pro_troquel
      ,pro_acuerdo
	  ,pro_NoTransfersEnClientesPerf,pro_AceptaVencidos,pro_ProductoRequiereLote


----
IF OBJECT_ID ( 'tempdb..#tbl_Transfers_Temporal' ) IS NOT NULL 
BEGIN
   DROP TABLE #tbl_Transfers_Temporal
END
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
INTO #tbl_Transfers_Temporal
FROM [tbl_Transfers]
where [tfr_codigo] in (select distinct tcl_IdTransfer
FROM  tbl_TransfersClientes where tcl_NumeroCliente = @fpc_codCliente) or [tfr_codigo] not in (select distinct tcl_IdTransfer
FROM  tbl_TransfersClientes )
----


SELECT pro_codigo,pro_nombre ,pro_precio ,pro_preciofarmacia ,pro_ofeunidades ,pro_ofeporcentaje ,pro_neto ,pro_codtpopro ,pro_descuentoweb ,pro_laboratorio,pro_monodroga,pro_codtpovta,pro_codigobarra,pro_codigoalfabeta,pro_Ranking, tde_codpro as isTransfer,pro_isTrazable,pro_isCadenaFrio,pro_entransfer,pro_canmaxima,pro_vtasolotransfer,
CASE ISNULL(val_codprov,'0')
         WHEN '0' THEN 0
         ELSE 1
         END as 'RequiereVale'
         ,pro_troquel
         ,pro_acuerdo
		 ,pro_NoTransfersEnClientesPerf
		 --
		 ,fpc_codSucursal
	     ,suc_nombre 	
         ,fpc_codCliente
         ,fpc_nombreProducto
         ,fpc_cantidad
         ,fpc_tipo
         ,stk_stock,pro_AceptaVencidos,pro_ProductoRequiereLote
FROM #tempTablaProductos
LEFT JOIN (select distinct tde_codpro from tbl_Transfers_Detalle INNER JOIN  #tbl_Transfers_Temporal ON tde_codtfr = tfr_codigo WHERE (tfr_facturaciondirecta  is null OR tfr_facturaciondirecta  = 0) AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov)) as tb1 
on tb1.tde_codpro = pro_nombre and not (ISNULL( pro_NoTransfersEnClientesPerf,0) = 1 and @cli_tipo = 'P') --(ISNULL( pro_NoTransfersEnClientesPerf,0) = 0 and @cli_tipo = 'P')
LEFT JOIN (SELECT val_codprov,val_codpro FROM tbl_Productos_Vales WHERE val_codprov =  @cli_codprov) as tblProductosVales ON pro_codigo = val_codpro


SELECT [tde_codtfr],[tde_codpro],[tde_descripcion],[tde_prepublico],[tde_predescuento],[tde_minuni],[tde_maxuni],[tde_muluni],[tde_fijuni],[tde_proobligatorio],tde_unidadesbonificadas,tde_unidadesbonificadasdescripcion,[tfr_codigo],[tfr_accion],[tfr_nombre],[tfr_deshab],[tfr_pordesadi],[tfr_tipo],[tfr_mospap],[tfr_minrenglones],[tfr_minunidades],[tfr_maxunidades],[tfr_mulunidades],[tfr_fijunidades],[tfr_descripcion],[tfr_facturaciondirecta]
,tcl_IdTransfer,tde_DescripcionDeProducto
from tbl_Transfers_Detalle
INNER JOIN  (SELECT * FROM #tbl_Transfers_Temporal WHERE (tfr_facturaciondirecta = 1 AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov))) as tb1 on  [tde_codtfr] = tb1.tfr_codigo
LEFT JOIN tbl_TransfersClientes on [tde_codtfr] = tcl_IdTransfer and tcl_NumeroCliente = @fpc_codCliente
WHERE tde_codpro in (select pro_nombre from #tempTablaProductos)


--
SELECT stk_codpro,stk_codsuc,stk_stock
FROM tbl_Productos_Stocks
WHERE stk_codpro in (select pro_codigo from #tempTablaProductos)
AND stk_codsuc in (select sde_sucursalDependiente from Clientes.tbl_sucursalDependiente where sde_sucursal = @Sucursal)
GO
---