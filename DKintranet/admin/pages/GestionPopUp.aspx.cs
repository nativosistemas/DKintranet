using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using DKintranet.Codigo;
using DKintranet.Codigo.clases;
using DKintranet.Codigo.capaDatos;
using DKbase.web.capaDatos;

namespace DKintranet.admin.pages
{
    public partial class GestionPopUp : cBaseAdmin
    {
        public const string consPalabraClave = "gestionhome";
        protected void Page_Load(object sender, EventArgs e)
        {
            Seguridad(consPalabraClave);
            if (!IsPostBack)
            {

            }
        }
        [WebMethod(EnableSession = true)]
        public static string RecuperarTodosArchivos()
        {
            List<cArchivo> resultado = WebService.RecuperarTodosArchivos(1, "popup", string.Empty);
            return resultado == null ? string.Empty : DKintranet.Codigo.clases.Generales.Serializador.SerializarAJson(resultado);
        }
        [WebMethod(EnableSession = true)]
        public static string EliminarArchivoPorId(int pValor)
        {
            WebService.EliminarArchivoPorId(pValor);
            return string.Empty;
        }
        [WebMethod(EnableSession = true)]
        public static bool CambiarEstadoArchivoPorId(int pValue)
        {
            WebService.CambiarEstadoArchivoPorId(pValue, Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["codigoUsuarioSinRegistrar"]));
            return true;
        }
    }
}