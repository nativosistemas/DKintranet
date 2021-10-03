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
    public partial class generar_comprobantes_txt : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string rutaTemporal = Constantes.cRaizArchivos + @"\archivos\comprobantes\";

            DirectoryInfo DIR = new DirectoryInfo(rutaTemporal);
            if (!DIR.Exists)
            {
                DIR.Create();
            }
            string nombreTXT = null;
            if (HttpContext.Current.Session["ConsultaDeComprobantes_NumerosDeComprobantes"] != null && HttpContext.Current.Session["ConsultaDeComprobantes_ComprobantesEntreFecha"] != null)
            {
                List<DKintranet.ServiceReferenceDLL.cComprobanteDiscriminado> l2 = (List<DKintranet.ServiceReferenceDLL.cComprobanteDiscriminado>)Session["ConsultaDeComprobantes_ComprobantesEntreFecha"];
                List<object> l = (List<object>)Session["ConsultaDeComprobantes_NumerosDeComprobantes"];
                nombreTXT = grabarComprobastesTXT(l,l2);
            }
            //string nombreTXT = GrabarComprobantesTXT(rutaTemporal);
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
        public string grabarComprobastesTXT(List<object> pLista , List<DKintranet.ServiceReferenceDLL.cComprobanteDiscriminado> pLista2)
        {
            string resultado = string.Empty;

            if (pLista != null && HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                string ruta = Constantes.cRaizArchivos + @"/archivos/comprobantes/";
                DirectoryInfo DIR = new DirectoryInfo(ruta);
                if (!DIR.Exists)
                {
                    DIR.Create();
                }
                string nombreArchivoCSV = string.Empty;
                DateTime fechaDesde = (DateTime)HttpContext.Current.Session["comprobantescompleto_FechaDesde"];
                DateTime fechaHasta = (DateTime)HttpContext.Current.Session["comprobantescompleto_FechaHasta"];
                string fechaArchivoCSV = fechaDesde.Year.ToString().Substring(2, 2) + fechaDesde.Month.ToString("00") + fechaDesde.Day.ToString("00") + "A" + fechaHasta.Year.ToString().Substring(2, 2) + fechaHasta.Month.ToString("00") + fechaHasta.Day.ToString("00");
                nombreArchivoCSV = ((cClientes)(HttpContext.Current.Session["clientesDefault_Cliente"])).cli_login + "-Comprobantes" + fechaArchivoCSV + ".txt";
                //romanello-Comprobantes130901A130904.csv
                resultado = nombreArchivoCSV;
                System.IO.StreamWriter FAC_txt = new System.IO.StreamWriter(ruta + nombreArchivoCSV, false, System.Text.Encoding.UTF8);

                for (int i = 0; i < pLista.Count; i++)
                { 
                    ServiceReferenceDLL.cFactura objFactura = WebService.ObtenerFactura(pLista[i].ToString(), ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_login);
                    string strCabeceraFAC = string.Empty;
                    // identificación cabecera
                    strCabeceraFAC += "C-";
                    // número C(13) 
                    strCabeceraFAC += objFactura.Numero.PadRight(13, ' ');
                    // Fecha
                    if (objFactura.Fecha != null)
                    {
                        strCabeceraFAC += ((DateTime)objFactura.Fecha).Day.ToString("00") + ((DateTime)objFactura.Fecha).Month.ToString("00") + ((DateTime)objFactura.Fecha).Year.ToString("0000");
                    }
                    else
                    {
                        strCabeceraFAC += "00" + "00" + "0000";
                    }
                    // fin fecha
                    //monto total N(10) [1]        
                    string montoTotal = string.Empty;
                    montoTotal += Numerica.toString_NumeroTXT_N10(objFactura.MontoTotal);
                    strCabeceraFAC += montoTotal;
                    // fin monto total N(10) [1] 

                    //4 monto exento N(10) [1] 
                    strCabeceraFAC += Numerica.toString_NumeroTXT_N10(objFactura.MontoExento);
                    //5 monto gravado N(10) [1] 
                    strCabeceraFAC += Numerica.toString_NumeroTXT_N10(objFactura.MontoGravado);
                    //6 monto IVA inscripto N(10) [1] 
                    strCabeceraFAC += Numerica.toString_NumeroTXT_N10(objFactura.MontoIvaInscripto);
                    //7 monto IVA no inscripto N(10) [1] 
                    strCabeceraFAC += Numerica.toString_NumeroTXT_N10(objFactura.MontoIvaNoInscripto);
                    //8 monto percepción DGR N(10) [1] 
                    strCabeceraFAC += Numerica.toString_NumeroTXT_N10(objFactura.MontoPercepcionDGR);
                    //9 descuento especial N(10) [1] 
                    strCabeceraFAC += Numerica.toString_NumeroTXT_N10(objFactura.DescuentoEspecial);
                    //10 descuento netos N(10) [1] 
                    strCabeceraFAC += Numerica.toString_NumeroTXT_N10(objFactura.DescuentoNetos);
                    //11 descuento perfumería N(10) [1] 
                    strCabeceraFAC += Numerica.toString_NumeroTXT_N10(objFactura.DescuentoPerfumeria);
                    //12 descuento web N(10) [1] 
                    strCabeceraFAC += Numerica.toString_NumeroTXT_N10(objFactura.DescuentoWeb);
                    //13 Monto Percepcion Municipal N(10) [1] 
                    strCabeceraFAC += Numerica.toString_NumeroTXT_N10(objFactura.MontoPercepcionMunicipal);

                    FAC_txt.WriteLine(strCabeceraFAC);
                    foreach (ServiceReferenceDLL.cFacturaDetalle item in objFactura.lista)
                    {
                        if (item.Troquel != null)
                        {
                            if (item.Troquel != string.Empty)
                            {
                                //If NOT ISNULL(Importe)
                                if (item.Importe != null)
                                {
                                    if (item.Importe.Trim() != string.Empty)
                                    {
                                        string detalleFAC = string.Empty;
                                        //Identificador de item
                                        detalleFAC += "I-";
                                        //Nro. Campo Tipo Comentario
                                        //1 código de barras producto C(13)
                                        //2 descripción producto C(60)
                                        //3 cantidad N(5)
                                        //4 característica C(1)
                                        //Espacio en blanco - Sin característica
                                        //F - Farmabono
                                        //D - Tarjeta D
                                        //C - Colfacor
                                        //B - Bonos CIL
                                        //P - Bonos PAP
                                        //$ - Ofertas
                                        //T - Transfer
                                        //5 neto N(1) 0 - Normail / 1 - Neto + IVA
                                        //6 precio público N(10) [1]
                                        //7 precio unitario N(10) [1]
                                        //8 importe N(10) [1]   
                                        cProductos producto = WebService.RecuperarProductoPorNombre(item.Descripcion);
                                        bool isNoTieneCodigoBarra = true;//código de barras producto C(13)
                                        if (producto != null)
                                        {
                                            if (producto.pro_codigobarra != null)
                                            {
                                                isNoTieneCodigoBarra = false;
                                                detalleFAC += producto.pro_codigobarra.PadRight(13, ' ');
                                            }
                                        }
                                        if (isNoTieneCodigoBarra)
                                        {
                                            detalleFAC += " ".PadRight(13, ' ');
                                        }
                                        detalleFAC += item.Descripcion.PadRight(60, ' ');
                                        detalleFAC += item.Cantidad.PadLeft(5, '0');
                                        if (item.Caracteristica == null)
                                        {
                                            detalleFAC += " ";
                                        }
                                        else
                                        {
                                            if (item.Caracteristica == string.Empty)
                                            {
                                                detalleFAC += " ";
                                            }
                                            else
                                            {
                                                detalleFAC += item.Caracteristica.PadLeft(1, ' ');
                                            }
                                        }
                                        if (producto != null)
                                        {
                                            detalleFAC += producto.pro_neto ? "1" : "0"; // Neto --- neto N(1) 0 - Normail / 1 - Neto + IVA                            
                                        }
                                        else
                                        {
                                            detalleFAC += " ";
                                        }
                                        detalleFAC += Numerica.toString_NumeroTXT_N10(item.PrecioPublico);
                                        detalleFAC += Numerica.toString_NumeroTXT_N10(item.PrecioUnitario);
                                        detalleFAC += Numerica.toString_NumeroTXT_N10(item.Importe);

                                        //resultado += detalleFAC + "\n";
                                        FAC_txt.WriteLine(detalleFAC);
                                        //listaResultado.Add(resultado);

                                    }//   if (item.Importe.Trim() != string.Empty) { 
                                }//     if (item.Importe != null) { 

                            }// fin if (item.Troquel != string.Empty)

                        }// fin if (item.Troquel != null)
                    }
                }
                FAC_txt.Close();

            }

            return resultado;
        }
    }
}