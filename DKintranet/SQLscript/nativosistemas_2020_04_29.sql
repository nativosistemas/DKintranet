--------
alter procedure [CAR].[spRecuperarCarritosPorSucursalYProductos]
@car_codCliente int,
@car_tipo [nvarchar](100)
AS
BEGIN TRY
----
IF OBJECT_ID ( 'tempdb..#tbl_Transfers_Temporal' ) IS NOT NULL 
BEGIN
   DROP TABLE #tbl_Transfers_Temporal
END
SELECT tfr.*
INTO #tbl_Transfers_Temporal
FROM [tbl_Transfers] as tfr
where [tfr_codigo] in (select distinct tcl_IdTransfer
FROM  tbl_TransfersClientes where tcl_NumeroCliente = @car_codCliente) or [tfr_codigo] not in (select distinct tcl_IdTransfer
FROM  tbl_TransfersClientes )
----


 IF OBJECT_ID ( 'tempdb..#TempTablaCarrito' ) IS NOT NULL 
begin
   DROP TABLE #TempTablaCarrito
end

SELECT car_id,car_codSucursal
  INTO #TempTablaCarrito
  FROM [CAR].[Carritos] with (NOLOCK)
WHERE car_codCliente = @car_codCliente and  car_tipo = @car_tipo

SELECT cad.*
      ,pro.*
      ,car.*
      ,stk.*
  FROM [CAR].[CarritosDetalles] as cad with (NOLOCK)
  INNER JOIN tbl_Productos as pro with (NOLOCK) on cad_codProducto = pro_codigo
  INNER JOIN #TempTablaCarrito as car on cad_codCarrito = car_id
  INNER JOIN tbl_Productos_Stocks as stk with (NOLOCK) on (stk_codpro = cad_codProducto AND stk_codsuc = car_codSucursal)
  WHERE  cad_cantidad > 0
 ORDER BY cad_id
 
 
SELECT car_id,car_codSucursal
FROM #TempTablaCarrito

---- NUEVO
SELECT tde.*, tfr.*
FROM tbl_Transfers_Detalle as tde with (NOLOCK)
INNER JOIN  (SELECT * FROM #tbl_Transfers_Temporal with (NOLOCK) WHERE tfr_facturaciondirecta = 1) as tfr on  [tde_codtfr] = tfr.tfr_codigo
WHERE tde_codpro in (SELECT pro_nombre FROM [CAR].[CarritosDetalles] with (NOLOCK) INNER JOIN #TempTablaCarrito on cad_codCarrito = car_id INNER JOIN tbl_Productos on cad_codProducto = pro_codigo)

-----FIN NUEVO
 DROP TABLE #TempTablaCarrito
   
END TRY
BEGIN CATCH
EXEC LogRegistro.spLogError @mensaje = N'';
END CATCH
