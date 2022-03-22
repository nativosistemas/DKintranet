using DKbase.web.capaDatos;
using DKintranet.Codigo.capaDatos;
using DKintranet.Codigo.clases;
using DKintranet.Codigo.clases.Generales;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DKintranet.servicios
{
    public partial class generar_comprobantes_discriminado : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rutaTemporal = Constantes.cRaizArchivos + @"\archivos\comprobantes\";

            DirectoryInfo DIR = new DirectoryInfo(rutaTemporal);
            if (!DIR.Exists)
            {
                DIR.Create();
            }
            string nombreTXT = GrabarComprobantesDiscriminadoCSV(rutaTemporal);
            if (nombreTXT != string.Empty)
            {
                String path = Constantes.cRaizArchivos + @"/archivos/comprobantes/" + nombreTXT;

                System.IO.FileInfo toDownload = new System.IO.FileInfo(path);

                if (toDownload.Exists)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
                    Response.AddHeader("Content-Length", toDownload.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.WriteFile(Constantes.cRaizArchivos + @"/archivos/comprobantes/" + nombreTXT);
                    Response.End();
                }
            }
        }
        public string GrabarComprobantesDiscriminadoCSV(string pRuta)
        {
            string resultado = string.Empty;

            if (HttpContext.Current.Session["comprobantescompleto_Lista"] != null && HttpContext.Current.Session["comprobantescompleto_FechaDesde"] != null && HttpContext.Current.Session["comprobantescompleto_FechaHasta"] != null && HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                List<ServiceReferenceDLL.cComprobantesDiscriminadosDePuntoDeVenta> resultadoObjLista = (List<ServiceReferenceDLL.cComprobantesDiscriminadosDePuntoDeVenta>)HttpContext.Current.Session["comprobantescompleto_Lista"];
                string nombreArchivoCSV = string.Empty;
                DateTime fechaDesde = (DateTime)HttpContext.Current.Session["comprobantescompleto_FechaDesde"];
                DateTime fechaHasta = (DateTime)HttpContext.Current.Session["comprobantescompleto_FechaHasta"];
                string fechaArchivoCSV = fechaDesde.Year.ToString().Substring(2, 2) + fechaDesde.Month.ToString("00") + fechaDesde.Day.ToString("00") + "A" + fechaHasta.Year.ToString().Substring(2, 2) + fechaHasta.Month.ToString("00") + fechaHasta.Day.ToString("00");
                nombreArchivoCSV = ((cClientes)(HttpContext.Current.Session["clientesDefault_Cliente"])).cli_login + "-Comprobantes" + fechaArchivoCSV + ".csv";
                //romanello-Comprobantes130901A130904.csv
                resultado = nombreArchivoCSV;
                System.IO.StreamWriter FAC_txt = new System.IO.StreamWriter(pRuta + nombreArchivoCSV, false, System.Text.Encoding.UTF8);

                string strCabeceraCSV = string.Empty;

                strCabeceraCSV += "Fecha";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Comprobante";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Número Comprobante";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Monto Exento";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Monto Gravado";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Monto Iva Inscripto";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Monto Iva No Inscripto";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Monto Percepciones DGR";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Monto Percepciones Municipal";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Monto Total";
                FAC_txt.WriteLine(strCabeceraCSV);
                for (int i = 0; i < resultadoObjLista.Count; i++)
                {
                    string detalleCSV = string.Empty;
                    detalleCSV += resultadoObjLista[i].FechaToString;
                    detalleCSV += ";";
                    detalleCSV += resultadoObjLista[i].Comprobante;
                    detalleCSV += ";";
                    detalleCSV += resultadoObjLista[i].NumeroComprobante;
                    detalleCSV += ";";
                    detalleCSV += Numerica.FormatoNumeroPuntoMilesComaDecimal(resultadoObjLista[i].MontoExento);
                    detalleCSV += ";";
                    detalleCSV += Numerica.FormatoNumeroPuntoMilesComaDecimal(resultadoObjLista[i].MontoGravado);
                    detalleCSV += ";";
                    detalleCSV += Numerica.FormatoNumeroPuntoMilesComaDecimal(resultadoObjLista[i].MontoIvaInscripto);
                    detalleCSV += ";";
                    detalleCSV += Numerica.FormatoNumeroPuntoMilesComaDecimal(resultadoObjLista[i].MontoIvaNoInscripto);
                    detalleCSV += ";";
                    detalleCSV += Numerica.FormatoNumeroPuntoMilesComaDecimal(resultadoObjLista[i].MontoPercepcionesDGR);
                    detalleCSV += ";";
                    detalleCSV += Numerica.FormatoNumeroPuntoMilesComaDecimal(resultadoObjLista[i].MontoPercepcionesMunicipal);
                    detalleCSV += ";";
                    detalleCSV += Numerica.FormatoNumeroPuntoMilesComaDecimal(resultadoObjLista[i].MontoTotal);
                    FAC_txt.WriteLine(detalleCSV);
                }
                FAC_txt.Close();

            }

            return resultado;
        }
    }
}