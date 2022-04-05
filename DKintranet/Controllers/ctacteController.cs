using DKbase.web;
using DKbase.web.capaDatos;
using DKintranet.Codigo.capaDatos;
using DKintranet.Codigo.clases;
using DKintranet.Codigo.clases.Generales;
using DKintranet.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKintranet.Controllers
{
    public class ctacteController : Controller
    {
        [AuthorizePermisoAttribute(Permiso = "CUENTASCORRIENTES")]
        public ActionResult Documento(string t, string id)
        {
            Session["clientes_pages_Documento_ID"] = id;
            Session["clientes_pages_Documento_TIPO"] = t.ToUpper();
            //Documento.aspx?t=RES&id=X000101933698
            //System.Web.HttpContext.Current.Session["url_type"] = "Documento";
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "CUENTASCORRIENTES")]
        public ActionResult FichaDebeHaberSaldo()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "CUENTASCORRIENTES")]
        public ActionResult descargaResumenes()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "CUENTASCORRIENTES")]
        public ActionResult descargaResumenesDetalle(string id)
        {
            if (id != null)
            {
                Session["clientes_pages_descargaResumenes_NumeroResumen"] = id;
            }
            if (Session["clientes_pages_descargaResumenes_NumeroResumen"] != null)
            {
                List<ServiceReferenceDLL.cCbteParaImprimir> obj = WebService.ObtenerComprobantesAImprimirEnBaseAResumen(Session["clientes_pages_descargaResumenes_NumeroResumen"].ToString());
                Session["clientes_pages_descargaResumenes_listaComprobantesAImprimir"] = obj;
            }
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "CUENTASCORRIENTES")]
        public ActionResult composicionsaldo()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "CUENTASCORRIENTES")]
        public ActionResult composicionsaldoCtaCte()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "CUENTASCORRIENTES")]
        public ActionResult composicionsaldoCtaResumen()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "CUENTASCORRIENTES")]
        public ActionResult composicionsaldochequecuenta()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "CUENTASCORRIENTES")]
        public ActionResult ConsultaDeComprobantes()
        {
            if (Session["clientesDefault_Cliente"] != null)
            {
                List<string> lista = new List<string>();
                lista = WebService.ObtenerTiposDeComprobantesAMostrar(((cClientes)Session["clientesDefault_Cliente"]).cli_login);
                Session["ConsultaDeComprobantes_tipoComprobante"] = lista;
            }
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "CUENTASCORRIENTES")]
        public ActionResult RespuestaConsultaDeComprobantes()
        {
            if (Request.QueryString["t"] != null)
            {
                Session["RespuestaConsultaDeComprobantes_TIPO"] = Request.QueryString["t"].ToString().ToUpper();
            }
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "CUENTASCORRIENTES")]
        public ActionResult comprobantescompleto()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "CUENTASCORRIENTES")]
        public ActionResult ConsultaDeComprobantesObraSocial()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "CUENTASCORRIENTES")]
        public ActionResult ConsultaDeComprobantesObraSocialResultado()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "CUENTASCORRIENTES")]
        public ActionResult ConsultaDeComprobantesObraSocialResultadoRango()
        {
           // ObtenerComprobantesObrasSocialesDePuntoDeVentaEntreFechas("", "", 1, 1, 1, 1, 1, 1);
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "CUENTASCORRIENTES")]
        public ActionResult deudaVencida()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string ObtenerCreditoDisponible(string pCli_login)
        {
            //var fdfdf = ObtenerComprobantesObrasSocialesDePuntoDeVentaEntreFechas("romanello", "OSDE", DateTime.Now.AddDays(-7), DateTime.Now);
            ResultCreditoDisponible o =new ResultCreditoDisponible();
            o.CreditoDisponibleSemanal = WebService.ObtenerCreditoDisponibleSemanal(pCli_login);
            o.CreditoDisponibleTotal = WebService.ObtenerCreditoDisponibleTotal(pCli_login);
            return Codigo.clases.Generales.Serializador.SerializarAJson(o);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public double? ObtenerSaldoFinalADiciembrePorCliente(string pCli_login)
        {
            return capaWebServiceDLL.ObtenerSaldoFinalADiciembrePorCliente(pCli_login);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string ObtenerComprobantesObrasSocialesDePuntoDeVentaEntreFechas(string pLoginWeb, string pPlan, int diaDesde, int mesDesde, int añoDesde, int diaHasta, int mesHasta, int añoHasta)
        {
            DateTime fechaDesde = new DateTime(añoDesde, mesDesde, diaDesde);//, 0, 0, 0
            DateTime fechaHasta = new DateTime(añoHasta, mesHasta, diaHasta);//, 23, 59, 59
            List<ServiceReferenceDLL.cConsObraSocial> l = WebService.ObtenerComprobantesObrasSocialesDePuntoDeVentaEntreFechas( pLoginWeb,  pPlan,  fechaDesde,  fechaHasta);
            //List<ServiceReferenceDLL.cConsObraSocial> l = new List<ServiceReferenceDLL.cConsObraSocial>();
            //ServiceReferenceDLL.cConsObraSocial o1 = new ServiceReferenceDLL.cConsObraSocial();
            //o1.Detalle = "4324324";
            //o1.FechaComprobante =DateTime.Now;
            //o1.FechaComprobanteToString = DateTime.Now.ToShortDateString();
            //o1.Importe = 2323;
            //o1.NumeroComprobante = "34324234";
            //o1.TipoComprobante = "FAC";
            //l.Add(o1);
            //ServiceReferenceDLL.cConsObraSocial o2 = new ServiceReferenceDLL.cConsObraSocial();
            //o2.Detalle = "6786gh";
            //o2.FechaComprobante = DateTime.Now.AddDays(2);
            //o2.FechaComprobanteToString = DateTime.Now.AddDays(2).ToShortDateString();
            //o2.Importe = 123;
            //o2.NumeroComprobante = "86754646";
            //o2.TipoComprobante = "RES";
            //l.Add(o2);
            Session["ObrasSociales_EntreFechas"] = l;
            return "Ok";// Codigo.clases.Generales.Serializador.SerializarAJson(l);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public static List<ServiceReferenceDLL.cCtaCteMovimiento> getHiddenDeudaVencida(string pCli_login)
        {
            int pDia = 7;
            int pPendiente = 1;
            int pCancelado = 0;
            DateTime fechaDesde = DateTime.Now.AddDays(pDia * -1);
            DateTime fechaHasta = DateTime.Now;
            List<ServiceReferenceDLL.cCtaCteMovimiento> l = AgregarVariableSessionComposicionSaldo(fechaDesde, fechaHasta, pPendiente, pCancelado, pCli_login);
            return l;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public int enviarConsultaCtaCte(string pMail, string pComentario)
        {
           return WebService.enviarConsultaCtaCte(pMail, pComentario);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public void ObtenerComprobantesDiscriminadosDePuntoDeVentaEntreFechas(int diaDesde, int mesDesde, int añoDesde, int diaHasta, int mesHasta, int añoHasta)
        {
            if (Session["clientesDefault_Cliente"] != null)
            {
                DateTime fechaDesde = new DateTime(añoDesde, mesDesde, diaDesde);//, 0, 0, 0
                DateTime fechaHasta = new DateTime(añoHasta, mesHasta, diaHasta);//, 23, 59, 59
                List<ServiceReferenceDLL.cComprobantesDiscriminadosDePuntoDeVenta> resultadoObj = WebService.ObtenerComprobantesDiscriminadosDePuntoDeVentaEntreFechas(((cClientes)Session["clientesDefault_Cliente"]).cli_login, fechaDesde, fechaHasta);
                if (resultadoObj != null)
                    Session["comprobantescompleto_Lista"] = resultadoObj;
                Session["comprobantescompleto_FechaDesde"] = fechaDesde;
                Session["comprobantescompleto_FechaHasta"] = fechaHasta;
            }
            else
            {
                Session["comprobantescompleto_Lista"] = null;
            }
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public void AgregarVariableSessionConsultaDeComprobantes(string pTipo, int diaDesde, int mesDesde, int añoDesde, int diaHasta, int mesHasta, int añoHasta)
        {
            if (Session["clientesDefault_Cliente"] != null)
            {
                string resultado = string.Empty;
                object NroComprobante = null;
                List<object> NrosDeComprobante = new List<object>();
                DateTime fechaDesde = new DateTime(añoDesde, mesDesde, diaDesde);
                DateTime fechaHasta = new DateTime(añoHasta, mesHasta, diaHasta);
                List<ServiceReferenceDLL.cComprobanteDiscriminado> resultadoObj = WebService.ObtenerComprobantesEntreFecha(pTipo, fechaDesde, fechaHasta, ((cClientes)Session["clientesDefault_Cliente"]).cli_login);
                if (resultadoObj != null)
                {
                    Session["ConsultaDeComprobantes_ComprobantesEntreFecha"] = resultadoObj;
                    Session["comprobantescompleto_FechaDesde"] = fechaDesde;
                    Session["comprobantescompleto_FechaHasta"] = fechaHasta;
                    for (var i = 0; i < resultadoObj.Count;i++) {
                        NroComprobante = resultadoObj[i].NumeroComprobante;
                        NrosDeComprobante.Add(NroComprobante);
                    }
                    Session["ConsultaDeComprobantes_NumerosDeComprobantes"] = NrosDeComprobante;
                }
            }
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public List<string> ObtenerRangoFecha(int pDia, int pPendiente, int pCancelado)
        {
            if (Session["clientesDefault_Cliente"] == null)
                return null;
            List<string> lista = new List<string>();
            DateTime fechaDesde = DateTime.Now.AddDays(pDia * -1);
            DateTime fechaHasta = DateTime.Now;
            lista.Add(fechaDesde.Day.ToString());
            lista.Add((fechaDesde.Month).ToString());
            lista.Add((fechaDesde.Year).ToString());

            lista.Add(fechaHasta.Day.ToString());
            lista.Add((fechaHasta.Month).ToString());
            lista.Add((fechaHasta.Year).ToString());
            Session["CompocisionSaldo_ResultadoMovimientosDeCuentaCorriente"] = AgregarVariableSessionComposicionSaldo(fechaDesde, fechaHasta, pPendiente, pCancelado, ((cClientes)Session["clientesDefault_Cliente"]).cli_login);
            return lista;

        }
        public static List<ServiceReferenceDLL.cCtaCteMovimiento> AgregarVariableSessionComposicionSaldo(DateTime pFechaDesde, DateTime pFechaHasta, int pPendiente, int pCancelado, string pCli_login)
        {
            string resultado = string.Empty;
            DateTime fechaDesde = pFechaDesde;
            DateTime fechaHasta = pFechaHasta;

            int pendiente = pPendiente;
            int cancelado = pCancelado;

            List<ServiceReferenceDLL.cCtaCteMovimiento> resultadoObj = WebService.ObtenerMovimientosDeCuentaCorriente((pendiente == 1 ? true : false), fechaDesde, fechaHasta, pCli_login);

            if (resultadoObj != null)
            {
                List<ServiceReferenceDLL.cCtaCteMovimiento> resultadoAUX = new List<ServiceReferenceDLL.cCtaCteMovimiento>();
                List<ServiceReferenceDLL.cCtaCteMovimiento> parteAUX = null;
                bool isPaso = false;
                bool isPasoPorPaso = false;
                for (int i = 0; i < resultadoObj.Count; i++)
                {
                    bool isAgregarAhora = false;
                    if (isPaso)
                    {
                        parteAUX.Add(resultadoObj[i]);
                        isPaso = false;
                        isPasoPorPaso = true;
                    }
                    if (resultadoObj[i].FechaVencimiento == null)
                    {
                        isAgregarAhora = true;
                    }
                    else
                    {
                        if (i == resultadoObj.Count - 1)
                        {
                            isAgregarAhora = true;
                        }
                        else
                        {
                            if (resultadoObj[i].NumeroComprobante != "" && Convert.ToInt32(resultadoObj[i].TipoComprobante) < 14)
                            {
                                if (resultadoObj[i].NumeroComprobante == resultadoObj[i + 1].NumeroComprobante && resultadoObj[i].TipoComprobante == resultadoObj[i + 1].TipoComprobante)
                                {
                                    if (parteAUX == null)
                                    {
                                        parteAUX = new List<ServiceReferenceDLL.cCtaCteMovimiento>();
                                    }
                                    if (!isPasoPorPaso)
                                    {
                                        parteAUX.Add(resultadoObj[i]);
                                    }
                                    isPaso = true;
                                }
                                else
                                {
                                    isAgregarAhora = true;
                                }
                            }
                            else
                            {
                                isAgregarAhora = true;
                            }
                        }
                    }
                    if (isAgregarAhora)
                    {
                        if (parteAUX != null)
                        {
                            resultadoAUX.AddRange(parteAUX.OrderBy(x => x.FechaVencimiento).ToList());
                            parteAUX = null;
                        }
                        if (!isPasoPorPaso)
                        {
                            resultadoAUX.Add(resultadoObj[i]);
                        }
                    }
                    isPasoPorPaso = false;
                }
                return resultadoAUX;
            }
            return null;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string ObtenerMovimientosDeFicha(int pSemana)
        {
            if (Session["clientesDefault_Cliente"] == null)
                return null;
            Session["FichaDebeHaberSaldo_NroSemana"] = pSemana;
            DateTime FechaDesde = DateTime.Now.AddDays(-(pSemana * 7));
            DateTime FechaHasta = DateTime.Now.AddDays(1);
            List<ServiceReferenceDLL.cFichaCtaCte> listaFicha = capaWebServiceDLL.ObtenerMovimientosDeFichaCtaCte(((cClientes)Session["clientesDefault_Cliente"]).cli_login, FechaDesde, FechaHasta);
            if (listaFicha != null)
                return Codigo.clases.Generales.Serializador.SerializarAJson(listaFicha);
            return null;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string IsExistenciaComprobanteResumenes_todos(string pNombreArchivo, int pContadorAUX)
        {
            cPdfComprobante resultado = new cPdfComprobante();
            string nombreArchivo = pNombreArchivo + ".pdf";
            resultado.isOk = System.IO.File.Exists(Constantes.cArchivo_ImpresionesComprobante + nombreArchivo);
            if (!resultado.isOk && Session["clientes_pages_descargaResumenes_NumeroResumen"] != null && pContadorAUX == 0)
            {
                string NumeroComprobante = Session["clientes_pages_descargaResumenes_NumeroResumen"].ToString();
                WebService.ImprimirComprobante("RCO", NumeroComprobante);
            }
            resultado.nombreArchivo = pNombreArchivo;
            if (resultado != null)
                return Codigo.clases.Generales.Serializador.SerializarAJson(resultado);
            return null;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string IsExistenciaComprobanteResumenes(string pNombreArchivo, int pIndex, int pContadorAUX)
        {
            cPdfComprobante resultado = new cPdfComprobante();
            string nombreArchivo = pNombreArchivo + ".pdf";
            resultado.isOk = System.IO.File.Exists(Constantes.cArchivo_ImpresionesComprobante + nombreArchivo);
            if (!resultado.isOk && Session["clientes_pages_descargaResumenes_listaComprobantesAImprimir"] != null && pContadorAUX == 0)
            {
                List<ServiceReferenceDLL.cCbteParaImprimir> obj = (List<ServiceReferenceDLL.cCbteParaImprimir>)Session["clientes_pages_descargaResumenes_listaComprobantesAImprimir"];
                WebService.ImprimirComprobante(obj[pIndex].TipoComprobante, obj[pIndex].NumeroComprobante);
            }
            resultado.nombreArchivo = pNombreArchivo;
            resultado.index = pIndex;
            if (resultado != null)
                return Codigo.clases.Generales.Serializador.SerializarAJson(resultado);
            return null;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public bool IsExistenciaComprobante(string pNombreArchivo)
        {
            bool resultado = false;
            string nombreArchivo = pNombreArchivo + ".pdf";
            resultado = System.IO.File.Exists(Constantes.cArchivo_ImpresionesComprobante + nombreArchivo);
            return resultado;
        }
        public void CambiarClientePromotor(int IdCliente)
        {
            System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] = WebService.RecuperarClientePorId((int)IdCliente);
        }
        public void ActualizarFacturasDescarga(List<object> NrosComprobantes)
        {
            if (Session["clientesDefault_Cliente"] != null)
            {
                object NroComprobante = null;
                List<object> NrosDeComprobante = new List<object>();
                string listado = NrosComprobantes[0].ToString();
                listado = listado.Replace('n', ' ');
                listado = listado.Replace('o', ' ');
                listado = listado.Replace('=', ' ');
                listado = listado.Replace('\"', ' ');
                var ncomp = listado.Split('&');
                for (var i = 0;i< ncomp.Length;i++)
                {
                    NroComprobante = ncomp[i].ToString().Trim();
                    NrosDeComprobante.Add(NroComprobante);
                }
                Session["ConsultaDeComprobantes_NumerosDeComprobantes"] = NrosDeComprobante;
            }
        }
        public string ObtenerVencimientosResumenPorFecha(string pNumeroResumen, DateTime pFechaVencimiento)
        {
            string resultado = string.Empty;
            List<ServiceReferenceDLL.cVencimientoResumen> resultadoObj = WebService.ObtenerVencimientosResumenPorFecha(pNumeroResumen, pFechaVencimiento);

            if (resultadoObj != null)
            {
                return Serializador.SerializarAJson(resultadoObj);
            } else
            {
                return resultado;
            }
        }
    }
}