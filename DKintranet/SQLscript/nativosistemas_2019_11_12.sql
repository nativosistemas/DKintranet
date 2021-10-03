

CREATE procedure [LogRegistro].[spCargarCarritoProductoSucursalDesdeArchivoPedidosV5]
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
FROM  tbl_TransfersClientes where tcl_NumeroCliente = @lrc_codCliente) or [tfr_codigo] not in (select distinct tcl_IdTransfer
FROM  tbl_TransfersClientes )
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


SELECT [tde_codtfr],[tde_codpro],[tde_descripcion],[tde_prepublico],[tde_predescuento],[tde_minuni],[tde_maxuni],[tde_muluni],[tde_fijuni],[tde_proobligatorio],tde_unidadesbonificadas,tde_unidadesbonificadasdescripcion,[tfr_codigo],[tfr_accion],[tfr_nombre],[tfr_deshab],[tfr_pordesadi],[tfr_tipo],[tfr_mospap],[tfr_minrenglones],[tfr_minunidades],[tfr_maxunidades],[tfr_mulunidades],[tfr_fijunidades],[tfr_descripcion],[tfr_facturaciondirecta]
,tcl_IdTransfer,tde_DescripcionDeProducto
from tbl_Transfers_Detalle
INNER JOIN  (SELECT * FROM #tbl_Transfers_Temporal WHERE tfr_facturaciondirecta = 1) as tb1 on  [tde_codtfr] = tb1.tfr_codigo
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

SELECT [tde_codtfr],[tde_codpro],[tde_descripcion],[tde_prepublico],[tde_predescuento],[tde_minuni],[tde_maxuni],[tde_muluni],[tde_fijuni],[tde_proobligatorio],tde_unidadesbonificadas,tde_unidadesbonificadasdescripcion,[tfr_codigo],[tfr_accion],[tfr_nombre],[tfr_deshab],[tfr_pordesadi],[tfr_tipo],[tfr_mospap],[tfr_minrenglones],[tfr_minunidades],[tfr_maxunidades],[tfr_mulunidades],[tfr_fijunidades],[tfr_descripcion],[tfr_facturaciondirecta]
,tcl_IdTransfer
from tbl_Transfers_Detalle
INNER JOIN  (SELECT * FROM #tbl_Transfers_Temporal WHERE tfr_facturaciondirecta = 1) as tb1 on  [tde_codtfr] = tb1.tfr_codigo
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

--------------
--ALTER TABLE [dbo].[tbl_Productos] 
--ADD [pro_PackDeVenta] [smallint] NULL
--GO
----
--ALTER TABLE [dbo].[tmp_Productos]
--ADD [PackDeVenta] [smallint] NULL
--GO
----
--ALTER TABLE [dbo].[tmp_Productos_Todos] 
--ADD [PackDeVenta] [smallint] NULL
--GO
----

--ALTER PROCEDURE [Productos].[spSincronizarProductos_Todos]
--AS 
--BEGIN TRANSACTION
--BEGIN TRY

--DECLARE @CANT INT

--SELECT  @CANT = COUNT(*) FROM [dbo].[tmp_Productos_Todos]

--IF @CANT <> 0
--BEGIN

--DELETE FROM [dbo].[tbl_Productos]

--INSERT [dbo].[tbl_Productos]
--	 ([pro_codigo]
--      ,[pro_nombre]
--      ,[pro_laboratorio]
--      ,[pro_accionterapeutica]
--      ,[pro_monodroga]
--      ,[pro_codtpovta]
--      ,[pro_precio]
--      ,[pro_preciofarmacia]
--      ,[pro_neto]
--      ,[pro_isCadenaFrio]
--      ,[pro_ofeporcentaje]
--      ,[pro_ofeunidades]
--      ,[pro_codtpopro]
--      ,[pro_troquel]
--      ,[pro_codigobarra]
--      ,[pro_codigoalfabeta]
--      ,[pro_entransfer]
--      ,[pro_canmaxima]
--      ,[pro_vtasolotransfer]
--      ,[pro_descuentoweb]
--      ,[pro_isTrazable]
--      ,pro_acuerdo
--	  ,pro_NoTransfersEnClientesPerf
--	  ,pro_Familia
--	  ,pro_PackDeVenta)
--SELECT [Numero]
--      ,[Nombre]
--      ,[Laboratorio]
--      ,[AccionTerapeutica]
--      ,[MonoDroga]
--      ,[TipoDeVenta]
--      ,[Precio]
--      ,[PrecioFarmacia]
--      ,[Neto]
--      ,[CadenaFrio]
--      ,[OfertaMensualPorcentajeDescuento]
--      ,[OfertaMensualUnidadesMinimas]
--      ,[TipoProducto]
--      ,[Troquel]
--      ,[CodigoBarra]
--      ,[CodigoAlfaBeta]
--      ,[EnTransfer]
--      ,[CantidadMaximaVentaPorPedido]
--      ,[SeVendeSoloPorTransfer]
--      ,[PorcentajeDescuentoWeb]
--      ,[Trazable]
--      ,Acuerdo
--	  ,NoTransfersEnClientesPerf
--	  ,Familia
--	  ,PackDeVenta
--  FROM [dbo].[tmp_Productos_Todos]

--  DELETE FROM [dbo].[tmp_Productos_Todos]
  
--END -- IF @CANT <> 0

--EXEC LogRegistro.spHistorialProcesos @descripcion = N'', @nombreProcedimiento=N'Productos.spSincronizarProductos_Todos';

--COMMIT TRANSACTION 
--END TRY
--BEGIN CATCH
--ROLLBACK TRANSACTION 
--EXEC LogRegistro.spLogError @mensaje = N'';
--END CATCH
--GO
----
--ALTER PROCEDURE [Productos].[spSincronizarProductos]
--AS 
--BEGIN TRANSACTION
--BEGIN TRY


----DELETE FROM [dbo].[tbl_Productos]

--INSERT [dbo].[tbl_Productos]
--	 ([pro_codigo]
--      ,[pro_nombre]
--      ,[pro_laboratorio]
--      ,[pro_accionterapeutica]
--      ,[pro_monodroga]
--      ,[pro_codtpovta]
--      ,[pro_precio]
--      ,[pro_preciofarmacia]
--      ,[pro_neto]
--      ,[pro_isCadenaFrio]
--      ,[pro_ofeporcentaje]
--      ,[pro_ofeunidades]
--      ,[pro_codtpopro]
--      ,[pro_troquel]
--      ,[pro_codigobarra]
--      ,[pro_codigoalfabeta]
--      ,[pro_entransfer]
--      ,[pro_canmaxima]
--      ,[pro_vtasolotransfer]
--      ,[pro_descuentoweb]
--      ,[pro_isTrazable]
--      ,pro_acuerdo
--	  ,pro_NoTransfersEnClientesPerf
--	  ,pro_Familia
--	  ,pro_PackDeVenta)
--SELECT [Numero]
--      ,[Nombre]
--      ,[Laboratorio]
--      ,[AccionTerapeutica]
--      ,[MonoDroga]
--      ,[TipoDeVenta]
--      ,[Precio]
--      ,[PrecioFarmacia]
--      ,[Neto]
--      ,[CadenaFrio]
--      ,[OfertaMensualPorcentajeDescuento]
--      ,[OfertaMensualUnidadesMinimas]
--      ,[TipoProducto]
--      ,[Troquel]
--      ,[CodigoBarra]
--      ,[CodigoAlfaBeta]
--      ,[EnTransfer]
--      ,[CantidadMaximaVentaPorPedido]
--      ,[SeVendeSoloPorTransfer]
--      ,[PorcentajeDescuentoWeb]
--      ,[Trazable]
--      ,Acuerdo
--	  ,NoTransfersEnClientesPerf
--	  ,Familia
--	  ,PackDeVenta
--  FROM [dbo].[tmp_Productos]
--  WHERE Accion = 'I'


--UPDATE tbl_Productos
--SET [pro_codigo] =  [Numero]
--      ,[pro_nombre] =[Nombre]
--      ,[pro_laboratorio] = [Laboratorio]
--      ,[pro_accionterapeutica] = [AccionTerapeutica]
--      ,[pro_monodroga] = [MonoDroga]
--      ,[pro_codtpovta] = [TipoDeVenta]
--      ,[pro_precio] = [Precio]
--      ,[pro_preciofarmacia] = [PrecioFarmacia]
--      ,[pro_neto] = [Neto]
--      ,[pro_isCadenaFrio] = [CadenaFrio]
--      ,[pro_ofeporcentaje] = [OfertaMensualPorcentajeDescuento]
--      ,[pro_ofeunidades] = [OfertaMensualUnidadesMinimas]
--      ,[pro_codtpopro] = [TipoProducto]
--      ,[pro_troquel] = [Troquel]
--      ,[pro_codigobarra] = [CodigoBarra]
--      ,[pro_codigoalfabeta] = [CodigoAlfaBeta]
--      ,[pro_entransfer] = [EnTransfer]
--      ,[pro_canmaxima] = [CantidadMaximaVentaPorPedido]
--      ,[pro_vtasolotransfer] = [SeVendeSoloPorTransfer]
--      ,[pro_descuentoweb] = [PorcentajeDescuentoWeb]
--      ,[pro_isTrazable] = [Trazable]
--      ,pro_acuerdo = Acuerdo
--	  ,pro_NoTransfersEnClientesPerf = NoTransfersEnClientesPerf
--	  ,pro_Familia = Familia
--	  ,pro_PackDeVenta = PackDeVenta
--FROM tbl_Productos 
--INNER JOIN tmp_Productos ON Numero = pro_codigo
--WHERE  Accion = 'U'

--  DELETE FROM [dbo].[tmp_Productos]
--  WHERE Accion = 'I' OR Accion = 'U'
  
--EXEC LogRegistro.spHistorialProcesos @descripcion = N'', @nombreProcedimiento=N'Productos.spSincronizarProductos';

--COMMIT TRANSACTION 
--END TRY
--BEGIN CATCH
--ROLLBACK TRANSACTION 
--EXEC LogRegistro.spLogError @mensaje = N'';
--END CATCH
--GO
----

ALTER VIEW [Productos].[vistaProductosBusqueda]
WITH SCHEMABINDING
AS
SELECT pro_codigo,pro_nombre,pro_precio,pro_preciofarmacia,pro_ofeunidades,pro_ofeporcentaje ,pro_neto,pro_codtpopro,pro_descuentoweb,pro_laboratorio,pro_monodroga,pro_codtpovta,pro_codigobarra,pro_codigoalfabeta,
pro_codigo + ' ' + ISNULL(pro_nombre,'')  + ' ' + ISNULL(pro_laboratorio,'') + ' ' + ISNULL(pro_monodroga,'') + ' ' + ISNULL(pro_codigobarra,'') + ' ' + ISNULL(pro_codigoalfabeta,'') as pro_columnaWhere
,ISNULL(pro_nombre,'')  + ' ' +  ISNULL(pro_codigobarra,'')  + ' ' + ISNULL(pro_laboratorio,'') as pop_columnaWhereDefault,pro_isTrazable,pro_isCadenaFrio,pro_entransfer,pro_canmaxima,pro_vtasolotransfer,pro_troquel,pro_acuerdo
,pro_NoTransfersEnClientesPerf,pro_familia,pro_PackDeVenta
FROM dbo.tbl_Productos

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
FROM  tbl_TransfersClientes where tcl_NumeroCliente = @codCliente) or [tfr_codigo] not in (select distinct tcl_IdTransfer
FROM  tbl_TransfersClientes )
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

SELECT [tde_codtfr],[tde_codpro],[tde_descripcion],[tde_prepublico],[tde_predescuento],[tde_minuni],[tde_maxuni],[tde_muluni],[tde_fijuni],[tde_proobligatorio],tde_unidadesbonificadas,tde_unidadesbonificadasdescripcion,[tfr_codigo],[tfr_accion],[tfr_nombre],[tfr_deshab],[tfr_pordesadi],[tfr_tipo],[tfr_mospap],[tfr_minrenglones],[tfr_minunidades],[tfr_maxunidades],[tfr_mulunidades],[tfr_fijunidades],[tfr_descripcion],[tfr_facturaciondirecta]
,tcl_IdTransfer,tde_DescripcionDeProducto
from tbl_Transfers_Detalle
INNER JOIN  (SELECT * FROM #tbl_Transfers_Temporal WHERE (tfr_facturaciondirecta = 1 AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov))) as tb1 on  [tde_codtfr] = tb1.tfr_codigo
LEFT JOIN tbl_TransfersClientes on [tde_codtfr] = tcl_IdTransfer and tcl_NumeroCliente = @codCliente
WHERE tde_codpro in (select pro_nombre from #tempTablaProductos)

 DROP TABLE #tempTablaProductos
GO

---------------

alter  procedure [Productos].[spRecuperarTodosProductosBuscadorV3]
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

SET @SQLStringPrimeraOrdenacion = N'Select  pro_codigo,pro_nombre,pro_precio,pro_preciofarmacia,pro_ofeunidades,pro_ofeporcentaje ,pro_neto,pro_codtpopro,pro_descuentoweb,pro_laboratorio,pro_monodroga,pro_codtpovta,pro_codigobarra,pro_codigoalfabeta, 0 as pro_Ranking,pro_isTrazable,pro_isCadenaFrio,pro_entransfer,pro_canmaxima,pro_vtasolotransfer,pro_troquel,pro_acuerdo,pro_NoTransfersEnClientesPerf,pro_familia,pro_PackDeVenta
from Productos.vistaProductosBusqueda
where ' + @WherePrimeraOrdenacion + ' AND ' +  @Where + ' '

SET @SQLString = N'Select  pro_codigo,pro_nombre,pro_precio,pro_preciofarmacia,pro_ofeunidades,pro_ofeporcentaje ,pro_neto,pro_codtpopro,pro_descuentoweb,pro_laboratorio,pro_monodroga,pro_codtpovta,pro_codigobarra,pro_codigoalfabeta, 0 as pro_Ranking,pro_isTrazable,pro_isCadenaFrio,pro_entransfer,pro_canmaxima,pro_vtasolotransfer,pro_troquel,pro_acuerdo,pro_NoTransfersEnClientesPerf,pro_familia,pro_PackDeVenta
from Productos.vistaProductosBusqueda 
where ' + @Where 

 IF OBJECT_ID ( 'tempdb..#tempTablaProductos' ) IS NOT NULL 
BEGIN
   DROP TABLE #tempTablaProductos
END
create table #tempTablaProductos (pro_codigo  nvarchar(50) ,pro_nombre nvarchar(75) ,pro_precio decimal(9,2),pro_preciofarmacia decimal(9,2),pro_ofeunidades int,pro_ofeporcentaje decimal(6,2),pro_neto bit,pro_codtpopro nvarchar(1),pro_descuentoweb decimal(6,2),pro_laboratorio nvarchar(75),pro_monodroga nvarchar(75),pro_codtpovta  nvarchar(2),pro_codigobarra nvarchar(13),pro_codigoalfabeta nvarchar(10), pro_Ranking int, pro_isTrazable bit,pro_isCadenaFrio bit,pro_entransfer bit null,pro_canmaxima int null, pro_vtasolotransfer bit null,pro_troquel nvarchar(50),pro_acuerdo int,pro_NoTransfersEnClientesPerf bit null,pro_familia nvarchar(75) null,pro_PackDeVenta int null)

insert into #tempTablaProductos  EXEC sp_executesql @SQLString

 IF OBJECT_ID ( 'tempdb..#tempTablaProductosPrimeraOrdenacion' ) IS NOT NULL 
BEGIN
   DROP TABLE #tempTablaProductosPrimeraOrdenacion
END
create table #tempTablaProductosPrimeraOrdenacion (pro_codigo  nvarchar(50) ,pro_nombre nvarchar(75) ,pro_precio decimal(9,2),pro_preciofarmacia decimal(9,2),pro_ofeunidades int,pro_ofeporcentaje decimal(6,2),pro_neto bit,pro_codtpopro nvarchar(1),pro_descuentoweb decimal(6,2),pro_laboratorio nvarchar(75),pro_monodroga nvarchar(75),pro_codtpovta  nvarchar(2),pro_codigobarra nvarchar(13),pro_codigoalfabeta nvarchar(10), pro_Ranking int, pro_isTrazable bit,pro_isCadenaFrio bit,pro_entransfer bit null,pro_canmaxima int null, pro_vtasolotransfer bit null,pro_troquel nvarchar(50),pro_acuerdo int,pro_NoTransfersEnClientesPerf bit null,pro_familia nvarchar(75) null,pro_PackDeVenta int null)

insert into #tempTablaProductosPrimeraOrdenacion  EXEC sp_executesql @SQLStringPrimeraOrdenacion

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
FROM  tbl_TransfersClientes where tcl_NumeroCliente = @codCliente) or [tfr_codigo] not in (select distinct tcl_IdTransfer
FROM  tbl_TransfersClientes )
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

SELECT [tde_codtfr],[tde_codpro],[tde_descripcion],[tde_prepublico],[tde_predescuento],[tde_minuni],[tde_maxuni],[tde_muluni],[tde_fijuni],[tde_proobligatorio],tde_unidadesbonificadas,tde_unidadesbonificadasdescripcion,[tfr_codigo],[tfr_accion],[tfr_nombre],[tfr_deshab],[tfr_pordesadi],[tfr_tipo],[tfr_mospap],[tfr_minrenglones],[tfr_minunidades],[tfr_maxunidades],[tfr_mulunidades],[tfr_fijunidades],[tfr_descripcion],[tfr_facturaciondirecta]
,tcl_IdTransfer,tde_DescripcionDeProducto
from tbl_Transfers_Detalle
INNER JOIN  (SELECT * FROM #tbl_Transfers_Temporal WHERE (tfr_facturaciondirecta = 1 AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov))) as tb1 on  [tde_codtfr] = tb1.tfr_codigo
LEFT JOIN tbl_TransfersClientes on [tde_codtfr] = tcl_IdTransfer and tcl_NumeroCliente = @codCliente
WHERE tde_codpro in (select pro_nombre from #tempTablaProductos)

--select distinct tcl_IdTransfer
--FROM  tbl_TransfersClientes where tcl_NumeroCliente = @codCliente
go
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
FROM  tbl_TransfersClientes where tcl_NumeroCliente = @codCliente) or [tfr_codigo] not in (select distinct tcl_IdTransfer
FROM  tbl_TransfersClientes )
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

SELECT [tde_codtfr],[tde_codpro],[tde_descripcion],[tde_prepublico],[tde_predescuento],[tde_minuni],[tde_maxuni],[tde_muluni],[tde_fijuni],[tde_proobligatorio],tde_unidadesbonificadas,tde_unidadesbonificadasdescripcion,[tfr_codigo],[tfr_accion],[tfr_nombre],[tfr_deshab],[tfr_pordesadi],[tfr_tipo],[tfr_mospap],[tfr_minrenglones],[tfr_minunidades],[tfr_maxunidades],[tfr_mulunidades],[tfr_fijunidades],[tfr_descripcion],[tfr_facturaciondirecta]
,tcl_IdTransfer,tde_DescripcionDeProducto
from tbl_Transfers_Detalle
INNER JOIN  (SELECT * FROM #tbl_Transfers_Temporal WHERE (tfr_facturaciondirecta = 1 AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov))) as tb1 on  [tde_codtfr] = tb1.tfr_codigo
LEFT JOIN tbl_TransfersClientes on [tde_codtfr] = tcl_IdTransfer and tcl_NumeroCliente = @codCliente
WHERE tde_codpro in (select pro_nombre from #tempTablaProductos)

 DROP TABLE #tempTablaProductos
go
-------

alter procedure [Productos].[spRecuperarTodosProductosBuscadorOferta]
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


SELECT [tde_codtfr],[tde_codpro],[tde_descripcion],[tde_prepublico],[tde_predescuento],[tde_minuni],[tde_maxuni],[tde_muluni],[tde_fijuni],[tde_proobligatorio],tde_unidadesbonificadas,tde_unidadesbonificadasdescripcion,[tfr_codigo],[tfr_accion],[tfr_nombre],[tfr_deshab],[tfr_pordesadi],[tfr_tipo],[tfr_mospap],[tfr_minrenglones],[tfr_minunidades],[tfr_maxunidades],[tfr_mulunidades],[tfr_fijunidades],[tfr_descripcion],[tfr_facturaciondirecta]
from tbl_Transfers_Detalle
INNER JOIN  (SELECT * FROM tbl_Transfers WHERE (tfr_facturaciondirecta = 1 AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov))) as tb1 on  [tde_codtfr] = tb1.tfr_codigo
WHERE tde_codpro in (select pro_nombre from #tempTablaProductos)

go