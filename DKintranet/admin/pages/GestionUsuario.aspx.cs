using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DKbase.web;
using DKintranet.Codigo.clases;

namespace DKintranet.admin.pages
{
    public partial class GestionUsuario : cBaseAdmin
    {
        public const string consPalabraClave = "gestionusuario";
        protected void Page_Load(object sender, EventArgs e)
        {
            Seguridad(consPalabraClave);
            if (!IsPostBack)
            {
                Session["GestionUsuario_Filtro"] = null;
                Session["GestionUsuario_Usu_codigo"] = null;

            }
        }
        protected void cmd_nuevo_Click(object sender, EventArgs e)
        {
            LlamarMetodosAcciones(Constantes.cSQL_INSERT, null, consPalabraClave);
        }

        protected void cmd_buscar_Click(object sender, EventArgs e)
        {
            Session["GestionUsuario_Filtro"] = txt_buscar.Text;
            gv_datos.DataBind();
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
                DKintranet.Codigo.clases.Seguridad.EliminarUsuario(Convert.ToInt32(e.CommandArgument));
                gv_datos.DataBind();
            }
            else if (e.CommandName == "Contraseña")
            {
                Session["GestionUsuario_Usu_codigo"] = e.CommandArgument;
                pnl_grilla.Visible = false;
                pnl_Contraseña.Visible = true;
            }
        }
        public override void Modificar(int pIdUsuario)
        {
            Session["GestionUsuario_Usu_codigo"] = pIdUsuario;
            cUsuario usuario = DKintranet.Codigo.clases.Seguridad.RecuperarUsuarioPorId(pIdUsuario);
            txtNombre.Text = usuario.usu_nombre;
            txtApellido.Text = usuario.usu_apellido;
            txtObservaciones1.Text = usuario.usu_observacion;
            txtMail.Text = usuario.usu_mail;
            txtLogin.Text = usuario.usu_login;
            cmbRol.DataBind();
            cmbRol.SelectedIndex = cmbRol.Items.IndexOf(cmbRol.Items.FindByValue(usuario.usu_codRol.ToString()));
            if (usuario.usu_codRol == Constantes.cROL_ADMINISTRADORCLIENTE || usuario.usu_codRol == Constantes.cROL_OPERADORCLIENTE)
            {
                cmbCliente.Enabled = true;
            }
            else
            {
                cmbCliente.SelectedIndex = -1;
                cmbCliente.Enabled = false;
            }
            cmbCliente.DataBind();
            if (usuario.usu_codCliente != null)
            {
                cmbCliente.SelectedIndex = cmbCliente.Items.IndexOf(cmbCliente.Items.FindByValue(usuario.usu_codCliente.ToString()));
            }
            else
            {
                cmbCliente.SelectedIndex = -1;
            }
            PanelContraseña.Visible = false;
            pnl_grilla.Visible = false;
            pnl_formulario.Visible = true;
        }
        public override void Insertar()
        {
            Session["GestionUsuario_Usu_codigo"] = 0;
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtContraseña.Text = string.Empty;
            txtRepetirContraseña.Text = string.Empty;
            txtLogin.Text = string.Empty;
            txtObservaciones1.Text = string.Empty;
            txtMail.Text = string.Empty;
            cmbRol.SelectedIndex = -1;
            cmbCliente.Enabled = false;
            cmbCliente.SelectedIndex = -1;
            PanelContraseña.Visible = true;
            pnl_grilla.Visible = false;
            pnl_formulario.Visible = true;
        }
        public override void CambiarEstado(int pIdUsuario)
        {
            if (Session["BaseAdmin_Usuario"] != null)
            {
                int codigoUsuarioEnSession = ((Usuario)Session["BaseAdmin_Usuario"]).id;
                cUsuario usuario = DKintranet.Codigo.clases.Seguridad.RecuperarUsuarioPorId(pIdUsuario);
                int estadoUsuario = usuario.usu_estado == Constantes.cESTADO_ACTIVO ? Constantes.cESTADO_INACTIVO : Constantes.cESTADO_ACTIVO;
                DKintranet.Codigo.clases.Seguridad.CambiarEstadoUsuarioPorId(usuario.usu_codigo, estadoUsuario, codigoUsuarioEnSession);
                gv_datos.DataBind();
            }
        }
        protected void cmd_cancelar_Click(object sender, EventArgs e)
        {
            pnl_grilla.Visible = true;
            pnl_formulario.Visible = false;
            pnl_Contraseña.Visible = false;
        }

