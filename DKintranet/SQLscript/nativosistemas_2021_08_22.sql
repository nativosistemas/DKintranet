
---
alter table [dbo].[tbl_ProductosImagen]
add [pri_ancho_ampliar] int,
 [pri_alto_ampliar] int
go
---
alter table [dbo].[tbl_Oferta]
add [ofe_nuevosLanzamiento] bit default 0
go
---
alter table [dbo].[tbl_Oferta]
add [ofe_descrHtml] nvarchar(max)
go
---
IF OBJECT_ID ( 'spActualizarProductosImagenAnchoAlto', 'P' ) IS NOT NULL 
    DROP PROCEDURE spActualizarProductosImagenAnchoAlto;
GO
CREATE PROCEDURE spActualizarProductosImagenAnchoAlto
@pri_codigo nvarchar(50),
@pri_ancho_ampliar int,
@pri_alto_ampliar int
AS 
update  [dbo].[tbl_ProductosImagen]
set [pri_ancho_ampliar] =@pri_ancho_ampliar
      ,[pri_alto_ampliar] = @pri_alto_ampliar
  WHERE [pri_codigo] = @pri_codigo
  
GO	
---
IF OBJECT_ID ( 'spBorrarAnchoAltoImagen', 'P' ) IS NOT NULL 
    DROP PROCEDURE spBorrarAnchoAltoImagen;
GO
CREATE PROCEDURE  [spBorrarAnchoAltoImagen]
AS 
update  [dbo].[tbl_ProductosImagen]
set [pri_ancho_ampliar] = NULL
      ,[pri_alto_ampliar] = NULL
  
GO
---
ALTER procedure [dbo].[spRecuperarTodasOferta]
 AS

SELECT [ofe_idOferta]
      ,[ofe_titulo]
      ,[ofe_descr]
      ,[ofe_tipo]
      ,[ofe_descuento]
	  ,[ofe_etiqueta]
	  ,[ofe_etiquetaColor]
	  ,ofe_publicar
      ,[ofe_activo]
	  ,ofe_fecha
	  ,ofe_nombreTransfer 
	  ,ofe_nuevosLanzamiento
	  ,ofe_descrHtml
	  ,tfr_codigo
	  ,count(DISTINCT[ofd_idOfertaDetalle]) as 'countOfertaDetalles'
	  ,countRating as 'Rating'
	  ,tbImg.[arc_nombre] as 'nameImagen'
	  ,tbPdf.[arc_nombre] as 'namePdf'
	  ,tbImgAmpliar.[arc_nombre] as 'nameImagenAmpliar'
	  ,ofe_fechaFinOferta
  FROM [dbo].[tbl_Oferta] as ofe
  left join [dbo].[tbl_OfertaDetalle] on [ofd_idOferta] = [ofe_idOferta]
  left join vistaOferta_Rating on [ofr_idOferta] = [ofe_idOferta]
  left join [dbo].[tbl_Transfers] on tfr_nombre = ofe_nombreTransfer
  left join [Recursos].[tbl_archivos] as tbImg on [ofe_idOferta] = tbImg.[arc_codRelacion] and tbImg.[arc_galeria] = 'ofertas'
  left join [Recursos].[tbl_archivos] as tbPdf on [ofe_idOferta] = tbPdf.[arc_codRelacion] and tbPdf.[arc_galeria] = 'ofertaspdf'
  left join [Recursos].[tbl_archivos] as tbImgAmpliar on [ofe_idOferta] = tbImgAmpliar.[arc_codRelacion] and tbImgAmpliar.[arc_galeria] = 'ofertasampliar'
  group by [ofe_idOferta]
      ,[ofe_titulo]
      ,[ofe_descr]
      ,[ofe_tipo]
      ,[ofe_descuento]
	  ,[ofe_etiqueta]
	  ,[ofe_etiquetaColor]
	  ,ofe_publicar
      ,[ofe_activo]
	  ,ofe_fecha
	  ,ofe_nombreTransfer
	  ,ofe_nuevosLanzamiento
	  ,ofe_descrHtml
	  ,tfr_codigo
	  ,tbImg.[arc_nombre]
	  ,tbPdf.[arc_nombre] 
	  ,tbImgAmpliar.[arc_nombre] 
	  ,ofe_fechaFinOferta
	  ,countRating
GO
--------

