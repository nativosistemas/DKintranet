using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DKintranet.home
{
    public partial class registracion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Session["homeBodyCss"] = "bd_sec";
        }
        [WebMethod(EnableSession = true)]
        public static string SendRegistracion(bool isEs24, bool isCliente, string txtTitularFarmacia, string txtUsuarioWeb, string txtFechaNacimiento, string txtContacto, string txtNombreFarmacia, string txtEmail, string txtTel, string txtDireccion, string txtCodigoPostal, string txtLocalidad, string txtProvincia, string txtPaginaWeb, string g_recaptcha_response)
        {
            string result = string.Empty;
            HttpContext.Current.Session["registracion_msg"] = null;
            try
            {
                if (DKintranet.Codigo.clases.Generales.Captcha.ReCaptchaClass.Validate(g_recaptcha_response))
                {
                    string cuerpo = "<b>Nombre Titular de la Farmacia: </b>" + txtTitularFarmacia + "<br/>";
                    cuerpo += "<b> ¿Es cliente?: </b>" + (isCliente ? "Si" : "No") + "<br/>";
                    cuerpo += "<b>Usuario para ingresar a la web: </b>" + txtUsuarioWeb + "<br />";
                    cuerpo += " <b>Fecha nacimiento: </b>" + txtFechaNacimiento + "<br />";
                    //cuerpo += " <b>CUIT: </b>" + txtCUIT + "<br />";  
                    cuerpo += " <b>Nombre de Contacto: </b>" + txtContacto + " <br/>";//<br />
                    cuerpo += " <b>Nombre de la farmacia: </b>" + txtNombreFarmacia + "<br/>";
                    cuerpo += "<b> es 24hs: </b>" + (isEs24 ? "Si" : "No") + "<br/>";
                    cuerpo += "<b>E-mail: </b>" + txtEmail + " <br/>";
                    cuerpo += "<b> Dirección: </b>" + txtDireccion + "<br/>";
                    cuerpo += "<b> Localidad: </b>" + txtLocalidad + " <br/>";
                    cuerpo += "<b> Código Postal: </b>" + txtCodigoPostal + "<br/>";
                    cuerpo += "<b> Provincia: </b>" + txtProvincia + " <br/>";
                    cuerpo += " <b> Teléfono: </b>" + txtTel + "<br/>";

                    //cuerpo += " < b> Publicar Email: </b>" + publicarMail + "<br /> ";
                    cuerpo += "<b>Página web: </b>" + txtPaginaWeb + "<br/> ";
                    // cuerpo += " < b> Publicar Página web: </b>" + publicarWeb;

                    string[] split = null;
                    if (isCliente)
                        split = System.Configuration.ConfigurationManager.AppSettings["mailRegistracion"].ToString().Split(new Char[] { ';' });
                    else
                        split = System.Configuration.ConfigurationManager.AppSettings["mailRegistracionNoCliente"].ToString().Split(new Char[] { ';' });

                    List<string> listaMail = split.ToList();
                    //String mail = System.Configuration.ConfigurationManager.AppSettings["mailRegistracion"].ToString();
                    String mail_from = System.Configuration.ConfigurationManager.AppSettings["mail_from"].ToString();
                    String mail_pass = System.Configuration.ConfigurationManager.AppSettings["mail_pass"].ToString();
                    //SmtpClient smtp = new System.Net.Mail.SmtpClient();

                    MailMessage correo = new System.Net.Mail.MailMessage();
                    string asunto = "Registración";
                    correo.From = new MailAddress(mail_from);
                    foreach (string itemMail in listaMail)
                    {
                        correo.To.Add(itemMail);
                    }
                    correo.Subject = asunto;
                    correo.Body = cuerpo;
                    correo.IsBodyHtml = true;
                    correo.Priority = MailPriority.Normal;
                    SmtpClient smtp = new System.Net.Mail.SmtpClient("186.153.136.19", 25);
                    smtp.UseDefaultCredentials = false;
                    smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtp.Credentials = new System.Net.NetworkCredential(mail_from, mail_pass);
                    smtp.Send(correo);
                    result = "Ok";
                    HttpContext.Current.Session["registracion_msg"] = "Ok";
                }
                else
                    result = "reCAPTCHA invalido, por favor inténtelo de nuevo.";
            }
            catch (Exception ex)
            {
                DKintranet.Codigo.clases.FuncionesPersonalizadas.grabarLog(System.Reflection.MethodBase.GetCurrentMethod(), ex, DateTime.Now);
                result = "No pudo ser realizada, intente nuevamente en unos minutos.";

            }
            return result;
        }
        public void AgregarHtmlOculto()
        {
            string resultado = string.Empty;
            if (HttpContext.Current.Session["registracion_msg"] != null)
            {
                resultado += "<input type=\"hidden\" id=\"hiddenRegistracionMsg\" value=\"" + HttpContext.Current.Session["registracion_msg"].ToString() + "\" />";
                HttpContext.Current.Session["registracion_msg"] = null;
            }
            Response.Write(resultado);
        }

    }
}