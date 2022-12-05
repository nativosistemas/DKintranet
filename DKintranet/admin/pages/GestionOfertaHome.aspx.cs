using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using DKintranet.Codigo.clases;
using DKintranet.Codigo.capaDatos;
using DKbase.web.capaDatos;

namespace DKintranet.admin.pages
{
    public partial class GestionOfertaHome : cBaseAdmin
    {
        public const string consPalabraClave = "gestionhome";
        public static string strHtmlOptionOferta = null;
        public static string strHtmlOptionLanzamiento = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            Seguridad(consPalabraClave);
            if (!IsPostBack)
            {
                strHtmlOptionOferta = null;
                strHtmlOptionLanzamiento = null;
            }

        }
        public static string getHtmlOptionOferta()
        {
            return getHtmlOptionOferta_generico(false);
        }
        public static string getHtmlOptionLanzamiento()
        {
            return getHtmlOptionOferta_generico(true);
        }
        public static string getHtmlOptionOferta_generico(bool isNuevosLanzamiento)
        {

            if ((isNuevosLanzamiento && strHtmlOptionLanzamiento == null) || (!isNuevosLanzamiento && strHtmlOptionOferta == null))
            {
                string result = string.Empty;
                List<cOferta> l = WebService.RecuperarTodasOfertas(isNuevosLanzamiento);
                if (l != null)
                {
                    result += "<option value='-1'>((Sin Seleccionar))</option>";
                    for (int i = 0; i < l.Count; i++)
                    {
                        result += "<option value='" + l[i].ofe_idOferta.ToString() + "'>" + l[i].ofe_titulo + "</option>";
                    }
                }
                if (isNuevosLanzamiento)
                {
                    strHtmlOptionLanzamiento = result;
                }            
                else
                {
                    strHtmlOptionOferta = result;
                }
            }
            if (isNuevosLanzamiento)
            {
                return strHtmlOptionLanzamiento;
            }
            else
            {
                return strHtmlOptionOferta;
            }
           

        }
        [WebMethod(EnableSession = true)]
        public static string InsertarActualizarOfertaHome(int p1, int p2, int p3, int p4, int p5, int p6, int p7, int p8)
        {
            if (HttpContext.Current.Session["GestionOfertaHome_listOfertaHome"] != null)
            {
                List<cOfertaHome> l = WebService.RecuperarTodasOfertaHome();
                cOfertaHome o1 = l.FirstOrDefault(x => x.ofh_orden == 1);
                if (o1 == null && p1 > 0)
                    WebService.InsertarActualizarOfertaHome(0, 1, p1);
                else if (o1 != null && p1 != o1.ofh_idOferta && p1 > 0)
                    WebService.InsertarActualizarOfertaHome(o1.ofh_idOfertaHome, 1, p1);
                else if (p1 == -1 && o1 != null)
                    WebService.EliminarOfertaHome(o1.ofh_idOfertaHome);
                cOfertaHome o2 = l.FirstOrDefault(x => x.ofh_orden == 2);
                if (o2 == null && p2 > 0)
                    WebService.InsertarActualizarOfertaHome(0, 2, p2);
                else if (o2 != null && p2 != o2.ofh_idOferta && p2 > 0)
                    WebService.InsertarActualizarOfertaHome(o2.ofh_idOfertaHome, 2, p2);
                else if (p2 == -1 && o2 != null)
                    WebService.EliminarOfertaHome(o2.ofh_idOfertaHome);
                cOfertaHome o3 = l.FirstOrDefault(x => x.ofh_orden == 3);
                if (o3 == null && p3 > 0)
                    WebService.InsertarActualizarOfertaHome(0, 3, p3);
                else if (o3 != null && p3 != o3.ofh_idOferta && p3 > 0)
                    WebService.InsertarActualizarOfertaHome(o3.ofh_idOfertaHome, 3, p3);
                else if (p3 == -1 && o3 != null)
                    WebService.EliminarOfertaHome(o3.ofh_idOfertaHome);
                cOfertaHome o4 = l.FirstOrDefault(x => x.ofh_orden == 4);
                if (o4 == null && p4 > 0)
                    WebService.InsertarActualizarOfertaHome(0, 4, p4);
                else if (o4 != null && p4 != o4.ofh_idOferta && p4 > 0)
                    WebService.InsertarActualizarOfertaHome(o4.ofh_idOfertaHome, 4, p4);
                else if (p4 == -1 && o4 != null)
                    WebService.EliminarOfertaHome(o4.ofh_idOfertaHome);
                //
                cOfertaHome o5 = l.FirstOrDefault(x => x.ofh_orden == 5);
                if (o5 == null && p5 > 0)
                    WebService.InsertarActualizarOfertaHome(0, 5, p5);
                else if (o5 != null && p5 != o5.ofh_idOferta && p5 > 0)
                    WebService.InsertarActualizarOfertaHome(o5.ofh_idOfertaHome, 5, p5);
                else if (p5 == -1 && o5 != null)
                    WebService.EliminarOfertaHome(o5.ofh_idOfertaHome);

                //
                cOfertaHome o6 = l.FirstOrDefault(x => x.ofh_orden == 6);
                if (o6 == null && p6 > 0)
                    WebService.InsertarActualizarOfertaHome(0, 6, p6);
                else if (o6 != null && p6 != o6.ofh_idOferta && p6 > 0)
                    WebService.InsertarActualizarOfertaHome(o6.ofh_idOfertaHome, 6, p6);
                else if (p6 == -1 && o6 != null)
                    WebService.EliminarOfertaHome(o6.ofh_idOfertaHome);
                //
                cOfertaHome o7 = l.FirstOrDefault(x => x.ofh_orden == 7);
                if (o7 == null && p7 > 0)
                    WebService.InsertarActualizarOfertaHome(0, 7, p7);
                else if (o7 != null && p7 != o7.ofh_idOferta && p7 > 0)
                    WebService.InsertarActualizarOfertaHome(o7.ofh_idOfertaHome, 7, p7);
                else if (p7 == -1 && o7 != null)
                    WebService.EliminarOfertaHome(o7.ofh_idOfertaHome);
                //
                cOfertaHome o8 = l.FirstOrDefault(x => x.ofh_orden == 8);
                if (o8 == null && p8 > 0)
                    WebService.InsertarActualizarOfertaHome(0, 8, p8);
                else if (o8 != null && p8 != o8.ofh_idOferta && p8 > 0)
                    WebService.InsertarActualizarOfertaHome(o8.ofh_idOfertaHome, 8, p8);
                else if (p8 == -1 && o8 != null)
                    WebService.EliminarOfertaHome(o8.ofh_idOfertaHome);

            }
            return "";
        }
        public void AgregarHtmlOculto()
        {
            string resultado = string.Empty;
            var l = WebService.RecuperarTodasOfertaHome();
            if (l != null)
            {
                HttpContext.Current.Session["GestionOfertaHome_listOfertaHome"] = l;
                resultado += "<input type=\"hidden\" id=\"hiddenListOfeta\" value=\"" + Server.HtmlEncode(DKintranet.Codigo.clases.Generales.Serializador.SerializarAJson(l)) + "\" />";
            }

            Response.Write(resultado);
        }
    }
}