ALTER PROCEDURE [dbo].[spInsertarActualizarOferta]
@ofe_idOferta [int] ,
@ofe_titulo [nvarchar](500),
@ofe_descr [nvarchar](max),
@ofe_tipo int ,
@ofe_nombreTransfer [nvarchar](75) ,
@ofe_descuento [nvarchar](50) ,
@ofe_etiqueta [nvarchar](50) ,
@ofe_etiquetaColor [nvarchar](10) ,
@ofe_fechaFinOferta datetime,
@ofe_nuevosLanzamiento bit,
@ofe_descrHtml nvarchar(max)
--@ofe_publicar bit,
--@ofe_activo bit 
 AS
BEGIN TRANSACTION
BEGIN TRY

IF @ofe_idOferta = 0
BEGIN

INSERT INTO tbl_Oferta
(ofe_titulo,ofe_descr,ofe_tipo,ofe_nombreTransfer ,ofe_descuento,ofe_publicar,ofe_etiqueta,ofe_etiquetaColor,ofe_fechaFinOferta,ofe_nuevosLanzamiento,ofe_descrHtml)
values (@ofe_titulo,@ofe_descr,@ofe_tipo,@ofe_nombreTransfer ,@ofe_descuento,0,@ofe_etiqueta,@ofe_etiquetaColor,@ofe_fechaFinOferta,@ofe_nuevosLanzamiento,@ofe_descrHtml)

set  @ofe_idOferta = SCOPE_IDENTITY()

END
ELSE
BEGIN

UPDATE tbl_Oferta
SET   ofe_titulo = @ofe_titulo,
	  ofe_descr = @ofe_descr,
	  ofe_tipo = @ofe_tipo,
	  ofe_nombreTransfer = @ofe_nombreTransfer ,
	  ofe_descuento = @ofe_descuento,
	  --ofe_publicar = @ofe_publicar,
	  --ofe_activo = @ofe_activo,
	  ofe_etiqueta = @ofe_etiqueta,
	  ofe_etiquetaColor = @ofe_etiquetaColor,
	  ofe_fechaFinOferta = @ofe_fechaFinOferta,
	  ofe_nuevosLanzamiento = @ofe_nuevosLanzamiento,
	  ofe_descrHtml = @ofe_descrHtml
WHERE ofe_idOferta = @ofe_idOferta

END

SELECT @ofe_idOferta

COMMIT TRANSACTION 
END TRY
BEGIN CATCH
ROLLBACK TRANSACTION 
EXEC LogRegistro.spLogError @mensaje = N'';
END CATCH
GO
-----
ALTER procedure [dbo].[spRecuperarTodasOfertaPublicar]
 AS

SELECT [ofe_idOferta]
      ,[ofe_titulo]
      ,[ofe_descr]
      ,[ofe_tipo]
      ,[ofe_descuento]
	  ,[ofe_etiqueta]
	  ,[ofe_etiquetaColor]
	  ,ofe_publicar
      ,[ofe_activo]
	  ,ofe_fecha
	  ,ofe_nombreTransfer 
	  ,tfr_codigo
	  ,count([ofd_idOfertaDetalle]) 'countOfertaDetalles'
	  ,tbImg.[arc_nombre] as 'nameImagen'
	  ,tbPdf.[arc_nombre] as 'namePdf'
	  ,tbImgAmpliar.[arc_nombre] as 'nameImagenAmpliar'
	  ,ofe_fechaFinOferta
	  ,ofe_nuevosLanzamiento
	  ,ofe_descrHtml
  FROM [dbo].[tbl_Oferta] 
  left join [dbo].[tbl_OfertaDetalle] on [ofd_idOferta] = [ofe_idOferta]
  left join [dbo].[tbl_Transfers] on tfr_nombre = ofe_nombreTransfer
  left join [Recursos].[tbl_archivos] as tbImg on [ofe_idOferta] = tbImg.[arc_codRelacion] and tbImg.[arc_galeria] = 'ofertas'
  left join [Recursos].[tbl_archivos] as tbPdf on [ofe_idOferta] = tbPdf.[arc_codRelacion] and tbPdf.[arc_galeria] = 'ofertaspdf'
  left join [Recursos].[tbl_archivos] as tbImgAmpliar on [ofe_idOferta] = tbImgAmpliar.[arc_codRelacion] and tbImgAmpliar.[arc_galeria] = 'ofertasampliar'
  where ofe_publicar = 1 and (ofe_fechaFinOferta is NULL or ofe_fechaFinOferta >= CONVERT (date, GETDATE()))
  group by [ofe_idOferta]
      ,[ofe_titulo]
      ,[ofe_descr]
      ,[ofe_tipo]
      ,[ofe_descuento]
	  ,[ofe_etiqueta]
	  ,[ofe_etiquetaColor]
	  ,ofe_publicar
      ,[ofe_activo]
	  ,ofe_fecha
	  ,ofe_nombreTransfer
	  ,tfr_codigo
	  ,tbImg.[arc_nombre]
	  ,tbPdf.[arc_nombre] 
	  ,tbImgAmpliar.[arc_nombre]
	  ,ofe_fechaFinOferta
	  ,ofe_nuevosLanzamiento
	  ,ofe_descrHtml
