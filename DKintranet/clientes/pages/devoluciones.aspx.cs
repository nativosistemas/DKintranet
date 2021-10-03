using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DKintranet.clientes.pages
{
    public partial class devoluciones : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] == null)
            {
                Response.Redirect("~/home/index.aspx");
            }
            else
            {
                Response.Redirect("~/devoluciones/Devoluciones");
            }
        }
    }
}