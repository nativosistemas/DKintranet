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
    public partial class LaboratorioEditar : cBaseAdmin
    {
        public const string consPalabraClave = "gestionapp";
        protected void Page_Load(object sender, EventArgs e)
        {
            Seguridad(consPalabraClave);
            if (Request.QueryString.AllKeys.Contains("id"))
            {
                HttpContext.Current.Session["LaboratorioEditar_id"] = Request.QueryString.Get("id");
                DKbase.Entities.Laboratorio obj = WebService.GetLaboratorios().FirstOrDefault(x => x.id == Convert.ToInt32(HttpContext.Current.Session["LaboratorioEditar_id"]));
                HttpContext.Current.Session["LaboratorioEditar_obj"] = obj;
            }
        }
        [WebMethod(EnableSession = true)]
        public static string AddUpdate(string nombre)
        {
            if (HttpContext.Current.Session["LaboratorioEditar_id"] != null)
            {
               int resultado = WebService.AddUpdateLaboratorios(Convert.ToInt32(HttpContext.Current.Session["LaboratorioEditar_id"]), nombre);
            }
            return "";
        }
        public void AgregarHtmlOculto()
        {
            string resultado = string.Empty;
            if (HttpContext.Current.Session["LaboratorioEditar_id"] != null)
                resultado += "<input type=\"hidden\" id=\"hidden_idLaboratorio\" value=\"" + Server.HtmlEncode(HttpContext.Current.Session["LaboratorioEditar_id"].ToString()) + "\" />";
            if (HttpContext.Current.Session["LaboratorioEditar_obj"] != null)
            {
                DKbase.Entities.Laboratorio o = (DKbase.Entities.Laboratorio)HttpContext.Current.Session["LaboratorioEditar_obj"];

                resultado += "<input type=\"hidden\" id=\"hidden_nombre\" value=\"" + Server.HtmlEncode(o.nombre) + "\" />";

            }
            Response.Write(resultado);
        }
    }
}