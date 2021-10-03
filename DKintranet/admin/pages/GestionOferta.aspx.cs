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
    public partial class GestionOferta : cBaseAdmin
    {
        public const string consPalabraClave = "gestionhome";
        protected void Page_Load(object sender, EventArgs e)
        {
            Seguridad(consPalabraClave);
            if (!Request.QueryString.AllKeys.Contains("isVolver"))
            {

                if (Request.QueryString.AllKeys.Contains("isNuevoLanzamiento"))
                {
                    HttpContext.Current.Session["GestionOferta_isNuevoLanzamiento"] = true;
                }
                else
                {
                    HttpContext.Current.Session["GestionOferta_isNuevoLanzamiento"] = false;
                }
            }
            if (!IsPostBack)
            {

            }
        }
        [WebMethod(EnableSession = true)]
        public static string RecuperarTodasOfertas()
        {
            bool isNuevoLanzamiento = false;
            if (HttpContext.Current.Session["GestionOferta_isNuevoLanzamiento"] != null)
            {
                isNuevoLanzamiento = Convert.ToBoolean(HttpContext.Current.Session["GestionOferta_isNuevoLanzamiento"]);
            }
            List<cOferta> resultado = WebService.RecuperarTodasOfertas(isNuevoLanzamiento);
            return resultado == null ? string.Empty : DKintranet.Codigo.clases.Generales.Serializador.SerializarAJson(resultado);
        }
        [WebMethod(EnableSession = true)]
        public static bool ElimimarOfertaPorId(int pValue)
        {
            bool? resultado = WebService.ElimimarOfertaPorId(pValue);
            return true;
        }
        [WebMethod(EnableSession = true)]
        public static bool CambiarEstadoPublicarOferta(int pValue)
        {
            bool? resultado = WebService.CambiarEstadoPublicarOferta(pValue);
            return true;
        }
        public void AgregarHtmlOculto()
        {
            string resultado = string.Empty;
            int isNuevoLanzamiento = 0;
            if (HttpContext.Current.Session["GestionOferta_isNuevoLanzamiento"] != null)
            {
                isNuevoLanzamiento = Convert.ToBoolean(HttpContext.Current.Session["GestionOferta_isNuevoLanzamiento"]) ? 1 : 0; ;
            }
            resultado += "<input type=\"hidden\" id=\"hiddenOfe_isNuevoLanzamiento\" value=\"" + Server.HtmlEncode(isNuevoLanzamiento.ToString()) + "\" />";
            Response.Write(resultado);
        }
    }
}