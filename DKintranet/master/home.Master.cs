using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DKintranet.master
{
    public partial class home : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpBrowserCapabilities brObject = Request.Browser;
            if (brObject.Type == "IE6" || brObject.Type == "IE7" || brObject.Type == "IE8" || brObject.Type == "IE9")
                Response.Redirect("~/home/versionexplorer.html");

        }
        public void cssActive(string name)
        {
            if (Request.Url.Segments[Request.Url.Segments.Length - 1] == name)
                Response.Write(" active ");
        }

        public void hrefLinkSucursales()
        {
            if (Request.Url.Segments[Request.Url.Segments.Length - 1] == "index.aspx")
                Response.Write("#sucursales");
            else
                Response.Write("../home/index.aspx#sucursales");
        }
        public void hrefLinkSucursalesMobile()
        {
            if (Request.Url.Segments[Request.Url.Segments.Length - 1] == "index.aspx")
                Response.Write("../home/index.aspx#idFooter");
            else
                Response.Write("../home/index.aspx#idFooter");
        }
        public void htmlCssBody()
        {
            if (HttpContext.Current.Session["homeBodyCss"] != null)
                Response.Write(HttpContext.Current.Session["homeBodyCss"]);
            else
                Response.Write("bd_home");
        }

    }
}