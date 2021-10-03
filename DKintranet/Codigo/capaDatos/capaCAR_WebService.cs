using DKintranet.Codigo.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace DKintranet.Codigo.capaDatos
{
    public class capaCAR_WebService
    {
        public static List<cCarrito> RecuperarCarritosPorSucursalYProductos(int pIdCliente)
        {
            return RecuperarCarritosPorSucursalYProductos_generica(pIdCliente, Constantes.cTipo_Carrito);
        }
        public static List<cCarrito> RecuperarCarritosPorSucursalYProductos_generica(int pIdCliente, string pTipo)
        {
            if (WebService.VerificarPermisos(WebService.CredencialAutenticacion))
            {
                cClientes objClientes = WebService.RecuperarClientePorId(pIdCliente);
                DataSet dsProductoCarrito = new DataSet();
                if (pTipo == Constantes.cTipo_Carrito)
                {
                    dsProductoCarrito = capaCAR.RecuperarCarritosPorSucursalYProductos(pIdCliente);
                }
                else if (pTipo == Constantes.cTipo_CarritoDiferido)
                {
                    dsProductoCarrito = capaCAR.RecuperarCarritosDiferidosPorCliente(pIdCliente);
                }

                List<cCarrito> listaSucursal = (from item in dsProductoCarrito.Tables[1].AsEnumerable()
                                                select new cCarrito { car_id = item.Field<int>("car_id"), lrc_id = item.Field<int>("car_id"), codSucursal = item.Field<string>("car_codSucursal") }).ToList();

                foreach (cCarrito item in listaSucursal)
                {
                    item.proximoHorarioEntrega = FuncionesPersonalizadas.ObtenerHorarioCierre(objClientes.cli_codsuc, item.codSucursal, objClientes.cli_codrep);
                    List<cProductosGenerico> listaProductoCarrtios = new List<cProductosGenerico>();
                    foreach (DataRow itemProductoCarrtio in dsProductoCarrito.Tables[0].Select("cad_codCarrito = " + item.lrc_id))
                    {
                        cProductos objProducto = WebService.ConvertToProductos(itemProductoCarrtio);
                        cProductosGenerico objProductosGenerico = new cProductosGenerico(objProducto);
                        if (itemProductoCarrtio.Table.Columns.Contains("stk_stock") && itemProductoCarrtio["stk_stock"] != DBNull.Value)
                            objProductosGenerico.stk_stock = itemProductoCarrtio["stk_stock"].ToString(); 
                        if (itemProductoCarrtio.Table.Columns.Contains("cad_codProducto") && itemProductoCarrtio["cad_codProducto"] != DBNull.Value)
                            objProductosGenerico.codProducto = itemProductoCarrtio["cad_codProducto"].ToString(); 
                        if (itemProductoCarrtio.Table.Columns.Contains("cad_cantidad") && itemProductoCarrtio["cad_cantidad"] != DBNull.Value)
                            objProductosGenerico.cantidad = Convert.ToInt32(itemProductoCarrtio["cad_cantidad"]); 
                        listaProductoCarrtios.Add(objProductosGenerico);
                    }
                    /// Nuevo
                    List<cTransferDetalle> listaTransferDetalle = null;
                    if (dsProductoCarrito.Tables.Count > 2)
                    {
                        listaTransferDetalle = new List<cTransferDetalle>();
                        DataTable tablaTransferDetalle = dsProductoCarrito.Tables[2];
                        foreach (DataRow itemTransferDetalle in tablaTransferDetalle.Rows)
                        {
                            cTransferDetalle objTransferDetalle = WebService.ConvertToTransferDetalle(itemTransferDetalle);
                            objTransferDetalle.CargarTransfer(WebService.ConvertToTransfer(itemTransferDetalle));
                            listaTransferDetalle.Add(objTransferDetalle);
                        }
                    }
                    /// FIN Nuevo
                    for (int iPrecioFinal = 0; iPrecioFinal < listaProductoCarrtios.Count; iPrecioFinal++)
                    {
                        listaProductoCarrtios[iPrecioFinal].PrecioFinal = FuncionesPersonalizadas.ObtenerPrecioFinal(objClientes, listaProductoCarrtios[iPrecioFinal]);
                        /// Nuevo
                        listaProductoCarrtios[iPrecioFinal].isProductoFacturacionDirecta = false;
                        if (listaTransferDetalle != null)
                        {
                            List<cTransferDetalle> listaAUXtransferDetalle = listaTransferDetalle.Where(x => x.tde_codpro == listaProductoCarrtios[iPrecioFinal].pro_nombre).ToList();
                            if (listaAUXtransferDetalle.Count > 0)
                            {
                                listaProductoCarrtios[iPrecioFinal].isProductoFacturacionDirecta = true;
                                listaProductoCarrtios[iPrecioFinal].CargarTransferYTransferDetalle(listaAUXtransferDetalle[0]);
                                listaProductoCarrtios[iPrecioFinal].PrecioFinalTransfer = FuncionesPersonalizadas.ObtenerPrecioFinalTransferBase(objClientes, listaProductoCarrtios[iPrecioFinal].tfr_deshab, listaProductoCarrtios[iPrecioFinal].tfr_pordesadi, listaProductoCarrtios[iPrecioFinal].pro_neto, listaProductoCarrtios[iPrecioFinal].pro_codtpopro, listaProductoCarrtios[iPrecioFinal].pro_descuentoweb, listaProductoCarrtios[iPrecioFinal].tde_predescuento == null ? 0 : (decimal)listaProductoCarrtios[iPrecioFinal].tde_predescuento, listaProductoCarrtios[iPrecioFinal].tde_PrecioConDescuentoDirecto, listaProductoCarrtios[iPrecioFinal].tde_PorcARestarDelDtoDeCliente);
                            }
                        }
                        /// FIN Nuevo
                    }
                    item.listaProductos = listaProductoCarrtios;
                }
                listaSucursal.RemoveAll(x => x.listaProductos.Count == 0);
                return listaSucursal;
            }
            // sin no valida la credencial
            return null;
        }
        public static List<cCarrito> RecuperarCarritosDiferidosPorCliente(int pIdCliente)
        {
            return RecuperarCarritosPorSucursalYProductos_generica(pIdCliente, Constantes.cTipo_CarritoDiferido);
        }

        public static int BorrarCarritoTransfer(int pIdCliente, string pSucursal, string pAccion)
        {
            if (WebService.VerificarPermisos(WebService.CredencialAutenticacion))
            {
                return capaCAR.BorrarCarrito(pIdCliente, pSucursal, Constantes.cTipo_CarritoTransfers, pAccion);
            }
            return -1;
        }
        public static int BorrarCarrito(int pIdCliente, string pSucursal, string pAccion)
        {
            if (WebService.VerificarPermisos(WebService.CredencialAutenticacion))
            {
                return capaCAR.BorrarCarrito(pIdCliente, pSucursal, Constantes.cTipo_Carrito, pAccion);
            }
            return -1;
        }
        public static int BorrarCarritosDiferidos(int pIdCliente, string pSucursal, string pAccion)
        {
            if (WebService.VerificarPermisos(WebService.CredencialAutenticacion))
            {
                return capaCAR.BorrarCarrito(pIdCliente, pSucursal, Constantes.cTipo_CarritoDiferido, pAccion);
            }
            return -1;
        }
        public static List<cSucursalCarritoTransfer> RecuperarCarritosTransferPorIdClienteOrdenadosPorSucursal(cClientes pCliente, string pTipo)
        {
            if (WebService.VerificarPermisos(WebService.CredencialAutenticacion))
            {

                DataSet dsProductoCarrito = capaCAR.RecuperarCarritoTransferPorIdCliente(pCliente.cli_codigo, pTipo);
                return WebService.convertDataSetToSucursalCarritoTransfer(pCliente, dsProductoCarrito);
            }
            // sin no valida la credencial
            return null;
        }
        public static bool ActualizarProductoCarritoSubirArchivo(List<cProductosAndCantidad> pListaValor, int pIdCliente, int pIdUsuario)
        {
            string strXML = string.Empty;
            strXML += "<Root>";
            foreach (cProductosAndCantidad item in pListaValor)
            {
                List<XAttribute> listaAtributos = new List<XAttribute>();

                listaAtributos.Add(new XAttribute("lcp_cantidad", item.cantidad));
                listaAtributos.Add(new XAttribute("codigo", item.codProducto));
                listaAtributos.Add(new XAttribute("nombre", item.codProductoNombre));
                listaAtributos.Add(new XAttribute("codTransfer", item.tde_codtfr));
                listaAtributos.Add(new XAttribute("isTransferFacturacionDirecta", item.isTransferFacturacionDirecta));
                listaAtributos.Add(new XAttribute("codSucursal", item.codSucursal));
                XElement nodo = new XElement("DetallePedido", listaAtributos);
                strXML += nodo.ToString();
            }
            strXML += "</Root>";
            return capaCAR.SubirPedido(strXML, pIdCliente, pIdUsuario, Constantes.cTipo_Carrito, Constantes.cTipo_CarritoTransfers);
        }
    }
}