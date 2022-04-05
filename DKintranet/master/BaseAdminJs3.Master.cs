using DKbase.web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DKintranet.master
{
    public partial class BaseAdminJs3 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["BaseAdmin_Usuario"] == null)
            {
                Response.Redirect("~/admin/Default.aspx");
            }
            else
            {
                lblNombreUsuario.Text = ((Usuario)Session["BaseAdmin_Usuario"]).NombreYApellido;
            }
        }
    }
}