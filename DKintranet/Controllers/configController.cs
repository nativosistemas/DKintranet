using DKintranet.Codigo.capaDatos;
using DKintranet.Codigo.clases;
using DKintranet.Codigo.clases.Generales;
using DKintranet.Filters;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DKbase.web.capaDatos;
using DKbase.web;

namespace DKintranet.Controllers
{
    public class configController : Controller
    {
        public ActionResult LoginIntranet()
        {
            //return Redirect("/home/index.aspx");
            return View();
        }
        public string RecuperarOferta(int pId)
        {
            int? id = pId;
            List<cOferta> resultado = new List<cOferta>();
            cOferta o = WebService.RecuperarTodasOfertas_generico().FirstOrDefault(x => x.ofe_idOferta == id);
            if (o != null)
                resultado.Add(o);
            foreach (var item in resultado)
            {
                List<cArchivo> listaArchivo = WebService.RecuperarTodosArchivos(item.ofe_idOferta, "ofertas", string.Empty);
                if (listaArchivo != null)
                {
                    if (listaArchivo.Count > 0)
                    {
                        item.nameImagen = listaArchivo[0].arc_nombre;
                    }
                }
            }
            return resultado == null ? string.Empty : Serializador.SerializarAJson(resultado);
        }
        public string loginCarrito(string pName, string pPass, int pIdOferta)
        {
            string resultado = null;
            resultado = login(pName, pPass);
            if (resultado == "Ok" && Session["clientesDefault_Cliente"] != null)
            {
                cClientes oCliente = (cClientes)Session["clientesDefault_Cliente"];
                WebService.InsertarOfertaRating(pIdOferta, oCliente.cli_codigo, true);
                cOferta o = WebService.RecuperarOfertaPorId(pIdOferta);
                if (o != null)
                {
                    Session["home_Tipo"] = o.ofe_tipo;
                    if (o.tfr_codigo != null)
                        Session["home_IdTransfer"] = o.tfr_codigo;
                }
                Session["home_IdOferta"] = pIdOferta;
            }
            return resultado;
        }
        public ActionResult SignOff()
        {
            System.Web.HttpContext.Current.Session["main_ListaOferta"] = null;
            System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] = null;
            System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] = null;
            System.Web.HttpContext.Current.Session["clientesDefault_CantListaMensajeFechaHora"] = null;
            System.Web.HttpContext.Current.Session["isMostrarOferta"] = null;
            System.Web.HttpContext.Current.Session["master_ListaSinPermisoSecciones"] = null;
            System.Web.HttpContext.Current.Session["BaseAdmin_PermisosRol"] = null;
            return Content("Ok");
        }
        //public ActionResult loginTest(string pName, string pPass)
        //{
        //    return Content(login( pName,  pPass));
        //}
        public string logJS(List<string> pList)
        {

            string Parameters = string.Empty;
            if (System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                Parameters += "<" + "login" + ">";
                Parameters += ((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_login;
                Parameters += "</" + "login" + ">";
                Parameters += "<" + "codigoCliente" + ">";
                Parameters += ((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo;
                Parameters += "</" + "codigoCliente" + ">";

            }
            if (System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] != null)
            {
                Parameters += "<" + "idUsuarioLog" + ">";
                Parameters += ((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]).idUsuarioLog;
                Parameters += "</" + "idUsuarioLog" + ">";
            }
            string funFlecha = string.Empty;
            if (pList != null)
                funFlecha = string.Join(" | ", pList);
            bool isNotGeneroError = capaLogRegistro.spError("JavaScript", Parameters, funFlecha, null, null, null, null, null, DateTime.Now, "JAVASCRIPT");
            return "Ok";
        }
        public string login(string pName, string pPass)
        {
            string IdSuc = "CC";
            string GrupoCliente = "";
            string resultado = null;
            string userAgent = System.Web.HttpContext.Current.Request.UserAgent;
            string ip = System.Web.HttpContext.Current.Server.HtmlEncode(System.Web.HttpContext.Current.Request.UserHostAddress);
            string hostName = System.Web.HttpContext.Current.Request.UserHostName;
            Usuario user = Codigo.clases.Seguridad.Login(pName, pPass, ip, hostName, userAgent);
            if (user != null)
            {
                if (user.id != -1)
                {
                    if (user.usu_estado == Constantes.cESTADO_ACTIVO)
                    {
                        if (FuncionesPersonalizadas.isUserTomaPedidoIntranet(user)) //"TomarPedidoCC"
                        {
                            Autenticacion objAutenticacion = new Autenticacion();
                            objAutenticacion.UsuarioNombre = System.Configuration.ConfigurationManager.AppSettings["ws_usu"];
                            objAutenticacion.UsuarioClave = System.Configuration.ConfigurationManager.AppSettings["ws_psw"];
                            WebService.CredencialAutenticacion = objAutenticacion;

                            List<cClientes> clientes = WebService.RecuperarTodosClientes();
                            System.Web.HttpContext.Current.Session["usuario_TomarPedido_listaClientes"] = clientes;

                            //System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] = clientes.FirstOrDefault();//WebService.RecuperarClientePorId((int)clientes[0].cli_codigo);

                            if (user != null)
                            {
                                System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] = user;
                                List<string> listaPermisoDenegados = FuncionesPersonalizadas.RecuperarSinPermisosSecciones(user.id);
                                System.Web.HttpContext.Current.Session["master_ListaSinPermisoSecciones"] = listaPermisoDenegados;
                                CargarAccionesEnVariableSession();
                                System.Web.HttpContext.Current.Session["ClientesBase_isLogeo"] = true;
                                System.Web.HttpContext.Current.Session["isMostrarOferta"] = false;
                                resultado = "OkTomarPedido";
                            }
                        }
                        else
                        {
                            resultado = "Usuario con rol sin permiso.";
                        }
                    }
                    else
                    {
                        if (user.usu_codCliente == null)
                        {
                            resultado = "Usuario no asigando cliente";
                        }
                        else
                        {
                            resultado = "Usuario inactivo";
                        }
                    }
                }
                else
                {
                    resultado = "Mail o contraseña erróneo";
                }
            }
            else
            {
                resultado = "Error en el servidor";
            }


            return resultado;
        }
        public static void CargarAccionesEnVariableSession()
        {
            if (System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] != null)
            {
                ListaAcccionesRol listaAcciones = Codigo.clases.Seguridad.RecuperarTodasAccionesPorIdRol((((Usuario)(System.Web.HttpContext.Current.Session["clientesDefault_Usuario"])).idRol));
                System.Web.HttpContext.Current.Session["BaseAdmin_PermisosRol"] = listaAcciones;
            }
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult inhabilitado()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public ActionResult sinpermiso()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public ActionResult sinusodll()
        {
            return View();
        }
        public static int ObtenerCantidadMensaje()
        {
            int resultado = 0;
            if ((System.Web.HttpContext.Current.Session["clientesDefault_CantListaMensaje"] == null ||
                System.Web.HttpContext.Current.Session["clientesDefault_CantListaMensajeFechaHora"] == null)
                && System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
                Codigo.clases.FuncionesPersonalizadas.CargarMensajeActualizado(((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo);
            if (System.Web.HttpContext.Current.Session["clientesDefault_CantListaMensaje"] != null)
                resultado = Convert.ToInt32(System.Web.HttpContext.Current.Session["clientesDefault_CantListaMensaje"]);
            return resultado;
        }
        // GET: config
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public ActionResult mensajes()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public bool CambiarEstadoMensaje(int pIdMensaje, int pIdEstado)
        {
            WebService.CambiarEstadoMensajePorId(pIdMensaje, pIdEstado);
            System.Web.HttpContext.Current.Session["clientesDefault_CantListaMensajeFechaHora"] = null;
            return true;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public ActionResult perfil()
        {
            ViewBag.ContraseniaNueva = null;
            ViewBag.ContraseniaVieja = null;
            ViewBag.ContraseniaNuevaRepetir = null;
            if (System.Web.HttpContext.Current.Session["perfil_CambiarContraseña"] != null &&
                Convert.ToInt32(System.Web.HttpContext.Current.Session["perfil_CambiarContraseña"]) == 0 &&
                System.Web.HttpContext.Current.Session["perfil_idContraseniaVieja"] != null &&
                System.Web.HttpContext.Current.Session["perfil_idContraseniaNueva"] != null)
            {
                ViewBag.ContraseniaVieja = System.Web.HttpContext.Current.Session["perfil_idContraseniaVieja"].ToString();
                ViewBag.ContraseniaNueva = System.Web.HttpContext.Current.Session["perfil_idContraseniaNueva"].ToString();
                ViewBag.ContraseniaNuevaRepetir = System.Web.HttpContext.Current.Session["perfil_idContraseniaNueva"].ToString();
            }
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador", isCheckOPERADORCLIENTE = true)]
        public ActionResult usuarios()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        [HttpPost]
        public ActionResult ActionCambiarContrasenia(string idContraseniaVieja, string idContraseniaNueva)
        {
            System.Web.HttpContext.Current.Session["perfil_idContraseniaVieja"] = idContraseniaVieja;
            System.Web.HttpContext.Current.Session["perfil_idContraseniaNueva"] = idContraseniaNueva;
            System.Web.HttpContext.Current.Session["perfil_CambiarContraseña"] = CambiarContraseñaPersonal(idContraseniaVieja, idContraseniaNueva);
            return RedirectToAction("perfil");
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public ActionResult mediosdepago1()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public ActionResult mediosdepago2()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public ActionResult mediosdepago3()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public ActionResult mediosdepago4()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public ActionResult mediosdepago5()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "DESCARGAS")]
        public ActionResult catalogo()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "DESCARGAS")]
        public ActionResult descarga()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public int CambiarContraseñaPersonal(string pContraseñaVieja, string pContraseñaNueva)
        {
            int id = -1;
            if (System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] != null)
            {
                cUsuario objUsuario = null;
                objUsuario = Seguridad.RecuperarUsuarioPorId(((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]).id);
                if (pContraseñaVieja == objUsuario.usu_pswDesencriptado)
                {
                    id = WebService.CambiarContraseñaUsuarioPersonal(((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]).id, pContraseñaVieja, pContraseñaNueva);
                    if (((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]).idRol == Constantes.cROL_ADMINISTRADORCLIENTE)
                    {
                        WebService.ModificarPasswordWEB(((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_login, objUsuario.usu_pswDesencriptado, pContraseñaNueva);
                    }
                }
                else { id = 0; }
            }
            return id;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public bool IsBanderaUsarDll()
        {
            bool resultado = WebService.IsBanderaCodigo(Constantes.cBAN_SERVIDORDLL);
            return resultado;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public bool IsBanderaUsarDllSucursal(string pSucursal)
        {
            bool resultado = WebService.IsBanderaCodigo(pSucursal);
            return resultado;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public int IsHacerPedidos(string pSucursal)
        {
            int resultado = 0;
            if (!IsBanderaUsarDllSucursal(pSucursal))
            {
                resultado = 1;
            }
            else
            {
                if (System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
                {
                    cClientes objCliente = WebService.RecuperarClientePorId(((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo);
                    if (objCliente != null)
                    {
                        ((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_estado = objCliente.cli_estado;
                    }
                    if (((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_estado.ToUpper() == Constantes.cESTADO_INH)
                    {
                        resultado = 2;
                    }
                }
            }
            return resultado;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public int GuardarUsuario(int pIdUsuario, string pNombre, string pApellido, string pMail, string pLogin, string pContraseña, string pObservaciones1, List<string> pListaPermisos)
        {
            if (Session["clientesDefault_Cliente"] == null)
                return -1;
            int? codCliente = ((cClientes)Session["clientesDefault_Cliente"]).cli_codigo;// Convert.ToInt32(cmbCliente.SelectedValue) != -1 ? (int?)Convert.ToInt32(cmbCliente.SelectedValue) : null;
            int? codigoUsuarioEnSession = null;
            if (Codigo.clases.Seguridad.IsRepetidoLogin(pIdUsuario, pLogin))
                return -2;
            int codUsuarioInsertarActualizar = DKintranet.Codigo.clases.Seguridad.InsertarActualizarUsuario(pIdUsuario, Constantes.cROL_OPERADORCLIENTE, codCliente, pNombre, pApellido, pMail, pLogin, pContraseña, pObservaciones1, codigoUsuarioEnSession);
            WebService.InsertarSinPermisoUsuarioIntranetPorIdUsuario(codUsuarioInsertarActualizar, pListaPermisos);
            return codUsuarioInsertarActualizar;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public int CambiarEstadoUsuario(int pIdUsuario)
        {
            if (Session["clientesDefault_Usuario"] == null)
                return -1;
            int codigoUsuarioEnSession = ((Usuario)Session["clientesDefault_Usuario"]).id;
            cUsuario usuario = DKintranet.Codigo.clases.Seguridad.RecuperarUsuarioPorId(pIdUsuario);
            int estadoUsuario = usuario.usu_estado == Constantes.cESTADO_ACTIVO ? Constantes.cESTADO_INACTIVO : Constantes.cESTADO_ACTIVO;
            DKintranet.Codigo.clases.Seguridad.CambiarEstadoUsuarioPorId(usuario.usu_codigo, estadoUsuario, codigoUsuarioEnSession);
            return 0;

        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public int EliminarUsuario(int pIdUsuario)
        {
            DKintranet.Codigo.clases.Seguridad.EliminarUsuario(pIdUsuario);
            return 0;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public int CambiarContraseñaUsuario(int pIdUsuario, string pPass)
        {
            if (Session["clientesDefault_Usuario"] == null)
                return -1;
            int codigoUsuarioEnSession = ((Usuario)Session["clientesDefault_Usuario"]).id;
            DKintranet.Codigo.clases.Seguridad.CambiarContraseñaUsuario(pIdUsuario, pPass, codigoUsuarioEnSession);
            return 0;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string ObtenerUsuarios()
        {
            if (Session["clientesDefault_Cliente"] == null)
                return null;
            List<cUsuario> lista = DKintranet.Codigo.clases.AccesoGrilla.GetUsuariosDeCliente("usu_codigo", ((DKbase.web.capaDatos.cClientes)Session["clientesDefault_Cliente"]).cli_codigo, null);
            return Codigo.clases.Generales.Serializador.SerializarAJson(lista);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public FileStreamResult GenerateDocument(int id)
        {
            if (Session["clientesDefault_Cliente"] == null)
                return null;

            try
            {
                DataTable dt = null;
                string nameFile = string.Empty;
                switch (id)
                {
                    case 1:
                    case 5:
                        dt = Codigo.capaDatos.capaProductos.DescargaTodosProductos(((cClientes)Session["clientesDefault_Cliente"]).cli_codprov);
                        if (id == 1)
                            nameFile = "Productos.xls";
                        else
                            nameFile = "Productos.csv";
                        break;
                    case 2:
                    case 6:
                        dt = Codigo.capaDatos.capaProductos.DescargaTodosProductosDrogueria(((cClientes)Session["clientesDefault_Cliente"]).cli_codprov);
                        if (id == 2)
                            nameFile = "ProductosDrogueria.xls";
                        else
                            nameFile = "ProductosDrogueria.csv";
                        break;
                    case 3:
                    case 7:
                        dt = Codigo.capaDatos.capaProductos.DescargaTodosProductosPerfumeria(((cClientes)Session["clientesDefault_Cliente"]).cli_codprov);
                        if (id == 3)
                            nameFile = "ProductosPerfumeria.xls";
                        else
                            nameFile = "ProductosPerfumeria.csv";
                        break;
                    case 4:
                    case 8:
                        dt = Codigo.capaDatos.capaProductos.DescargaTodosProductosEnOferta();
                        if (id == 4)
                            nameFile = "ProductosEnOferta.xls";
                        else
                            nameFile = "ProductosEnOferta.csv";
                        break;
                    case 9:
                        dt = Codigo.capaDatos.capaProductos.DescargaMedicamentosYAccesoriosNoIncluidosEnAlfaBeta();
                        nameFile = "MedicamentosYAccesoriosNoIncluidosEnAlfaBeta.csv";
                        break;
                    default:
                        break;
                }
                if (dt != null)
                {
                    String path = Convert.ToString(Constantes.cRaizArchivos + @"\temp\");
                    String f = path + nameFile;
                    if (id == 1 || id == 2 || id == 3 || id == 4)
                    {
                        DataSet ds = new DataSet();
                        ds.Tables.Add(dt);
                        ds.WriteXml(f);
                    }
                    else if (id == 5 || id == 6 || id == 7)
                    {
                        GenerarArchivo(f, dt);
                    }
                    else if (id == 8)
                    {
                        GenerarArchivo_ProductosEnOferta(f, dt);
                    }
                    else if (id == 9)
                    {
                        GenerarArchivo_MedicamentosYAccesoriosNoIncluidosEnAlfaBeta(f, dt);
                    }
                    var fileStream = new FileStream(f, FileMode.Open, FileAccess.Read);
                    //return new FileStreamResult(fileStream, MimeMapping.GetMimeMapping(f));
                    return File(fileStream, MimeMapping.GetMimeMapping(f), nameFile);
                }
            }
            catch (Exception ex)
            {
                FuncionesPersonalizadas.grabarLog(System.Reflection.MethodBase.GetCurrentMethod(), ex, DateTime.Now, id);

            }
            return null;
        }
        public static void GenerarArchivo(string RutaNombreArchivo, DataTable pTabla)
        {
            if (pTabla != null && RutaNombreArchivo != null)
            {
                if (pTabla.Rows.Count > 0)
                {
                    string path = RutaNombreArchivo;
                    FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
                    //StreamWriter writer = new StreamWriter(stream);
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        string encabezado = string.Empty;
                        encabezado += "Tipo" + Constantes.cSeparadorCSV;
                        encabezado += "Producto" + Constantes.cSeparadorCSV;
                        encabezado += "AlfaBeta" + Constantes.cSeparadorCSV;
                        encabezado += "Troquel" + Constantes.cSeparadorCSV;
                        encabezado += "CodBarraPrinc" + Constantes.cSeparadorCSV;
                        encabezado += "Laboratorio" + Constantes.cSeparadorCSV;
                        encabezado += "Precio" + Constantes.cSeparadorCSV;
                        encabezado += "Neto" + Constantes.cSeparadorCSV;
                        encabezado += "CadenaFrio" + Constantes.cSeparadorCSV;
                        encabezado += "RequiereVale" + Constantes.cSeparadorCSV;
                        encabezado += "Trazable";

                        writer.WriteLine(encabezado);
                        //resultado += "\n";
                        foreach (DataRow item in pTabla.Rows)
                        {
                            string fila = string.Empty;
                            string Tipo = string.Empty;
                            if (item["Tipo"] != DBNull.Value)
                            {
                                Tipo = item["Tipo"].ToString();
                            }
                            fila += Tipo + Constantes.cSeparadorCSV;
                            string Producto = string.Empty;
                            if (item["Producto"] != DBNull.Value)
                            {
                                Producto = item["Producto"].ToString();
                            }
                            fila += Producto + Constantes.cSeparadorCSV;
                            string AlfaBeta = string.Empty;
                            if (item["AlfaBeta"] != DBNull.Value)
                            {
                                AlfaBeta = item["AlfaBeta"].ToString();
                            }
                            fila += AlfaBeta + Constantes.cSeparadorCSV;
                            string Troquel = string.Empty;
                            if (item["Troquel"] != DBNull.Value)
                            {
                                Troquel = item["Troquel"].ToString();
                            }
                            fila += Troquel + Constantes.cSeparadorCSV;
                            string CodBarraPrinc = string.Empty;
                            if (item["CodBarraPrinc"] != DBNull.Value)
                            {
                                CodBarraPrinc = item["CodBarraPrinc"].ToString();
                            }
                            fila += CodBarraPrinc + Constantes.cSeparadorCSV;
                            string Laboratorio = string.Empty;
                            if (item["Laboratorio"] != DBNull.Value)
                            {
                                Laboratorio = item["Laboratorio"].ToString();
                            }
                            fila += Laboratorio + Constantes.cSeparadorCSV;
                            string Precio = string.Empty;
                            if (item["Precio"] != DBNull.Value)
                            {
                                Precio = item["Precio"].ToString();
                            }
                            fila += Precio + Constantes.cSeparadorCSV;
                            string Neto = string.Empty;
                            if (item["Neto"] != DBNull.Value)
                            {
                                Neto = item["Neto"].ToString();
                            }
                            fila += Neto + Constantes.cSeparadorCSV;
                            string CadenaFrio = string.Empty;
                            if (item["CadenaFrio"] != DBNull.Value)
                            {
                                CadenaFrio = item["CadenaFrio"].ToString();
                            }
                            fila += CadenaFrio + Constantes.cSeparadorCSV;
                            string RequiereVale = string.Empty;
                            if (item["RequiereVale"] != DBNull.Value)
                            {
                                RequiereVale = item["RequiereVale"].ToString();
                            }
                            fila += RequiereVale + Constantes.cSeparadorCSV;
                            string Trazable = string.Empty;
                            if (item["Trazable"] != DBNull.Value)
                            {
                                Trazable = item["Trazable"].ToString();
                            }
                            fila += Trazable;
                            writer.WriteLine(fila);
                        }
                    }
                    //writer.Close();
                }
            }
        }
        public static void GenerarArchivo_ProductosEnOferta(string RutaNombreArchivo, DataTable pTabla)
        {
            if (pTabla != null && RutaNombreArchivo != null)
            {
                if (pTabla.Rows.Count > 0)
                {
                    string path = RutaNombreArchivo;
                    FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
                    //StreamWriter writer = new StreamWriter(stream);
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        string encabezado = string.Empty;
                        encabezado += "Nombre producto" + Constantes.cSeparadorCSV;
                        encabezado += "Codigo Barra" + Constantes.cSeparadorCSV;
                        encabezado += "Unidades Mínimas" + Constantes.cSeparadorCSV;
                        encabezado += "% de descuento";
                        writer.WriteLine(encabezado);
                        foreach (DataRow item in pTabla.Rows)
                        {
                            string fila = string.Empty;
                            string NombreProducto = string.Empty;
                            if (item["Nombre producto"] != DBNull.Value)
                            {
                                NombreProducto = item["Nombre producto"].ToString();
                            }
                            fila += NombreProducto + Constantes.cSeparadorCSV;
                            string CodigoBarra = string.Empty;
                            if (item["Codigo Barra"] != DBNull.Value)
                            {
                                CodigoBarra = item["Codigo Barra"].ToString();
                            }
                            fila += CodigoBarra + Constantes.cSeparadorCSV;
                            string UnidadesMínimas = string.Empty;
                            if (item["Unidades Mínimas"] != DBNull.Value)
                            {
                                UnidadesMínimas = item["Unidades Mínimas"].ToString();
                            }
                            fila += UnidadesMínimas + Constantes.cSeparadorCSV;
                            string Descuento = string.Empty;
                            if (item["% de descuento"] != DBNull.Value)
                            {
                                Descuento = item["% de descuento"].ToString();
                            }
                            fila += Descuento;
                            writer.WriteLine(fila);
                        }
                    }
                    //writer.Close();
                }
            }
        }

        public static void GenerarArchivo_MedicamentosYAccesoriosNoIncluidosEnAlfaBeta(string RutaNombreArchivo, DataTable pTabla)
        {
            if (pTabla != null && RutaNombreArchivo != null)
            {
                if (pTabla.Rows.Count > 0)
                {
                    string path = RutaNombreArchivo;
                    FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write);
                    //StreamWriter writer = new StreamWriter(stream);
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        string encabezado = string.Empty;
                        encabezado += "Tipo" + Constantes.cSeparadorCSV;
                        encabezado += "Producto" + Constantes.cSeparadorCSV;
                        encabezado += "AlfaBeta" + Constantes.cSeparadorCSV;
                        encabezado += "Troquel" + Constantes.cSeparadorCSV;
                        encabezado += "CodBarraPrinc" + Constantes.cSeparadorCSV;
                        encabezado += "Laboratorio" + Constantes.cSeparadorCSV;
                        encabezado += "Precio" + Constantes.cSeparadorCSV;
                        encabezado += "Neto" + Constantes.cSeparadorCSV;
                        encabezado += "CadenaFrio" + Constantes.cSeparadorCSV;
                        encabezado += "RequiereVale" + Constantes.cSeparadorCSV;
                        encabezado += "Trazable";
                        writer.WriteLine(encabezado);
                        foreach (DataRow item in pTabla.Rows)
                        {
                            string fila = string.Empty;
                            fila += item["Tipo"].ToString() + Constantes.cSeparadorCSV;
                            fila += item["Producto"].ToString() + Constantes.cSeparadorCSV;
                            fila += item["AlfaBeta"].ToString() + Constantes.cSeparadorCSV;
                            fila += item["Troquel"].ToString() + Constantes.cSeparadorCSV;
                            fila += item["CodBarraPrinc"].ToString() + Constantes.cSeparadorCSV;
                            fila += item["Laboratorio"].ToString() + Constantes.cSeparadorCSV;
                            fila += item["Precio"].ToString() + Constantes.cSeparadorCSV;
                            fila += item["Neto"].ToString() + Constantes.cSeparadorCSV;
                            fila += item["CadenaFrio"].ToString() + Constantes.cSeparadorCSV;
                            fila += item["RequiereVale"].ToString() + Constantes.cSeparadorCSV;
                            fila += item["Trazable"].ToString();

                            writer.WriteLine(fila);
                        }
                    }
                    //writer.Close();
                }
            }
        }
    }
}