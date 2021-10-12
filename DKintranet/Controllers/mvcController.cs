using DKintranet.Codigo;
using DKintranet.Codigo.capaDatos;
using DKintranet.Codigo.clases;
using DKintranet.Codigo.clases.Generales;
using DKintranet.Filters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DKintranet.Controllers
{
    public class mvcController : Controller
    {
        private string msgCarritoRepetido = "Carrito ya se encuentra facturado.";
        private string msgCarritoEnProceso = "Carrito se está procesando.";
        public static bool isDiferido()
        {
            //if (System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"] != null &&
            //    (System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString() == "Diferido" ||
            //    System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString() == "carritoDiferido"))
            if (System.Web.HttpContext.Current.Session["url_type"] != null &&
                 (System.Web.HttpContext.Current.Session["url_type"].ToString() == "Diferido" ||
                 System.Web.HttpContext.Current.Session["url_type"].ToString() == "carritoDiferido"))
            { return true; }
            return false;
        }
        public static bool isBuscador()
        {
            //if (System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"] != null &&
            //    (System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString() == "Buscador" ||
            //    System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString() == "carrito"))
            if (System.Web.HttpContext.Current.Session["url_type"] != null &&
                (System.Web.HttpContext.Current.Session["url_type"].ToString() == "Buscador" ||
                System.Web.HttpContext.Current.Session["url_type"].ToString() == "carrito"))
            { return true; }
            return false;
        }
        public static bool isSubirPedido()
        {
            //if (System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"] != null &&
            //    (System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString() == "subirpedido" ||
            //    System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString() == "subirarchivoresultado") ||
            //    System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString() == "subirarchivoresultado_msg")
            if (System.Web.HttpContext.Current.Session["url_type"] != null &&
               (System.Web.HttpContext.Current.Session["url_type"].ToString() == "subirpedido" ||
                 System.Web.HttpContext.Current.Session["url_type"].ToString() == "subirarchivoresultado" ||
               System.Web.HttpContext.Current.Session["url_type"].ToString() == "subirarchivoresultado_msg"))
            { return true; }
            return false;
        }
        public static bool isCarritoExclusivo()
        {
            //if (System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"] != null &&
            //    (System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString() == "carrito" ||
            //    System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString() == "carritoDiferido"))
            if (System.Web.HttpContext.Current.Session["url_type"] != null &&
                (System.Web.HttpContext.Current.Session["url_type"].ToString() == "carrito" ||
                System.Web.HttpContext.Current.Session["url_type"].ToString() == "carritoDiferido"))
            { return true; }
            return false;
        }
        public static bool isClienteTomaOferta()
        {
            if (System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null &&
                ((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_tomaOfertas)
                return true;
            return false;
        }
        public static bool isClienteTomaTransfer()
        {
            if (System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null &&
                ((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_tomaTransfers)
                return true;
            return false;
        }
        // GET: mvc
        public ActionResult Index()
        {
            return Redirect("/home/index.aspx");
            //return View();
        }
        [AuthorizePermisoAttribute(Permiso = "PEDIDOS", isCheckEstado = true)]
        public ActionResult Buscador()
        {
            //var rr = 2 / Convert.ToInt16("0");
            System.Web.HttpContext.Current.Session["url_type"] = "Buscador";
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "PEDIDOS", isCheckEstado = true)]
        public ActionResult Diferido()
        {
            System.Web.HttpContext.Current.Session["url_type"] = "Diferido";
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "PEDIDOS", isCheckEstado = true)]
        public ActionResult carrito()
        {
            System.Web.HttpContext.Current.Session["url_type"] = "carrito";
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "PEDIDOS", isCheckEstado = true)]
        public ActionResult carritoDiferido()
        {
            System.Web.HttpContext.Current.Session["url_type"] = "carritoDiferido";
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public ActionResult RecuperarProductos(string pTxtBuscador)
        {

            cjSonBuscadorProductos resultado = FuncionesPersonalizadas.RecuperarProductosBase_V3(null, pTxtBuscador, null, false, false);
            if (resultado != null)
            {
                System.Web.HttpContext.Current.Session["PedidosBuscador_productosOrdenar"] = resultado;
                return Content(Serializador.SerializarAJson(resultado));
            }
            else { return Content(string.Empty); }
        }
        [AuthorizePermisoAttribute(Permiso = "PEDIDOS", isCheckEstado = true)]
        public ActionResult subirpedido()
        {
            System.Web.HttpContext.Current.Session["url_type"] = "subirpedido";
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        [HttpPost]
        public ActionResult subirpedidoUpload()//HttpPostedFileBase pFile
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                if (Request.Form["HiddenFieldSucursalEleginda"] != null && file != null && file.ContentLength > 0)
                {
                    //////////////////////////////////
                    if (System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
                    {
                        cClientes objCliente = WebService.RecuperarClientePorId(((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo);
                        if (objCliente != null)
                        {
                            ((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_estado = objCliente.cli_estado;
                        }
                        if (((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_estado.ToUpper() == Constantes.cESTADO_INH)
                        {
                            //Response.Redirect(@System.Configuration.ConfigurationManager.AppSettings["raiz"] + @"clientes/pages/inhabilitado.aspx");
                            return RedirectToAction("inhabilitado", "config");
                        }
                        else
                        {
                            if (file.FileName != string.Empty)
                            {
                                string sucursal = Request.Form["HiddenFieldSucursalEleginda"];
                                Boolean? isNotRepetido = cSubirpedido.LeerArchivoPedido(file, sucursal);
                                if (isNotRepetido == null)
                                {
                                    return RedirectToAction("subirpedido");
                                }
                                else if (isNotRepetido.Value)
                                {
                                    return RedirectToAction("subirarchivoresultado");
                                }
                                else
                                {
                                    System.Web.HttpContext.Current.Session["subirpedido_isRepetido"] = true;
                                    return RedirectToAction("subirpedido");
                                }
                            }
                        }
                        //////////////////////////////////

                    }
                }
            }
            return RedirectToAction("subirpedido");
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public ActionResult subirarchivoresultado()
        {
            System.Web.HttpContext.Current.Session["url_type"] = "subirarchivoresultado";
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public ActionResult subirarchivoresultado_msg()
        {
            System.Web.HttpContext.Current.Session["url_type"] = "subirarchivoresultado_msg";
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "PEDIDOS")]
        public ActionResult estadopedidos()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public ActionResult estadopedidosresultado()
        {
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "PEDIDOS", isCheckEstado = true)]
        public ActionResult recuperador(string t)
        {
            int tipo = 0;
            if (int.TryParse(t, out tipo))
            {
                Session["clientes_pages_Recuperador_Tipo"] = t;
            }
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "PEDIDOS", isCheckEstado = true)]
        public ActionResult promocionescliente(string t)
        {
            Session["promocionescliente_TIPO"] = t;
            
            return View();
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public void funIsMostrarOferta(bool pIsMostrarOferta)
        {
            System.Web.HttpContext.Current.Session["isMostrarOferta"] = pIsMostrarOferta;
            //return Convert.ToBoolean(System.Web.HttpContext.Current.Session["isMostrarOferta"]);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public bool ModificarCantidadProductos(string pCodProducto, string pCodSucursal, int pCantidad)
        {
            bool resultado = false;
            try
            {
                for (int i = 0; i < ((cjSonBuscadorProductos)System.Web.HttpContext.Current.Session["PedidosBuscador_productosTodos"]).listaProductos.Count; i++)
                {
                    for (int x = 0; x < ((cjSonBuscadorProductos)System.Web.HttpContext.Current.Session["PedidosBuscador_productosTodos"]).listaProductos[i].listaSucursalStocks.Count; x++)
                    {
                        if (((cjSonBuscadorProductos)System.Web.HttpContext.Current.Session["PedidosBuscador_productosTodos"]).listaProductos[i].listaSucursalStocks[x].stk_codsuc == pCodSucursal)
                        {
                            ((cjSonBuscadorProductos)System.Web.HttpContext.Current.Session["PedidosBuscador_productosTodos"]).listaProductos[i].listaSucursalStocks[x].cantidadSucursal = pCantidad;
                            return true;
                            //resultado = true;
                            //break;
                        }
                    }
                }
                //((cjSonBuscadorProductos)System.Web.HttpContext.Current.Session["PedidosBuscador_productosTodos"]).listaProductos[pIndexProducto].listaSucursalStocks[pIndexSucursal].cantidadSucursal = pCantidad;

            }
            catch
            {

            }
            return resultado;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string RecuperarProductosVariasColumnas(string pTxtBuscador, string[] pListaColumna, bool pIsBuscarConOferta, bool pIsBuscarConTransfer)
        {
            cjSonBuscadorProductos resultado = FuncionesPersonalizadas.RecuperarProductosBase_V3(null, pTxtBuscador, pListaColumna.ToList(), pIsBuscarConOferta, pIsBuscarConTransfer);
            if (resultado != null)
            {
                System.Web.HttpContext.Current.Session["PedidosBuscador_productosTodos"] = new cjSonBuscadorProductos(resultado);
                int pPage = 1;
                System.Web.HttpContext.Current.Session["PedidosBuscador_pPage"] = pPage;
                return RecuperarProductos_generarPaginador(resultado, pPage);
            }
            else { return string.Empty; }
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string RecuperarProductosPaginador(int pPage)
        {
            if (System.Web.HttpContext.Current.Session["PedidosBuscador_productosTodos"] != null &&
                System.Web.HttpContext.Current.Session["PedidosBuscador_pPage"] != null)
            {
                System.Web.HttpContext.Current.Session["PedidosBuscador_pPage"] = pPage;
                cjSonBuscadorProductos resultado = new cjSonBuscadorProductos((cjSonBuscadorProductos)System.Web.HttpContext.Current.Session["PedidosBuscador_productosTodos"]);
                return RecuperarProductos_generarPaginador(resultado, pPage);
            }
            return string.Empty;
        }
        public static string RecuperarProductos_generarPaginador(cjSonBuscadorProductos resultado, int pPage)
        {
            int pageSize = Constantes.cCantidadFilaPorPagina;
            resultado.CantidadRegistroTotal = resultado.listaProductos.Count;
            if (!isSubirPedido() && resultado.listaProductos.Count > Constantes.cCantidadFilaPorPagina) //if (resultado.listaProductos.Count > Constantes.cCantidadFilaPorPagina)//
                resultado.listaProductos = resultado.listaProductos.Skip((pPage - 1) * pageSize).Take(pageSize).ToList();
            System.Web.HttpContext.Current.Session["PedidosBuscador_productosOrdenar"] = resultado;
            return Serializador.SerializarAJson(resultado);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string RecuperarProductosOrdenar(string pAscColumna, bool pAscOrdenar)
        {
            if (System.Web.HttpContext.Current.Session["PedidosBuscador_productosTodos"] != null &&
                System.Web.HttpContext.Current.Session["PedidosBuscador_pPage"] != null)
            {
                cjSonBuscadorProductos resultado = new cjSonBuscadorProductos((cjSonBuscadorProductos)System.Web.HttpContext.Current.Session["PedidosBuscador_productosTodos"]);

                if (pAscColumna == "PrecioFinal")
                {
                    if (pAscOrdenar)
                    {
                        resultado.listaProductos = resultado.listaProductos.OrderBy(x => x.PrecioFinal).ToList();
                    }
                    else
                    {
                        resultado.listaProductos = resultado.listaProductos.OrderByDescending(x => x.PrecioFinal).ToList();
                    }
                }
                else if (pAscColumna == "PrecioConDescuentoOferta")
                {
                    if (pAscOrdenar)
                    {
                        resultado.listaProductos = resultado.listaProductos.OrderBy(x => (x.pro_ofeporcentaje == 0 && x.pro_ofeunidades == 0) ? 0 : x.PrecioConDescuentoOferta).ToList();
                    }
                    else
                    {
                        resultado.listaProductos = resultado.listaProductos.OrderByDescending(x => (x.pro_ofeporcentaje == 0 && x.pro_ofeunidades == 0) ? 0 : x.PrecioConDescuentoOferta).ToList();
                    }
                }
                else if (pAscColumna == "pro_precio")
                {
                    if (pAscOrdenar)
                    {
                        resultado.listaProductos = resultado.listaProductos.OrderBy(x => x.pro_precio).ToList();
                    }
                    else
                    {
                        resultado.listaProductos = resultado.listaProductos.OrderByDescending(x => x.pro_precio).ToList();
                    }
                }
                else if (pAscColumna == "pro_nombre")
                {
                    if (pAscOrdenar)
                    {
                        resultado.listaProductos = resultado.listaProductos.OrderBy(x => x.pro_nombre).ToList();
                    }
                    else
                    {
                        resultado.listaProductos = resultado.listaProductos.OrderByDescending(x => x.pro_nombre).ToList();
                    }
                }
                else if (pAscColumna == "PrecioConTransfer")
                {
                    if (pAscOrdenar)
                    {
                        resultado.listaProductos = resultado.listaProductos.OrderBy(x => !x.isProductoFacturacionDirecta ? 0 : (decimal)x.tde_predescuento).ToList();
                    }
                    else
                    {
                        resultado.listaProductos = resultado.listaProductos.OrderByDescending(x => !x.isProductoFacturacionDirecta ? 0 : (decimal)x.tde_predescuento).ToList();
                    }
                }
                else if (pAscColumna == "nroordenamiento")
                {
                    if (pAscOrdenar)
                    {
                        resultado.listaProductos = resultado.listaProductos.OrderBy(x => x.nroordenamiento).ToList();
                    }
                    else
                    {
                        resultado.listaProductos = resultado.listaProductos.OrderByDescending(x => x.nroordenamiento).ToList();
                    }
                }
                //pAscOrdenar
                System.Web.HttpContext.Current.Session["PedidosBuscador_productosTodos"] = new cjSonBuscadorProductos(resultado);
                //
                int pPage = Convert.ToInt32(System.Web.HttpContext.Current.Session["PedidosBuscador_pPage"]);
                return RecuperarProductos_generarPaginador(resultado, pPage);
            }
            return string.Empty;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string AgregarProductosTransfersAlCarrito(List<cProductosAndCantidad> pListaProductosMasCantidad, int pIdTransfers, string pCodSucursal)
        {
            ResultTransfer objResult = new ResultTransfer();
            string resultado = string.Empty;
            if (System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] != null && System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                Usuario usuario = ((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]);
                DKintranet.Codigo.capaDatos.cClientes cliente = (DKintranet.Codigo.capaDatos.cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"];
                WebService.AgregarHistorialProductoCarritoTransfer(cliente.cli_codigo, pListaProductosMasCantidad, usuario.id);
                objResult.isNotError = capaCAR_decision.AgregarProductosTransfersAlCarrito(pListaProductosMasCantidad, cliente.cli_codigo, usuario.id, pIdTransfers, pCodSucursal, Constantes.cTipo_CarritoTransfers);
                objResult.oSucursalCarritoTransfer = capaCAR_decision.RecuperarCarritosTransferPorIdClienteIdSucursal(cliente, pCodSucursal, Constantes.cTipo_CarritoTransfers);
                objResult.listProductosAndCantidadError = pListaProductosMasCantidad;
                resultado = Serializador.SerializarAJson(objResult);
            }
            return resultado;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string AgregarProductosTransfersAlCarritoDiferido(List<cProductosAndCantidad> pListaProductosMasCantidad, int pIdTransfers, string pCodSucursal)
        {
            ResultTransfer objResult = new ResultTransfer();
            string resultado = string.Empty;
            if (System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] != null && System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                Usuario usuario = ((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]);
                DKintranet.Codigo.capaDatos.cClientes cliente = (DKintranet.Codigo.capaDatos.cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"];
                WebService.AgregarHistorialProductoCarritoTransfer(cliente.cli_codigo, pListaProductosMasCantidad, usuario.id);
                objResult.isNotError = capaCAR_decision.AgregarProductosTransfersAlCarrito(pListaProductosMasCantidad, cliente.cli_codigo, usuario.id, pIdTransfers, pCodSucursal, Constantes.cTipo_CarritoDiferidoTransfers);
                objResult.oSucursalCarritoTransfer = capaCAR_decision.RecuperarCarritosTransferPorIdClienteIdSucursal(cliente, pCodSucursal, Constantes.cTipo_CarritoDiferidoTransfers);
                objResult.listProductosAndCantidadError = pListaProductosMasCantidad;
                resultado = Serializador.SerializarAJson(objResult);
            }
            return resultado;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string CargarCarritoDiferido(string pIdSucursal, string pNombreProducto, int pCantidadProducto)
        {
            if (System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] != null && System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                Usuario usuario = ((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]);
                DKintranet.Codigo.capaDatos.cClientes cliente = (DKintranet.Codigo.capaDatos.cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"];
                ResultCargaProducto result = result = new ResultCargaProducto();
                result.isOk = capaCAR_decision.CargarCarritoDiferido(pIdSucursal, pNombreProducto, pCantidadProducto, cliente.cli_codigo, usuario.id);
                return Serializador.SerializarAJson(result);
            }
            return null;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string ActualizarProductoCarrito(string pIdProducto, string pCodSucursal, int pCantidadProducto)
        {
            if (System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] != null && System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                Usuario usuario = ((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]);
                DKintranet.Codigo.capaDatos.cClientes cliente = (DKintranet.Codigo.capaDatos.cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"];
                ResultCargaProducto result = result = new ResultCargaProducto();
                WebService.AgregarHistorialProductoCarrito(cliente.cli_codigo, pIdProducto, usuario.id);
                result.isOk = capaCAR_decision.AgregarProductoAlCarrito(pCodSucursal, pIdProducto, pCantidadProducto, cliente.cli_codigo, usuario.id);
                return Serializador.SerializarAJson(result);
            }
            return null;
        }

        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string RecuperarProductosEnOfertas(int pPage)
        {
            int pageSize = Constantes.cCantidadFilaPorPagina;
            cjSonBuscadorProductos resultado = FuncionesPersonalizadas.RecuperarProductosGeneral_OfertaTransfer(true, false);
            System.Web.HttpContext.Current.Session["PedidosBuscador_productosTodos"] = new cjSonBuscadorProductos(resultado);
            System.Web.HttpContext.Current.Session["PedidosBuscador_pPage"] = pPage;
            return RecuperarProductos_generarPaginador(resultado, pPage);

        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string RecuperarProductosEnTransfer(int pPage)
        {
            int pageSize = Constantes.cCantidadFilaPorPagina;
            cjSonBuscadorProductos resultado = FuncionesPersonalizadas.RecuperarProductosGeneral_OfertaTransfer(false, true);
            System.Web.HttpContext.Current.Session["PedidosBuscador_productosTodos"] = new cjSonBuscadorProductos(resultado);
            System.Web.HttpContext.Current.Session["PedidosBuscador_pPage"] = pPage;
            return RecuperarProductos_generarPaginador(resultado, pPage);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public int BorrarCarrito(int lrc_id, string lrc_codSucursal)
        {
            return capaCAR_decision.BorrarCarrito(lrc_id, lrc_codSucursal);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public int BorrarCarritosDiferidos(int lrc_id, string lrc_codSucursal)
        {
            return capaCAR_decision.BorrarCarritosDiferidos(lrc_id, lrc_codSucursal);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public int BorrarCarritoTransfer(string pSucursal)
        {
            return capaCAR_decision.BorrarCarritoTransfer(pSucursal);
        }
        private string ObtenerHorarioCierre_interno(string pSucursalDependiente)
        {
            string resultado = null;
            if (System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                DKintranet.Codigo.capaDatos.cClientes cliente = (DKintranet.Codigo.capaDatos.cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"];
                resultado = FuncionesPersonalizadas.ObtenerHorarioCierre(cliente.cli_codsuc, pSucursalDependiente, cliente.cli_codrep);
            }
            return resultado;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string ObtenerHorarioCierre(string pSucursalDependiente)
        {
            string resultado = null;
            resultado = ObtenerHorarioCierre_interno(pSucursalDependiente);// Serializador.SerializarAJson(ObtenerHorarioCierre_interno(pSucursalDependiente));
            return resultado;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string ObtenerHorarioCierreAndSiguiente(string pSucursalDependiente)
        {
            List<string> resultado = new List<string>();
            if (System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                DKintranet.Codigo.capaDatos.cClientes cliente = (DKintranet.Codigo.capaDatos.cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"];
                string resultado1 = FuncionesPersonalizadas.ObtenerHorarioCierre(cliente.cli_codsuc, pSucursalDependiente, cliente.cli_codrep);
                string resultado2 = FuncionesPersonalizadas.ObtenerHorarioCierreAnterior(cliente.cli_codsuc, pSucursalDependiente, cliente.cli_codrep, resultado1);
                resultado.Add(resultado1);
                if (resultado1 != resultado2)
                    resultado.Add(resultado2);

            }
            return Serializador.SerializarAJson(resultado);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string TomarPedidoCarrito(string pIdSucursal, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio, bool pIsUrgente)
        {
            return TomarPedidoCarrito_generico(Constantes.cTipo_Carrito, pIdSucursal, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, pIsUrgente);
        }
        public string TomarPedidoCarrito_generico(string pTipo,string pIdSucursal, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio, bool pIsUrgente)
        {
            ServiceReferenceDLL.cDllPedido resultadoPedido = null;
            if (System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                //System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] != null && 
                //Usuario usuario = ((Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"]);
                DKintranet.Codigo.capaDatos.cClientes cliente = (DKintranet.Codigo.capaDatos.cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"];
                List<cCarrito> listaCarrito = null;
                if (pTipo == Constantes.cTipo_Carrito)
                {
                    listaCarrito = capaCAR_decision.RecuperarCarritosPorSucursalYProductos(cliente.cli_codigo);
                }
                else if (pTipo == Constantes.cTipo_CarritoDiferido)
                {
                    listaCarrito = capaCAR_decision.RecuperarCarritosDiferidosPorCliente(cliente.cli_codigo);
                }
                if (listaCarrito == null)
                    return null;
                foreach (cCarrito item in listaCarrito)
                {
                    if (item.codSucursal == pIdSucursal)
                    {
                        if (capaCAR.IsCarritoEnProceso(item.car_id))
                        {
                            ServiceReferenceDLL.cDllPedido oEnProceso = new ServiceReferenceDLL.cDllPedido();
                            oEnProceso.Error = msgCarritoEnProceso;
                            return Serializador.SerializarAJson(oEnProceso);
                        }


                        List<ServiceReferenceDLL.cDllProductosAndCantidad> listaProductos = new List<ServiceReferenceDLL.cDllProductosAndCantidad>();
                        foreach (cProductosGenerico itemProductos in item.listaProductos)
                        {
                            listaProductos.Add(FuncionesPersonalizadas.ProductosEnCarrito_ToConvert_DllProductosAndCantidad(itemProductos));
                        }
                        if (capaWebServiceDLL.ValidarExistenciaDeCarritoWebPasado(item.car_id))
                        {
                            capaCAR.BorrarCarritoPorId_SleepTimer(item.car_id, Constantes.cAccionCarrito_BORRAR_CARRRITO_REPETIDO);
                            ServiceReferenceDLL.cDllPedido oRepetido = new ServiceReferenceDLL.cDllPedido();
                            oRepetido.Error = msgCarritoRepetido;
                            return Serializador.SerializarAJson(oRepetido);
                        }
                        resultadoPedido = capaCore_decision.TomarPedidoConIdCarrito(item.car_id,cliente.cli_login, pIdSucursal, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, listaProductos, pIsUrgente);
                        if (!capaWebServiceDLL.ValidarExistenciaDeCarritoWebPasado(item.car_id))
                            return null;
                        if (resultadoPedido == null)
                            return null;
                        else if (resultadoPedido != null)
                        {
                            bool isErrorPedido = true;
                            if (resultadoPedido.Error == null)
                            {
                                isErrorPedido = false;
                            }
                            else
                            {
                                if (resultadoPedido.Error.Trim() == string.Empty)
                                {
                                    isErrorPedido = false;
                                }
                            }
                            // Si se genero error
                            if (isErrorPedido)
                            {
                                resultadoPedido.Error = FuncionesPersonalizadas.LimpiarStringErrorPedido(resultadoPedido.Error);
                            }
                            else
                            {
                                // Obtener horario cierre
                                string horarioCierre = ObtenerHorarioCierre_interno(pIdSucursal);
                                resultadoPedido.Login = horarioCierre;
                                // fin Obtener horario cierre
                                // OPTIMIZAR //////////////////
                                foreach (ServiceReferenceDLL.cDllPedidoItem itemFaltantes in resultadoPedido.Items)
                                {
                                    if (itemFaltantes.Faltas > 0)
                                    {
                                        WebService.InsertarFaltantesProblemasCrediticios(item.lrc_id, pIdSucursal, cliente.cli_codigo, itemFaltantes.NombreObjetoComercial, itemFaltantes.Faltas, Constantes.cPEDIDO_FALTANTES);
                                    }
                                }
                                foreach (ServiceReferenceDLL.cDllPedidoItem itemConProblemasDeCreditos in resultadoPedido.ItemsConProblemasDeCreditos)
                                {
                                    int cantidadProblemaCrediticia = itemConProblemasDeCreditos.Cantidad + itemConProblemasDeCreditos.Faltas;
                                    if (cantidadProblemaCrediticia > 0)
                                    {
                                        WebService.InsertarFaltantesProblemasCrediticios(item.lrc_id, pIdSucursal, cliente.cli_codigo, itemConProblemasDeCreditos.NombreObjetoComercial, cantidadProblemaCrediticia, Constantes.cPEDIDO_PROBLEMACREDITICIO);
                                    }
                                }
                            
                                capaCAR_decision.GuardarPedidoBorrarCarrito(item, pTipo, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, pIsUrgente);
                            }
                        }
                        break;
                    }
                }
            }

            return Serializador.SerializarAJson(resultadoPedido);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string TomarPedidoCarritoDiferido(string pIdSucursal, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio, bool pIsUrgente)
        {
            return TomarPedidoCarrito_generico(Constantes.cTipo_CarritoDiferido, pIdSucursal, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, pIsUrgente);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public int BorrarCarritoTransferDiferido(string pSucursal)
        {
            return capaCAR_decision.BorrarCarritoTransferDiferido(pSucursal);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string RecuperarTransfer(string pNombreProducto)
        {
            List<cTransfer> listaTransfer = null;
            if (System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                DKintranet.Codigo.capaDatos.cClientes cliente = (DKintranet.Codigo.capaDatos.cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"];
                listaTransfer = WebService.RecuperarTodosTransferMasDetallePorIdProducto(pNombreProducto, cliente).Where(x => x.tfr_facturaciondirecta == null ? true : !(bool)x.tfr_facturaciondirecta).ToList();
            }
            return Serializador.SerializarAJson(listaTransfer);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string TomarTransferPedidoCarrito(bool pIsDiferido, string pIdSucursal, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio)
        {
            string tipo = pIsDiferido ? Constantes.cTipo_CarritoDiferidoTransfers : Constantes.cTipo_CarritoTransfers;
            List<ServiceReferenceDLL.cDllPedidoTransfer> resultadoPedido = null;
            int car_id_aux = 0;
            if (System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                DKintranet.Codigo.capaDatos.cClientes cliente = (DKintranet.Codigo.capaDatos.cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]; 
                List<ServiceReferenceDLL.cDllProductosAndCantidad> listaProductos = new List<ServiceReferenceDLL.cDllProductosAndCantidad>();

                List<cCarritoTransfer> listaCarrito = capaCAR_decision.RecuperarCarritosTransferPorIdCliente(cliente, tipo, pIdSucursal);
                if (listaCarrito == null)
                    return null;
                List<cProductosGenerico> listaProductos_Auditoria = new List<cProductosGenerico>();
                foreach (cCarritoTransfer item in listaCarrito)
                {
                    if (item.ctr_codSucursal == pIdSucursal)
                    {
                        car_id_aux = item.car_id_aux;
                        foreach (cProductosGenerico itemProductos in item.listaProductos)
                        {
                            ServiceReferenceDLL.cDllProductosAndCantidad objProductos = FuncionesPersonalizadas.ProductosEnCarrito_ToConvert_DllProductosAndCantidad(itemProductos);
                            objProductos.IdTransfer = item.tfr_codigo;
                            listaProductos.Add(objProductos);
                            itemProductos.tfr_codigo = item.tfr_codigo;
                            itemProductos.tde_codtfr = item.tfr_codigo;
                            listaProductos_Auditoria.Add(itemProductos);

                        }
                    }
                } // fin   foreach (cCarritoTransfer item in listaCarrito)
                if (capaCAR.IsCarritoEnProceso(car_id_aux))
                {
                    ServiceReferenceDLL.cDllPedidoTransfer oEnProceso = new ServiceReferenceDLL.cDllPedidoTransfer();
                    oEnProceso.Error = msgCarritoEnProceso;
                    resultadoPedido = new List<ServiceReferenceDLL.cDllPedidoTransfer>();
                    resultadoPedido.Add(oEnProceso);
                    return Serializador.SerializarAJson(resultadoPedido);
                }
                if (capaWebServiceDLL.ValidarExistenciaDeCarritoWebPasado(car_id_aux))
                {
                    capaCAR.BorrarCarritoPorId_SleepTimer(car_id_aux, Constantes.cAccionCarrito_BORRAR_CARRRITO_REPETIDO);
                    ServiceReferenceDLL.cDllPedidoTransfer oRepetido = new ServiceReferenceDLL.cDllPedidoTransfer();
                    oRepetido.Error = msgCarritoRepetido;
                    resultadoPedido = new List<ServiceReferenceDLL.cDllPedidoTransfer>();
                    resultadoPedido.Add(oRepetido);
                    return Serializador.SerializarAJson(resultadoPedido);
                }
                List<ServiceReferenceDLL.cDllPedidoTransfer> listaCarritoAux = capaWebServiceDLL.TomarPedidoDeTransfersConIdCarrito(car_id_aux, cliente.cli_login, pIdSucursal, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, listaProductos);
                if (!capaWebServiceDLL.ValidarExistenciaDeCarritoWebPasado(car_id_aux))
                    return null;

                if (listaCarritoAux != null)
                {
                    resultadoPedido = listaCarritoAux;
                    bool isErrorPedido = true;
                    if (listaCarritoAux.Count > 0)
                    {
                        if (listaCarritoAux[0].Error == null)
                        {
                            isErrorPedido = false;
                        }
                        else
                        {
                            if (listaCarritoAux[0].Error.Trim() == string.Empty)
                            {
                                isErrorPedido = false;
                            }
                        }
                        // INICIO FALTANTE
                        foreach (ServiceReferenceDLL.cDllPedidoTransfer itemPedidoTransferFaltante in listaCarritoAux)
                        {
                            if (itemPedidoTransferFaltante.Login == "REVISION")
                            {

                            }
                            else if (itemPedidoTransferFaltante.Login == "CONFIRMACION")
                            {

                            }
                            else
                            {
                                if (itemPedidoTransferFaltante.Items != null)
                                {
                                    if (itemPedidoTransferFaltante.Items.Count > 0)
                                    {
                                        for (int iArrayOfCDllPedidoItem = 0; iArrayOfCDllPedidoItem < itemPedidoTransferFaltante.Items.Count; iArrayOfCDllPedidoItem++)
                                        {
                                            if (itemPedidoTransferFaltante.Items[iArrayOfCDllPedidoItem].Faltas > 0)
                                            {
                                                WebService.InsertarFaltantesProblemasCrediticios(null, pIdSucursal, cliente.cli_codigo, itemPedidoTransferFaltante.Items[iArrayOfCDllPedidoItem].NombreObjetoComercial, itemPedidoTransferFaltante.Items[iArrayOfCDllPedidoItem].Faltas, Constantes.cPEDIDO_FALTANTES);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        // FIN FALTANTE
                    }
                    else
                    {
                        isErrorPedido = false;
                    }
                    // Si se genero error
                    if (isErrorPedido)
                    {
                        listaCarritoAux[0].Error = FuncionesPersonalizadas.LimpiarStringErrorPedido(listaCarritoAux[0].Error);
                    }
                    else
                    {
                        // borrar carrito transfer
                        capaCAR_decision.GuardarPedidoBorrarCarrito(listaProductos_Auditoria, car_id_aux, pIdSucursal, tipo, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, false);
                    }
                }
                else
                    return null;
            }
            return Serializador.SerializarAJson(resultadoPedido);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string TomarPedidoCarritoFacturarseFormaHabitual(string pIdSucursal, string pMensajeEnFactura, string pMensajeEnRemito, string pTipoEnvio, bool pIsUrgente, string[] pListaNombreComercial, int[] pListaCantidad)
        {
            ServiceReferenceDLL.cDllPedido resultadoPedido = null;
            if (System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                DKintranet.Codigo.capaDatos.cClientes cliente = (DKintranet.Codigo.capaDatos.cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"];
                List<ServiceReferenceDLL.cDllProductosAndCantidad> listaProductos = new List<ServiceReferenceDLL.cDllProductosAndCantidad>();
                for (int i = 0; i < pListaNombreComercial.Count(); i++)
                {
                    ServiceReferenceDLL.cDllProductosAndCantidad obj = new ServiceReferenceDLL.cDllProductosAndCantidad();
                    obj.codProductoNombre = pListaNombreComercial[i];
                    obj.cantidad = pListaCantidad[i];
                    cProductos objProductoBD = WebService.RecuperarProductoPorNombre(obj.codProductoNombre);
                    obj.isOferta = (objProductoBD.pro_ofeunidades == 0 && objProductoBD.pro_ofeporcentaje == 0) ? false : true;
                    listaProductos.Add(obj);
                }
                resultadoPedido = capaWebServiceDLL.TomarPedido(cliente.cli_login, pIdSucursal, pMensajeEnFactura, pMensajeEnRemito, pTipoEnvio, listaProductos, pIsUrgente);
                if (resultadoPedido == null)
                    return null;
                else if (resultadoPedido != null)
                {
                    bool isErrorPedido = true;
                    if (resultadoPedido.Error == null)
                    {
                        isErrorPedido = false;
                    }
                    else
                    {
                        if (resultadoPedido.Error.Trim() == string.Empty)
                        {
                            isErrorPedido = false;
                        }
                    }
                    // Si se genero error
                    if (isErrorPedido)
                    {
                        resultadoPedido.Error = FuncionesPersonalizadas.LimpiarStringErrorPedido(resultadoPedido.Error);
                    }
                    else
                    {
                        // Obtener horario cierre
                        string horarioCierre = ObtenerHorarioCierre_interno(pIdSucursal);
                        resultadoPedido.Login = horarioCierre;
                        // fin Obtener horario cierre
                        // OPTIMIZAR //////////////////
                        foreach (ServiceReferenceDLL.cDllPedidoItem itemFaltantes in resultadoPedido.Items)
                        {
                            if (itemFaltantes.Faltas > 0)
                            {
                                WebService.InsertarFaltantesProblemasCrediticios(null, pIdSucursal, cliente.cli_codigo, itemFaltantes.NombreObjetoComercial, itemFaltantes.Faltas, Constantes.cPEDIDO_FALTANTES);
                            }
                            //
                        }
                        foreach (ServiceReferenceDLL.cDllPedidoItem itemConProblemasDeCreditos in resultadoPedido.ItemsConProblemasDeCreditos)
                        {
                            int cantidadProblemaCrediticia = itemConProblemasDeCreditos.Cantidad + itemConProblemasDeCreditos.Faltas;
                            if (cantidadProblemaCrediticia > 0)
                            {
                                WebService.InsertarFaltantesProblemasCrediticios(null, pIdSucursal, cliente.cli_codigo, itemConProblemasDeCreditos.NombreObjetoComercial, cantidadProblemaCrediticia, Constantes.cPEDIDO_PROBLEMACREDITICIO);
                            }
                        }
                    }
                }
            }

            return Serializador.SerializarAJson(resultadoPedido);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string ActualizarProductoCarritoSubirArchivo(List<cProductosAndCantidad> pListaValor)
        {
            bool isOk = capaCAR_decision.ActualizarProductoCarritoSubirArchivo(pListaValor);
            return isOk ? "1" : "0";
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public bool cargarOferta(int pIdOferta)
        {
            try
            {
                cOferta o = WebService.RecuperarOfertaPorId(pIdOferta);
                if (o != null)
                {
                    if (System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
                    {
                        cClientes oCliente = (cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"];
                        WebService.InsertarOfertaRating(pIdOferta, oCliente.cli_codigo, false);
                    }

                    System.Web.HttpContext.Current.Session["home_Tipo"] = o.ofe_tipo;
                    if (o.tfr_codigo != null)
                        System.Web.HttpContext.Current.Session["home_IdTransfer"] = o.tfr_codigo;
                }
                System.Web.HttpContext.Current.Session["home_IdOferta"] = pIdOferta;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string RecuperarProductosHomeOferta(int pIdOferta)
        {
            cjSonBuscadorProductos resultado = FuncionesPersonalizadas.RecuperarProductosBase_V3(pIdOferta, null, null, false, false);
            if (resultado != null)
            {
                System.Web.HttpContext.Current.Session["PedidosBuscador_productosTodos"] = new cjSonBuscadorProductos(resultado);
                int pPage = 1;
                System.Web.HttpContext.Current.Session["PedidosBuscador_pPage"] = pPage;
                return RecuperarProductos_generarPaginador(resultado, pPage);
            }
            else { return string.Empty; }
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string RecuperarTransferPorId(int pIdTransfer)
        {

            cTransfer objTransfer = null;
            if (System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                objTransfer = WebService.RecuperarTransferMasDetallePorIdTransfer(pIdTransfer, (cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]);
            }
            if (objTransfer != null)
            {
                List<cTransfer> listaTransfer = new List<cTransfer>();
                listaTransfer.Add(objTransfer);
                return Serializador.SerializarAJson(listaTransfer);
            }
            else { return string.Empty; }
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public bool CargarArchivoPedidoDeNuevo(int has_id)
        {
            bool? result_aux = cSubirpedido.CargarArchivoPedidoDeNuevo(has_id);
            bool result = result_aux == null ? false : result_aux.Value;
            if (result)
                System.Web.HttpContext.Current.Session["subirpedido_isRepetido"] = true;
            return result;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public List<string> ObtenerRangoFecha_pedidos(int pDia)
        {
            List<string> lista = new List<string>();
            DateTime fechaDesdeAUX = DateTime.Now.AddDays(pDia * -1);
            DateTime fechaDesde = new DateTime(fechaDesdeAUX.Year, fechaDesdeAUX.Month, fechaDesdeAUX.Day, 0, 0, 0);
            DateTime fechaHasta = DateTime.Now.AddDays(1);
            lista.Add(fechaDesde.Day.ToString());
            lista.Add((fechaDesde.Month).ToString());
            lista.Add((fechaDesde.Year).ToString());

            lista.Add(fechaHasta.Day.ToString());
            lista.Add((fechaHasta.Month).ToString());
            lista.Add((fechaHasta.Year).ToString());

            List<ServiceReferenceDLL.cDllPedido> resultadoObj = WebService.ObtenerPedidosEntreFechas(fechaDesde, fechaHasta, ((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_login);

            System.Web.HttpContext.Current.Session["estadopedidos_Resultado"] = resultadoObj;
            return lista;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string RecuperarFaltasProblemasCrediticios(int pDia)
        {
            List<cFaltantesConProblemasCrediticiosPadre> listaRecuperador = null;
            if (System.Web.HttpContext.Current.Session["clientes_pages_Recuperador_Tipo"] != null && System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                System.Web.HttpContext.Current.Session["clientes_pages_Recuperador_CantidadDia"] = pDia;
                listaRecuperador = WebService.RecuperarFaltasProblemasCrediticios(((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo, Convert.ToInt32(System.Web.HttpContext.Current.Session["clientes_pages_Recuperador_Tipo"]), pDia, ((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc);
            }
            if (listaRecuperador != null)
                return Serializador.SerializarAJson(listaRecuperador);
            else
                return null;
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string RecuperarFaltasProblemasCrediticiosTodosEstados(int pDia)
        {
            List<cFaltantesConProblemasCrediticiosPadre> listaRecuperador = null;
            if (System.Web.HttpContext.Current.Session["clientes_pages_Recuperador_Tipo"] != null && System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                System.Web.HttpContext.Current.Session["clientes_pages_Recuperador_CantidadDia"] = pDia;
                listaRecuperador = WebService.RecuperarFaltasProblemasCrediticios_TodosEstados(((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo, Convert.ToInt32(System.Web.HttpContext.Current.Session["clientes_pages_Recuperador_Tipo"]), pDia, ((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc);
            }
            if (listaRecuperador != null)
                return Serializador.SerializarAJson(listaRecuperador);
            else
                return null;

        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public bool AgregarProductosDelRecuperardorAlCarrito(string pSucursal, string[] pArrayNombreProducto, int[] pArrayCantidad, bool[] pArrayOferta)
        {
            return capaCAR_decision.AgregarProductosDelRecuperardorAlCarrito(pSucursal, pArrayNombreProducto, pArrayCantidad, pArrayOferta);
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string BorrarPorProductosFaltasProblemasCrediticios(string pSucursal, string[] pArrayNombreProducto)
        {
            if (System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null && System.Web.HttpContext.Current.Session["clientes_pages_Recuperador_Tipo"] != null)
            {
                List<ServiceReferenceDLL.cDllProductosAndCantidad> listaProductos = new List<ServiceReferenceDLL.cDllProductosAndCantidad>();
                for (int i = 0; i < pArrayNombreProducto.Count(); i++)
                {
                    WebService.BorrarPorProductosFaltasProblemasCrediticios(pSucursal, ((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo, Convert.ToInt32(System.Web.HttpContext.Current.Session["clientes_pages_Recuperador_Tipo"]), pArrayNombreProducto[i]);
                }
            }
            System.Web.HttpContext.Current.Session["clientesDefault_CantRecuperadorFaltaFechaHora"] = null;
            //return null;// RecuperarFaltasProblemasCrediticios(14);
            return "Ok";
        }
        [AuthorizePermisoAttribute(Permiso = "mvc_Buscador")]
        public string ObtenerHistorialSubirArchivo(int pDia)
        {
            if (System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] == null)
                return null;
            List<cHistorialArchivoSubir> resultado = null;
            DateTime fechaDesdeAUX = DateTime.Now.AddDays(pDia * -1);
            DateTime fechaDesde = new DateTime(fechaDesdeAUX.Year, fechaDesdeAUX.Month, fechaDesdeAUX.Day, 0, 0, 0);
            List<cHistorialArchivoSubir> listaArchivosSubir = WebService.RecuperarHistorialSubirArchivo(((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo);
            if (listaArchivosSubir != null)
            {
                resultado = listaArchivosSubir.Where(x => x.has_fecha >= fechaDesde).ToList();
            }

            if (resultado != null)
                return Serializador.SerializarAJson(resultado);
            else
                return null;
        }
        public void CambiarCliente(int IdCliente)
        {
            cClientes o = null;
            if (Session["usuario_TomarPedido_listaClientes"] != null)
            {
                List<DKintranet.Codigo.capaDatos.cClientes> lista_Clientes = (List<DKintranet.Codigo.capaDatos.cClientes>)Session["usuario_TomarPedido_listaClientes"];
                o = lista_Clientes.FirstOrDefault(x => x.cli_codigo == IdCliente);
            }
            System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] = o;
        }
    }
}