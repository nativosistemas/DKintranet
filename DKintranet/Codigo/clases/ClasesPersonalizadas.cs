using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DKbase.web.capaDatos;
using DKintranet.Codigo.capaDatos;

namespace DKintranet.Codigo.clases
{
    /// <summary>
    /// Summary description for ClasesPersonalizadas
    /// </summary>
public class cPageClientes : System.Web.UI.Page
    {
        [WebMethod(EnableSession = true)]
        public static bool SetearNroSemana(int pNroSemana)
        {
            HttpContext.Current.Session["FichaDebeHaberSaldo_NroSemana"] = pNroSemana;
            return true;
        }
        [WebMethod(EnableSession = true)]
        public static bool IsBanderaUsarDllSucursal(string pSucursal)
        {
            bool resultado = WebService.IsBanderaCodigo(pSucursal);
            return resultado;
        }
        [WebMethod(EnableSession = true)]
        public static bool IsBanderaUsarDll()
        {
            bool resultado = WebService.IsBanderaCodigo(Constantes.cBAN_SERVIDORDLL);
            return resultado;
        }
        /// <summary>
        /// 0 si
        /// 1 no permite usar dll
        /// 2 cliente inhabilitado
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public static int IsHacerPedidos()
        {
            int resultado = 0;
            if (!IsBanderaUsarDll())
            {
                resultado = 1;
            }
            else
            {
                if (HttpContext.Current.Session["clientesDefault_Cliente"] != null)
                {
                    cClientes objCliente = WebService.RecuperarClientePorId(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo);
                    if (objCliente != null)
                    {
                        ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_estado = objCliente.cli_estado;
                    }
                    if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_estado.ToUpper() == Constantes.cESTADO_INH)
                    {
                        resultado = 2;
                    }
                }
            }
            return resultado;
        }
        [WebMethod(EnableSession = true)]
        public static int IsHacerPedidos(string pSucursal)
        {
            int resultado = 0;
            if (!IsBanderaUsarDllSucursal(pSucursal))
            {
                resultado = 1;
            }
            else
            {
                if (HttpContext.Current.Session["clientesDefault_Cliente"] != null)
                {
                    cClientes objCliente = WebService.RecuperarClientePorId(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo);
                    if (objCliente != null)
                    {
                        ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_estado = objCliente.cli_estado;
                    }
                    if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_estado.ToUpper() == Constantes.cESTADO_INH)
                    {
                        resultado = 2;
                    }
                }
            }
            return resultado;
        }
    }
    public class cGrillaHorarioSucursal
    {
        public string dia { get; set; }
        public string hora { get; set; }
    }  
    public class cUsuarioSinPermisosIntranet
    {
        public int usp_id { get; set; }
        public int usp_codUsuario { get; set; }
        public string usp_nombreSeccion { get; set; }
    }
    public class cGaleria
    {
        public cNoticia noticia { get; set; }
        public List<cArchivo> listaArchivo { get; set; }
    }
    public class cTipoEnvioClienteFront
    {
        public string sucursal { get; set; }
        public string tipoEnvio { get; set; }
        public List<cTiposEnvios> lista { get; set; }
    }
    public class htmlArchivo
    {
        public int codRecurso { get; set; }
        public string titulo { get; set; }
        public string descr { get; set; }
        public string arc_nombre { get; set; }
        public string tipo { get; set; }
        public int id { get; set; }
        public int ancho { get; set; }
        public int alto { get; set; }
        public cArchivo objArchivo { get; set; }

    }
    public class cPdfComprobante
    {
        public int index { get; set; }
        public string nombreArchivo { get; set; }
        public bool isOk { get; set; }
    }
    public class Class_Admin
    {

        public class cPaginador //: ICloneable
        {
            public cPaginador() { }
            public cPaginador(List<cCurriculumVitae> pValue)
            {
                //listaSucursal = pValue.listaSucursal;
                listaCurriculumVitae = pValue;
                CantidadRegistroTotal = pValue.Count;
            }
            public cPaginador(cPaginador pValue)
            {
                //listaSucursal = pValue.listaSucursal;
                listaCurriculumVitae = pValue.listaCurriculumVitae;
                CantidadRegistroTotal = pValue.CantidadRegistroTotal;
            }
            // public List<string> listaSucursal { get; set; }
            public List<cCurriculumVitae> listaCurriculumVitae { get; set; }
            public int CantidadRegistroTotal { get; set; }
        }
    }
}