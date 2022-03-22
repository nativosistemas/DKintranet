using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using DKintranet.Codigo.clases;

namespace DKintranet.Codigo.capaDatos
{
    /// <summary>
    /// Summary description for capaProductos
    /// </summary>
    public class capaProductos
    {
        public static DataSet RecuperarTodosProductosBuscadorV3(string pTextoBuscador, List<string> pListaColumna, string pSucursalPerteneciente, int? pIdCliente, string pCli_codprov)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spRecuperarTodosProductosBuscadorV3", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paWhere = cmdComandoInicio.Parameters.Add("@Where", SqlDbType.NVarChar, 4000);
            paWhere.Value = pListaColumna == null ? FuncionesPersonalizadas.GenerarWhereLikeConColumna(pTextoBuscador, "pop_columnaWhereDefault") : FuncionesPersonalizadas.GenerarWhereLikeConVariasColumnas(pTextoBuscador, pListaColumna);

            SqlParameter paWherePrimeraOrdenacion = cmdComandoInicio.Parameters.Add("@WherePrimeraOrdenacion", SqlDbType.NVarChar, 4000);
            paWherePrimeraOrdenacion.Value = FuncionesPersonalizadas.GenerarWhereLikeConColumna_EmpiezaCon(pTextoBuscador, "pro_nombre");

            SqlParameter paSucursal = cmdComandoInicio.Parameters.Add("@Sucursal", SqlDbType.NVarChar, 2);
            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@codCliente", SqlDbType.Int);
            SqlParameter paCli_codprov = cmdComandoInicio.Parameters.Add("@cli_codprov", SqlDbType.NVarChar, 75);
            paSucursal.Value = pSucursalPerteneciente;
            if (pIdCliente == null)
            {
                paIdCliente.Value = DBNull.Value;
            }
            else
            {
                paIdCliente.Value = (int)pIdCliente;
            }
            paCli_codprov.Value = pCli_codprov;
            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "ProductosBuscador");
                return dsResultado;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataSet RecuperarTodosProductosBuscadorOferta(int pIdOferta, string pSucursalPerteneciente, int? pIdCliente, string pCli_codprov)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spRecuperarTodosProductosBuscadorOferta", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paIdOferta = cmdComandoInicio.Parameters.Add("@ofe_idOferta", SqlDbType.Int);

