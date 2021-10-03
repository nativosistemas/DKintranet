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
    public partial class contacto : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Session["homeBodyCss"] = "bd_sec";
        }
        [WebMethod(EnableSession = true)]
        public static string SendContacto(string txtNombreApellido, string r_social, string txtEmailContacto, string txtAsunto, string txtCuerpo, string g_recaptcha_response)
        {
            string result = string.Empty;
            try
            {
                if (DKintranet.Codigo.clases.Generales.Captcha.ReCaptchaClass.Validate(g_recaptcha_response))
                {
                    string cuerpo = "<b>Nombre y Apellido:  </b>" + txtNombreApellido + "<br/>";
                    cuerpo += "<b>Empresa: </b>" + r_social + "<br />";
                    cuerpo += " <b>Email: </b>" + txtEmailContacto + "<br />";
                    cuerpo += " <b>Consulta: </b>" + txtCuerpo + " <br/>";//<br />
                    string[] split = System.Configuration.ConfigurationManager.AppSettings["mailContacto"].ToString().Split(new Char[] { ';' });
                    List<string> listaMail = split.ToList();
                    String mail_from = System.Configuration.ConfigurationManager.AppSettings["mail_from"].ToString();
                    String mail_pass = System.Configuration.ConfigurationManager.AppSettings["mail_pass"].ToString();
                    MailMessage correo = new System.Net.Mail.MailMessage();
                    string asunto = "Consulta - " + txtAsunto;
                    correo.From = new MailAddress(mail_from);
                    foreach (string itemMail in listaMail)
                        correo.To.Add(itemMail);
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
                }
                else
                    result = "reCAPTCHA invalido, por favor inténtelo de nuevo.";
            }
            catch (Exception ex)
            {
                var dd = ex;
                result = "No pudo ser realizada, intente nuevamente en unos minutos.";
            }
            return result;
        }
    }
}