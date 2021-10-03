using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DKintranet.Codigo.clases;
using DKintranet.Codigo.capaDatos;
using System.Data;
using System.IO;

namespace DKintranet.admin.pages
{
    public partial class GestionProductoImagenProceso : cBaseAdmin
    {
        public const string consPalabraClave = "gestionproductoimagenproceso";
        protected void Page_Load(object sender, EventArgs e)
        {
            Seguridad(consPalabraClave);
            if (!IsPostBack)
            { }
        }
        protected void Button1_Click(object sender,   EventArgs e)
        {
            WebService.ActualizarAnchoAltoImagenProductosAlAmpliar(CheckBoxTodosLosNull.Checked);
        }
        protected void Button2_Click(object sender, EventArgs e)
        {
            WebService.BorrarAnchoAltoImagen();
        }
    }
}