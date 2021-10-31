using DKintranet.Codigo.capaDatos;
using DKintranet.Codigo.clases;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace DKintranet.Codigo.capaDatos
{
    public class capaCAR_decision
    {
        public static bool isCAR = true;
        public static bool isIntranet = true;
        public static bool isDiferido_CAR()
        {
            return true;
        }
        public static int idCliente_Actual()
        {
            int idCliente = 0;
            if (isIntranet && HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                DKintranet.Codigo.capaDatos.cClientes cliente = (DKintranet.Codigo.capaDatos.cClientes)HttpContext.Current.Session["clientesDefault_Cliente"];
                idCliente = cliente.cli_codigo;
            }
            else if (HttpContext.Current.Session["clientesDefault_Usuario"] != null)
            {
                idCliente = (int)((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]).usu_codCliente;
            }
            return idCliente;
        }
        public static int BorrarCarrito(int lrc_id, string lrc_codSucursal)
        {
            int idCliente = idCliente_Actual();
            if (isIntranet)
            {
                return capaCAR_intranet.BorrarCarrito(idCliente, lrc_codSucursal, Constantes.cTipo_Carrito, Constantes.cAccionCarrito_VACIAR);
            }
            else
            {
                return capaCAR_WebService.BorrarCarrito(idCliente, lrc_codSucursal, Constantes.cAccionCarrito_VACIAR);
            }
        }
        public static int BorrarCarritosDiferidos(int lrc_id, string lrc_codSucursal)
        {
            int idCliente = idCliente_Actual();
            if (isIntranet)
            {
                return capaCAR_intranet.BorrarCarrito(idCliente, lrc_codSucursal, Constantes.cTipo_CarritoDiferido, Constantes.cAccionCarrito_VACIAR);
            }
            else
            {
                return capaCAR_WebService.BorrarCarritosDiferidos(idCliente, lrc_codSucursal, Constantes.cAccionCarrito_VACIAR);
            }
        }
        public static int BorrarCarritoTransfer(string pSucursal)
        {
            int idCliente = idCliente_Actual();
            if (isIntranet)
            {
                return capaCAR_intranet.BorrarCarrito(idCliente, pSucursal, Constantes.cTipo_CarritoTransfers, Constantes.cAccionCarrito_VACIAR);
            }
            else
            {
                return capaCAR_WebService.BorrarCarritoTransfer(idCliente, pSucursal, Constantes.cAccionCarrito_VACIAR);
            }
        }
        public static int BorrarCarritoTransferDiferido(string pSucursal)
        {
            int idCliente = idCliente_Actual();
            if (isIntranet)
            {
                return capaCAR_intranet.BorrarCarrito(idCliente, pSucursal, Constantes.cTipo_CarritoDiferidoTransfers, Constantes.cAccionCarrito_VACIAR);
            }
            else
            {
                return capaCAR.BorrarCarrito(idCliente, pSucursal, Constantes.cTipo_CarritoDiferidoTransfers, Constantes.cAccionCarrito_VACIAR);
            }
        }
        public static void GuardarPedidoBorrarCarrito(List<cProductosGenerico> pListaProductos, int car_id, string pSucursal, string pTipo, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio, bool pIsUrgente)
        {
            capaCAR.BorrarCarritoPorId_SleepTimer(car_id, Constantes.cAccionCarrito_TOMAR);
            capaCAR.guardarPedido(pListaProductos, car_id, pSucursal, pTipo, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, pIsUrgente);
        }
        public static void guardarPedido_base_decision(string strXML, int car_id, string codSucursal, string pTipo, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio, bool pIsUrgente)
        {
            if (isIntranet)
            {
                capaCAR_intranet.guardarPedido_base(strXML, car_id, codSucursal, pTipo, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, pIsUrgente);
            }
            else
            {
                capaCAR.guardarPedido_base(strXML, car_id, codSucursal, pTipo, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, pIsUrgente);
            }
        }
        public static void GuardarPedidoBorrarCarrito(cCarrito pCarrito, string pTipo, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio, bool pIsUrgente)
        {
            capaCAR.BorrarCarritoPorId_SleepTimer(pCarrito.car_id, Constantes.cAccionCarrito_TOMAR);
            capaCAR.guardarPedido(pCarrito, pTipo, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, pIsUrgente);
        }
        public static cSucursalCarritoTransfer RecuperarCarritosTransferPorIdClienteIdSucursal(cClientes pCliente, string pCodSucursal, string pTipo)
        {
            if (isIntranet)
            {
                return capaCAR_intranet.RecuperarCarritosTransferPorIdClienteOrdenadosPorSucursal(pCliente, pTipo).Where(x => x.Sucursal == pCodSucursal).FirstOrDefault();
            }
            else
            {
                return capaCAR_WebService.RecuperarCarritosTransferPorIdClienteOrdenadosPorSucursal(pCliente, pTipo).Where(x => x.Sucursal == pCodSucursal).FirstOrDefault();
            }
        }
        public static bool AgregarProductosTransfersAlCarrito(List<cProductosAndCantidad> pListaProductosMasCantidad, int pIdCliente, int pIdUsuario, int pIdTransfers, string pCodSucursal, string pTipo)
        {
            if (isIntranet)
            {
                return capaCAR_intranet.AgregarProductosTransfersAlCarrito(pListaProductosMasCantidad, pIdCliente, pIdUsuario, pIdTransfers, pCodSucursal, pTipo);
            }
            else
            {
                if (WebService.VerificarPermisos(WebService.CredencialAutenticacion))
                {
                    DataTable pTablaDetalle = FuncionesPersonalizadas.ConvertProductosAndCantidadToDataTable(pListaProductosMasCantidad);
                    return capaCAR.AgregarProductosTransfersAlCarrito(pTablaDetalle, pIdCliente, pIdUsuario, pIdTransfers, pCodSucursal, pTipo);
                }
                return false;
            }
        }
        public static bool CargarCarritoDiferido(string pSucursal, string pIdProducto, int pCantidadProducto, int pIdCliente, int? pIdUsuario)
        {
            if (isIntranet)
            {
                return capaCAR_intranet.CargarCarritoDiferido(pSucursal, pIdProducto, pCantidadProducto, pIdCliente, pIdUsuario);
            }
            else
            {
                return capaCAR.CargarCarritoDiferido(pSucursal, pIdProducto, pCantidadProducto, pIdCliente, pIdUsuario);
            }
        }
        public static bool AgregarProductoAlCarrito(string pSucursal, string pIdProducto, int pCantidadProducto, int pIdCliente, int? pIdUsuario)
        {
            if (isIntranet)
            {
                return capaCAR_intranet.AgregarProductoAlCarrito(pSucursal, pIdProducto, pCantidadProducto, pIdCliente, pIdUsuario);
            }
            else
            {
                return capaCAR.AgregarProductoAlCarrito(pSucursal, pIdProducto, pCantidadProducto, pIdCliente, pIdUsuario);
            }
        }
        public static List<cCarrito> RecuperarCarritosDiferidosPorCliente(int pIdCliente)
        {
            return capaCAR_WebService.RecuperarCarritosDiferidosPorCliente(pIdCliente);
        }
        public static List<cCarrito> RecuperarCarritosPorSucursalYProductos(int pIdCliente)
        {
            return capaCAR_WebService.RecuperarCarritosPorSucursalYProductos(pIdCliente);
        }
        public static DataSet RecuperarCarritosPorSucursalYProductos_decision(int pIdCliente)
        {
            if (isIntranet)
            {
                return capaCAR_intranet.RecuperarCarritosPorSucursalYProductos(pIdCliente);
            }
            else
            {
                return capaCAR.RecuperarCarritosPorSucursalYProductos(pIdCliente);
            }
        }
        public static DataSet RecuperarCarritosDiferidosPorCliente_decision(int pIdCliente)
        {
            if (isIntranet)
            {
                return capaCAR_intranet.RecuperarCarritosDiferidosPorCliente(pIdCliente);
            }
            else
            {
                return capaCAR.RecuperarCarritosDiferidosPorCliente(pIdCliente);
            }
        }

        public static List<cSucursalCarritoTransfer> RecuperarCarritosTransferPorIdClienteOrdenadosPorSucursal(cClientes pCliente, string pTipo)
        {
            if (isIntranet)
            {
                return capaCAR_intranet.RecuperarCarritosTransferPorIdClienteOrdenadosPorSucursal(pCliente, pTipo);
            }
            else
            {
                return capaCAR_WebService.RecuperarCarritosTransferPorIdClienteOrdenadosPorSucursal(pCliente, pTipo);
            }
        }
        public static List<cCarritoTransfer> RecuperarCarritosTransferPorIdCliente(cClientes pCliente, string pTipo, string pIdSucursal)
        {
            cSucursalCarritoTransfer o = null;
            if (isIntranet)
            {
                o = capaCAR_intranet.RecuperarCarritosTransferPorIdClienteOrdenadosPorSucursal(pCliente, pTipo).Where(x => x.Sucursal == pIdSucursal).FirstOrDefault();
            }
            else
            {
                o = capaCAR_WebService.RecuperarCarritosTransferPorIdClienteOrdenadosPorSucursal(pCliente, pTipo).Where(x => x.Sucursal == pIdSucursal).FirstOrDefault();
            }
            return o == null ? null : o.listaTransfer;
        }
        public static bool ActualizarProductoCarritoSubirArchivo(List<cProductosAndCantidad> pListaValor)
        {
            int idCliente = idCliente_Actual();
            return capaCAR_WebService.ActualizarProductoCarritoSubirArchivo(pListaValor, idCliente, ((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]).id);
        }
        public static bool AgregarProductosDelRecuperardorAlCarrito(string pSucursal, string[] pArrayNombreProducto, int[] pArrayCantidad, bool[] pArrayOferta)
        {
            bool resultado = false;
            if (System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] != null && System.Web.HttpContext.Current.Session["clientes_pages_Recuperador_Tipo"] != null && System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null && System.Web.HttpContext.Current.Session["clientes_pages_Recuperador_CantidadDia"] != null)
            {
                int idCliente = idCliente_Actual();
                List<cProductosAndCantidad> listaAUX = new List<cProductosAndCantidad>();
                for (int i = 0; i < pArrayNombreProducto.Count(); i++)
                {
                    cProductosAndCantidad obj = new cProductosAndCantidad();
                    obj.cantidad = pArrayCantidad[i];
                    obj.codProductoNombre = pArrayNombreProducto[i];
                    listaAUX.Add(obj);
                }
                List<cProductosGenerico> listaProductos = WebService.RecuperarTodosProductosDesdeTabla(listaAUX, ((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc, ((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codprov, ((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo);
                if (isCAR)
                {
                    List<cProductosAndCantidad> listaProductos_capaCAR = new List<cProductosAndCantidad>();
                    for (int i = 0; i < pArrayNombreProducto.Count(); i++)
                    {
                        cProductosGenerico objProducto = listaProductos.Where(x => x.pro_nombre == pArrayNombreProducto[i]).First();
                        int cantidad = pArrayCantidad[i];
                        int cantidadAgregar = 0;

                        List<int> cantidadBD = WebService.RecuperarCantidadProductoEnCarritoYCarritoTransferFacturacionDirecta(((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo, pSucursal, objProducto.pro_codigo, objProducto.pro_nombre);
                        int cantidadTotalMasBD = cantidad;
                        if (cantidadBD != null)
                        {
                            cantidadTotalMasBD += cantidadBD[0] + cantidadBD[1];
                        }
                        List<int> listaCantidades = FuncionesPersonalizadas.CargarProductoCantidadDependiendoTransfer(objProducto, cantidadTotalMasBD);

                        cProductosAndCantidad objProductosAndCantidad = new cProductosAndCantidad();
                        objProductosAndCantidad.codProductoNombre = objProducto.pro_nombre;
                        objProductosAndCantidad.codProducto = objProducto.pro_codigo;
                        objProductosAndCantidad.codSucursal = pSucursal;
                        objProductosAndCantidad.cantidad = listaCantidades[0];
                        objProductosAndCantidad.isTransferFacturacionDirecta = false;
                        listaProductos_capaCAR.Add(objProductosAndCantidad);



                        cProductosAndCantidad objProductosAndCantidad_transfer = new cProductosAndCantidad();
                        objProductosAndCantidad_transfer.codProductoNombre = objProducto.pro_nombre;
                        objProductosAndCantidad_transfer.codProducto = objProducto.pro_codigo;
                        objProductosAndCantidad_transfer.codSucursal = pSucursal;
                        objProductosAndCantidad_transfer.cantidad = listaCantidades[1];
                        objProductosAndCantidad_transfer.isTransferFacturacionDirecta = true;
                        objProductosAndCantidad_transfer.tde_codtfr = objProducto.tde_codtfr;
                        listaProductos_capaCAR.Add(objProductosAndCantidad_transfer);

                        WebService.BorrarPorProductosFaltasProblemasCrediticiosV2(pSucursal, idCliente, Convert.ToInt32(System.Web.HttpContext.Current.Session["clientes_pages_Recuperador_Tipo"]), objProducto.pro_nombre, Convert.ToInt32(System.Web.HttpContext.Current.Session["clientes_pages_Recuperador_CantidadDia"]), cantidadAgregar);
                    }
                    return ActualizarProductoCarritoSubirArchivo(listaProductos_capaCAR);
                }
                else
                {
                    for (int i = 0; i < pArrayNombreProducto.Count(); i++)
                    {
                        cProductosGenerico objProducto = listaProductos.Where(x => x.pro_nombre == pArrayNombreProducto[i]).First();
                        int cantidad = pArrayCantidad[i];
                        int cantidadAgregar = 0;

                        List<int> cantidadBD = WebService.RecuperarCantidadProductoEnCarritoYCarritoTransferFacturacionDirecta(((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo, pSucursal, objProducto.pro_codigo, objProducto.pro_nombre);
                        int cantidadTotalMasBD = cantidad;
                        if (cantidadBD != null)
                        {
                            cantidadTotalMasBD += cantidadBD[0] + cantidadBD[1];
                        }
                        List<int> listaCantidades = FuncionesPersonalizadas.CargarProductoCantidadDependiendoTransfer(objProducto, cantidadTotalMasBD);

                        // Carrito comun
                        WebService.AgregarProductoAlCarritoDesdeRecuperador(pSucursal, objProducto.pro_nombre, listaCantidades[0], idCliente, ((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]).id);

                        List<cProductosAndCantidad> tempListaProductos = new List<cProductosAndCantidad>();
                        cProductosAndCantidad objProductosAndCantidad = new cProductosAndCantidad();
                        objProductosAndCantidad.codProductoNombre = objProducto.pro_nombre;
                        objProductosAndCantidad.cantidad = listaCantidades[1];
                        tempListaProductos.Add(objProductosAndCantidad);
                        capaCAR_decision.AgregarProductosTransfersAlCarrito(tempListaProductos, idCliente, ((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]).id, objProducto.tde_codtfr, pSucursal, Constantes.cTipo_CarritoTransfers);

                        WebService.BorrarPorProductosFaltasProblemasCrediticiosV2(pSucursal, idCliente, Convert.ToInt32(System.Web.HttpContext.Current.Session["clientes_pages_Recuperador_Tipo"]), objProducto.pro_nombre, Convert.ToInt32(System.Web.HttpContext.Current.Session["clientes_pages_Recuperador_CantidadDia"]), cantidadAgregar);
                    }
                    resultado = true;
                }
            }
            return resultado;
        }
    }

}
