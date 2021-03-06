USE [db_Kellerhoff]
GO
/****** Object:  StoredProcedure [Devoluciones].[spAgregarItemDevolucion]    Script Date: 09/04/2020 12:49:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [Devoluciones].[spAgregarItemDevolucion]
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
	
	/*Si es por VENCIDO y viene mismo Producto y mismo Lote sumo las cantidades*/
	IF @numerofactura IS NULL
		SELECT TOP 1 @TempNroItem = [dev_numeroitem]
		FROM tmp_Solicitudes_Devoluciones_Items  with (NOLOCK)
		WHERE [dev_numerocliente] = @numerocliente 
		AND [dev_nombreproductodevolucion] = @nombreproductodevolucion
		AND [dev_numerolote] = @numerolote
	ELSE	
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