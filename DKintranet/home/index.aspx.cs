using DKbase.web;
using DKbase.web.capaDatos;
using DKintranet.Codigo.capaDatos;
using DKintranet.Codigo.clases;
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
    public partial class index : cHomePage //System.Web.UI.Page //
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Session["homeBodyCss"] = "bd_home";
            /////
            string isCerrarSesion = Request.QueryString["c"];
            if (isCerrarSesion != string.Empty)
            {
                if (Session["clientesDefault_Usuario"] != null)
                {
                    Seguridad.CerrarSession(((Usuario)Session["clientesDefault_Usuario"]).idUsuarioLog);
                }
                Session["clientesDefault_Usuario"] = null;
                Session["clientesDefault_Cliente"] = null;
                Session["clientesDefault_CantListaMensaje"] = null;
                Session["clientesDefault_CantListaMensajeFechaHora"] = null;
                Session["clientesDefault_CantRecuperadorFalta"] = null;
                Session["clientesDefault_CantRecuperadorFaltaFechaHora"] = null;
            }

            //////
            try
            {
                cCatalogo o = WebService.RecuperarTodosCatalogos().Where(x => x.tbc_publicarHome != null).FirstOrDefault(x => x.tbc_publicarHome.Value);
                if (o != null)
                {
                    cArchivo oArchivo = WebService.RecuperarTodosArchivos(o.tbc_codigo, Constantes.cTABLA_CATALOGO, string.Empty).FirstOrDefault();
                    if (oArchivo != null)
                    {
                        Panel_Revista.Visible = true;
                        //HttpContext.Current.Session["homeIndex_Revista"] = "<a  class=\"pdf\" target =\"_blank\" href =\"../../" + Constantes.cArchivo_Raiz + "/" + Constantes.cTABLA_CATALOGO + "/" + oArchivo.arc_nombre + "\" >DESCARGAR</a>";
                        HttpContext.Current.Session["homeIndex_Revista"] = "<a  class=\"pdf\" target =\"_blank\" href =\"../../" + "servicios/descargarArchivo.aspx?t=" + Constantes.cTABLA_CATALOGO + "&n=" + oArchivo.arc_nombre + "&inline=yes" + "\" >DESCARGAR</a>";
                    }

                }
            }
            catch (Exception ex)
            {
                var lll = ex.Message;
                //throw;
            }
            Response.Redirect("~/config/LoginIntranet");
        }
        [WebMethod(EnableSession = true)]
        public static string RecuperarTodasOfertas(List<cOfertaHome> pResultado, bool isNuevoLanzamiento = false)
        {
            List<cOfertaHome> resultado = pResultado;
            if (isNuevoLanzamiento)
            {
                resultado =  resultado.Where(x => x.ofh_orden > 4).ToList();
              
            }
            else
            {
                resultado =  resultado.Where(x => x.ofh_orden <= 4).ToList();
            }
            return resultado == null ? string.Empty : Serializador.SerializarAJson(resultado);
        }
        public void htmlPublicarRevista()
        {
            if (HttpContext.Current.Session["homeIndex_Revista"] != null)
            {
                Response.Write(HttpContext.Current.Session["homeIndex_Revista"]);
            }
        }
        public static string RecuperarTodasHomeSlide()
        {

            List<cHomeSlide> resultado = WebService.RecuperarTodasHomeSlidePublicar();
            return resultado == null ? string.Empty : Serializador.SerializarAJson(resultado);

        }
        [WebMethod(EnableSession = true)]
        public static string ContadorHomeSlideRating(int hsr_idHomeSlide)
        {
            WebService.ContadorHomeSlideRating(hsr_idHomeSlide, null);
            return "Ok";
        }
        public void AgregarHtmlOculto()
        {
            string resultado = string.Empty;
            List<cOfertaHome> resultado_lista_OfertaHome = WebService.RecuperarTodasOfertaParaHome();
            if (resultado_lista_OfertaHome == null) {
                resultado_lista_OfertaHome = new List<cOfertaHome>();
            }
            resultado += "<input type=\"hidden\" id=\"hiddenListaOfertas\" value=\"" + Server.HtmlEncode(RecuperarTodasOfertas(resultado_lista_OfertaHome,false)) + "\" />";
            resultado += "<input type=\"hidden\" id=\"hiddenListaLanzamiento\" value=\"" + Server.HtmlEncode(RecuperarTodasOfertas(resultado_lista_OfertaHome,true)) + "\" />";
            resultado += "<input type=\"hidden\" id=\"hiddenListaSlider\" value=\"" + Server.HtmlEncode(RecuperarTodasHomeSlide()) + "\" />";
            List<Codigo.tbl_Recall> l = WebService.RecuperarTodaReCall();
            if (l != null)
            {
                l = l.Where(x => x.rec_visible.HasValue && x.rec_visible.Value).OrderByDescending(x => x.rec_FechaNoticia.HasValue ? x.rec_FechaNoticia : x.rec_Fecha).Take(3).ToList();
                resultado += "<input type=\"hidden\" id=\"hiddenListaReCallIndex\" value=\"" + Server.HtmlEncode(Serializador.SerializarAJson(l)) + "\" />";
            }

            Response.Write(resultado);
        }
    }
}