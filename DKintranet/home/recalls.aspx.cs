using DKintranet.Codigo.clases.Generales;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DKintranet.home
{
    public partial class recalls : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Session["homeBodyCss"] = "bd_sec";
        }
        public void AgregarHtmlOculto()
        {
            string resultado = string.Empty;
            List<Codigo.tbl_Recall> l = WebService.RecuperarTodaReCall();
            if (l != null)
            {
                l = l.Where(x => x.rec_visible.HasValue && x.rec_visible.Value).OrderByDescending(x => x.rec_FechaNoticia.HasValue ? x.rec_FechaNoticia : x.rec_Fecha).ToList();
                resultado += "<input type=\"hidden\" id=\"hiddenListaReCall\" value=\"" + Server.HtmlEncode(Serializador.SerializarAJson(l)) + "\" />";
            }

            Response.Write(resultado);
        }
    }
}