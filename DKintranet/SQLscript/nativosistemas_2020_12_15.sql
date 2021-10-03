IF OBJECT_ID ( 'CAR.spBorrarCarrito', 'P' ) IS NOT NULL 
    DROP PROCEDURE CAR.spBorrarCarrito;
GO
CREATE procedure [CAR].[spBorrarCarrito]
@codCliente int,
@sucursal nvarchar(2),
@tipo [nvarchar](100),
@accion [nvarchar](100),
@idCarrito int OUTPUT
 AS
 ----
 DECLARE @isCarritoEnProceso int

SELECT TOP 1 @idCarrito = car_id 
FROM [CAR].[Carritos] with (NOLOCK) 
WHERE car_codCliente = @codCliente AND car_codSucursal = @sucursal AND car_tipo = @tipo AND car_activo = 1


IF ISNULL(@idCarrito,-1) > 0
BEGIN
exec CAR.spIsCarritoEnProceso @idCarrito, @isCarritoEnProceso OUTPUT; 
SET @isCarritoEnProceso = ISNULL(@isCarritoEnProceso,0) 
---
IF @isCarritoEnProceso = 1
BEGIN
   set  @idCarrito =  -1
   RETURN  
END


declare @isOkAUDIT_Carrito bit
 EXEC [CAR].[spAUDIT_Carrito]
@car_id = @idCarrito,
@accion = @accion,
@isOk = @isOkAUDIT_Carrito output;


BEGIN TRANSACTION
BEGIN TRY
------

UPDATE [CAR].[Carritos]
SET car_activo = 0
WHERE car_id = @idCarrito

--set  @idCarrito =  -1
COMMIT TRANSACTION 
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION 
set  @idCarrito =  -1
EXEC LogRegistro.spLogError @mensaje = N'';
END CATCH

--END --IF  @idCarrito  IS NULL

END

GO

-------------------------------
---
IF OBJECT_ID ( 'CAR.spBorrarCarritoPorId', 'P' ) IS NOT NULL 
    DROP PROCEDURE CAR.spBorrarCarritoPorId;
GO
CREATE procedure [CAR].[spBorrarCarritoPorId]
@car_id int,
@accion [nvarchar](100),
@isOk bit OUTPUT
 AS
 ----

declare @isOkAUDIT_Carrito bit
 EXEC [CAR].[spAUDIT_Carrito]
@car_id = @car_id,
@accion = @accion,
@isOk = @isOkAUDIT_Carrito output;


BEGIN TRANSACTION
BEGIN TRY
------
UPDATE [CAR].[Carritos]
SET car_activo = 0
WHERE car_id = @car_id

SET @isOk =  1
COMMIT TRANSACTION 
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION 
SET @isOk =  0
EXEC LogRegistro.spLogError @mensaje = N'' 
END CATCH
GO
--------------------------------------
IF OBJECT_ID ( 'CAR.spCargarCarritoProductoSucursal', 'P' ) IS NOT NULL 
    DROP PROCEDURE CAR.spCargarCarritoProductoSucursal;
GO
CREATE procedure [CAR].[spCargarCarritoProductoSucursal]
@codSucursal nvarchar(2),
@codCliente int,
@codUsuario int,
@codProducto nvarchar(75),
@cantidad int,
@car_tipo [nvarchar](100),
@isOk bit OUTPUT
 AS

BEGIN TRANSACTION [Tran1]
BEGIN TRY
DECLARE @TempIdCarrito int
DECLARE @TempIdCarritoDetalle int   
    
DECLARE @FechaActual datetime;
SET @FechaActual = GETDATE();

SELECT  TOP 1 @TempIdCarrito = [car_id]
  FROM CAR.Carritos with (NOLOCK)
WHERE car_codSucursal = @codSucursal AND [car_codCliente] = @codCliente AND car_tipo = @car_tipo AND car_activo = 1


IF ISNULL(@TempIdCarrito,-1) = -1
BEGIN
INSERT INTO CAR.Carritos
(car_codSucursal,car_codCliente,car_tipo,car_Fecha)
VALUES(@codSucursal,@codCliente,@car_tipo,@FechaActual)

set @TempIdCarrito = SCOPE_IDENTITY() 

END 

SELECT  TOP 1 @TempIdCarritoDetalle = cad_id
  FROM  CAR.CarritosDetalles with (NOLOCK)
WHERE cad_codCarrito = @TempIdCarrito AND cad_codProducto = @codProducto

IF ISNULL(@TempIdCarritoDetalle,-1) = -1
BEGIN

