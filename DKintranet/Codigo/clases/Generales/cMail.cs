using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;

namespace DKintranet.Codigo.clases.Generales
{
    /// <summary>
    /// Summary description for cMail
    /// </summary>
    public class cMail
    {
        public cMail()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public static bool enviarMail(string pCorreoMail, string pAsunto, string pCuerpo)
        {
            bool resultado = true;
            try
            {
                //String mail = System.Configuration.ConfigurationManager.AppSettings["mailRegistracion"].ToString();
                String mail_from = System.Configuration.ConfigurationManager.AppSettings["mail_from"].ToString();
                String mail_pass = System.Configuration.ConfigurationManager.AppSettings["mail_pass"].ToString();
                string SMTP_SERVER = System.Configuration.ConfigurationManager.AppSettings["SMTP_SERVER"].ToString();
                int SMTP_PORT = Convert.ToInt32( System.Configuration.ConfigurationManager.AppSettings["SMTP_PORT"].ToString());

                MailMessage correo = new System.Net.Mail.MailMessage();
                string asunto = pAsunto;
                correo.From = new MailAddress(mail_from);
                correo.To.Add(pCorreoMail);
                correo.Subject = asunto;
                correo.Body = pCuerpo;
                correo.IsBodyHtml = true;
                correo.Priority = MailPriority.Normal;


                SmtpClient smtp = new System.Net.Mail.SmtpClient(SMTP_SERVER, SMTP_PORT);

                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Credentials = new System.Net.NetworkCredential(mail_from, mail_pass);
                  // smtp.EnableSsl = true;

                smtp.Send(correo);
            }
            catch (Exception ex)
            {
                resultado = false;
            }
            return resultado;
        }
        //public static bool enviarMail_viejo(string pFrom, string pCorreoMail, string pAsunto, string pCuerpo)
        //{
        //    bool resultado = true;
        //    SmtpClient smtpClient = new SmtpClient();

        //    MailMessage m = new MailMessage(pFrom, // From
        //        pCorreoMail, // To
        //        pAsunto, // Subject
        //       pCuerpo); // Body
        //    m.IsBodyHtml = true;
        //    try
        //    {
        //        smtpClient.Send(m);
        //    }
        //    catch (Exception ex)
        //    {
        //        resultado = false;
        //    }
        //    return resultado;

        //}
    }
}