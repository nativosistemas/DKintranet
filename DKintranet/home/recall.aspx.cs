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
    public partial class recall : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Session["homeBodyCss"] = "bd_sec";
            if (Request.QueryString.AllKeys.Contains("id"))
            {
                HttpContext.Current.Session["recall_id"] = Request.QueryString.Get("id");
            }
        }
        public void AgregarHtmlOculto()
        {
            if (HttpContext.Current.Session["recall_id"] != null)
            {
                int id = Convert.ToInt32(HttpContext.Current.Session["recall_id"]);
                string resultado = string.Empty;
                List<Codigo.tbl_Recall> l = WebService.RecuperarTodaReCall();
                if (l != null)
                {
                    Codigo.tbl_Recall  o = l.Where(x => x.rec_id == id).FirstOrDefault();
                    List<cArchivo> listaArchivo = WebService.RecuperarTodosArchivos(id, "recallpdf", string.Empty);
                    if (listaArchivo != null)
                    {
                        if (listaArchivo.Count > 0)
                        {
                            o.arc_nombre =  listaArchivo[0].arc_nombre;
                        }
                    }
                   
                    resultado += "<input type=\"hidden\" id=\"hiddenObjReCall\" value=\"" + Server.HtmlEncode(Serializador.SerializarAJson(o)) + "\" />";
                }

                Response.Write(resultado);
            }
        }
    }
}