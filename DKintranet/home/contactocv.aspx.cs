using DKintranet.Codigo.capaDatos;
using DKintranet.Codigo.clases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DKintranet.home
{
    public partial class contactocv : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Session["homeBodyCss"] = "bd_sec";

            if (!IsPostBack)
            {

            }
            else
            {
                string result = string.Empty;
                try
                {
                    if (Request.Form["nombre_cv"] != "" && Request.Form["email_cv"] != "" && Request.Form["dni_cv"] != "" && Request.Form["cuerpo_cv"] != "" && Request.Form["file_cv"] != ""
                        && Request.Form["puesto_cv"] != "" && Request.Form["sucursal_cv"] != "" && Request.Form["g-Recaptcha-Response"] != "")//
                    {
                        string g_recaptcha_response = Request.Form["g-Recaptcha-Response"];
                       if (DKintranet.Codigo.clases.Generales.Captcha.ReCaptchaClass.Validate(g_recaptcha_response))//
                        {
                            HttpPostedFile file = Request.Files["file_cv"];
                            if (file != null && file.ContentLength > 0)
                            {
                                DateTime fechaPresentacion = DateTime.Now;// DateTime.Parse(Request.Form["date_cv"]); //&& Request.Form["date_cv"] != ""
                                int codigoCV = WebService.InsertarCurriculumVitae(Request.Form["nombre_cv"], Request.Form["cuerpo_cv"], Request.Form["email_cv"], Request.Form["dni_cv"], Request.Form["puesto_cv"], Request.Form["sucursal_cv"], fechaPresentacion);
                                string fname = Path.GetFileName(file.FileName);
                                string extencion = capaRecurso.obtenerExtencion(fname);
                              //  string pathDestinoRaiz = @"../" + Constantes.cArchivo_Raiz;
                                string pathDestino  = Constantes.cRaizArchivos + @"\archivos\"  + Constantes.cTABLA_CV + @"/";
                              //  string mapPathDestino = HttpContext.Current.Server.MapPath(pathDestino);
                                if (!Directory.Exists(pathDestino))
                                    Directory.CreateDirectory(pathDestino);
                                string nombreArchivo = capaRecurso.nombreArchivoSinRepetir(pathDestino, fname);
                                string destino = pathDestino + nombreArchivo;
                                file.SaveAs(destino);
                                WebService.InsertarActualizarArchivo(0, codigoCV, Constantes.cTABLA_CV, extencion, file.ContentType, nombreArchivo, string.Empty, string.Empty, string.Empty, Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["codigoUsuarioSinRegistrar"]));
                                result = "Ok";
                                HttpContext.Current.Session["contactocv_result"] = "Ok";
                                //Redirect to clear the form.
                                Response.Redirect(Request.Url.AbsoluteUri);
                            }
                        }
                        else
                            result = "reCAPTCHA invalido, por favor inténtelo de nuevo.";
                    }
                }
                catch (Exception ex)
                {
                    var dd = ex;
                    result = "No pudo ser realizada, intente nuevamente en unos minutos.";
                }
                HttpContext.Current.Session["contactocv_result"] = result;
            }

        }
        public void AgregarHtmlOculto()
        {
            string resultado = string.Empty;
            if (HttpContext.Current.Session["contactocv_result"] != null && !string.IsNullOrEmpty(HttpContext.Current.Session["contactocv_result"].ToString()))
            {
                resultado += "<input type=\"hidden\" id=\"hiddenResultadoCV\" value=\"" + Server.HtmlEncode(HttpContext.Current.Session["contactocv_result"].ToString()) + "\" />";
                HttpContext.Current.Session["contactocv_result"] = null;
            }
            Response.Write(resultado);
        }
    }
}