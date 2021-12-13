using DKintranet.Codigo.clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace DKintranet.Codigo.capaDatos
{
    public class capaCore_decision
    {
        public static bool _isCore = false;
        public static bool isCore
        {
            get
            {
                return _isCore;
            }
        }
        public static DKbase.dll.cDllPedido TomarPedidoTelefonistaAsync(int pIdCarrito, string pLoginCliente, string pIdSucursal, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio, List<DKbase.dll.cDllProductosAndCantidad> pListaProducto, bool pIsUrgente)
        {
            try
            {
                string pLoginTelefonista = string.Empty;
                if (System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] != null)
                {
                    Usuario usuario = ((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]);
                    pLoginTelefonista = usuario.usu_login;
                }
                capaCAR.InicioCarritoEnProceso(pIdCarrito, Constantes.cAccionCarrito_TOMAR);
                var t = Task.Run(() => capaCore_WebService.TomarPedidoTelefonistaAsync(pIdCarrito, pLoginCliente, pIdSucursal, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, pListaProducto, pLoginTelefonista));
                t.Wait();
                if (t.Result == null)
                {
                    throw new Exception("Result == null");
                }
                DKbase.dll.cDllPedido objResult = t.Result;
                return objResult;
            }
            catch (Exception ex)
            {
                FuncionesPersonalizadas.grabarLog(MethodBase.GetCurrentMethod(), ex, DateTime.Now, pIdCarrito, pLoginCliente, pIdSucursal, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, pListaProducto, pIsUrgente);
                return null;
            }
            finally
            {
                capaCAR.EndCarritoEnProceso(pIdCarrito);
            }
        }
        public static List<DKbase.dll.cDllPedidoTransfer> TomarPedidoDeTransfersTelefonistaAsync(int pIdCarrito, string pLoginCliente, string pIdSucursal, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio, List<DKbase.dll.cDllProductosAndCantidad> pListaProducto)
        {
            try
            {
                string pLoginTelefonista = string.Empty;
                if (System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] != null)
                {
                    Usuario usuario = ((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]);
                    pLoginTelefonista = usuario.usu_login;
                }
                capaCAR.InicioCarritoEnProceso(pIdCarrito, Constantes.cAccionCarrito_TOMAR);
                var t = Task.Run(() => capaCore_WebService.TomarPedidoDeTransfersTelefonistaAsync(pIdCarrito, pLoginCliente, pIdSucursal, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, pListaProducto, pLoginTelefonista));
                t.Wait();
                if (t.Result == null)
                {
                    throw new Exception("Result == null");
                }
                List<DKbase.dll.cDllPedidoTransfer> objResult = t.Result;
                return objResult;
            }
            catch (Exception ex)
            {
                FuncionesPersonalizadas.grabarLog(MethodBase.GetCurrentMethod(), ex, DateTime.Now, pIdCarrito, pLoginCliente, pIdSucursal, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, pListaProducto, pListaProducto);
                return null;
            }
            finally
            {
                capaCAR.EndCarritoEnProceso(pIdCarrito);
            }
        }

        public static decimal ObtenerCreditoDisponible(string pLoginWeb)
        {
            try
            {
                var t = Task.Run(() => capaCore_WebService.ObtenerCreditoDisponibleAsync(pLoginWeb));
                t.Wait();
                return t.Result;
            }
            catch (Exception ex)
            {
                FuncionesPersonalizadas.grabarLog(MethodBase.GetCurrentMethod(), ex, DateTime.Now, pLoginWeb);
                return 0;
            }
        }

    }
}