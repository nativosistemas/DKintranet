USE [db_Kellerhoff]
GO

/****** Object:  Table [dbo].[tmp_Solicitudes_Devoluciones_Items]    Script Date: 06/08/2020 14:43:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tmp_Solicitudes_Devoluciones_Items](
	[dev_numeroitem] [int] IDENTITY(1,1) NOT NULL,
	[dev_numerocliente] [int] NOT NULL,
	[dev_numerofactura] [nvarchar](13) NULL,
	[dev_nombreproductodevolucion] [nvarchar](75) NOT NULL,
	[dev_fecha] [datetime] NOT NULL,
	[dev_motivo] [int] NOT NULL,
	[dev_numeroitemfactura] [int] NULL,
	[dev_nombreproductofactura] [nvarchar](75) NULL,
	[dev_cantidad] [int] NOT NULL,
	[dev_numerolote] [nvarchar](75) NULL,
	[dev_fechavencimientolote] [date] NULL,
	[dev_idsucursal] [nvarchar](2) NOT NULL,
 CONSTRAINT [PK_tmp_Solicitudes_Devoluciones_Items] PRIMARY KEY CLUSTERED 
(
	[dev_numeroitem] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tmp_Solicitudes_Devoluciones_Items] ADD  CONSTRAINT [DF_tmp_Solicitudes_Devoluciones_Items_dev_idsucursal]  DEFAULT ('CC') FOR [dev_idsucursal]
GO

USE [db_Kellerhoff]
GO
/****** Object:  StoredProcedure [Devoluciones].[spAgregarItemDevolucion]    Script Date: 06/08/2020 14:37:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [Devoluciones].[spAgregarItemDevolucion]
	@numerocliente int,
	@numerofactura nvarchar(13),
	@nombreproductodevolucion nvarchar(75),
	@motivo int,
	@numeroitemfactura int,
	@nombreproductofactura nvarchar(75),
	@cantidad int,
	@numerolote nvarchar(75),
	@fechavencimientolote nvarchar(10),
	@idsucursal nvarchar(2),
	@isOk bit OUTPUT
AS

BEGIN TRANSACTION [Tran1]
BEGIN TRY
	DECLARE @TempNroItem int
	DECLARE @Fecha datetime;
	SET @Fecha = GETDATE();
	DECLARE @FechaVto date;
	SET @FechaVto = CONVERT(datetime,@fechavencimientolote,102);
	
	SELECT TOP 1 @TempNroItem = [dev_numeroitem]
	FROM tmp_Solicitudes_Devoluciones_Items  with (NOLOCK)
	WHERE [dev_numerocliente] = @numerocliente 
	AND [dev_numerofactura] = @numerofactura
	AND [dev_nombreproductodevolucion] = @nombreproductodevolucion
	AND [dev_numerolote] = @numerolote
	AND [dev_motivo] = @motivo
		
		
	IF ISNULL(@TempNroItem,-1) = -1
		BEGIN
			INSERT INTO tmp_Solicitudes_Devoluciones_Items
			(dev_numerocliente,dev_numerofactura,dev_nombreproductodevolucion,dev_fecha,
			dev_motivo,dev_numeroitemfactura,dev_nombreproductofactura,dev_cantidad,
			dev_numerolote,dev_fechavencimientolote,dev_idsucursal)
			VALUES(@numerocliente,@numerofactura,@nombreproductodevolucion,@Fecha,@motivo,@numeroitemfactura,
			@nombreproductofactura,@cantidad,@numerolote,@FechaVto,@idsucursal)

		END 
	ELSE
		BEGIN
			UPDATE tmp_Solicitudes_Devoluciones_Items
			SET dev_motivo = @motivo,
			dev_cantidad = dev_cantidad + @cantidad,
			dev_numeroitemfactura = @numeroitemfactura,
			dev_nombreproductofactura = @nombreproductofactura,
			dev_numerolote = @numerolote,
			dev_fechavencimientolote = @FechaVto,
			dev_fecha = @Fecha,
			dev_idsucursal = @idsucursal
			WHERE [dev_numeroitem] = @TempNroItem
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

USE [db_Kellerhoff]
GO
/****** Object:  StoredProcedure [Devoluciones].[spEliminarPrecargaDevolucionFacturaCompletaPorCliente]    Script Date: 06/08/2020 14:40:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Devoluciones].[spEliminarPrecargaDevolucionFacturaCompletaPorCliente]
	@NumeroCliente int,
	@isOk bit OUTPUT
AS

BEGIN TRANSACTION [Tran1]
BEGIN TRY
	DELETE FROM [tmp_Solicitudes_Devoluciones_Items]
	WHERE [dev_numerocliente] = @NumeroCliente
	AND [dev_nombreproductofactura] = 'F.Completa'
	
	COMMIT TRANSACTION [Tran1]
	SET @isOk = 1
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION [Tran1]
	EXEC LogRegistro.spLogError @mensaje = N'';
	SET @isOk = 0
END CATCH
GO
USE [db_Kellerhoff]
GO
/****** Object:  StoredProcedure [Devoluciones].[spEliminarPrecargaDevolucionPorCliente]    Script Date: 06/08/2020 14:41:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Devoluciones].[spEliminarPrecargaDevolucionPorCliente]
	@NumeroCliente int,
	@isOk bit OUTPUT
AS

BEGIN TRANSACTION [Tran1]
BEGIN TRY
	DELETE FROM [tmp_Solicitudes_Devoluciones_Items]
	WHERE [dev_numerocliente] = @NumeroCliente
	AND NOT [dev_numerofactura] IS NULL
	
	COMMIT TRANSACTION [Tran1]
	SET @isOk = 1
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION [Tran1]
	EXEC LogRegistro.spLogError @mensaje = N'';
	SET @isOk = 0
END CATCH
GO

USE [db_Kellerhoff]
GO
/****** Object:  StoredProcedure [Devoluciones].[spEliminarPrecargaDevolucionVencidosPorCliente]    Script Date: 06/08/2020 14:41:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Devoluciones].[spEliminarPrecargaDevolucionVencidosPorCliente]
	@NumeroCliente int,
	@isOk bit OUTPUT
AS

BEGIN TRANSACTION [Tran1]
BEGIN TRY
	DELETE FROM [tmp_Solicitudes_Devoluciones_Items]
	WHERE [dev_numerocliente] = @NumeroCliente
	AND [dev_numerofactura] IS NULL
	
	COMMIT TRANSACTION [Tran1]
	SET @isOk = 1
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION [Tran1]
	EXEC LogRegistro.spLogError @mensaje = N'';
	SET @isOk = 0
END CATCH
GO

USE [db_Kellerhoff]
GO
/****** Object:  StoredProcedure [Devoluciones].[spElimminarItemDevolucionPrecarga]    Script Date: 06/08/2020 14:41:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Devoluciones].[spElimminarItemDevolucionPrecarga]
	@NumeroItem int,
	@isOK  bit OUTPUT
AS
BEGIN TRANSACTION [Tran1]
BEGIN TRY
	DELETE FROM [tmp_Solicitudes_Devoluciones_Items]
	WHERE [dev_numeroitem] = @NumeroItem
	
	COMMIT TRANSACTION [Tran1]
	SET @isOk = 1
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION [Tran1]
	EXEC LogRegistro.spLogError @mensaje = N'';
	SET @isOk = 0
END CATCH
GO

USE [db_Kellerhoff]
GO
/****** Object:  StoredProcedure [LogRegistro].[spRecuperarFaltasProblemasCrediticiosTodosEstadosV2]    Script Date: 06/08/2020 13:59:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
----------------------------


----------------
ALTER procedure [LogRegistro].[spRecuperarFaltasProblemasCrediticiosTodosEstadosV2]
@fpc_codCliente int,
@fpc_tipo int,
@cantidadDia int
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


GO

USE [db_Kellerhoff]
GO
/****** Object:  StoredProcedure [LogRegistro].[spRecuperarFaltasProblemasCrediticiosV2]    Script Date: 06/08/2020 14:25:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [LogRegistro].[spRecuperarFaltasProblemasCrediticiosV2]
@fpc_codCliente int,
@fpc_tipo int,
@cantidadDia int
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
GO

USE [db_Kellerhoff]
GO
/****** Object:  StoredProcedure [Devoluciones].[spRecuperarItemsDevolucionesPrecargaFacturaCompletaPorCliente]    Script Date: 06/08/2020 14:41:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Devoluciones].[spRecuperarItemsDevolucionesPrecargaFacturaCompletaPorCliente]
	@numerocliente int
AS
BEGIN
	SELECT [dev_numeroitem]
		,[dev_numerocliente]
		,[dev_numerofactura]
		,[dev_nombreproductodevolucion]
		,[dev_fecha]
		,[dev_motivo]
		,[dev_numeroitemfactura]
		,[dev_nombreproductofactura]
		,[dev_cantidad]
		,[dev_numerolote]
		,[dev_fechavencimientolote]
		,[dev_idsucursal]
	FROM [tmp_Solicitudes_Devoluciones_Items]
	WHERE [dev_numerocliente] = @numerocliente
	AND [dev_nombreproductofactura] = 'F.Completa'
	ORDER BY [dev_numeroitemfactura]
END
GO

USE [db_Kellerhoff]
GO
/****** Object:  StoredProcedure [Devoluciones].[spRecuperarItemsDevolucionesPrecargaPorCliente]    Script Date: 06/08/2020 14:42:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Devoluciones].[spRecuperarItemsDevolucionesPrecargaPorCliente]
	@numerocliente int
AS
BEGIN
	SELECT [dev_numeroitem]
		,[dev_numerocliente]
		,[dev_numerofactura]
		,[dev_nombreproductodevolucion]
		,[dev_fecha]
		,[dev_motivo]
		,[dev_numeroitemfactura]
		,[dev_nombreproductofactura]
		,[dev_cantidad]
		,[dev_numerolote]
		,[dev_fechavencimientolote]
		,[dev_idsucursal]
	FROM [tmp_Solicitudes_Devoluciones_Items]
	WHERE [dev_numerocliente] = @numerocliente
	AND ([dev_nombreproductofactura] <> 'F.Completa' AND NOT [dev_nombreproductofactura] IS NULL)
	ORDER BY [dev_fecha]
END
GO

USE [db_Kellerhoff]
GO
/****** Object:  StoredProcedure [Devoluciones].[spRecuperarItemsDevolucionesPrecargaVencidosPorCliente]    Script Date: 06/08/2020 14:42:22 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [Devoluciones].[spRecuperarItemsDevolucionesPrecargaVencidosPorCliente]
	@numerocliente int
AS
BEGIN
	SELECT [dev_numeroitem]
		,[dev_numerocliente]
		,[dev_numerofactura]
		,[dev_nombreproductodevolucion]
		,[dev_fecha]
		,[dev_motivo]
		,[dev_numeroitemfactura]
		,[dev_nombreproductofactura]
		,[dev_cantidad]
		,[dev_numerolote]
		,[dev_fechavencimientolote]
		,[dev_idsucursal]
	FROM [tmp_Solicitudes_Devoluciones_Items]
	WHERE [dev_numerocliente] = @numerocliente
	AND [dev_numerofactura] IS NULL
	ORDER BY [dev_fecha]
END
GO

USE [db_Kellerhoff]
GO
/****** Object:  StoredProcedure [Productos].[spRecuperarProductosDesdeTabla]    Script Date: 06/08/2020 13:51:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER procedure [Productos].[spRecuperarProductosDesdeTabla]
@Tabla_Detalle ProductosArchivosPedidosTableType READONLY,
@Sucursal nvarchar(2),
@cli_codprov nvarchar(75),
@codCliente int
AS


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

SELECT pro_codigo,pro_nombre ,pro_precio ,pro_preciofarmacia ,pro_ofeunidades ,pro_ofeporcentaje ,pro_neto ,pro_codtpopro ,pro_descuentoweb ,pro_laboratorio,pro_monodroga,pro_codtpovta,pro_codigobarra,pro_codigoalfabeta,0 as 'pro_Ranking', tde_codpro as isTransfer,pro_isTrazable,pro_isCadenaFrio,pro_entransfer,pro_canmaxima,pro_vtasolotransfer,pro_AceptaVencidos,pro_ProductoRequiereLote,
CASE ISNULL(val_codprov,'0')
         WHEN '0' THEN 0
         ELSE 1
         END as 'RequiereVale'
         ,pro_troquel
         ,pro_acuerdo
from tbl_Productos
LEFT JOIN (select distinct tde_codpro from tbl_Transfers_Detalle INNER JOIN  #tbl_Transfers_Temporal ON tde_codtfr = tfr_codigo WHERE (tfr_facturaciondirecta  is null OR tfr_facturaciondirecta  = 0) AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov)) as tb1 on tb1.tde_codpro = pro_nombre
LEFT JOIN (SELECT val_codprov,val_codpro FROM tbl_Productos_Vales WHERE val_codprov =  @cli_codprov) as tblProductosVales ON pro_codigo = val_codpro
WHERE pro_nombre in (select codProducto from @Tabla_Detalle)


SELECT stk_codpro,stk_codsuc,stk_stock
FROM tbl_Productos_Stocks
WHERE stk_codpro in (select pro_codigo from tbl_Productos WHERE pro_nombre in (select codProducto from @Tabla_Detalle))
AND stk_codsuc in (select sde_sucursalDependiente from Clientes.tbl_sucursalDependiente where sde_sucursal = @Sucursal)

SELECT [tde_codtfr],[tde_codpro],[tde_descripcion],[tde_prepublico],[tde_predescuento],[tde_minuni],[tde_maxuni],[tde_muluni],[tde_fijuni],[tde_proobligatorio],tde_unidadesbonificadas,tde_unidadesbonificadasdescripcion,[tfr_codigo],[tfr_accion],[tfr_nombre],[tfr_deshab],[tfr_pordesadi],[tfr_tipo],[tfr_mospap],[tfr_minrenglones],[tfr_minunidades],[tfr_maxunidades],[tfr_mulunidades],[tfr_fijunidades],[tfr_descripcion],[tfr_facturaciondirecta]
from tbl_Transfers_Detalle
INNER JOIN  (SELECT * FROM #tbl_Transfers_Temporal WHERE (tfr_facturaciondirecta = 1 AND (tfr_provincia is null OR  tfr_provincia = @cli_codprov))) as tb1 on  [tde_codtfr] = tb1.tfr_codigo
WHERE tde_codpro in (select codProducto from @Tabla_Detalle)
GO

USE [db_Kellerhoff]
GO
/****** Object:  StoredProcedure [Productos].[spRecuperarTodosProductosBuscadorV3]    Script Date: 06/08/2020 13:55:15 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

--SET @SQLStringPrimeraOrdenacion = N'Select  pro_codigo,pro_nombre,pro_precio,pro_preciofarmacia,pro_ofeunidades,pro_ofeporcentaje ,pro_neto,pro_codtpopro,pro_descuentoweb,pro_laboratorio,pro_monodroga,pro_codtpovta,pro_codigobarra,pro_codigoalfabeta, 0 as pro_Ranking,pro_isTrazable,pro_isCadenaFrio,pro_entransfer,pro_canmaxima,pro_vtasolotransfer,pro_troquel,pro_acuerdo,pro_NoTransfersEnClientesPerf,pro_familia,pro_PackDeVenta,pro_PrecioBase, pro_PorcARestarDelDtoDeCliente
SET @SQLStringPrimeraOrdenacion = N'Select  pro_codigo,pro_nombre,pro_precio,pro_preciofarmacia,pro_ofeunidades,pro_ofeporcentaje ,pro_neto,pro_codtpopro,pro_descuentoweb,pro_laboratorio,pro_monodroga,pro_codtpovta,pro_codigobarra,pro_codigoalfabeta, 0 as pro_Ranking,pro_isTrazable,pro_isCadenaFrio,pro_entransfer,pro_canmaxima,pro_vtasolotransfer,pro_troquel,pro_acuerdo,pro_NoTransfersEnClientesPerf,pro_familia,pro_PackDeVenta,pro_PrecioBase, pro_PorcARestarDelDtoDeCliente,pro_AceptaVencidos,pro_ProductoRequiereLote
from Productos.vistaProductosBusqueda
where ' + @WherePrimeraOrdenacion + ' AND ' +  @Where + ' '

--SET @SQLString = N'Select  pro_codigo,pro_nombre,pro_precio,pro_preciofarmacia,pro_ofeunidades,pro_ofeporcentaje ,pro_neto,pro_codtpopro,pro_descuentoweb,pro_laboratorio,pro_monodroga,pro_codtpovta,pro_codigobarra,pro_codigoalfabeta, 0 as pro_Ranking,pro_isTrazable,pro_isCadenaFrio,pro_entransfer,pro_canmaxima,pro_vtasolotransfer,pro_troquel,pro_acuerdo,pro_NoTransfersEnClientesPerf,pro_familia,pro_PackDeVenta,pro_PrecioBase, pro_PorcARestarDelDtoDeCliente
SET @SQLString = N'Select  pro_codigo,pro_nombre,pro_precio,pro_preciofarmacia,pro_ofeunidades,pro_ofeporcentaje ,pro_neto,pro_codtpopro,pro_descuentoweb,pro_laboratorio,pro_monodroga,pro_codtpovta,pro_codigobarra,pro_codigoalfabeta, 0 as pro_Ranking,pro_isTrazable,pro_isCadenaFrio,pro_entransfer,pro_canmaxima,pro_vtasolotransfer,pro_troquel,pro_acuerdo,pro_NoTransfersEnClientesPerf,pro_familia,pro_PackDeVenta,pro_PrecioBase, pro_PorcARestarDelDtoDeCliente,pro_AceptaVencidos,pro_ProductoRequiereLote
from Productos.vistaProductosBusqueda 
where ' + @Where 

 IF OBJECT_ID ( 'tempdb..#tempTablaProductos' ) IS NOT NULL 
BEGIN
   DROP TABLE #tempTablaProductos
END
--create table #tempTablaProductos (pro_codigo  nvarchar(50) ,pro_nombre nvarchar(75) ,pro_precio decimal(9,2),pro_preciofarmacia decimal(9,2),pro_ofeunidades int,pro_ofeporcentaje decimal(6,2),pro_neto bit,pro_codtpopro nvarchar(1),pro_descuentoweb decimal(6,2),pro_laboratorio nvarchar(75),pro_monodroga nvarchar(75),pro_codtpovta  nvarchar(2),pro_codigobarra nvarchar(13),pro_codigoalfabeta nvarchar(10), pro_Ranking int, pro_isTrazable bit,pro_isCadenaFrio bit,pro_entransfer bit null,pro_canmaxima int null, pro_vtasolotransfer bit null,pro_troquel nvarchar(50),pro_acuerdo int,pro_NoTransfersEnClientesPerf bit null,pro_familia nvarchar(75) null,pro_PackDeVenta int null,pro_PrecioBase decimal(9, 2) NULL, pro_PorcARestarDelDtoDeCliente decimal(6, 2) NULL)
create table #tempTablaProductos (pro_codigo  nvarchar(50) ,pro_nombre nvarchar(75) ,pro_precio decimal(9,2),pro_preciofarmacia decimal(9,2),pro_ofeunidades int,pro_ofeporcentaje decimal(6,2),pro_neto bit,pro_codtpopro nvarchar(1),pro_descuentoweb decimal(6,2),pro_laboratorio nvarchar(75),pro_monodroga nvarchar(75),pro_codtpovta  nvarchar(2),pro_codigobarra nvarchar(13),pro_codigoalfabeta nvarchar(10), pro_Ranking int, pro_isTrazable bit,pro_isCadenaFrio bit,pro_entransfer bit null,pro_canmaxima int null, pro_vtasolotransfer bit null,pro_troquel nvarchar(50),pro_acuerdo int,pro_NoTransfersEnClientesPerf bit null,pro_familia nvarchar(75) null,pro_PackDeVenta int null,pro_PrecioBase decimal(9, 2) NULL, pro_PorcARestarDelDtoDeCliente decimal(6, 2) NULL, pro_AceptaVencidos bit,pro_ProductoRequiereLote bit)

insert into #tempTablaProductos  EXEC sp_executesql @SQLString

 IF OBJECT_ID ( 'tempdb..#tempTablaProductosPrimeraOrdenacion' ) IS NOT NULL 
BEGIN
   DROP TABLE #tempTablaProductosPrimeraOrdenacion
END
--create table #tempTablaProductosPrimeraOrdenacion (pro_codigo  nvarchar(50) ,pro_nombre nvarchar(75) ,pro_precio decimal(9,2),pro_preciofarmacia decimal(9,2),pro_ofeunidades int,pro_ofeporcentaje decimal(6,2),pro_neto bit,pro_codtpopro nvarchar(1),pro_descuentoweb decimal(6,2),pro_laboratorio nvarchar(75),pro_monodroga nvarchar(75),pro_codtpovta  nvarchar(2),pro_codigobarra nvarchar(13),pro_codigoalfabeta nvarchar(10), pro_Ranking int, pro_isTrazable bit,pro_isCadenaFrio bit,pro_entransfer bit null,pro_canmaxima int null, pro_vtasolotransfer bit null,pro_troquel nvarchar(50),pro_acuerdo int,pro_NoTransfersEnClientesPerf bit null,pro_familia nvarchar(75) null,pro_PackDeVenta int null,pro_PrecioBase decimal(9, 2) NULL, pro_PorcARestarDelDtoDeCliente decimal(6, 2) NULL)
create table #tempTablaProductosPrimeraOrdenacion (pro_codigo  nvarchar(50) ,pro_nombre nvarchar(75) ,pro_precio decimal(9,2),pro_preciofarmacia decimal(9,2),pro_ofeunidades int,pro_ofeporcentaje decimal(6,2),pro_neto bit,pro_codtpopro nvarchar(1),pro_descuentoweb decimal(6,2),pro_laboratorio nvarchar(75),pro_monodroga nvarchar(75),pro_codtpovta  nvarchar(2),pro_codigobarra nvarchar(13),pro_codigoalfabeta nvarchar(10), pro_Ranking int, pro_isTrazable bit,pro_isCadenaFrio bit,pro_entransfer bit null,pro_canmaxima int null, pro_vtasolotransfer bit null,pro_troquel nvarchar(50),pro_acuerdo int,pro_NoTransfersEnClientesPerf bit null,pro_familia nvarchar(75) null,pro_PackDeVenta int null,pro_PrecioBase decimal(9, 2) NULL, pro_PorcARestarDelDtoDeCliente decimal(6, 2) NULL, pro_AceptaVencidos bit,pro_ProductoRequiereLote bit)

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
GO

USE [db_Kellerhoff]
GO

/****** Object:  View [Productos].[vistaProductosBusqueda]    Script Date: 06/11/2020 08:43:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [Productos].[vistaProductosBusqueda]
WITH SCHEMABINDING 
AS
SELECT     pro_codigo, pro_nombre, pro_precio, pro_preciofarmacia, pro_ofeunidades, pro_ofeporcentaje, pro_neto, pro_codtpopro, pro_descuentoweb, pro_laboratorio, 
                      pro_monodroga, pro_codtpovta, pro_codigobarra, pro_codigoalfabeta, pro_codigo + ' ' + ISNULL(pro_nombre, '') + ' ' + ISNULL(pro_laboratorio, '') 
                      + ' ' + ISNULL(pro_monodroga, '') + ' ' + ISNULL(pro_codigobarra, '') + ' ' + ISNULL(pro_codigoalfabeta, '') AS pro_columnaWhere, ISNULL(pro_nombre, '') 
                      + ' ' + ISNULL(pro_codigobarra, '') + ' ' + ISNULL(pro_laboratorio, '') AS pop_columnaWhereDefault, pro_isTrazable, pro_isCadenaFrio, pro_entransfer, 
                      pro_canmaxima, pro_vtasolotransfer, pro_troquel, pro_acuerdo, pro_NoTransfersEnClientesPerf, pro_Familia, pro_PackDeVenta, pro_PrecioBase, 
                      pro_PorcARestarDelDtoDeCliente, pro_AceptaVencidos, pro_ProductoRequiereLote
FROM         dbo.tbl_Productos

GO

