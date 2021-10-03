alter table  [dbo].[tbl_Sucursales]
 add suc_pedirCC_ok bit NOT NULL default 1,
  suc_pedirCC_sucursalReferencia nvarchar(2)  NULL,
  suc_pedirCC_tomaSoloPerfumeria bit NOT NULL default 0
  GO
  ---