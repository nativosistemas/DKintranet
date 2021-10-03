using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using DKintranet.Codigo.clases;
using DKintranet.Codigo.capaDatos;

namespace DKintranet.admin.pages
{
    public partial class Modulo : cBaseAdmin
    {
        public const string consPalabraClave = "gestionapp";
        protected void Page_Load(object sender, EventArgs e)
        {
            Seguridad(consPalabraClave);
            if (!IsPostBack)
            {

            }
        }
        [WebMethod(EnableSession = true)]
        public static string GetAll()
        {
            List<DKbase.Entities.Modulo> resultado = WebService.GetModulos();
            return resultado == null ? string.Empty : DKintranet.Codigo.clases.Generales.Serializador.SerializarAJson(resultado);
        }
        [WebMethod(EnableSession = true)]
        public static void Delete(int id)
        {
            WebService.DeleteModulo(id);
        }
    }
}
