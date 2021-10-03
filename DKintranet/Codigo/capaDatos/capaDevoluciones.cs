using DKintranet.Codigo.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace DKintranet.Codigo.capaDatos
{
    public class cDevolucionItemPrecarga
    {
        public int dev_numeroitem { get; set; }
        public int dev_numerocliente { get; set; }
        public string dev_numerofactura { get; set; }
        public string dev_numerosolicituddevolucion { get; set; }
        public string dev_nombreproductodevolucion { get; set; }
        public DateTime dev_fecha { get; set; }
        public string dev_fechaToString { get; set; }
        public int dev_motivo { get; set; }
        public int dev_numeroitemfactura { get; set; }
        public string dev_nombreproductofactura { get; set; }
        public int dev_cantidad { get; set; }
        public string dev_numerolote { get; set; }
        public DateTime dev_fechavencimientolote { get; set; }
        public string dev_fechavencimientoloteToString { get; set; }
        public string dev_estado { get; set; }
        public string dev_mensaje { get; set; }
        public int dev_cantidadrecibida { get; set; }
        public int dev_cantidadrechazada { get; set; }
        public string dev_idsucursal { get; set; }
    }

    public class capaDevoluciones
    {
        public static DataTable RecuperarItemsDevolucionPrecargaPorCliente(int pIdCliente)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Devoluciones.spRecuperarItemsDevolucionesPrecargaPorCliente", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@numerocliente", SqlDbType.Int);
            paIdCliente.Value = pIdCliente;

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

        public static DataTable RecuperarItemsDevolucionPrecargaVencidosPorCliente(int pIdCliente)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Devoluciones.spRecuperarItemsDevolucionesPrecargaVencidosPorCliente", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@numerocliente", SqlDbType.Int);
            paIdCliente.Value = pIdCliente;

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

        public static DataTable RecuperarItemsDevolucionPrecargaFacturaCompletaPorCliente(int pIdCliente)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Devoluciones.spRecuperarItemsDevolucionesPrecargaFacturaCompletaPorCliente", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@numerocliente", SqlDbType.Int);
            paIdCliente.Value = pIdCliente;

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



        public static DataTable RecuperarItemsReclamoFacturadoNoEnviado(int pIdCliente)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Devoluciones.spRecuperarItemsReclamoFacturadoNoEnviado", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@numerocliente", SqlDbType.Int);
            paIdCliente.Value = pIdCliente;

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
        
        public static bool AgregarDevolucionItemPrecarga(cDevolucionItemPrecarga Item)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Devoluciones.spAgregarItemDevolucion", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paNumeroCliente = cmdComandoInicio.Parameters.Add("@numerocliente", SqlDbType.Int);
            SqlParameter paNumeroFactura = cmdComandoInicio.Parameters.Add("@numerofactura", SqlDbType.NVarChar,13);
            SqlParameter paNombreProductoDevolucion = cmdComandoInicio.Parameters.Add("@nombreproductodevolucion", SqlDbType.NVarChar,75);
            SqlParameter paMotivo = cmdComandoInicio.Parameters.Add("@motivo", SqlDbType.Int);
            SqlParameter paNumeroItemFactura = cmdComandoInicio.Parameters.Add("@numeroitemfactura", SqlDbType.Int);
            SqlParameter paNombreProductoFactura = cmdComandoInicio.Parameters.Add("@nombreproductofactura", SqlDbType.NVarChar,75);
            SqlParameter paCantidad = cmdComandoInicio.Parameters.Add("@cantidad", SqlDbType.Int);
            SqlParameter paNumeroLote = cmdComandoInicio.Parameters.Add("@numerolote", SqlDbType.NVarChar,75);
            SqlParameter paFechaVencimmientoLote = cmdComandoInicio.Parameters.Add("@fechavencimientolote", SqlDbType.NVarChar,10);
            SqlParameter paIdSucursal = cmdComandoInicio.Parameters.Add("@idsucursal", SqlDbType.NVarChar,2);
            SqlParameter paOK = cmdComandoInicio.Parameters.Add("@isOk", SqlDbType.Bit);
            paOK.Direction = ParameterDirection.Output;

            paNumeroCliente.Value = Item.dev_numerocliente;
            if (Item.dev_numerofactura == null)
            {
                paNumeroFactura.Value = DBNull.Value;
            } else
            {
                paNumeroFactura.Value = Item.dev_numerofactura;
            }
            paNombreProductoDevolucion.Value = Item.dev_nombreproductodevolucion;
            paMotivo.Value = Item.dev_motivo;
            if (Item.dev_numeroitemfactura == 0)
            {
                paNumeroItemFactura.Value = DBNull.Value;
            } else
            {
                paNumeroItemFactura.Value = Item.dev_numeroitemfactura;
            }

            if (Item.dev_nombreproductofactura == null)
            {
                paNombreProductoFactura.Value = DBNull.Value;
            }
            else
            {
                paNombreProductoFactura.Value = Item.dev_nombreproductofactura;
            }

            paCantidad.Value = Item.dev_cantidad;

            if (Item.dev_idsucursal == null)
            {
                paIdSucursal.Value = DBNull.Value;
            }
            else
            {
                paIdSucursal.Value = Item.dev_idsucursal;
            }

            if (Item.dev_numerolote == null)
            {
                paNumeroLote.Value = DBNull.Value;
            }
            else
            {
                paNumeroLote.Value = Item.dev_numerolote;
            }

            if (Item.dev_fechavencimientoloteToString == null)
            {
                paFechaVencimmientoLote.Value = DBNull.Value;
            }
            else
            {
                paFechaVencimmientoLote.Value = Item.dev_fechavencimientoloteToString;
            }
            //paOK.Value = false;

            try
            {
                Conn.Open();
                //object objResultado = cmdComandoInicio.ExecuteNonQuery();
                cmdComandoInicio.ExecuteNonQuery();
                return Convert.ToBoolean(paOK.Value);
            }
            catch (Exception ex)
            {
                //throw ex;
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
        
        public static bool AgregarReclamoFacturadoNoEnviadoItemPrecarga(cDevolucionItemPrecarga Item)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Devoluciones.spAgregarItemReclamoFacturadoNoEnviado", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paNumeroCliente = cmdComandoInicio.Parameters.Add("@numerocliente", SqlDbType.Int);
            SqlParameter paNumeroFactura = cmdComandoInicio.Parameters.Add("@numerofactura", SqlDbType.NVarChar, 13);
            SqlParameter paNombreProducto = cmdComandoInicio.Parameters.Add("@nombreproducto", SqlDbType.NVarChar, 75);
            SqlParameter paCantidad = cmdComandoInicio.Parameters.Add("@cantidad", SqlDbType.Int);
            SqlParameter paIdSucursal = cmdComandoInicio.Parameters.Add("@idsucursal", SqlDbType.NVarChar, 2);
            SqlParameter paOK = cmdComandoInicio.Parameters.Add("@isOk", SqlDbType.Bit);
            paOK.Direction = ParameterDirection.Output;

            paNumeroCliente.Value = Item.dev_numerocliente;
            if (Item.dev_numerofactura == null)
            {
                paNumeroFactura.Value = DBNull.Value;
            }
            else
            {
                paNumeroFactura.Value = Item.dev_numerofactura;
            }
            paNombreProducto.Value = Item.dev_nombreproductofactura;
            paCantidad.Value = Item.dev_cantidad;

            if (Item.dev_idsucursal == null)
            {
                paIdSucursal.Value = DBNull.Value;
            }
            else
            {
                paIdSucursal.Value = Item.dev_idsucursal;
            }
            //paOK.Value = false;

            try
            {
                Conn.Open();
                //object objResultado = cmdComandoInicio.ExecuteNonQuery();
                cmdComandoInicio.ExecuteNonQuery();
                return Convert.ToBoolean(paOK.Value);
            }
            catch (Exception ex)
            {
                //throw ex;
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
        
        public static bool EliminarDevolucionItemPrecarga(int NumeroItem)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Devoluciones.spElimminarItemDevolucionPrecarga", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paNumeroItem = cmdComandoInicio.Parameters.Add("@NumeroItem", SqlDbType.Int);
            SqlParameter paOK = cmdComandoInicio.Parameters.Add("@isOk", SqlDbType.Bit);
            paOK.Direction = ParameterDirection.Output;

            paNumeroItem.Value = NumeroItem;
            //paOK.Value = false;

            try
            {
                Conn.Open();
                //object objResultado = cmdComandoInicio.ExecuteNonQuery();
                cmdComandoInicio.ExecuteNonQuery();
                return Convert.ToBoolean(paOK.Value);
            }
            catch (Exception ex)
            {
                //throw ex;
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
        
        public static bool ElimminarItemReclamoFNEPrecarga(int NumeroItem)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Devoluciones.spElimminarItemReclamoFNEPrecarga", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paNumeroItem = cmdComandoInicio.Parameters.Add("@NumeroItem", SqlDbType.Int);
            SqlParameter paOK = cmdComandoInicio.Parameters.Add("@isOk", SqlDbType.Bit);
            paOK.Direction = ParameterDirection.Output;

            paNumeroItem.Value = NumeroItem;
            //paOK.Value = false;

            try
            {
                Conn.Open();
                //object objResultado = cmdComandoInicio.ExecuteNonQuery();
                cmdComandoInicio.ExecuteNonQuery();
                return Convert.ToBoolean(paOK.Value);
            }
            catch (Exception ex)
            {
                //throw ex;
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

        public static bool EliminarPrecargaDevolucionPorCliente(int NumeroCliente)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Devoluciones.spEliminarPrecargaDevolucionPorCliente", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paNumeroCliente = cmdComandoInicio.Parameters.Add("@NumeroCliente", SqlDbType.Int);
            SqlParameter paOK = cmdComandoInicio.Parameters.Add("@isOk", SqlDbType.Bit);
            paOK.Direction = ParameterDirection.Output;

            paNumeroCliente.Value = NumeroCliente;
            //paOK.Value = false;

            try
            {
                Conn.Open();
                //object objResultado = cmdComandoInicio.ExecuteNonQuery();
                cmdComandoInicio.ExecuteNonQuery();
                return Convert.ToBoolean(paOK.Value);
            }
            catch (Exception ex)
            {
                //throw ex;
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

        public static bool EliminarPrecargaDevolucionVencidosPorCliente(int NumeroCliente)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Devoluciones.spEliminarPrecargaDevolucionVencidosPorCliente", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paNumeroCliente = cmdComandoInicio.Parameters.Add("@NumeroCliente", SqlDbType.Int);
            SqlParameter paOK = cmdComandoInicio.Parameters.Add("@isOk", SqlDbType.Bit);
            paOK.Direction = ParameterDirection.Output;

            paNumeroCliente.Value = NumeroCliente;
            //paOK.Value = false;

            try
            {
                Conn.Open();
                //object objResultado = cmdComandoInicio.ExecuteNonQuery();
                cmdComandoInicio.ExecuteNonQuery();
                return Convert.ToBoolean(paOK.Value);
            }
            catch (Exception ex)
            {
                //throw ex;
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

        public static bool EliminarPrecargaDevolucionFacturaCompletaPorCliente(int NumeroCliente)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Devoluciones.spEliminarPrecargaDevolucionFacturaCompletaPorCliente", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paNumeroCliente = cmdComandoInicio.Parameters.Add("@NumeroCliente", SqlDbType.Int);
            SqlParameter paOK = cmdComandoInicio.Parameters.Add("@isOk", SqlDbType.Bit);
            paOK.Direction = ParameterDirection.Output;

            paNumeroCliente.Value = NumeroCliente;
            //paOK.Value = false;

            try
            {
                Conn.Open();
                //object objResultado = cmdComandoInicio.ExecuteNonQuery();
                cmdComandoInicio.ExecuteNonQuery();
                return Convert.ToBoolean(paOK.Value);
            }
            catch (Exception ex)
            {
                //throw ex;
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
        
        public static bool EliminarPrecargaReclamoFNEPorCliente(int NumeroCliente)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Devoluciones.spEliminarPrecargaReclamoFNEPorCliente", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paNumeroCliente = cmdComandoInicio.Parameters.Add("@NumeroCliente", SqlDbType.Int);
            SqlParameter paOK = cmdComandoInicio.Parameters.Add("@isOk", SqlDbType.Bit);
            paOK.Direction = ParameterDirection.Output;

            paNumeroCliente.Value = NumeroCliente;
            //paOK.Value = false;

            try
            {
                Conn.Open();
                //object objResultado = cmdComandoInicio.ExecuteNonQuery();
                cmdComandoInicio.ExecuteNonQuery();
                return Convert.ToBoolean(paOK.Value);
            }
            catch (Exception ex)
            {
                //throw ex;
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
    }
}