GO
----
ALTER procedure [dbo].[spRecuperarTodasOfertaParaHome]
 AS

SELECT [ofe_idOferta]
      ,[ofe_titulo]
      ,[ofe_descr]
      ,[ofe_tipo]
      ,[ofe_descuento]
	  ,[ofe_etiqueta]
	  ,[ofe_etiquetaColor]
	  ,ofe_publicar
      ,[ofe_activo]
	  ,ofe_fecha
	  ,ofe_nombreTransfer
	  ,tfr_codigo
	  ,count(DISTINCT[ofd_idOfertaDetalle]) as 'countOfertaDetalles'
	  --,count(DISTINCT [ofr_idOferta_Rating]) as 'Rating'
	  ,[ofh_idOfertaHome]
      ,[ofh_orden]
      ,[ofh_idOferta]
	  ,tbImg.[arc_nombre] as 'nameImagen'
	  ,tbPdf.[arc_nombre] as 'namePdf'
	  ,tbImgAmpliar.[arc_nombre] as 'nameImagenAmpliar'
	  ,ofe_fechaFinOferta
	  ,ofe_nuevosLanzamiento
	  ,ofe_descrHtml
  FROM [dbo].[tbl_OfertaHome] 
  inner join [dbo].[tbl_Oferta] on [ofh_idOferta] = [ofe_idOferta]
  left join [dbo].[tbl_OfertaDetalle] on [ofd_idOferta] = [ofe_idOferta]
  --left join [dbo].[tbl_Oferta_Rating] on [ofr_idOferta] = [ofe_idOferta]
  left join [dbo].[tbl_Transfers] on tfr_nombre = ofe_nombreTransfer
  left join [Recursos].[tbl_archivos] as tbImg on [ofe_idOferta] = tbImg.[arc_codRelacion] and tbImg.[arc_galeria] = 'ofertas'
  left join [Recursos].[tbl_archivos] as tbPdf on [ofe_idOferta] = tbPdf.[arc_codRelacion] and tbPdf.[arc_galeria] = 'ofertaspdf'
  left join [Recursos].[tbl_archivos] as tbImgAmpliar on [ofe_idOferta] = tbImgAmpliar.[arc_codRelacion] and tbImgAmpliar.[arc_galeria] = 'ofertasampliar'
  where ofe_publicar = 1 and (ofe_fechaFinOferta is NULL or ofe_fechaFinOferta >= CONVERT (date, GETDATE()))
  group by [ofe_idOferta]
      ,[ofe_titulo]
      ,[ofe_descr]
      ,[ofe_tipo]
      ,[ofe_descuento]
	  ,[ofe_etiqueta]
	  ,[ofe_etiquetaColor]
	  ,ofe_publicar
      ,[ofe_activo]
	  ,ofe_nombreTransfer
	  ,tfr_codigo
	  ,ofe_fecha
	  ,[ofh_idOfertaHome]
      ,[ofh_orden]
      ,[ofh_idOferta]
	  ,tbImg.[arc_nombre]
	  ,tbPdf.[arc_nombre] 
	  ,tbImgAmpliar.[arc_nombre]
	  ,ofe_fechaFinOferta
	  ,ofe_nuevosLanzamiento
	  ,ofe_descrHtml
	  order by ofh_orden
GO
----
ALTER procedure [dbo].[spObtenerProductosImagenes]
 AS


 SELECT i.[pri_codigo] as [pro_codigo]
	  ,i.*
  FROM [dbo].[tbl_ProductosImagen] as i

GO
---