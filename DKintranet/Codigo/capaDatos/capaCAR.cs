using DKbase.web;
using DKbase.web.capaDatos;
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
    public class capaCAR
    {
        public static void guardarPedido(cCarrito pCarrito, string pTipo, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio, bool pIsUrgente)
        {
            int usu_codCliente = (int)((Usuario)HttpContext.Current.Session["clientesDefault_Usuario"]).usu_codCliente;
            DKbase.web.capaDatos.capaCAR_base.guardarPedido(usu_codCliente, pCarrito, pTipo, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, pIsUrgente);
        }
        public static void guardarPedido(List<cProductosGenerico> pListaProductos, int car_id, string codSucursal, string pTipo, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio, bool pIsUrgente)
        {
            int usu_codCliente = (int)((Usuario)HttpContext.Current.Session["clientesDefault_Usuario"]).usu_codCliente;
            DKbase.web.capaDatos.capaCAR_base.guardarPedido(usu_codCliente, pListaProductos, car_id, codSucursal, pTipo, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, pIsUrgente);
        }
    }
}