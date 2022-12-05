using DKbase.web.capaDatos;
using DKintranet.Codigo.capaDatos;
using DKintranet.Codigo.clases.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DKintranet.home
{
    public partial class vistapreviaslider : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.AllKeys.Contains("id"))
            {
                HttpContext.Current.Session["vistapreviaslider_idSlider"] = Request.QueryString.Get("id");
            }

        }
        public static string RecuperarTodasHomeSlide()
        {
            int? id = 0;
            if (HttpContext.Current.Session["vistapreviaslider_idSlider"] != null)
                id = Convert.ToInt32(HttpContext.Current.Session["vistapreviaslider_idSlider"]);
            List<cHomeSlide> resultado = new List<cHomeSlide>();
            cHomeSlide o = WebService.RecuperarHomeSlidePorId(id.Value);
            if (o != null)
                resultado.Add(o);
            return resultado == null ? string.Empty : Serializador.SerializarAJson(resultado);

        }
        public void AgregarHtmlOculto()
        {
            string resultado = string.Empty;
            resultado += "<input type=\"hidden\" id=\"hiddenListaSlider\" value=\"" + Server.HtmlEncode(RecuperarTodasHomeSlide()) + "\" />";
            Response.Write(resultado);
        }

    }
}