            SqlParameter paSucursal = cmdComandoInicio.Parameters.Add("@Sucursal", SqlDbType.NVarChar, 2);
            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@codCliente", SqlDbType.Int);
            SqlParameter paCli_codprov = cmdComandoInicio.Parameters.Add("@cli_codprov", SqlDbType.NVarChar, 75);
            paIdOferta.Value = pIdOferta;
            paSucursal.Value = pSucursalPerteneciente;
            if (pIdCliente == null)
            {
                paIdCliente.Value = DBNull.Value;
            }
            else
            {
                paIdCliente.Value = (int)pIdCliente;
            }
            paCli_codprov.Value = pCli_codprov;
            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "ProductosBuscador");
                return dsResultado;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataSet RecuperarTodosProductosBuscadorV2(string pTextoBuscador, string pSucursalPerteneciente, int? pIdCliente, string pCli_codprov)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spRecuperarTodosProductosBuscadorV2", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paWhere = cmdComandoInicio.Parameters.Add("@Where", SqlDbType.NVarChar, 4000);
            string strWhere = string.Empty;
            strWhere += FuncionesPersonalizadas.GenerarWhereLikeConColumna(pTextoBuscador, "pop_columnaWhereDefault"); // pop_columnaWhere
            paWhere.Value = strWhere;

            SqlParameter paWherePrimeraOrdenacion = cmdComandoInicio.Parameters.Add("@WherePrimeraOrdenacion", SqlDbType.NVarChar, 4000);
            string strWherePrimeraOrdenacion = string.Empty;
            strWherePrimeraOrdenacion += FuncionesPersonalizadas.GenerarWhereLikeConColumna_EmpiezaCon(pTextoBuscador, "pro_nombre");
            paWherePrimeraOrdenacion.Value = strWherePrimeraOrdenacion;



            SqlParameter paSucursal = cmdComandoInicio.Parameters.Add("@Sucursal", SqlDbType.NVarChar, 2);
            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@codCliente", SqlDbType.Int);
            SqlParameter paCli_codprov = cmdComandoInicio.Parameters.Add("@cli_codprov", SqlDbType.NVarChar, 75);
            paSucursal.Value = pSucursalPerteneciente;
            if (pIdCliente == null)
            {
                paIdCliente.Value = DBNull.Value;
            }
            else
            {
                paIdCliente.Value = (int)pIdCliente;
            }
            paCli_codprov.Value = pCli_codprov;
            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "ProductosBuscador");
                return dsResultado;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataSet RecuperarTodosProductosBuscador(string pTextoBuscador, string pSucursalPerteneciente, int? pIdCliente, string pCli_codprov)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spRecuperarTodosProductosBuscador", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paWhere = cmdComandoInicio.Parameters.Add("@Where", SqlDbType.NVarChar, 4000);
            string strWhere = string.Empty;

            strWhere += FuncionesPersonalizadas.GenerarWhereLikeConColumna(pTextoBuscador, "pop_columnaWhereDefault"); // pop_columnaWhere

            //strWhere += FuncionesPersonalizadas.GenerarWhereLikeConColumna(pTextoBuscador, "pro_nombre"); // Nombre producto
            //strWhere +=  " OR " + FuncionesPersonalizadas.GenerarWhereLikeConColumna(pTextoBuscador, "pro_codigo"); // Código
            //strWhere += " OR " + FuncionesPersonalizadas.GenerarWhereLikeConColumna(pTextoBuscador, "pro_laboratorio");  // Laboratorio
            //strWhere += " OR " + FuncionesPersonalizadas.GenerarWhereLikeConColumna(pTextoBuscador, "pro_monodroga"); // Mono droga
            //strWhere += " OR " + FuncionesPersonalizadas.GenerarWhereLikeConColumna(pTextoBuscador, "pro_codigobarra"); // Código de barra
            //strWhere += " OR " + FuncionesPersonalizadas.GenerarWhereLikeConColumna(pTextoBuscador, "pro_codigoalfabeta"); // Código alfa beta
            paWhere.Value = strWhere;

            SqlParameter paSucursal = cmdComandoInicio.Parameters.Add("@Sucursal", SqlDbType.NVarChar, 2);
            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@codCliente", SqlDbType.Int);
            SqlParameter paCli_codprov = cmdComandoInicio.Parameters.Add("@cli_codprov", SqlDbType.NVarChar, 75);
            paSucursal.Value = pSucursalPerteneciente;
            if (pIdCliente == null)
            {
                paIdCliente.Value = DBNull.Value;
            }
            else
            {
                paIdCliente.Value = (int)pIdCliente;
            }
            paCli_codprov.Value = pCli_codprov;
            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "ProductosBuscador");
                return dsResultado;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataSet RecuperarTodosProductosBuscadorConVariasColumnasV2(string pTextoBuscador, List<string> pListaColumna, string pSucursalPerteneciente, int? pIdCliente, string pCli_codprov)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spRecuperarTodosProductosBuscadorV2", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paWhere = cmdComandoInicio.Parameters.Add("@Where", SqlDbType.NVarChar, 4000);
            string strWhere = string.Empty;
            strWhere += FuncionesPersonalizadas.GenerarWhereLikeConVariasColumnas(pTextoBuscador, pListaColumna);
            paWhere.Value = strWhere;

            SqlParameter paWherePrimeraOrdenacion = cmdComandoInicio.Parameters.Add("@WherePrimeraOrdenacion", SqlDbType.NVarChar, 4000);
            string strWherePrimeraOrdenacion = string.Empty;
            strWherePrimeraOrdenacion += FuncionesPersonalizadas.GenerarWhereLikeConColumna_EmpiezaCon(pTextoBuscador, "pro_nombre");
            paWherePrimeraOrdenacion.Value = strWherePrimeraOrdenacion;

            SqlParameter paSucursal = cmdComandoInicio.Parameters.Add("@Sucursal", SqlDbType.NVarChar, 2);
            paSucursal.Value = pSucursalPerteneciente;
            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@codCliente", SqlDbType.Int);
            SqlParameter paCli_codprov = cmdComandoInicio.Parameters.Add("@cli_codprov", SqlDbType.NVarChar, 75);
            paCli_codprov.Value = pCli_codprov;
            if (pIdCliente == null)
            {
                paIdCliente.Value = DBNull.Value;
            }
            else
            {
                paIdCliente.Value = (int)pIdCliente;
            }
            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "ProductosBuscador");
                return dsResultado;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataSet RecuperarTodosProductosBuscadorConVariasColumnas(string pTextoBuscador, List<string> pListaColumna, string pSucursalPerteneciente, int? pIdCliente, string pCli_codprov)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spRecuperarTodosProductosBuscador", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paWhere = cmdComandoInicio.Parameters.Add("@Where", SqlDbType.NVarChar, 4000);
            string strWhere = string.Empty;

            strWhere += FuncionesPersonalizadas.GenerarWhereLikeConVariasColumnas(pTextoBuscador, pListaColumna);
            paWhere.Value = strWhere;

            SqlParameter paSucursal = cmdComandoInicio.Parameters.Add("@Sucursal", SqlDbType.NVarChar, 2);
            paSucursal.Value = pSucursalPerteneciente;
            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@codCliente", SqlDbType.Int);
            SqlParameter paCli_codprov = cmdComandoInicio.Parameters.Add("@cli_codprov", SqlDbType.NVarChar, 75);
            paCli_codprov.Value = pCli_codprov;
            if (pIdCliente == null)
            {
                paIdCliente.Value = DBNull.Value;
            }
            else
            {
                paIdCliente.Value = (int)pIdCliente;
            }
            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "ProductosBuscador");
                return dsResultado;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataSet RecuperarTodosProductosBuscadorEnTransfer(string pSucursalPerteneciente, int? pIdCliente, string pCli_codprov)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spRecuperarTodosProductosBuscadorEnTransfer", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paSucursal = cmdComandoInicio.Parameters.Add("@Sucursal", SqlDbType.NVarChar, 2);
            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@codCliente", SqlDbType.Int);
            SqlParameter paCli_codprov = cmdComandoInicio.Parameters.Add("@cli_codprov", SqlDbType.NVarChar, 75);
            paSucursal.Value = pSucursalPerteneciente;
            if (pIdCliente == null)
            {
                paIdCliente.Value = DBNull.Value;
            }
            else
            {
                paIdCliente.Value = (int)pIdCliente;
            }
            paCli_codprov.Value = pCli_codprov;
            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "ProductosBuscador");
                return dsResultado;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataSet RecuperarTodosProductosBuscadorEnOferta(string pSucursalPerteneciente, int? pIdCliente, string pCli_codprov)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spRecuperarTodosProductosBuscadorEnOferta", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paSucursal = cmdComandoInicio.Parameters.Add("@Sucursal", SqlDbType.NVarChar, 2);
            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@codCliente", SqlDbType.Int);
            SqlParameter paCli_codprov = cmdComandoInicio.Parameters.Add("@cli_codprov", SqlDbType.NVarChar, 75);
            paSucursal.Value = pSucursalPerteneciente;
            if (pIdCliente == null)
            {
                paIdCliente.Value = DBNull.Value;
            }
            else
            {
                paIdCliente.Value = (int)pIdCliente;
            }
            paCli_codprov.Value = pCli_codprov;
            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "ProductosBuscador");
                return dsResultado;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataSet RecuperarTodosProductosBuscadorOptimizado(string pTextoBuscador)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spRecuperarTodosProductosBuscadorOptimizado", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paWhere = cmdComandoInicio.Parameters.Add("@Where", SqlDbType.NVarChar, 4000);
            string strWhere = string.Empty;

            strWhere += FuncionesPersonalizadas.GenerarWhereLikeConColumna(pTextoBuscador, "pop_columnaWhereDefault"); //pop_columnaWhere

            paWhere.Value = strWhere;

            //SqlParameter paSucursal = cmdComandoInicio.Parameters.Add("@Sucursal", SqlDbType.NVarChar, 2);
            //paSucursal.Value = pSucursalPerteneciente;
            //SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@codCliente", SqlDbType.Int);
            //if (pIdCliente == null)
            //{
            //    paIdCliente.Value = DBNull.Value;
            //}
            //else
            //{
            //    paIdCliente.Value = (int)pIdCliente;
            //}
            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "ProductosBuscador");
                return dsResultado;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataTable RecuperarTodosProductos()
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spRecuperadorTodosProductos", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "Productos");
                return dsResultado.Tables["Productos"];
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataTable RecuperadorProductoPorCodigo(string pCodigoProducto)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spRecuperadorProductoPorCodigo", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;
            SqlParameter paPro_nombre = cmdComandoInicio.Parameters.Add("@pro_codigo", SqlDbType.NVarChar, 50);
            paPro_nombre.Value = pCodigoProducto;
            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "Productos");
                return dsResultado.Tables["Productos"];
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataTable RecuperarProductoPorNombre(string pNombreProducto)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spRecuperadorTodosProductosPorNombre", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paPro_nombre = cmdComandoInicio.Parameters.Add("@pro_nombre", SqlDbType.NVarChar, 75);
            paPro_nombre.Value = pNombreProducto;

            try
            {
                Conn.Open();
                DataTable dt = new DataTable();
                SqlDataReader LectorSQLdata = cmdComandoInicio.ExecuteReader();
                dt.Load(LectorSQLdata);
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataSet RecuperarTodosProductosEnOferta()
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spRecuperadorTodosProductosEnOferta", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "Productos");
                return dsResultado;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataTable DescargaTodosProductos(string pProvincia)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spDescargaTodosProductos", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paProvincia = cmdComandoInicio.Parameters.Add("@cli_codprov", SqlDbType.NVarChar, 75);
            paProvincia.Value = pProvincia;

            try
            {
                Conn.Open();
                DataTable table = new DataTable();
                table.Load(cmdComandoInicio.ExecuteReader());
                return table;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataTable DescargaTodosProductosDrogueria(string pProvincia)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spDescargaTodosProductosDrogueria", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paProvincia = cmdComandoInicio.Parameters.Add("@cli_codprov", SqlDbType.NVarChar, 75);
            paProvincia.Value = pProvincia;

            try
            {
                Conn.Open();
                DataTable table = new DataTable();
                table.Load(cmdComandoInicio.ExecuteReader());
                return table;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataTable DescargaTodosProductosPerfumeria(string pProvincia)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spDescargaTodosProductosPerfumeria", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paProvincia = cmdComandoInicio.Parameters.Add("@cli_codprov", SqlDbType.NVarChar, 75);
            paProvincia.Value = pProvincia;


            try
            {
                Conn.Open();
                DataTable table = new DataTable();
                table.Load(cmdComandoInicio.ExecuteReader());
                return table;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataTable DescargaTodosProductosEnOferta()
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spDescargaTodosProductosEnOferta", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;
            try
            {
                Conn.Open();
                DataTable table = new DataTable();
                table.Load(cmdComandoInicio.ExecuteReader());
                return table;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        //
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static DataTable RecuperarProductoParametrizadoCantidad()
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spRecuperarProductoParametrizadoCantidad", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;
            try
            {
                Conn.Open();
                DataTable table = new DataTable();
                table.Load(cmdComandoInicio.ExecuteReader());
                return table;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static int InsertarActualizarProductoParametrizadoCantidad(int pCpc_cantidadParametrizada)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spInsertarActualizarProductoParametrizadoCantidad", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paCpc_cantidadParametrizada = cmdComandoInicio.Parameters.Add("@cpc_cantidadParametrizada", SqlDbType.Int);
            SqlParameter paCpc_tipoParametro = cmdComandoInicio.Parameters.Add("@cpc_tipoParametro", SqlDbType.Int);
            paCpc_tipoParametro.Value = 1;
            paCpc_cantidadParametrizada.Value = pCpc_cantidadParametrizada;

            try
            {
                Conn.Open();
                object objResultado = cmdComandoInicio.ExecuteScalar();
                return Convert.ToInt32(objResultado);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataSet RecuperarProductosDesdeTabla(DataTable pTablaProducto, string pSucursalPerteneciente, string pCli_codprov, int pCodCliente)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spRecuperarProductosDesdeTabla", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;
            SqlParameter paTablaProductos = cmdComandoInicio.Parameters.Add("@Tabla_Detalle", SqlDbType.Structured);
            SqlParameter paSucursal = cmdComandoInicio.Parameters.Add("@Sucursal", SqlDbType.NVarChar, 2);
            SqlParameter paCli_codprov = cmdComandoInicio.Parameters.Add("@cli_codprov", SqlDbType.NVarChar, 75);
            SqlParameter paCodCliente = cmdComandoInicio.Parameters.Add("@codCliente", SqlDbType.Int);
            paCodCliente.Value = pCodCliente;
            paSucursal.Value = pSucursalPerteneciente;
            paCli_codprov.Value = pCli_codprov;
            if (pTablaProducto == null)
            {
                paTablaProductos.Value = DBNull.Value;
            }
            else
            {
                paTablaProductos.Value = pTablaProducto;
            }
            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "Productos");
                return dsResultado;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataTable RecuperarStockPorProductosAndSucursal(DataTable pTablaSucursales, DataTable pTablaProductos)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spRecuperarStockPorProductosAndSucursal", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paTabla_Sucursales = cmdComandoInicio.Parameters.Add("@Tabla_Sucursales", SqlDbType.Structured);
            SqlParameter paTabla_Productos = cmdComandoInicio.Parameters.Add("@Tabla_Productos", SqlDbType.Structured);
            paTabla_Sucursales.Value = pTablaSucursales;
            paTabla_Productos.Value = pTablaProductos;

            try
            {
                Conn.Open();
                DataTable dt = new DataTable();
                SqlDataReader LectorSQLdata = cmdComandoInicio.ExecuteReader();
                dt.Load(LectorSQLdata);
                return dt;

            }
            catch (Exception ex)
            {
                //throw ex;
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        // Nuevo
        public static DataTable ObtenerProductosImagenesBusqueda(string pTxtBuscador)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("dbo.spObtenerProductosImagenesBusqueda", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;
            SqlParameter paWhere = cmdComandoInicio.Parameters.Add("@Where", SqlDbType.NVarChar, 4000);
            List<string> pListaColumna = new List<string>();
            pListaColumna.Add("pro_nombre");
            pListaColumna.Add("pro_codigo");
            pListaColumna.Add("pro_codigobarra");
            paWhere.Value = FuncionesPersonalizadas.GenerarWhereLikeConVariasColumnas(pTxtBuscador, pListaColumna);
            try
            {
                Conn.Open();
                DataTable dt = new DataTable();
                SqlDataReader LectorSQLdata = cmdComandoInicio.ExecuteReader();
                dt.Load(LectorSQLdata);
                return dt;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static DataTable ObtenerProductosImagenes()
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("dbo.spObtenerProductosImagenes", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;
            try
            {
                Conn.Open();
                DataTable dt = new DataTable();
                SqlDataReader LectorSQLdata = cmdComandoInicio.ExecuteReader();
                dt.Load(LectorSQLdata);
                return dt;

            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static bool ActualizarInsertarProductosImagen(string pCodigoProducto, string pNombreArchivo)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("dbo.spActualizarInsertarProductosImagen", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paPri_codigo = cmdComandoInicio.Parameters.Add("@pri_codigo", SqlDbType.NVarChar, 50);
            SqlParameter paPri_nombreArchivo = cmdComandoInicio.Parameters.Add("@pri_nombreArchivo", SqlDbType.NVarChar, 100);
            paPri_codigo.Value = pCodigoProducto;
            paPri_nombreArchivo.Value = pNombreArchivo;

            try
            {
                Conn.Open();
                cmdComandoInicio.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static bool ActualizarProductosImagenAnchoAlto(string pri_codigo, int pri_ancho_ampliar, int pri_alto_ampliar)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("spActualizarProductosImagenAnchoAlto", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paPri_codigo = cmdComandoInicio.Parameters.Add("@pri_codigo", SqlDbType.NVarChar, 50);
            SqlParameter paPri_ancho_ampliar = cmdComandoInicio.Parameters.Add("@pri_ancho_ampliar", SqlDbType.Int);
            SqlParameter paPri_alto_ampliar = cmdComandoInicio.Parameters.Add("@pri_alto_ampliar", SqlDbType.Int);
            paPri_codigo.Value = pri_codigo;
            paPri_ancho_ampliar.Value = pri_ancho_ampliar;
            paPri_alto_ampliar.Value = pri_alto_ampliar;
            try
            {
                Conn.Open();
                cmdComandoInicio.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static bool BorrarAnchoAltoImagen()
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("spBorrarAnchoAltoImagen", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;
            try
            {
                Conn.Open();
                cmdComandoInicio.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
        public static bool ElimimarProductoImagenPorId(string pCodigoProducto)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("dbo.spElimimarProductoImagenPorId", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;
            SqlParameter paPri_codigo = cmdComandoInicio.Parameters.Add("@pri_codigo", SqlDbType.NVarChar, 50);
            paPri_codigo.Value = pCodigoProducto;
            try
            {
                Conn.Open();
                cmdComandoInicio.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }

        public static DataTable DescargaMedicamentosYAccesoriosNoIncluidosEnAlfaBeta()
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Productos.spMedicamentosYAccesoriosNoIncluidosEnAlfaBeta", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;
            try
            {
                Conn.Open();
                DataTable table = new DataTable();
                table.Load(cmdComandoInicio.ExecuteReader());
                return table;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }
            }
        }
    }
}