INSERT INTO CAR.CarritosDetalles
(cad_codCarrito,cad_codProducto,cad_cantidad,cad_codUsuario,cad_Fecha)
values(@TempIdCarrito,@codProducto,@cantidad,@codUsuario,@FechaActual)

set @TempIdCarritoDetalle = SCOPE_IDENTITY() 

END
ELSE
BEGIN

update  CAR.CarritosDetalles
set cad_cantidad = @cantidad,
cad_codUsuario = @codUsuario,
cad_Fecha = @FechaActual
where cad_codCarrito = @TempIdCarrito AND cad_codProducto = @codProducto

END


COMMIT TRANSACTION [Tran1]
SET @isOk = 1
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION [Tran1]
EXEC LogRegistro.spLogError @mensaje = N'';
SET @isOk = 0
END CATCH

GO
-----------------------------------
IF OBJECT_ID ( 'CAR.spCargarCarritoTransferPorDetalles', 'P' ) IS NOT NULL 
    DROP PROCEDURE CAR.spCargarCarritoTransferPorDetalles;
GO
CREATE PROCEDURE [CAR].[spCargarCarritoTransferPorDetalles]
@idUsuario int,
@idCliente int,
@Tabla_Detalle TransferDetalleTableType READONLY,
@idTransfer int,
@ctr_codSucursal nvarchar(2),
@tipo [nvarchar](100),
@isOk bit OUTPUT
 AS


DECLARE @TempIdCarritoTrasnfers_meta int

SELECT TOP 1 @TempIdCarritoTrasnfers_meta =  car_id
  FROM [CAR].[Carritos] with (NOLOCK)
  WHERE car_codCliente = @idCliente AND car_tipo = @tipo AND car_codSucursal = @ctr_codSucursal AND car_activo = 1

BEGIN TRANSACTION [Tran1]
BEGIN TRY

IF ISNULL(@TempIdCarritoTrasnfers_meta,-1) = -1
BEGIN

INSERT INTO [CAR].[Carritos]
(car_codCliente,car_Fecha,car_codSucursal,car_tipo)
VALUES(@idCliente,GETDATE(), @ctr_codSucursal,@tipo)

SET @TempIdCarritoTrasnfers_meta = SCOPE_IDENTITY() 

END 

DECLARE @TempIdCarritoTrasnfers int
      
SELECT TOP 1 @TempIdCarritoTrasnfers =  cat_id
FROM [CAR].CarritosTransfer with (NOLOCK)
WHERE cat_codCarrito = @TempIdCarritoTrasnfers_meta AND cat_codTransfer  = @idTransfer 

IF ISNULL(@TempIdCarritoTrasnfers,-1) = -1
BEGIN

INSERT INTO [CAR].CarritosTransfer
(cat_codCarrito,cat_codTransfer)
VALUES(@TempIdCarritoTrasnfers_meta,@idTransfer)

SET @TempIdCarritoTrasnfers = SCOPE_IDENTITY() 

END 

UPDATE [CAR].[CarritosDetalles]
SET [cad_cantidad] = cantidad,
	[cad_Fecha] = GETDATE(),
	[cad_codUsuario] = @idUsuario
FROM [CAR].[CarritosDetalles]
INNER JOIN @Tabla_Detalle on [cad_nameProducto] = codProducto  
WHERE cad_codCarritosTransfer = @TempIdCarritoTrasnfers 

INSERT INTO [CAR].[CarritosDetalles]
(cad_codCarrito,cad_codCarritosTransfer,cad_nameProducto,cad_cantidad,cad_codUsuario ,cad_Fecha)
SELECT @TempIdCarritoTrasnfers_meta
	   ,@TempIdCarritoTrasnfers
	   ,codProducto
	   ,cantidad
	   ,@idUsuario
	   ,GETDATE()  	 
FROM  @Tabla_Detalle
WHERE codProducto NOT IN  (SELECT codProducto 
FROM @Tabla_Detalle 
INNER JOIN [CAR].[CarritosDetalles] on cad_nameProducto = codProducto 
WHERE cad_codCarritosTransfer = @TempIdCarritoTrasnfers  )

COMMIT TRANSACTION [Tran1]
SET @isOk = 1
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION [Tran1]
EXEC LogRegistro.spLogError @mensaje = N'';
SET @isOk = 0
END CATCH

GO
------------------------
IF OBJECT_ID ( 'CAR.spRecuperarCarritosPorSucursalYProductos', 'P' ) IS NOT NULL 
    DROP PROCEDURE CAR.spRecuperarCarritosPorSucursalYProductos;
