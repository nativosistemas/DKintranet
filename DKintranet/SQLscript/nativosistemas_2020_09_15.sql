ALTER PROCEDURE [Clientes].[spRecuperarClientePorId]
@cli_codigo int
AS 
SELECT *
  FROM tbl_Clientes  
  WHERE cli_codigo = @cli_codigo 

GO