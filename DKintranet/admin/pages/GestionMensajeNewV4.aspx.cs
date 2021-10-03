using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DKintranet.Codigo.clases;
using DKintranet.Codigo.capaDatos;

namespace DKintranet.admin.pages
{
    public partial class GestionMensajeNewV4 : cBaseAdmin
    {
        public const string consPalabraClave = "gestionmensaje";
        protected void Page_Load(object sender, EventArgs e)
        {
            Seguridad(consPalabraClave);
            if (!IsPostBack)
            {
                Session["GestionMensajeNewV4_Filtro"] = null;
                Session["GestionMensajeNewV4_tme_codigo"] = null;
            }
        }
        public override void Modificar(int pId)
        {
            Session["GestionMensajeNewV4_tme_codigo"] = pId;
            cMensaje mensaje = WebService.RecuperarMensajeNewV4PorId(pId);

            if (mensaje != null)
            {
                int clienteCombo = 0;
                Session["GestionMensajeNewV4_tme_todosSucursales"] = mensaje.tme_todosSucursales;
                Session["GestionMensajeNewV4_tme_todos"] = mensaje.tme_todos;
                Session["GestionMensajeNewV4_isNuevo"] = false;

                checkboxImportante.Checked = mensaje.tme_importante;
                if (mensaje.tme_importante)
                {
                    PanelFecha.Visible = checkboxImportante.Checked;
                    CalendarFechaDesde.SelectedDate = (DateTime)mensaje.tme_fechaDesde;
                    CalendarFechaHasta.SelectedDate = (DateTime)mensaje.tme_fechaHasta;
                }
                else
                {
                    PanelFecha.Visible = checkboxImportante.Checked;
                    clienteCombo = mensaje.tme_codClienteDestinatario;
                }
                txt_asunto.Text = mensaje.tme_asunto;
                txt_mensaje.Text = mensaje.tme_mensaje; //  txt_mensaje.Content = mensaje.tme_mensaje;

                pnl_grilla.Visible = false;
                pnl_formulario.Visible = true;

            }
            else
            {
                CheckBoxListSucursales.Enabled = false;
                if (CheckBoxListSucursales.Items != null)
                {
                    for (int i = 0; i < CheckBoxListSucursales.Items.Count; i++)
                    {
                        CheckBoxListSucursales.Items[i].Selected = false;
                    }
                }
            }
        }

        public override void Insertar()
        {
            Session["GestionMensajeNewV4_isNuevo"] = true;
            Session["GestionMensajeNewV4_tme_todos"] = 0;
            Session["GestionMensajeNewV4_tme_codigo"] = 0;
            Session["GestionMensajeNewV4_tme_todosSucursales"] = 0;
            PanelFecha.Visible = false;
            checkboxImportante.Checked = false;
            CalendarFechaDesde.SelectedDate = DateTime.Now;
            CalendarFechaHasta.SelectedDate = DateTime.Now;
            pnl_grilla.Visible = false;
            pnl_formulario.Visible = true;
            txt_asunto.Text = string.Empty;
            txt_mensaje.Text = string.Empty; //txt_mensaje.Content = string.Empty;
            //
            SetearComboSucursalCliente();
        }
        public void SetearComboSucursalCliente()
        {

            if (CheckBoxListSucursales.Items != null)
            {
                for (int i = 0; i < CheckBoxListSucursales.Items.Count; i++)
                {
                    CheckBoxListSucursales.Items[i].Selected = false;
                }
            }
        }
        public override void Eliminar(int pIdMensaje)
        {
            if (DKintranet.Codigo.clases.cBaseAdmin.isEliminar(consPalabraClave))
            {
                WebService.ElimimarMensajeNewPorId(pIdMensaje);
                gv_datos.DataBind();
            }
        }
        protected void cmd_nuevo_Click(object sender, EventArgs e)
        {

            Response.Redirect("~/admin/pages/GestionMensajeNewV4Editar.aspx?id=" + "0");
        }
        protected void cmd_buscar_Click(object sender, EventArgs e)
        {
            Session["GestionMensajeNewV4_Filtro"] = txt_buscar.Text;
        }
        protected void gv_datos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Modificar")
            {
                Response.Redirect("~/admin/pages/GestionMensajeNewV4Editar.aspx?id=" + e.CommandArgument);
            }
            else if (e.CommandName == "Eliminar")
            {
                LlamarMetodosAcciones(Constantes.cSQL_DELETE, Convert.ToInt32(e.CommandArgument), consPalabraClave);
            }
        }
        protected void cmd_cancelar_Click(object sender, EventArgs e)
        {
            pnl_grilla.Visible = true;
            pnl_formulario.Visible = false;
        }
        protected void cmd_guardar_Click(object sender, EventArgs e)
        {
            if (Session["GestionMensajeNewV4_tme_codigo"] != null && Session["BaseAdmin_Usuario"] != null)
            {
                int codigoMensaje = Convert.ToInt32(Session["GestionMensajeNewV4_tme_codigo"]);
                if ((codigoMensaje == 0 && DKintranet.Codigo.clases.cBaseAdmin.isAgregar(consPalabraClave)) || (codigoMensaje != 0 && DKintranet.Codigo.clases.cBaseAdmin.isEditar(consPalabraClave)))
                {

                    int codigoUsuarioEnSession = ((DKintranet.Codigo.capaDatos.Usuario)Session["BaseAdmin_Usuario"]).id;
                    DateTime? fechaDesde = null;
                    DateTime? fechaHasta = null;
                    bool importante = checkboxImportante.Checked;

                    if (checkboxImportante.Checked)
                    {
                        fechaDesde = CalendarFechaDesde.SelectedDate;
                        fechaHasta = CalendarFechaHasta.SelectedDate;
                    }
                    List<string> listaSucursal = new List<string>();
                    //fechaDesde, fechaHasta, importante
                    bool isTodos = false;
                    bool isTodosSucursal = false;
                    if (Convert.ToBoolean(Session["GestionMensajeNewV4_isNuevo"]) == true)
                    {
                        isTodosSucursal = true;
                        if (CheckBoxListSucursales.Items != null)
                        {
                            for (int i = 0; i < CheckBoxListSucursales.Items.Count; i++)
                            {
                                if (CheckBoxListSucursales.Items[i].Selected)
                                {
                                    listaSucursal.Add(CheckBoxListSucursales.Items[i].Value);
                                }
                            }
                        }
                    }
                    string sucursales = null;
                    int resultado = WebService.ActualizarInsertarMensajeNew(codigoMensaje, txt_asunto.Text, txt_mensaje.Text, fechaDesde, fechaHasta, importante, sucursales);
                }
            }
            gv_datos.DataBind();
            pnl_grilla.Visible = true;
            pnl_formulario.Visible = false;
        }
        protected void checkboxImportante_CheckedChanged(object sender, EventArgs e)
        {
            PanelFecha.Visible = checkboxImportante.Checked;
            CalendarFechaDesde.SelectedDate = DateTime.Now;
            CalendarFechaHasta.SelectedDate = DateTime.Now;

        }



        protected void CheckBoxListSucursales_Load1(object sender, EventArgs e)
        {

        }

        protected void ObjectDataSource1_DataBinding(object sender, EventArgs e)
        {

        }
    }
}