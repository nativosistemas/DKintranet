﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DKintranet.Codigo.capaDatos
{
    /// <summary>
    /// Summary description for capaTransfer
    /// </summary>
    public class capaTransfer
    {
        public static DataSet RecuperarTodosTransfer()
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Transfers.RecuperarTodosTransfer", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;
            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "Transfers");
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
        public static DataSet RecuperarTodosTransferMasDetallePorIdProducto(string pSucursal, string pNombreProducto,int pIdCliente)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Transfers.spRecuperarTodosTransferMasDetallePorIdProducto", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paTde_codpro = cmdComandoInicio.Parameters.Add("@tde_codpro", SqlDbType.NVarChar, 75);
            SqlParameter paSucursal = cmdComandoInicio.Parameters.Add("@sucursal", SqlDbType.NVarChar, 2);
            SqlParameter paCodCliente = cmdComandoInicio.Parameters.Add("@codCliente", SqlDbType.Int);
            paTde_codpro.Value = pNombreProducto;
            paSucursal.Value = pSucursal;
            paCodCliente.Value = pIdCliente;
            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "Transfers");
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
        public static DataSet RecuperarTransferMasDetallePorIdTransfer(string pSucursal, int pTfr_codigo,int pIdCliente)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("Transfers.spRecuperarTransferMasDetallePorIdTransfer", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paTfr_codigo = cmdComandoInicio.Parameters.Add("@tfr_codigo", SqlDbType.Int);
            SqlParameter paSucursal = cmdComandoInicio.Parameters.Add("@sucursal", SqlDbType.NVarChar, 2);
            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@codCliente", SqlDbType.Int);
            paIdCliente.Value = pIdCliente;
            paTfr_codigo.Value = pTfr_codigo;
            paSucursal.Value = pSucursal;
            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "Transfers");
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
        public static bool AgregarProductosTransfersAlCarrito(DataTable pTablaDetalleProductos, int pIdCliente, int pIdUsuario, int pIdTransfers, string pCodSucursal)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("LogRegistro.spCargarCarritoTransferPorDetalles", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paIdUsuario = cmdComandoInicio.Parameters.Add("@idUsuario", SqlDbType.Int);
            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@idCliente", SqlDbType.Int);
            SqlParameter paTabla_Detalle = cmdComandoInicio.Parameters.Add("@Tabla_Detalle", SqlDbType.Structured);
            SqlParameter paIdTransfer = cmdComandoInicio.Parameters.Add("@idTransfer", SqlDbType.Int);
            SqlParameter paCodSucursal = cmdComandoInicio.Parameters.Add("@ctr_codSucursal", SqlDbType.NVarChar, 2);
            SqlParameter paSumar = cmdComandoInicio.Parameters.Add("@isOk", SqlDbType.Bit);
            paSumar.Direction = ParameterDirection.Output;

            paTabla_Detalle.TypeName = "TransferDetalleTableType";
            paTabla_Detalle.Value = pTablaDetalleProductos;
            paIdUsuario.Value = pIdUsuario;
            paIdCliente.Value = pIdCliente;
            paIdTransfer.Value = pIdTransfers;
            paCodSucursal.Value = pCodSucursal;


            try
            {
                Conn.Open();
                cmdComandoInicio.ExecuteNonQuery();
                return Convert.ToBoolean(paSumar.Value);
                //object objResultado = cmdComandoInicio.ExecuteNonQuery();
                //return true;

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
        public static bool AgregarProductosTransfersAlCarrito_TempSubirArchivo(DataTable pTablaDetalleProductos, int pIdCliente, int pIdUsuario, int pIdTransfers, string pCodSucursal)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("LogRegistro.spCargarCarritoTransferPorDetalles_Temp", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paIdUsuario = cmdComandoInicio.Parameters.Add("@idUsuario", SqlDbType.Int);
            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@idCliente", SqlDbType.Int);
            SqlParameter paTabla_Detalle = cmdComandoInicio.Parameters.Add("@Tabla_Detalle", SqlDbType.Structured);
            SqlParameter paIdTransfer = cmdComandoInicio.Parameters.Add("@idTransfer", SqlDbType.Int);
            SqlParameter paCodSucursal = cmdComandoInicio.Parameters.Add("@ctr_codSucursal", SqlDbType.NVarChar, 2);
            SqlParameter paSumar = cmdComandoInicio.Parameters.Add("@isOk", SqlDbType.Bit);
            paSumar.Direction = ParameterDirection.Output;

            paTabla_Detalle.TypeName = "TransferDetalleTableType";
            paTabla_Detalle.Value = pTablaDetalleProductos;
            paIdUsuario.Value = pIdUsuario;
            paIdCliente.Value = pIdCliente;
            paIdTransfer.Value = pIdTransfers;
            paCodSucursal.Value = pCodSucursal;


            try
            {
                Conn.Open();
                cmdComandoInicio.ExecuteNonQuery();
                return Convert.ToBoolean(paSumar.Value);
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
        public static DataSet RecuperarCarritoTransferPorIdCliente(int pIdCliente)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("LogRegistro.spRecuperarCarritoTransferPorIdCliente", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@lrc_codCliente", SqlDbType.Int);
            paIdCliente.Value = pIdCliente;

            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "Transfers");
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
        public static DataSet RecuperarCarritoTransferPorIdClienteIdSucursal(int pIdCliente, string pSucursal)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("LogRegistro.spRecuperarCarritoTransferPorIdClienteIdSucursal", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paIdCliente = cmdComandoInicio.Parameters.Add("@lrc_codCliente", SqlDbType.Int);
            SqlParameter paSucursal = cmdComandoInicio.Parameters.Add("@lrc_codSucursal", SqlDbType.NVarChar, 2);

            paSucursal.Value = pSucursal;
            paIdCliente.Value = pIdCliente;

            try
            {
                Conn.Open();
                DataSet dsResultado = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(cmdComandoInicio);
                da.Fill(dsResultado, "Transfers");
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
        public static bool BorrarCarritoTransfer(int pIdCliente, string pSucursal)
        {
            SqlConnection Conn = new SqlConnection(accesoBD.ObtenerConexión());
            SqlCommand cmdComandoInicio = new SqlCommand("LogRegistro.spBorrarCarritoTransfer", Conn);
            cmdComandoInicio.CommandType = CommandType.StoredProcedure;

            SqlParameter paCtr_codCliente = cmdComandoInicio.Parameters.Add("@ctr_codCliente", SqlDbType.Int);
            SqlParameter paSucursal = cmdComandoInicio.Parameters.Add("@sucursal", SqlDbType.NVarChar, 2);

            paCtr_codCliente.Value = pIdCliente;
            paSucursal.Value = pSucursal;
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

    }
}