GO
CREATE procedure [CAR].[spRecuperarCarritosPorSucursalYProductos]
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
WHERE car_codCliente = @car_codCliente and  car_tipo = @car_tipo AND car_activo = 1

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
GO
-------------------------
IF OBJECT_ID ( 'CAR.spRecuperarCarritoTransferPorIdCliente', 'P' ) IS NOT NULL 
    DROP PROCEDURE CAR.spRecuperarCarritoTransferPorIdCliente;
GO
CREATE procedure [CAR].[spRecuperarCarritoTransferPorIdCliente]
@lrc_codCliente int,
@tipo [nvarchar](100)
 AS

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
  WHERE car_codCliente = @lrc_codCliente AND car_tipo = @tipo AND car_activo = 1


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


SELECT *
FROM #tempTablaProductosCarritoTransfer

SELECT ctr_id ,tfr_codigo,tfr_nombre,tfr_deshab,tfr_pordesadi,tfr_tipo,ctr_codSucursal,car_id
  FROM #tempTblCarritoTransfer
  INNER JOIN tbl_Transfers on tfr_codigo = ctr_codTransfer

SELECT tde.*,tfr.*
from tbl_Transfers_Detalle as tde
INNER JOIN  (SELECT * FROM tbl_Transfers WHERE tfr_facturaciondirecta = 1) as tfr on  [tde_codtfr] = tfr.tfr_codigo
WHERE tde_codpro in (SELECT ctd_codProducto   FROM #tempTablaProductosCarritoTransfer)

DROP TABLE #tempTablaProductosCarritoTransfer 
   
GO
------------------------------
IF OBJECT_ID ( 'CAR.spSubirPedido', 'P' ) IS NOT NULL 
    DROP PROCEDURE CAR.spSubirPedido;
GO
CREATE PROCEDURE [CAR].[spSubirPedido]
@idUsuario int,
@idCliente int,
@strXML as xml,
@tipoCarrito nvarchar(100),
@tipoCarritoTransfer nvarchar(100),
@isOk bit OUTPUT
 AS
 BEGIN TRANSACTION [Tran1]
BEGIN TRY


DECLARE @FechaActual datetime;
SET @FechaActual = GETDATE();
IF OBJECT_ID ( 'tempdb..#TempDetalleProducto' ) IS NOT NULL 
begin
   DROP TABLE #TempDetalleProducto
end

SELECT codigo  = T.Item.value('@codigo',  'nvarchar(50)'),
	   nombre = T.Item.value('@nombre',  'nvarchar(75)'),
       cantidad  = T.Item.value('@lcp_cantidad',  'int'),
	   codTransfer  = T.Item.value('@codTransfer',  'int'),
	   isTransferFacturacionDirecta  = T.Item.value('@isTransferFacturacionDirecta',  'bit'),
	   codSucursal = T.Item.value('@codSucursal',  'nvarchar(2)')
into #TempDetalleProducto
FROM   @strXML.nodes('/Root/DetallePedido') AS T(Item)

DECLARE @codigo nvarchar(50), @nombre nvarchar(75),@cantidad int ,@codTransfer int ,@isTransferFacturacionDirecta bit,@codSucursal nvarchar(2);  

---
DECLARE @TempIdCarritoTrasnfers_meta int
DECLARE @TempIdCarritoTrasnfers int
DECLARE @TempIdCarritoTrasnfersDetalle int
DECLARE @TempIdCarrito int
DECLARE @TempIdCarritoDetalle int 
---


DECLARE MY_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR 
SELECT codigo, nombre, cantidad,codTransfer,isTransferFacturacionDirecta,codSucursal
FROM #TempDetalleProducto

OPEN MY_CURSOR
FETCH NEXT FROM MY_CURSOR INTO @codigo, @nombre, @cantidad,@codTransfer,@isTransferFacturacionDirecta,@codSucursal
WHILE @@FETCH_STATUS = 0
BEGIN 
    --Do something with Id here
	IF @isTransferFacturacionDirecta = 1
	BEGIN
	--------------------------------------------------------
	--DECLARE @TempIdCarritoTrasnfers_meta int
	SET @TempIdCarritoTrasnfers_meta = -1

SELECT TOP 1 @TempIdCarritoTrasnfers_meta =  car_id
  FROM [CAR].[Carritos] with (NOLOCK)
  WHERE car_codCliente = @idCliente AND car_tipo = @tipoCarritoTransfer AND car_codSucursal = @codSucursal AND car_activo = 1

IF ISNULL(@TempIdCarritoTrasnfers_meta,-1) = -1
BEGIN

INSERT INTO [CAR].[Carritos]
(car_codCliente,car_Fecha,car_codSucursal,car_tipo)
VALUES(@idCliente,@FechaActual, @codSucursal,@tipoCarritoTransfer)

SET @TempIdCarritoTrasnfers_meta = SCOPE_IDENTITY() 

END 



--DECLARE @TempIdCarritoTrasnfers int
SET @TempIdCarritoTrasnfers = -1
SELECT  TOP 1 @TempIdCarritoTrasnfers =  cat_id
FROM [CAR].CarritosTransfer with (NOLOCK)
WHERE cat_codCarrito = @TempIdCarritoTrasnfers_meta AND cat_codTransfer  = @codTransfer 

IF ISNULL(@TempIdCarritoTrasnfers,-1) = -1
BEGIN

INSERT INTO [CAR].CarritosTransfer
(cat_codCarrito,cat_codTransfer)
VALUES(@TempIdCarritoTrasnfers_meta,@codTransfer)

SET @TempIdCarritoTrasnfers = SCOPE_IDENTITY() 

END 

SET @TempIdCarritoTrasnfersDetalle = -1

SELECT  TOP 1 @TempIdCarritoTrasnfersDetalle = cad_id
FROM  CAR.CarritosDetalles with (NOLOCK)
WHERE  [cad_nameProducto] = @nombre and cad_codCarritosTransfer = @TempIdCarritoTrasnfers 

IF ISNULL(@TempIdCarritoTrasnfersDetalle,-1) = -1
BEGIN

INSERT INTO CAR.CarritosDetalles
(cad_codCarrito,cad_codCarritosTransfer,cad_nameProducto,cad_cantidad,cad_codUsuario ,cad_Fecha)
values(@TempIdCarritoTrasnfers_meta,@TempIdCarritoTrasnfers,@nombre,@cantidad,@idUsuario,@FechaActual)

END 
ELSE
BEGIN

UPDATE [CAR].[CarritosDetalles]
SET [cad_cantidad] = @cantidad,
	[cad_Fecha] = @FechaActual,
	[cad_codUsuario] = @idUsuario
FROM [CAR].[CarritosDetalles]
WHERE  [cad_nameProducto] = @nombre and cad_codCarritosTransfer = @TempIdCarritoTrasnfers 

END

	--------------------------------------------------------
	END
	ELSE
	BEGIN
	--------------------------------------------------------
	--DECLARE @TempIdCarrito int
	SET @TempIdCarrito =-1
SELECT TOP 1 @TempIdCarrito = [car_id]
  FROM CAR.Carritos with (NOLOCK)
WHERE car_codSucursal = @codSucursal AND [car_codCliente] = @idCliente AND car_tipo = @tipoCarrito AND car_activo = 1


IF ISNULL(@TempIdCarrito,-1) = -1
BEGIN
INSERT INTO CAR.Carritos
(car_codSucursal,car_codCliente,car_tipo,car_Fecha)
VALUES(@codSucursal,@idCliente,@tipoCarrito,@FechaActual)

set @TempIdCarrito = SCOPE_IDENTITY() 

END 

SET  @TempIdCarritoDetalle = -1

SELECT  TOP 1 @TempIdCarritoDetalle = cad_id
FROM  CAR.CarritosDetalles with (NOLOCK)
where cad_codCarrito = @TempIdCarrito AND cad_codProducto = @codigo

IF ISNULL(@TempIdCarritoDetalle,-1) = -1
BEGIN

INSERT INTO CAR.CarritosDetalles
(cad_codCarrito,cad_codProducto,cad_cantidad,cad_codUsuario,cad_Fecha)
values(@TempIdCarrito,@codigo,@cantidad,@idUsuario,@FechaActual)

END
ELSE
BEGIN

update  CAR.CarritosDetalles
set cad_cantidad = @cantidad,
cad_codUsuario = @idUsuario,
cad_Fecha = @FechaActual
where cad_codCarrito = @TempIdCarrito AND cad_codProducto = @codigo

END

------------------------------------
	END

    --PRINT @PractitionerId
    FETCH NEXT FROM MY_CURSOR INTO @codigo, @nombre, @cantidad,@codTransfer,@isTransferFacturacionDirecta,@codSucursal
END
CLOSE MY_CURSOR
DEALLOCATE MY_CURSOR

COMMIT TRANSACTION [Tran1]
SET @isOk = 1
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION [Tran1]
EXEC LogRegistro.spLogError @mensaje = N'';
SET @isOk = 0
END CATCH

GO
------------------------