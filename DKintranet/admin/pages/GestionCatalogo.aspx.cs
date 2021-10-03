using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using DKintranet.Codigo.clases;
using DKintranet.Codigo.capaDatos;
using System.IO;

namespace DKintranet.admin.pages
{
    public partial class GestionCatalogo : cBaseAdmin
    {
        public const string consPalabraClave = "gestioncatalogo";
        protected void Page_Load(object sender, EventArgs e)
        {
            Seguridad(consPalabraClave);
            if (!IsPostBack)
            {
                Session["GestionCatalogo_Tbc_codigo"] = null;
                Session["GestionCatalogo_Filtro"] = null;
            }
        }
        protected void cmd_nuevo_Click(object sender, EventArgs e)
        {
            LlamarMetodosAcciones(Constantes.cSQL_INSERT, null, consPalabraClave);
        }
        protected void cmd_buscar_Click(object sender, EventArgs e)
        {
            Session["GestionCatalogo_Filtro"] = txt_buscar.Text;
        }
        protected void cmd_guardar_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtTitulo.Text))
                return;
            else
            {
                List<cCatalogo> l = WebService.RecuperarTodosCatalogos().Where(x => x.tbc_titulo == txtTitulo.Text.ToUpper()).ToList();
                int codigoCatalogoTemp = 0;
                if (Session["GestionCatalogo_Tbc_codigo"] != null)
                    codigoCatalogoTemp = Convert.ToInt32(Session["GestionCatalogo_Tbc_codigo"]);
                if ((l.Count > 0 && codigoCatalogoTemp == 0) || (l.Count > 0 && codigoCatalogoTemp != l[0].tbc_codigo))
                    return;
            }

            if (Session["GestionCatalogo_Tbc_codigo"] != null && Session["BaseAdmin_Usuario"] != null)
            {
                int codigoUsuarioEnSession = ((Usuario)Session["BaseAdmin_Usuario"]).id;
                int codigoCatalogo = Convert.ToInt32(Session["GestionCatalogo_Tbc_codigo"]);
                string titulo = txtTitulo.Text.ToUpper();
                if (codigoCatalogo == 0)
                {
                    int? resultadoInsertar = WebService.InsertarActualizarCatalogo(0, titulo, string.Empty, 0, DateTime.Now, Constantes.cESTADO_ACTIVO);
                    if (resultadoInsertar != null)
                    {
                        codigoCatalogo = (int)resultadoInsertar;
                    }
                }
                else
                {
                    List<cCatalogo> listaCatalogo = WebService.RecuperarTodosCatalogos();
                    cCatalogo catalogo = null;
                    catalogo = listaCatalogo.Where(x => x.tbc_codigo == codigoCatalogo).First();
                    if (catalogo != null)
                    {
                        WebService.InsertarActualizarCatalogo(codigoCatalogo, titulo, catalogo.tbc_descripcion, catalogo.tbc_orden, catalogo.tbc_fecha, catalogo.tbc_estado);
                    }
                }
                if (codigoCatalogo > 0)
                {
                    if (FileUpload1.HasFile)
                    {
                        if (FileUpload1.PostedFile.ContentType == Constantes.cMIME_pdf)
                        {
                            string extencion = capaRecurso.obtenerExtencion(FileUpload1.FileName);
                            string pathDestinoRaiz = Constantes.cRaizArchivos + @"\archivos\";
                            string pathDestino = pathDestinoRaiz + Constantes.cTABLA_CATALOGO;
                            if (Directory.Exists(pathDestino) == false)
                            {
                                Directory.CreateDirectory(pathDestino);
                            }
                            string filename = capaRecurso.nombreArchivoSinRepetir(pathDestino, FileUpload1.FileName);
                            string nombreArchivo = filename;
                            string destino = pathDestino + @"\" + nombreArchivo; 
                            FileUpload1.SaveAs(destino);
                            int codRecurso = 0;
                            List<cArchivo> listaArchivo = WebService.RecuperarTodosArchivos(codigoCatalogo, Constantes.cTABLA_CATALOGO, string.Empty);
                            if (listaArchivo != null)
                            {
                                if (listaArchivo.Count > 0)
                                {
                                    codRecurso = listaArchivo[0].arc_codRecurso;
                                }
                            }
                            WebService.InsertarActualizarArchivo(codRecurso, codigoCatalogo, Constantes.cTABLA_CATALOGO, extencion, FileUpload1.PostedFile.ContentType, nombreArchivo, string.Empty, string.Empty, string.Empty, codigoUsuarioEnSession);
                        }
                    }
                }

                gv_datos.DataBind();
                pnl_grilla.Visible = true;
                pnl_formulario.Visible = false;
            }
        }
        protected void cmd_cancelar_Click(object sender, EventArgs e)
        {
            pnl_grilla.Visible = true;
            pnl_formulario.Visible = false;
        }
        public override void Modificar(int pId)
        {
            Session["GestionCatalogo_Tbc_codigo"] = pId;
            List<cCatalogo> listaCatalogo = WebService.RecuperarTodosCatalogos();
            cCatalogo catalogo = null;
            catalogo = listaCatalogo.Where(x => x.tbc_codigo == pId).First();
            if (catalogo != null)
            {
                txtTitulo.Text = catalogo.tbc_titulo;
                List<cArchivo> listaArchivo = WebService.RecuperarTodosArchivos(pId, Constantes.cTABLA_CATALOGO, string.Empty);
                if (listaArchivo != null)
                {
                    if (listaArchivo.Count > 0)
                    {
                        lblArchivo.Text = "<div><a id=\"a_" + listaArchivo[0].arc_codRecurso + "\" href=\"../../servicios/" + "descargarArchivo.aspx?t=" + Constantes.cTABLA_CATALOGO + "&n=" + listaArchivo[0].arc_nombre + "\" >" + listaArchivo[0].arc_nombre + "</a>&nbsp; </div>";
                    }
                }
                PanelArchivo.Visible = false;
                PanelArchivoTexto.Visible = true;
                pnl_grilla.Visible = false;
                pnl_formulario.Visible = true;
            }
        }
        public string ArmarLink(string archivo, string grupo, string tipo)
        {
            string ruta = System.Configuration.ConfigurationManager.AppSettings["raiz"] + Constantes.cArchivo_Raiz + @"/" + grupo + "/" + tipo + "/" + Server.UrlEncode(archivo);
            string url = string.Empty;
            url = "<a href='" + ruta + "' target='_blank' />" + archivo + "</a>";
            return url;
        }
        public override void Eliminar(int pId)
        {
            WebService.ElininarCatalogo(pId);
        }
        public override void Insertar()
        {
            Session["GestionCatalogo_Tbc_codigo"] = 0;
            pnl_grilla.Visible = false;
            pnl_formulario.Visible = true;
            lblArchivo.Text = string.Empty;
            txtTitulo.Text = string.Empty;
            PanelArchivo.Visible = true;
            PanelArchivoTexto.Visible = false;
        }
        public override void CambiarEstado(int pId)
        {
            Session["GestionCatalogo_Tbc_codigo"] = pId;
            List<cCatalogo> listaCatalogo = WebService.RecuperarTodosCatalogos();
            cCatalogo catalogo = null;
            catalogo = listaCatalogo.Where(x => x.tbc_codigo == pId).First();
            if (catalogo != null)
            {
                int estado = 0;
                if (catalogo.tbc_estado == Constantes.cESTADO_ACTIVO)
                {
                    estado = Constantes.cESTADO_INACTIVO;
                }
                else
                {
                    estado = Constantes.cESTADO_ACTIVO;
                }
                WebService.InsertarActualizarCatalogo(catalogo.tbc_codigo, catalogo.tbc_titulo, catalogo.tbc_descripcion, catalogo.tbc_orden, catalogo.tbc_fecha, estado);
            }
        }
        protected void gv_datos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {
                LlamarMetodosAcciones(Constantes.cSQL_UPDATE, Convert.ToInt32(e.CommandArgument), consPalabraClave);
            }
            else if (e.CommandName == "Estado")
            {
                LlamarMetodosAcciones(Constantes.cSQL_ESTADO, Convert.ToInt32(e.CommandArgument), consPalabraClave);
            }
            else if (e.CommandName == "Eliminar")
            {
                LlamarMetodosAcciones(Constantes.cSQL_DELETE, Convert.ToInt32(e.CommandArgument), consPalabraClave);
            }
            else if (e.CommandName == "Publicar")
            {
                LlamarMetodosAcciones(Constantes.cSQL_PUBLICAR, Convert.ToInt32(e.CommandArgument), consPalabraClave);
            }
            gv_datos.DataBind();
        }
        public override void Publicar(int pId)
        {
            if (Session["BaseAdmin_Usuario"] != null)
            {
                bool tbc_publicarHome = true;
                cCatalogo o = WebService.RecuperarTodosCatalogos().Where(x => x.tbc_codigo == pId).FirstOrDefault();
                cCatalogo oPublicarHome = WebService.RecuperarTodosCatalogos().Where(x => (x.tbc_publicarHome != null && x.tbc_publicarHome.Value)).FirstOrDefault();
                if (o != null && o.tbc_publicarHome != null)
                    tbc_publicarHome = !o.tbc_publicarHome.Value;
                if (oPublicarHome != null && oPublicarHome.tbc_codigo != pId)
                    WebService.PublicarHomeCatalogo(oPublicarHome.tbc_codigo, false);
                WebService.PublicarHomeCatalogo(pId, tbc_publicarHome);
                gv_datos.DataBind();
            }
        }
        protected void btnArchivo_Click(object sender, EventArgs e)
        {
            PanelArchivo.Visible = true;
            PanelArchivoTexto.Visible = false;
        }
    }
}