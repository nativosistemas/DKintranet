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
    public partial class vistapreviaoferta : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.AllKeys.Contains("id"))
            {
                HttpContext.Current.Session["vistapreviaoferta_idOferta"] = Request.QueryString.Get("id");

            }
        }
        [WebMethod(EnableSession = true)]
        public static string RecuperarTodasOfertas()
        {
            int? id = 0;
            if (HttpContext.Current.Session["vistapreviaoferta_idOferta"] != null)
                id = Convert.ToInt32(HttpContext.Current.Session["vistapreviaoferta_idOferta"]);

            List<cOferta> resultado = new List<cOferta>();
            cOferta o = WebService.RecuperarTodasOfertas_generico().FirstOrDefault(x => x.ofe_idOferta == id);
            if (o != null)
                resultado.Add(o);
            foreach (var item in resultado)
            {
                List<cArchivo> listaArchivo = WebService.RecuperarTodosArchivos(item.ofe_idOferta, "ofertas", string.Empty);
                if (listaArchivo != null)
                {
                    if (listaArchivo.Count > 0)
                    {
                        item.nameImagen = listaArchivo[0].arc_nombre;
                    }
                }
            }


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