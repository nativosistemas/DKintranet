using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DKintranet.Codigo.clases;
using System.Web.Services;
using System.Xml.Linq;

namespace DKintranet.admin.pages
{
    public partial class GestionRolesYReglas : cBaseAdmin
    {
        public const string consPalabraClave = "gestionrolesyreglas";
        protected void Page_Load(object sender, EventArgs e)
        {
            Seguridad(consPalabraClave);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "clientscript", "var listaJsonReglasRol = \'" + ServiciosCargarArbolHTML() + "\';", true);
        }
        [WebMethod(EnableSession = true)]
        public static bool IsGrabarReglaRol(int pIdRol, List<DKintranet.Codigo.capaDatos.ReglaAGrabar> lista)
        {
            if (cBaseAdmin.isAgregar(consPalabraClave) || cBaseAdmin.isEditar(consPalabraClave) || cBaseAdmin.isEliminar(consPalabraClave))
            {
                if (lista.Count > 0)
                {
                    string strXML = string.Empty;
                    strXML += "<Root>";
                    foreach (DKintranet.Codigo.capaDatos.ReglaAGrabar item in lista)
                    {
                        List<XAttribute> listaAtributos = new List<XAttribute>();

                        listaAtributos.Add(new XAttribute("idRegla", item.idRegla));
                        listaAtributos.Add(new XAttribute("idRelacionReglaRol", item.idRelacionReglaRol));
                        listaAtributos.Add(new XAttribute("isActivo", item.isActivo));

                        if (item.isAgregado == null)
                        {

                        }
                        else
                        {
                            listaAtributos.Add(new XAttribute("isAgregado", item.isAgregado));
                        }
                        if (item.isEditado == null)
                        {
                        }
                        else
                        {
                            listaAtributos.Add(new XAttribute("isEditado", item.isEditado));
                        }
                        if (item.isEliminado == null)
                        {
                        }
                        else
                        {
                            listaAtributos.Add(new XAttribute("isEliminado", item.isEliminado));
                        }

                        XElement nodo = new XElement("Regla", listaAtributos);
                        strXML += nodo.ToString();
                    }
                    strXML += "</Root>";
                    string parameXML = strXML;

                    DKintranet.Codigo.clases.Seguridad.InsertarActualizarRelacionRolRegla(pIdRol, parameXML);
                    cBaseAdmin.CargarAccionesEnVariableSession();
                    return true;
                }
            }
            return false;
        }
        [WebMethod(EnableSession = true)]
        public static string RecuperarReglasPorRol(int pIdRol)
        {
            List<DKintranet.Codigo.capaDatos.ReglaPorRol> listaResultado = DKintranet.Codigo.clases.Seguridad.RecuperarRelacionRolReglasPorRol(pIdRol);
            return Codigo.clases.Generales.Serializador.SerializarAJson(listaResultado);
        }
        //[WebMethod(EnableSession = true)]
        public static string ServiciosCargarArbolHTML()
        {
            List<DKintranet.Codigo.capaDatos.ListaCheck> listaResultado = DKintranet.Codigo.clases.Seguridad.RecuperarTodasReglasPorNivel();
            return Codigo.clases.Generales.Serializador.SerializarAJson(listaResultado);
        }
    }

}