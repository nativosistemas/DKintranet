using DKbase.web.capaDatos;
using DKintranet.Codigo.capaDatos;
using DKintranet.Codigo.clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKintranet.Filters
{
    public class AuthorizePermisoAttribute : AuthorizeAttribute
    {
        public string Permiso { get; set; }
        public bool isCheckEstado { get; set; }
        public bool isCheckDLL { get; set; }
        public bool isCheckOPERADORCLIENTE { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            if (httpContext == null)
                throw new ArgumentNullException("httpContext");
            try
            {
                bool resultado = false;
                object area = HttpContext.Current.Request.RequestContext.RouteData.DataTokens["area"];
                object user = null;
                if (area != null && area.ToString().ToLower().Equals("admin"))
                {
                    user = HttpContext.Current.Session["UsrAdmin"];
                    HttpContext.Current.Session["clientesDefault_Usuario"] = null;
                }
                else
                {
                    user = HttpContext.Current.Session["clientesDefault_Usuario"];
                    HttpContext.Current.Session["UsrAdmin"] = null;
                }
                if (user != null)
                {


                    if (Permiso == Constantes.cSECCION_CUENTASCORRIENTES ||
                        Permiso == Constantes.cSECCION_PEDIDOS ||
                        Permiso == Constantes.cSECCION_DESCARGAS)
                        return false;
                    if (isCheckEstado || isCheckDLL || isCheckOPERADORCLIENTE)
                        return false;
                    return true;
                }

                return resultado;
            }
            catch (Exception)
            {
                return false;
            }
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            object area = filterContext.RouteData.DataTokens["area"];
            object user = null;
            if (area != null && area.ToString().ToLower().Equals("admin"))
            {
                user = HttpContext.Current.Session["UsrAdmin"];
                HttpContext.Current.Session["clientesDefault_Usuario"] = null;
            }
            else
            {
                user = HttpContext.Current.Session["clientesDefault_Usuario"];
                HttpContext.Current.Session["UsrAdmin"] = null;
            }

            if (user != null)
            {
                if (Permiso == Constantes.cSECCION_CUENTASCORRIENTES)
                {
                    if (!FuncionesPersonalizadas.IsPermisoSeccion(Constantes.cSECCION_CUENTASCORRIENTES))
                    {
                        // Response.Redirect("~/clientes/pages/sinpermiso.aspx");
                        filterContext.Result = new RedirectResult("/config/sinpermiso");
                        return;
                    }
                    else
                    {
                        if (!cPageClientes.IsBanderaUsarDll())
                        {
                            // Response.Redirect("~/clientes/pages/sinusodll.aspx");
                            filterContext.Result = new RedirectResult("/config/sinusodll");
                            return;
                        }
                    }
                }
                else if (Permiso == Constantes.cSECCION_DESCARGAS &&
                    !FuncionesPersonalizadas.IsPermisoSeccion(Constantes.cSECCION_DESCARGAS))
                {
                    filterContext.Result = new RedirectResult("/config/sinpermiso");
                    return;
                }
                else if (Permiso == Constantes.cSECCION_PEDIDOS &&
                     !FuncionesPersonalizadas.IsPermisoSeccion(Constantes.cSECCION_PEDIDOS))
                {
                    filterContext.Result = new RedirectResult("/config/sinpermiso");
                    return;
                }
                if (isCheckEstado && HttpContext.Current.Session["clientesDefault_Cliente"] != null)
                {
                    cClientes objCliente = WebService.RecuperarClientePorId(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo);
                    if (objCliente != null)
                    {
                        ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_estado = objCliente.cli_estado;
                    }
                    if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_estado.ToUpper() == Constantes.cESTADO_INH)
                    {
                        filterContext.Result = new RedirectResult("/config/inhabilitado");
                        return;
                    }
                }
                if (isCheckDLL && !cPageClientes.IsBanderaUsarDll())
                {
                    filterContext.Result = new RedirectResult("/config/sinusodll");
                    return;
                }
                if (isCheckOPERADORCLIENTE && HttpContext.Current.Session["clientesDefault_Usuario"] != null
                    && ((Usuario)HttpContext.Current.Session["clientesDefault_Usuario"]).idRol == Constantes.cROL_OPERADORCLIENTE)
                {
                    filterContext.Result = new RedirectResult("/config/sinpermiso");
                    return;
                }
            }
            else
            {
                string url = "";
                if (area == null)
                    url = "/mvc/Index";
                else
                    url = "/" + area.ToString() + "/mvc/Index";
                filterContext.Result = new RedirectResult(url);
            }
        }
        public class ErrorActionResult : ActionResult
        {
            public string ErrorMessage { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Response.Write(this.ErrorMessage);
            }
        }

    }
}