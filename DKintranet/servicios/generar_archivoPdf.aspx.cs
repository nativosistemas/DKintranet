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
    public partial class generar_archivoPdf : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tipo = Request.QueryString["tipo"];
            string nro = Request.QueryString["nro"];
            if (!string.IsNullOrEmpty(tipo) && !string.IsNullOrEmpty(nro))
            {
                string nombrePDF = tipo + "_" + nro + ".pdf";
                String path = Constantes.cArchivo_ImpresionesComprobante + nombrePDF;

                System.IO.FileInfo toDownload = new System.IO.FileInfo(path);
                if (!toDownload.Exists)
                {
                    WebService.ImprimirComprobante(tipo, nro);
                }
                if (toDownload.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
                    Response.AddHeader("Content-Length", toDownload.Length.ToString());
                    Response.ContentType = Constantes.cMIME_pdf;
                    try
                    {
                        Response.WriteFile(Constantes.cArchivo_ImpresionesComprobante + nombrePDF);
                    }
                    catch (Exception)
                    {
                        Thread.Sleep(1000);
                        Response.WriteFile(Constantes.cArchivo_ImpresionesComprobante + nombrePDF);
                    }

                    Response.End();
                }
            }
        }
    }
}