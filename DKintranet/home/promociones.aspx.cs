using DKbase.web.capaDatos;
using DKintranet.Codigo.capaDatos;
using DKintranet.Codigo.clases.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DKintranet.home
{
    public partial class promociones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.AllKeys.Contains("isNuevoLanzamiento"))
            {
                HttpContext.Current.Session["promociones_isNuevoLanzamiento"] = true;
            }
            else
            {
                HttpContext.Current.Session["promociones_isNuevoLanzamiento"] = false;
            }
            HttpContext.Current.Session["homeBodyCss"] = "bd_sec";
        }
        [WebMethod(EnableSession = true)]
        public static string RecuperarTodasOfertas()
        {
            bool isNuevoLanzamiento = false;
            if (HttpContext.Current.Session["promociones_isNuevoLanzamiento"] != null)
            {
                isNuevoLanzamiento = Convert.ToBoolean(HttpContext.Current.Session["promociones_isNuevoLanzamiento"]);
            }
            List<cOferta> resultado = WebService.RecuperarTodasOfertaPublicar(isNuevoLanzamiento);
            return resultado == null ? string.Empty : Serializador.SerializarAJson(resultado);
        }
        public void AgregarHtmlOculto()
        {
            string resultado = string.Empty;
            resultado += "<input type=\"hidden\" id=\"hiddenListaOfertas\" value=\"" + Server.HtmlEncode(RecuperarTodasOfertas()) + "\" />";
            Response.Write(resultado);
        }

    }
}