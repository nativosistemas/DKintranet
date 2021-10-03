using DKintranet.Codigo.clases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DKintranet.servicios
{
    public partial class descargarArchivo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tipo = Request.QueryString["t"];
            string name = Request.QueryString["n"];
            string Content_Disposition = string.Empty;
            if (!string.IsNullOrWhiteSpace(Request.QueryString["inline"]))
            {
                Content_Disposition = "inline";
            }
            if (!string.IsNullOrEmpty(tipo) && !string.IsNullOrEmpty(name))
            {
                string nombreArchivo = name;
                String path = Constantes.cRaizArchivos + @"/archivos/" + tipo + @"/" + nombreArchivo;

                System.IO.FileInfo toDownload = new System.IO.FileInfo(path);
                if (toDownload.Exists)
                {
                    Response.Clear();
                    if (string.IsNullOrWhiteSpace(Content_Disposition))
                    {
                        Content_Disposition = "attachment; filename=" + toDownload.Name;
                    }
                    Response.AddHeader("Content-Disposition", Content_Disposition);
                    Response.AddHeader("Content-Length", toDownload.Length.ToString());
                    string contentType_aux = MimeMapping.GetMimeMapping(path);
                    Response.ContentType = contentType_aux;// Constantes.cMIME_pdf;
                
                    try
                    {
                        Response.WriteFile(path);
                    }
                    catch (Exception ex)
                    {
                        //Thread.Sleep(1000);
                        //Response.WriteFile(Constantes.cArchivo_ImpresionesComprobante + nombrePDF);
                    }

                    Response.End();
                }
            }
        }
    }
}