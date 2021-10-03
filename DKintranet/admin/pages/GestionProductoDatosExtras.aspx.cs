using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using DKintranet.Codigo.clases;

namespace DKintranet.admin.pages
{
    public partial class GestionProductoDatosExtras : cBaseAdmin
    {
        public const string consPalabraClave = "gestionproductodatosextras";
        protected void Page_Load(object sender, EventArgs e)
        {
            Seguridad(consPalabraClave);
            if (!IsPostBack)
            {
                txt_Cantidad.Text = WebService.RecuperarProductoParametrizadoCantidad().ToString();
            }

        }
        protected void cmd_guardar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_Cantidad.Text))
            {
                WebService.InsertarActualizarProductoParametrizadoCantidad(Convert.ToInt32(txt_Cantidad.Text));
            }
        }

    }
}