        protected void cmd_guardar_Click(object sender, EventArgs e)
        {
            if (CustomValidatorLogin.IsValid)
            {
                if (Session["GestionUsuario_Usu_codigo"] != null && Session["BaseAdmin_Usuario"] != null)
                {
                    int codUsuario = Convert.ToInt32(Session["GestionUsuario_Usu_codigo"]);
                    if ((codUsuario == 0 && DKintranet.Codigo.clases.cBaseAdmin.isAgregar(consPalabraClave)) || (codUsuario != 0 && DKintranet.Codigo.clases.cBaseAdmin.isEditar(consPalabraClave)))
                    {
                        int? codCliente = Convert.ToInt32(cmbCliente.SelectedValue) != -1 ? (int?)Convert.ToInt32(cmbCliente.SelectedValue) : null;
                        int? codigoUsuarioEnSession = ((Usuario)Session["BaseAdmin_Usuario"]).id;
                        DKintranet.Codigo.clases.Seguridad.InsertarActualizarUsuario(codUsuario, Convert.ToInt32(cmbRol.SelectedValue), codCliente, txtNombre.Text, txtApellido.Text, txtMail.Text, txtLogin.Text, txtContraseña.Text, txtObservaciones1.Text, codigoUsuarioEnSession);
                    }
                }
                gv_datos.DataBind();
                pnl_grilla.Visible = true;
                pnl_formulario.Visible = false;
            }
        }
        protected void CustomValidatorLogin_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool resultado = true;
            if (Session["GestionUsuario_Usu_codigo"] != null)
            {
                resultado = !DKintranet.Codigo.clases.Seguridad.IsRepetidoLogin(Convert.ToInt32(Session["GestionUsuario_Usu_codigo"]), args.Value);
            }
            CustomValidatorLogin.ErrorMessage = "Login repetido";
            args.IsValid = resultado;
        }
        protected void cmbRol_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbRol.SelectedValue) == Constantes.cROL_ADMINISTRADORCLIENTE || Convert.ToInt32(cmbRol.SelectedValue) == Constantes.cROL_OPERADORCLIENTE)
            {
                cmbCliente.Enabled = true;
            }
            else
            {
                cmbCliente.SelectedIndex = -1;
                cmbCliente.Enabled = false;
            }
        }
        protected void btnGuardarContraseña_Click(object sender, EventArgs e)
        {
            if (Session["GestionUsuario_Usu_codigo"] != null && Session["BaseAdmin_Usuario"] != null)
            {
                int codigoUsuarioEnSession = ((Usuario)Session["BaseAdmin_Usuario"]).id;
                cUsuario objUsuario = null;
                DKbase.web.capaDatos.cClientes objCliente = null;
                objUsuario = DKintranet.Codigo.clases.Seguridad.RecuperarUsuarioPorId(Convert.ToInt32(Session["GestionUsuario_Usu_codigo"]));
                DKintranet.Codigo.clases.Seguridad.CambiarContraseñaUsuario(Convert.ToInt32(Session["GestionUsuario_Usu_codigo"]), txtContraseñaCambiar.Text, codigoUsuarioEnSession);
                if (objUsuario.usu_codRol == Constantes.cROL_ADMINISTRADORCLIENTE)
                {
                    objCliente = WebService.RecuperarClienteAdministradorPorIdUsuarios(objUsuario.usu_codigo);
                }
                gv_datos.DataBind();
                pnl_grilla.Visible = true;
                pnl_Contraseña.Visible = false;
            }
        }
    }
}