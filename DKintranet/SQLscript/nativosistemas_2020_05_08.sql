
alter table  [dbo].[tbl_Sucursales]
add suc_provincia [nvarchar](75) NULL,
suc_facturaTrazables bit NOT NULL default 1,
suc_facturaTrazablesEnOtrasProvincias bit NOT NULL default 1
GO
--------

ALTER procedure [Clientes].[spRecuperarTodasSucursal]
 AS
SELECT *
  FROM tbl_Sucursales
GO
----