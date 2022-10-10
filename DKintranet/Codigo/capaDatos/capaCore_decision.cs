using DKbase.web;
using DKbase.web.capaDatos;
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
            if (System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] != null)
            {
                Usuario usuario = ((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]);
                return capaDLL.TomarPedidoTelefonistaAsync(usuario, pIdCarrito, pLoginCliente, pIdSucursal, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, pListaProducto, pIsUrgente);
            }
            return null;
        }
        public static List<DKbase.dll.cDllPedidoTransfer> TomarPedidoDeTransfersTelefonistaAsync(int pIdCarrito, string pLoginCliente, string pIdSucursal, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio, List<DKbase.dll.cDllProductosAndCantidad> pListaProducto)
        {
            if (System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] != null)
            {
                Usuario usuario = ((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]);
                return capaDLL.TomarPedidoDeTransfersTelefonistaAsync(usuario,  pIdCarrito,  pLoginCliente,  pIdSucursal,  pMensajeEnFactura,  pMensajeEnRemito,  pTipoEnvio,  pListaProducto);
            }
            return null;
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