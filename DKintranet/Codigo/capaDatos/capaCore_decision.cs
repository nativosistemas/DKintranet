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
        public static ServiceReferenceDLL.cDllPedido TomarPedidoConIdCarrito(int pIdCarrito, string pLoginCliente, string pIdSucursal, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio, List<ServiceReferenceDLL.cDllProductosAndCantidad> pListaProducto, bool pIsUrgente)
        {
            try
            {
                capaCAR.InicioCarritoEnProceso(pIdCarrito, Constantes.cAccionCarrito_TOMAR);
                if (isCore)
                {
                    List<DKbase.dll.cDllProductosAndCantidad> l_Productos = Codigo.clases.Generales.Serializador.DeserializarJson<List<DKbase.dll.cDllProductosAndCantidad>>(Codigo.clases.Generales.Serializador.SerializarAJson(pListaProducto));
                    var t = Task.Run(() => capaCore_WebService.TomarPedidoConIdCarritoAsync(pIdCarrito, pLoginCliente, pIdSucursal, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, l_Productos, pIsUrgente));
                    t.Wait();
                    if (t.Result == null)
                    {
                        throw new Exception("Result == null");
                    }
                    DKbase.dll.cDllPedido objResult = t.Result;
                    return Codigo.clases.Generales.Serializador.DeserializarJson<ServiceReferenceDLL.cDllPedido>(Codigo.clases.Generales.Serializador.SerializarAJson(objResult));
                }
                else
                {
                    return capaWebServiceDLL.TomarPedidoConIdCarrito(pIdCarrito, pLoginCliente, pIdSucursal, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, pListaProducto, pIsUrgente);
                }
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
    }
}