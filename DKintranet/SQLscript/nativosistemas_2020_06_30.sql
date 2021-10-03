alter table [dbo].[tbl_Transfers_Detalle]
add tde_PorcDtoSobrePVP  [decimal](6, 2) NULL
GO
alter table [dbo].[tmp_Transfers_Detalle_Todos]
add PorcDtoSobrePVP  [decimal](6, 2) NULL
GO

-----------
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
	  ,tde_PorcARestarDelDtoDeCliente
	  ,tde_PorcDtoSobrePVP)
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
	  ,PorcDtoSobrePVP
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
---