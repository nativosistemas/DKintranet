using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using DKintranet.Codigo.clases;
using DKintranet.Codigo.capaDatos;

namespace DKintranet.admin.pages
{
    public partial class ModuloEditar : cBaseAdmin
    {
        public const string consPalabraClave = "gestionapp";
        protected void Page_Load(object sender, EventArgs e)
        {
            Seguridad(consPalabraClave);
            if (Request.QueryString.AllKeys.Contains("id"))
            {
                HttpContext.Current.Session["ModuloEditar_id"] = Request.QueryString.Get("id");
                DKbase.Entities.Modulo obj = WebService.GetModulos().FirstOrDefault(x => x.id == Convert.ToInt32(HttpContext.Current.Session["ModuloEditar_id"]));
                HttpContext.Current.Session["ModuloEditar_obj"] = obj;
            }
        }
        [WebMethod(EnableSession = true)]
        public static string AddUpdate(string nombre)
        {
            if (HttpContext.Current.Session["ModuloEditar_id"] != null)
            {
                int resultado = WebService.AddUpdateLaboratorios(Convert.ToInt32(HttpContext.Current.Session["ModuloEditar_id"]), nombre);
            }
            return "";
        }
        public void AgregarHtmlOculto()
        {
            string resultado = string.Empty;
            if (HttpContext.Current.Session["ModuloEditar_id"] != null)
                resultado += "<input type=\"hidden\" id=\"hidden_idLaboratorio\" value=\"" + Server.HtmlEncode(HttpContext.Current.Session["ModuloEditar_id"].ToString()) + "\" />";
            if (HttpContext.Current.Session["ModuloEditar_obj"] != null)
            {
                DKbase.Entities.Modulo o = (DKbase.Entities.Modulo)HttpContext.Current.Session["ModuloEditar_obj"];

                resultado += "<input type=\"hidden\" id=\"hidden_nombre\" value=\"" + Server.HtmlEncode(o.descripcion) + "\" />";

            }
            Response.Write(resultado);
        }
    }
}