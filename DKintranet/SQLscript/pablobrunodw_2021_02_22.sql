USE [db_Kellerhoff]
GO

/****** Object:  Table [dbo].[tmp_Solicitudes_Devoluciones_Items]    Script Date: 08/11/2020 08:38:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tmp_Reclamo_Facturado_No_Enviado_Items](
	[fne_numeroitem] [int] IDENTITY(1,1) NOT NULL,
	[fne_numerocliente] [int] NOT NULL,
	[fne_numerofactura] [nvarchar](13) NULL,
	[fne_nombreproducto] [nvarchar](75) NOT NULL,
	[fne_fecha] [datetime] NOT NULL,
	[fne_cantidad] [int] NOT NULL,
	[fne_idsucursal] [nvarchar](2) NOT NULL,
 CONSTRAINT [PK_tmp_Reclamo_Facturado_No_Enviado_Items] PRIMARY KEY CLUSTERED 
(
	[fne_numeroitem] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tmp_Reclamo_Facturado_No_Enviado_Items] ADD  CONSTRAINT [DF_tmp_Reclamo_Facturado_No_Enviado_Items_fne_idsucursal]  DEFAULT ('CC') FOR [fne_idsucursal]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [Devoluciones].[spAgregarItemReclamoFacturadoNoEnviado]
	@numerocliente int,
	@numerofactura nvarchar(13),
	@nombreproducto nvarchar(75),
	@cantidad int,
	@idsucursal nvarchar(2),
	@isOk bit OUTPUT
AS

BEGIN TRANSACTION [Tran1]
BEGIN TRY
	DECLARE @TempNroItem int
	DECLARE @Fecha datetime;
	SET @Fecha = GETDATE();
	
	SELECT TOP 1 @TempNroItem = [fne_numeroitem]
	FROM tmp_Reclamo_Facturado_No_Enviado_Items  with (NOLOCK)
	WHERE [fne_numerocliente] = @numerocliente 
	AND [fne_numerofactura] = @numerofactura
	AND [fne_nombreproducto] = @nombreproducto
		
		
	IF ISNULL(@TempNroItem,-1) = -1
		BEGIN
			INSERT INTO tmp_Reclamo_Facturado_No_Enviado_Items
			(fne_numerocliente,fne_numerofactura,fne_nombreproducto,fne_fecha,fne_cantidad,fne_idsucursal)
			VALUES(@numerocliente,@numerofactura,@nombreproducto,@Fecha,@cantidad,@idsucursal)

		END 
	ELSE
		BEGIN
			UPDATE tmp_Reclamo_Facturado_No_Enviado_Items
			SET fne_cantidad = fne_cantidad + @cantidad,
			fne_fecha = @Fecha,
			fne_idsucursal = @idsucursal
			WHERE [fne_numeroitem] = @TempNroItem
		END
	
	COMMIT TRANSACTION [Tran1]
	SET @isOk = 1
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION [Tran1]
	EXEC LogRegistro.spLogError @mensaje = N'';
	SET @isOk = 0
END CATCH

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [Devoluciones].[spRecuperarItemsReclamoFacturadoNoEnviado]
	@numerocliente int
AS
BEGIN
	SELECT [fne_numeroitem] AS [dev_numeroitem]
	,[fne_numerocliente] AS [dev_numerocliente]
	,[fne_numerofactura] AS [dev_numerofactura]
	,[fne_nombreproducto] AS [dev_nombreproductodevolucion]
	,[fne_fecha] AS [dev_fecha]
	,[fne_cantidad] AS [dev_cantidad]
	,[fne_idsucursal] AS [dev_idsucursal]
	,1 AS [dev_motivo]
	,1 AS [dev_numeroitemfactura]
	,[fne_nombreproducto] AS [dev_nombreproductofactura]
	,NULL AS [dev_numerolote]
	,NULL AS [dev_fechavencimientolote]
	FROM [tmp_Reclamo_Facturado_No_Enviado_Items]
	WHERE fne_numerocliente = @numerocliente
	ORDER BY [fne_numeroitem],fne_fecha
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [Devoluciones].[spEliminarPrecargaReclamoFNEPorCliente]
	@NumeroCliente int,
	@isOk bit OUTPUT
AS

BEGIN TRANSACTION [Tran1]
BEGIN TRY
	DELETE FROM [tmp_Reclamo_Facturado_No_Enviado_Items]
	WHERE [fne_numerocliente] = @NumeroCliente
	
	COMMIT TRANSACTION [Tran1]
	SET @isOk = 1
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION [Tran1]
	EXEC LogRegistro.spLogError @mensaje = N'';
	SET @isOk = 0
END CATCH
