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
    public partial class generarCSV : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string tipo = Request.QueryString["t"];
            string nombreTXT = null;
            if (tipo != null)
            {
                switch (tipo)
                {
                    case "deudaVencida":
                        if (HttpContext.Current.Session["ctacte_deudaVencida"] != null)
                        {
                            List<DKintranet.ServiceReferenceDLL.cCtaCteMovimiento> l = (List<DKintranet.ServiceReferenceDLL.cCtaCteMovimiento>)Session["ctacte_deudaVencida"];
                            nombreTXT = getDeudaVencidaCSV(l, Codigo.clases.Constantes.cDeudaVencida);
                        }
                        break;
                    case "saldoSinImputar":
                        if (HttpContext.Current.Session["ctacte_saldoSinImputar"] != null)
                        {
                            List<DKintranet.ServiceReferenceDLL.cCtaCteMovimiento> l = (List<DKintranet.ServiceReferenceDLL.cCtaCteMovimiento>)Session["ctacte_saldoSinImputar"];
                            nombreTXT = getDeudaVencidaCSV(l,Codigo.clases.Constantes.cSaldoSinImputar);
                        }
                        break;
                    case "ObraSocialEntreFechas":
                        if (HttpContext.Current.Session["ObrasSociales_EntreFechas"] != null)
                        {
                            List<DKintranet.ServiceReferenceDLL.cConsObraSocial> l = (List<DKintranet.ServiceReferenceDLL.cConsObraSocial>)Session["ObrasSociales_EntreFechas"];
                            nombreTXT = getObraSocialEntreFechasCSV(l);
                        }
                        break;
                    case "ConsultaDeComprobantesEntreFecha":
                        if (HttpContext.Current.Session["ConsultaDeComprobantes_ComprobantesEntreFecha"] != null)
                        {
                            List<DKintranet.ServiceReferenceDLL.cComprobanteDiscriminado> l = (List<DKintranet.ServiceReferenceDLL.cComprobanteDiscriminado>)Session["ConsultaDeComprobantes_ComprobantesEntreFecha"];
                            nombreTXT = getConsultaDeComprobantesEntreFechaCSV(l);
                        }
                        break;
                    default:
                        break;
                }

                if (nombreTXT != string.Empty)
                {
                    String path = Constantes.cRaizArchivos + @"/archivos/csv/" + nombreTXT;

                    System.IO.FileInfo toDownload = new System.IO.FileInfo(path);

                    if (toDownload.Exists)
                    {
                        Response.Clear();
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + toDownload.Name);
                        Response.AddHeader("Content-Length", toDownload.Length.ToString());
                        Response.ContentType = "application/octet-stream";
                        Response.WriteFile(Constantes.cRaizArchivos + @"/archivos/csv/" + nombreTXT);
                        Response.End();
                    }
                }
            }
        }
        public string getDeudaVencidaCSV(List<DKintranet.ServiceReferenceDLL.cCtaCteMovimiento> pLista,string pTipo)
        {
            string resultado = string.Empty;

            if (pLista != null && HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                string ruta = Constantes.cRaizArchivos + @"\archivos\csv\";
                DirectoryInfo DIR = new DirectoryInfo(ruta);
                if (!DIR.Exists)
                {
                    DIR.Create();
                }

                string nombreArchivoCSV = string.Empty;
                nombreArchivoCSV = ((cClientes)(HttpContext.Current.Session["clientesDefault_Cliente"])).cli_login + "-" + pTipo + ".csv";
                resultado = nombreArchivoCSV;
                System.IO.StreamWriter FAC_txt = new System.IO.StreamWriter(ruta + nombreArchivoCSV, false, System.Text.Encoding.UTF8);

                string strCabeceraCSV = string.Empty;

                strCabeceraCSV += "Fecha";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Vencimiento";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Comprobante";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Semana";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Importe";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Saldo";
                FAC_txt.WriteLine(strCabeceraCSV);
                for (int i = 0; i < pLista.Count; i++)
                {
                    string detalleCSV = string.Empty;
                    detalleCSV += pLista[i].FechaToString;
                    detalleCSV += ";";
                    detalleCSV += pLista[i].FechaVencimientoToString;
                    detalleCSV += ";";
                    detalleCSV += pLista[i].TipoComprobanteToString + " " + pLista[i].NumeroComprobante;
                    detalleCSV += ";";
                    detalleCSV += pLista[i].Semana;
                    detalleCSV += ";";
                    detalleCSV += pLista[i].Importe != null ? Numerica.FormatoNumeroPuntoMilesComaDecimal(pLista[i].Importe.Value) : "";
                    detalleCSV += ";";
                    detalleCSV += pLista[i].Saldo != null ? Numerica.FormatoNumeroPuntoMilesComaDecimal(pLista[i].Saldo.Value) : "";
                    FAC_txt.WriteLine(detalleCSV);
                }
                FAC_txt.Close();
            }
            return resultado;
        }
        public string getObraSocialEntreFechasCSV(List<DKintranet.ServiceReferenceDLL.cConsObraSocial> pLista)
        {
            string resultado = string.Empty;

            if (pLista != null && HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                string ruta = Constantes.cRaizArchivos + @"\archivos\csv\";
                DirectoryInfo DIR = new DirectoryInfo(ruta);
                if (!DIR.Exists)
                {
                    DIR.Create();
                }

                string nombreArchivoCSV = string.Empty;
                nombreArchivoCSV = ((cClientes)(HttpContext.Current.Session["clientesDefault_Cliente"])).cli_login + "-ObrasSociales" + ".csv";
                resultado = nombreArchivoCSV;
                System.IO.StreamWriter FAC_txt = new System.IO.StreamWriter(ruta + nombreArchivoCSV, false, System.Text.Encoding.UTF8);

                string strCabeceraCSV = string.Empty;

                strCabeceraCSV += "Fecha";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Comprobante";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Detalle";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Importe";
                FAC_txt.WriteLine(strCabeceraCSV);
                for (int i = 0; i < pLista.Count; i++)
                {
                    string detalleCSV = string.Empty;
                    detalleCSV += pLista[i].FechaComprobanteToString;
                    detalleCSV += ";";
                    detalleCSV += pLista[i].TipoComprobante + " " +pLista[i].NumeroComprobante;
                    detalleCSV += ";";
                    detalleCSV += pLista[i].Detalle;
                    detalleCSV += ";";
                    detalleCSV +=  Numerica.FormatoNumeroPuntoMilesComaDecimal(pLista[i].Importe);
                    FAC_txt.WriteLine(detalleCSV);
                }
                FAC_txt.Close();
            }
            return resultado;
        }
        public string getConsultaDeComprobantesEntreFechaCSV(List<DKintranet.ServiceReferenceDLL.cComprobanteDiscriminado> pLista)
        {
            string resultado = string.Empty;

            if (pLista != null && HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                string ruta = Constantes.cRaizArchivos + @"\archivos\csv\";
                DirectoryInfo DIR = new DirectoryInfo(ruta);
                if (!DIR.Exists)
                {
                    DIR.Create();
                }

                string nombreArchivoCSV = string.Empty;
                nombreArchivoCSV = ((cClientes)(HttpContext.Current.Session["clientesDefault_Cliente"])).cli_login + "-ConsultaDeComprobantes" + ".csv";
                resultado = nombreArchivoCSV;
                System.IO.StreamWriter FAC_txt = new System.IO.StreamWriter(ruta + nombreArchivoCSV, false, System.Text.Encoding.UTF8);

                string strCabeceraCSV = string.Empty;

                strCabeceraCSV += "Fecha";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Tipo";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Comprobante";
                strCabeceraCSV += ";";
                strCabeceraCSV += "Importe";
                FAC_txt.WriteLine(strCabeceraCSV);
                for (int i = 0; i < pLista.Count; i++)
                {
                    string detalleCSV = string.Empty;
                    detalleCSV += pLista[i].FechaToString;
                    detalleCSV += ";";
                    detalleCSV += pLista[i].Comprobante ;
                    detalleCSV += ";";
                    detalleCSV += pLista[i].NumeroComprobante;
                    detalleCSV += ";";
                    detalleCSV += Numerica.FormatoNumeroPuntoMilesComaDecimal(pLista[i].MontoTotal);
                    FAC_txt.WriteLine(detalleCSV);
                }
                FAC_txt.Close();
            }
            return resultado;
        }
    }
}