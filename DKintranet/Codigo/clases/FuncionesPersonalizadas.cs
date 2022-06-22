using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using DKintranet.Codigo.capaDatos;
using System.Reflection;
using DKbase.web.capaDatos;
using DKbase.web;

namespace DKintranet.Codigo.clases
{
    public class FuncionesPersonalizadas
    {
        public static List<cProductosGenerico> cargarProductosBuscadorArchivos(DataTable tablaProductos, DataTable tablaSucursalStocks, List<cTransferDetalle> listaTransferDetalle, DKbase.generales.Constantes.CargarProductosBuscador pCargarProductosBuscador, string pSucursalElejida)
        {
            cClientes oClientes = null;
            if (HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                oClientes = (cClientes)HttpContext.Current.Session["clientesDefault_Cliente"];
            }
            return DKbase.web.acceso.cargarProductosBuscadorArchivos(oClientes, tablaProductos, tablaSucursalStocks, listaTransferDetalle, pCargarProductosBuscador, pSucursalElejida);
        }
        public static void grabarLog(MethodBase method, Exception pException, DateTime pFechaActual, params object[] values)
        {
            DKbase.generales.Log.LogError(method, pException, pFechaActual, values);
        }
        public static List<cClientes> RecuperarTodosClientesActivosYSinClientes()
        {
            List<cClientes> resultado = new List<cClientes>();
            resultado.Add(new cClientes(0, "<< Sin clientes >>"));
            resultado.AddRange(WebService.RecuperarTodosClientes());
            return resultado;
        }
        public static List<cClientes> RecuperarTodosClientesParaCombo(bool pIsTodos)
        {
            List<cClientes> resultado = new List<cClientes>();
            //if (pIsTodos)
            //{
            //    resultado.Add(new cClientes(0, "<< Todos >>"));
            //}
            resultado.Add(new cClientes(0, "<< Todos >>"));
            resultado.AddRange(WebService.RecuperarTodosClientes());
            return resultado;
        }
        public static List<string> RecuperarPalabrasYaBuscadaSinRepetir(int? pIdUsuario, string pNombreTabla)
        {
            var listaPalabras = WebService.RecuperarTodasPalabrasYaBuscada(pIdUsuario, pNombreTabla);
            var listaStrPalabrasSinRepetir = (from ob in listaPalabras
                                              select ob.hbp_Palabra.ToUpper()).Distinct().ToList();
            return listaStrPalabrasSinRepetir;
        }
        public static decimal ObtenerPrecioFinalTransfer(cClientes pClientes, cTransfer pTransfer, cTransferDetalle pTransferDetalle)
        {
            return DKbase.web.FuncionesPersonalizadas_base.ObtenerPrecioFinalTransferBase(pClientes, pTransfer.tfr_deshab, pTransfer.tfr_pordesadi, pTransferDetalle.pro_neto, pTransferDetalle.pro_codtpopro, pTransferDetalle.pro_descuentoweb, (decimal)pTransferDetalle.tde_predescuento, pTransferDetalle.tde_PrecioConDescuentoDirecto, pTransferDetalle.tde_PorcARestarDelDtoDeCliente);
        }
        public static string LimpiarStringErrorPedido(string pValor)
        {
            string resultado = pValor;
            if (!string.IsNullOrEmpty(pValor))
            {
                resultado = resultado.Replace("-", string.Empty);
                resultado = resultado.Replace("0", string.Empty);
                resultado = resultado.Replace("1", string.Empty);
                resultado = resultado.Replace("2", string.Empty);
                resultado = resultado.Replace("3", string.Empty);
                resultado = resultado.Replace("4", string.Empty);
                resultado = resultado.Replace("5", string.Empty);
                resultado = resultado.Replace("6", string.Empty);
                resultado = resultado.Replace("7", string.Empty);
                resultado = resultado.Replace("8", string.Empty);
                resultado = resultado.Replace("9", string.Empty);
            }
            return resultado;
        }
        public static DataTable ConvertNombresSeccionToDataTable(List<string> pListaNombreSeccion)
        {
            return DKbase.web.FuncionesPersonalizadas_base.ConvertNombresSeccionToDataTable(pListaNombreSeccion);
        }
        public static DataTable ObtenerDataTableProductosCarritoArchivosPedidos()
        {
            DataTable pTablaDetalle = new DataTable();
            pTablaDetalle.Columns.Add(new DataColumn("codProducto", System.Type.GetType("System.String")));
            pTablaDetalle.Columns.Add(new DataColumn("codigobarra", System.Type.GetType("System.String")));
            pTablaDetalle.Columns.Add(new DataColumn("codigoalfabeta", System.Type.GetType("System.String")));
            pTablaDetalle.Columns.Add(new DataColumn("troquel", System.Type.GetType("System.String")));
            pTablaDetalle.Columns.Add(new DataColumn("cantidad", System.Type.GetType("System.Int32")));
            pTablaDetalle.Columns.Add(new DataColumn("nroordenamiento", System.Type.GetType("System.Int32")));
            return pTablaDetalle;
        }
        public static DataRow ConvertProductosCarritoArchivosPedidosToDataRow(DataTable pTabla, string pCodProducto, int pCantidad, string pCodigoBarra, string pCodigoAlfaBeta, string pTroquel, string pTipoArchivoPedidos)
        {
            DataRow fila = pTabla.NewRow();
            if (pTipoArchivoPedidos == "F")
            {
                fila["codProducto"] = pCodProducto;
                fila["cantidad"] = pCantidad;
            }
            else if (pTipoArchivoPedidos == "S")
            {
                fila["codigobarra"] = pCodigoBarra;
                fila["codigoalfabeta"] = pCodigoAlfaBeta;
                fila["troquel"] = pTroquel;//.Trim();
                fila["cantidad"] = pCantidad;
            }
            fila["nroordenamiento"] = pTabla.Rows.Count + 1;
            return fila;
        }
        public static DKbase.dll.cDllProductosAndCantidad ProductosEnCarrito_ToConvert_DllProductosAndCantidad(cProductosGenerico pValor)
        {
            DKbase.dll.cDllProductosAndCantidad resultado = new DKbase.dll.cDllProductosAndCantidad();
            resultado.cantidad = pValor.cantidad;
            resultado.codProductoNombre = pValor.pro_nombre;
            resultado.isOferta = (pValor.pro_ofeunidades == 0 && pValor.pro_ofeporcentaje == 0) ? false : true;
            return resultado;
        }
        public static void CargarMensajeActualizado(int pIdCliente)
        {
            bool isAgregar = true;
            if (HttpContext.Current.Session["clientesDefault_CantListaMensajeFechaHora"] != null)
            {
                if (((DateTime)HttpContext.Current.Session["clientesDefault_CantListaMensajeFechaHora"]) < DateTime.Now.AddMinutes(-5))
                {
                    isAgregar = false;
                }
            }
            if (isAgregar)
            {
                List<cMensaje> listaMensaje = WebService.RecuperarTodosMensajesPorIdCliente(pIdCliente);
                if (listaMensaje != null)
                {
                    //List<cMensaje> lista = ((List<cMensaje>)(Session["clientesDefault_ListaMensaje"])).Where(x => x.tme_estado == Convert.ToInt32(Constantes.cESTADO_SINLEER)).ToList();
                    HttpContext.Current.Session["clientesDefault_CantListaMensaje"] = listaMensaje.Where(x => (x.tme_estado == Convert.ToInt32(Constantes.cESTADO_SINLEER) && !x.tme_importante)).ToList().Count;
                    HttpContext.Current.Session["clientesDefault_CantListaMensajeFechaHora"] = DateTime.Now;
                }
            }
        }
        public static cjSonBuscadorProductos RecuperarProductosGeneral_OfertaTransfer(bool pIsOrfeta, bool pIsTransfer)
        {
            cjSonBuscadorProductos resultado = null;
            if (HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                List<cProductosGenerico> listaProductosBuscador = listaProductosBuscador = WebService.RecuperarTodosProductosDesdeBuscador_OfertaTransfer(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc, ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo, pIsOrfeta, pIsTransfer, ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codprov);
                if (listaProductosBuscador != null)
                {
                    // TIPO CLIENTE
                    if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_tipo == Constantes.cTipoCliente_Perfumeria) // Solamente perfumeria
                    {
                        listaProductosBuscador = listaProductosBuscador.Where(x => x.pro_codtpopro == Constantes.cTIPOPRODUCTO_Perfumeria || x.pro_codtpopro == Constantes.cTIPOPRODUCTO_PerfumeriaCuentaYOrden).ToList();
                    }
                    else if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_tipo == Constantes.cTipoCliente_Todos) // Todos los productos
                    {
                        // Si el cliente no toma perfumeria
                        if (!((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_tomaPerfumeria)
                        {
                            listaProductosBuscador = listaProductosBuscador.Where(x => x.pro_codtpopro != Constantes.cTIPOPRODUCTO_Perfumeria && x.pro_codtpopro != Constantes.cTIPOPRODUCTO_PerfumeriaCuentaYOrden).ToList();
                        }
                        // fin Si el cliente no toma perfumeria
                    }
                    // FIN TIPO CLIENTE
                    for (int iPrecioFinal = 0; iPrecioFinal < listaProductosBuscador.Count; iPrecioFinal++)
                    {
                        listaProductosBuscador[iPrecioFinal].PrecioFinal = DKbase.web.FuncionesPersonalizadas_base.ObtenerPrecioFinal(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]), listaProductosBuscador[iPrecioFinal]);
                        listaProductosBuscador[iPrecioFinal].PrecioConDescuentoOferta = DKbase.web.FuncionesPersonalizadas_base.ObtenerPrecioUnitarioConDescuentoOferta(listaProductosBuscador[iPrecioFinal].PrecioFinal, listaProductosBuscador[iPrecioFinal]);
                    }

                    List<string> ListaSucursal = RecuperarSucursalesParaBuscadorDeCliente();
                    listaProductosBuscador = ActualizarStockListaProductos(ListaSucursal, listaProductosBuscador);

                    // Fin 17/02/2016
                    cjSonBuscadorProductos ResultadoObj = new cjSonBuscadorProductos();
                    ResultadoObj.listaSucursal = ListaSucursal;
                    ResultadoObj.listaProductos = listaProductosBuscador;
                    resultado = ResultadoObj;
                }
            }
            return resultado;
        }

        public static List<string> RecuperarSucursalesParaBuscadorDeCliente()
        {
            List<string> ListaSucursal = new List<string>();
            if (HttpContext.Current.Session["intranet_listaSucursales"] == null)
            {
                // Optimizar
                ListaSucursal.Add(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc);
                if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codrep == "S7")//((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codtpoenv == "R" && 
                {
                    if (!ListaSucursal.Contains("SF"))
                        ListaSucursal.Add("SF");
                    if (!ListaSucursal.Contains("CC"))
                        ListaSucursal.Add("CC");
                }
                else
                {
                    List<cSucursal> listaSucursalesAUX = WebService.RecuperarTodasSucursalesDependientes().Where(x => x.sde_sucursal == ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc).ToList();
                    foreach (cSucursal itemSucursalesAUX in listaSucursalesAUX)
                    {
                        if (itemSucursalesAUX.sde_sucursalDependiente != ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc)
                        {
                            ListaSucursal.Add(itemSucursalesAUX.sde_sucursalDependiente);
                        }
                    }
                    if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_IdSucursalAlternativa != null &&
                        !ListaSucursal.Contains(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_IdSucursalAlternativa))
                    {
                        ListaSucursal.Add(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_IdSucursalAlternativa);
                    }
                }
                HttpContext.Current.Session["intranet_listaSucursales"] = ListaSucursal;
                // Fin Optimizar
            }
            else
            {
                ListaSucursal = (List<string>)HttpContext.Current.Session["intranet_listaSucursales"];
            }
            return ListaSucursal;
        }
        //
        public static List<cProductosGenerico> ActualizarStockListaProductos(List<string> pListaSucursal, List<cProductosGenerico> pListaProductos)
        {
            bool isActualizar = false;
            if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codrep == "S7")
                isActualizar = true;
            //else if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc == "CD" && ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codprov != "ENTRE RIOS")// cli_codsuc == "CD" concordia y de la provincia de entre rios        
            //    isActualizar = true;
            else if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_IdSucursalAlternativa != null)
                isActualizar = true;
            if (isActualizar)
            {
                List<cProductosAndCantidad> listaProductos = new List<cProductosAndCantidad>();
                foreach (cProductosGenerico item in pListaProductos)
                {
                    listaProductos.Add(new cProductosAndCantidad { codProductoNombre = item.pro_codigo });
                }
                DataTable table = capaDatos.capaProductos.RecuperarStockPorProductosAndSucursal(ConvertNombresSeccionToDataTable(pListaSucursal), DKbase.web.FuncionesPersonalizadas_base.ConvertProductosAndCantidadToDataTable(listaProductos));
                if (table != null)
                    for (int i = 0; i < pListaProductos.Count; i++)
                    {
                        pListaProductos[i].listaSucursalStocks = (from r in table.Select("stk_codpro = '" + pListaProductos[i].pro_codigo + "'").AsEnumerable()
                                                                  select new cSucursalStocks { stk_codpro = r["stk_codpro"].ToString(), stk_codsuc = r["stk_codsuc"].ToString(), stk_stock = r["stk_stock"].ToString() }).ToList();
                    }
            }
            return pListaProductos;
        }
        public static List<cProductosGenerico> ActualizarStockListaProductos_SubirArchico(List<string> pListaSucursal, List<cProductosGenerico> pListaProductos, string pSucursalElegida)
        {
            pListaProductos = ActualizarStockListaProductos(pListaSucursal, pListaProductos);
            List<cSucursal> listaSucursal = WebService.RecuperarTodasSucursales();
            bool trabajaPerfumeria = true;
            for (int i = 0; i < listaSucursal.Count; i++)
            {
                if (listaSucursal[i].suc_codigo == pSucursalElegida)
                {
                    trabajaPerfumeria = listaSucursal[i].suc_trabajaPerfumeria;
                }
            }
            string sucElegida = pSucursalElegida;
            bool isActualizar = false;
            if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codrep == "S7")
                isActualizar = true;
            //else if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc == "CD" && ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codprov != "ENTRE RIOS")// cli_codsuc == "CD" concordia y de la provincia de entre rios        
            //    isActualizar = true;
            else if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_IdSucursalAlternativa != null)
                isActualizar = true;
            if (isActualizar || !trabajaPerfumeria)
            {
                for (int i = 0; i < pListaProductos.Count; i++)
                {
                    if (pListaProductos[i].pro_codtpopro == "P" && !trabajaPerfumeria)
                    {
                        sucElegida = "CC";
                    }
                    else
                    {
                        sucElegida = pSucursalElegida;
                    }
                    foreach (cSucursalStocks item in pListaProductos[i].listaSucursalStocks)
                    {
                        if (item.stk_codsuc == sucElegida)
                        {
                            item.cantidadSucursal = pListaProductos[i].cantidad;
                        }
                    }
                }
            }
            else
            {

            }
            return pListaProductos;
        }
        public static List<cSucursalStocks> ActualizarStockListaProductos_Transfer(string pro_codigo, List<cSucursalStocks> pSucursalStocks)
        {
            List<cSucursalStocks> result = pSucursalStocks;
            bool isActualizar = false;
            if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codrep == "S7")
                isActualizar = true;
            //else if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc == "CD" && ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codprov != "ENTRE RIOS")// cli_codsuc == "CD" concordia y de la provincia de entre rios        
            //    isActualizar = true;
            else if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_IdSucursalAlternativa != null)
                isActualizar = true;
            if (isActualizar)
            {
                List<cProductosAndCantidad> listaProductos = new List<cProductosAndCantidad>();

                listaProductos.Add(new cProductosAndCantidad { codProductoNombre = pro_codigo });
                List<string> ListaSucursal = RecuperarSucursalesParaBuscadorDeCliente();
                DataTable table = capaDatos.capaProductos.RecuperarStockPorProductosAndSucursal(ConvertNombresSeccionToDataTable(ListaSucursal), DKbase.web.FuncionesPersonalizadas_base.ConvertProductosAndCantidadToDataTable(listaProductos));
                if (table != null)
                    result = (from r in table.Select("stk_codpro = '" + pro_codigo + "'").AsEnumerable()
                              select new cSucursalStocks { stk_codpro = r["stk_codpro"].ToString(), stk_codsuc = r["stk_codsuc"].ToString(), stk_stock = r["stk_stock"].ToString() }).ToList();

            }
            return result;
        }
        public static cjSonBuscadorProductos RecuperarProductosGeneral_V3(int? pIdOferta, string pTxtBuscador, List<string> pListaColumna, bool pIsOrfeta, bool pIsTransfer)
        {
            cjSonBuscadorProductos resultado = null;
            if (HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                cClientes oClientes = (cClientes)HttpContext.Current.Session["clientesDefault_Cliente"];
                resultado = DKbase.web.FuncionesPersonalizadas_base.RecuperarProductosGeneral_V3(oClientes, pIdOferta, pTxtBuscador, pListaColumna, pIsOrfeta, pIsTransfer);
            }
            return resultado;
        }
        public static cjSonBuscadorProductos RecuperarProductosBase_V3(int? pIdOferta, string pTxtBuscador, List<string> pListaColumna, bool pIsBuscarConOferta, bool pIsBuscarConTransfer)
        {
            cjSonBuscadorProductos resultado = null;
            if (!string.IsNullOrEmpty(pTxtBuscador) || pIdOferta != null)
            {
                if (!string.IsNullOrEmpty(pTxtBuscador) && pTxtBuscador.Trim() != string.Empty && HttpContext.Current.Session["clientesDefault_Usuario"] != null)
                    HttpContext.Current.Session["clientesPages_Buscador"] = WebService.InsertarPalabraBuscada(pTxtBuscador.ToUpper(), ((Usuario)HttpContext.Current.Session["clientesDefault_Usuario"]).id, Constantes.cTABLA_PRODUCTO);

                resultado = FuncionesPersonalizadas.RecuperarProductosGeneral_V3(pIdOferta, pTxtBuscador, pListaColumna, ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_tomaOfertas, ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_tomaTransfers);
                if (pIsBuscarConOferta || pIsBuscarConTransfer)
                {
                    if (resultado != null)
                    {
                        if (pIsBuscarConOferta && pIsBuscarConTransfer)
                        {
                            resultado.listaProductos = resultado.listaProductos.Where(x => x.pro_ofeporcentaje > 0 || x.isTieneTransfer || x.isProductoFacturacionDirecta).ToList();
                        }
                        else
                        {
                            if (pIsBuscarConOferta)
                            {
                                resultado.listaProductos = resultado.listaProductos.Where(x => x.pro_ofeporcentaje > 0).ToList();
                            }
                            else if (pIsBuscarConTransfer)
                            {
                                resultado.listaProductos = resultado.listaProductos.Where(x => x.isTieneTransfer || x.isProductoFacturacionDirecta).ToList();
                            }
                        }
                    }
                }

            }
            return resultado;
        }
        public static string ObtenerHorarioCierre(string pSucursal, string pSucursalDependiente, string pCodigoReparto)
        {
            return DKbase.web.FuncionesPersonalizadas_base.ObtenerHorarioCierre(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]), pSucursal, pSucursalDependiente, pCodigoReparto);
        }
        public static string ObtenerHorarioCierreAnterior(string pSucursal, string pSucursalDependiente, string pCodigoReparto, string pHorarioCierre)
        {
            return DKbase.web.FuncionesPersonalizadas_base.ObtenerHorarioCierreAnterior(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]), pSucursal, pSucursalDependiente, pCodigoReparto, pHorarioCierre);
        }
        public static void GenerarCSV()
        {
            List<string> lista = new List<string>();
            //nombreArchivoTXT = "Kell" + fechaArchivoTXT + "-" + pFactura + ".txt";
            //resultado = nombreArchivoTXT;
            System.IO.StreamWriter FAC_txt = new System.IO.StreamWriter("pRuta + nombreArchivoTXT", false, System.Text.Encoding.UTF8);
            // encabezado
            string encabezado = string.Empty;
            //Fecha;Comprobante;Numero Comprobante;Monto Exento;Monto Gravado;Monto Iva Inscipto;Monto Iva No Inscripto;Monto Percepciones DGR;Monto Total
            encabezado += "Fecha";
            encabezado += ";";
            encabezado += "Comprobante";
            encabezado += ";";
            encabezado += "Numero Comprobante";
            encabezado += ";";
            encabezado += "Monto Exento";
            encabezado += ";";
            encabezado += "Monto Gravado";
            encabezado += ";";
            encabezado += "Monto Iva Inscipto";
            encabezado += ";";
            encabezado += "Monto Iva No Inscripto";
            encabezado += ";";
            encabezado += "Monto Percepciones DGR";
            encabezado += ";";
            encabezado += "Monto Total";

            FAC_txt.WriteLine(encabezado);
            // fin encabezado


            foreach (string item in lista)
            {
                string fila = string.Empty;
                fila += DateTime.Now.ToString();
                fila += ";";
                fila += item;
                fila += ";";
                fila += item;
                FAC_txt.WriteLine(fila);
            }
            FAC_txt.Close();
        }
        public static List<string> RecuperarSinPermisosSecciones(int pIdUsuario)
        {
            List<string> resultado = null;
            List<cUsuarioSinPermisosIntranet> lista = WebService.RecuperarTodosSinPermisosIntranetPorIdUsuario(pIdUsuario);
            if (lista != null)
            {
                resultado = new List<string>();
                foreach (cUsuarioSinPermisosIntranet item in lista)
                {
                    resultado.Add(item.usp_nombreSeccion.ToUpper());
                }
            }
            //HttpContext.Current.Session["master_ListaSinPermisoSecciones"] = resultado;
            return resultado;
        }
        public static bool IsPermisoSeccion(string pNombreSeccion)
        {
            // falta analizar la variable de session: "master_ListaSinPermisoSecciones"
            //
            bool resultado = true;
            if (HttpContext.Current.Session["master_ListaSinPermisoSecciones"] != null)
            {
                List<string> listaSinPermiso = (List<string>)HttpContext.Current.Session["master_ListaSinPermisoSecciones"];
                foreach (string item in listaSinPermiso)
                {
                    if (item == pNombreSeccion)
                    {
                        resultado = false;
                        break;
                    }
                }
            }
            if (System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] != null)
            {
                Usuario user = (Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"];
                if (isUserTomaPedidoIntranet(user))
                {
                    switch (pNombreSeccion)
                    {
                        case "DESCARGAS":
                        case "CUENTASCORRIENTES":
                        case "DEVOLUCIONES":
                        case "MEDIOS_DE_PAGOS":
                        case "PEDIDOS_RECUPERADOR":
                        case "PEDIDOS_MIS_PEDIDOS":
                            resultado = false;
                            break;
                    }
                }
            }

            return resultado;
        }
        public static bool isUserTomaPedidoIntranet(Usuario pUser)
        {
            bool result = false;
            if (pUser != null && pUser.rol_Nombre == DKbase.generales.Constantes.cTomarPedidoCC)
            {
                result = true;
            }
            return result;
        }
        public static List<cTipoEnvioClienteFront> RecuperarTiposDeEnvios()
        {
            List<cTipoEnvioClienteFront> resultado = null;
            if (HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                cClientes objCliente = ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]);
                List<cSucursalDependienteTipoEnviosCliente> lista = WebService.RecuperarTodosSucursalDependienteTipoEnvioCliente().Where(x => x.sde_sucursal == objCliente.cli_codsuc && (x.tsd_idTipoEnvioCliente == null || x.env_codigo == objCliente.cli_codtpoenv)).ToList();
                if (lista != null)
                {
                    resultado = new List<cTipoEnvioClienteFront>();
                    for (int i = 0; i < lista.Count; i++)
                    {
                        cTipoEnvioClienteFront obj = new cTipoEnvioClienteFront();
                        obj.sucursal = lista[i].sde_sucursalDependiente;
                        obj.tipoEnvio = lista[i].env_codigo;

                        List<cSucursalDependienteTipoEnviosCliente_TiposEnvios> listaTiposEnvios = WebService.RecuperarTodosSucursalDependienteTipoEnvioCliente_TiposEnvios_Excepciones(lista[i].tsd_id, objCliente.cli_codrep);
                        if (listaTiposEnvios == null || listaTiposEnvios.Count == 0)
                        {
                            listaTiposEnvios = WebService.RecuperarTodosSucursalDependienteTipoEnvioCliente_TiposEnvios().Where(x => x.tdt_idSucursalDependienteTipoEnvioCliente == lista[i].tsd_id).ToList();
                        }
                        if (listaTiposEnvios != null)
                        {
                            obj.lista = new List<cTiposEnvios>();
                            foreach (cSucursalDependienteTipoEnviosCliente_TiposEnvios itemSucursalDependienteTipoEnviosCliente_TiposEnvios in listaTiposEnvios)
                            {
                                cTiposEnvios objTipoEnvio = new cTiposEnvios();
                                objTipoEnvio.env_codigo = itemSucursalDependienteTipoEnviosCliente_TiposEnvios.env_codigo;
                                objTipoEnvio.env_nombre = itemSucursalDependienteTipoEnviosCliente_TiposEnvios.env_nombre;
                                objTipoEnvio.env_id = itemSucursalDependienteTipoEnviosCliente_TiposEnvios.env_id;
                                obj.lista.Add(objTipoEnvio);
                            }
                        }
                        //
                        resultado.Add(obj);
                    }
                }
                // Inicio S7
                if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codrep == "S7")
                {
                    cTipoEnvioClienteFront obj = new cTipoEnvioClienteFront();
                    obj.sucursal = "SF";
                    obj.tipoEnvio = null;
                    obj.lista = new List<cTiposEnvios>();
                    cTiposEnvios objTipoEnvioR = new cTiposEnvios();
                    objTipoEnvioR.env_codigo = "R";
                    objTipoEnvioR.env_nombre = "Reparto";
                    obj.lista.Add(objTipoEnvioR);
                    cTiposEnvios objTipoEnvioE = new cTiposEnvios();
                    objTipoEnvioE.env_codigo = "E";
                    objTipoEnvioE.env_nombre = "Encomienda";
                    obj.lista.Add(objTipoEnvioE);
                    cTiposEnvios objTipoEnvioM = new cTiposEnvios();
                    objTipoEnvioM.env_codigo = "M";
                    objTipoEnvioM.env_nombre = "Mostrador";
                    obj.lista.Add(objTipoEnvioM);
                    cTiposEnvios objTipoEnvioC = new cTiposEnvios();
                    objTipoEnvioC.env_codigo = "C";
                    objTipoEnvioC.env_nombre = "Cadeteria";
                    obj.lista.Add(objTipoEnvioC);
                    resultado.Add(obj);
                }
                // Fin S7
                if (!string.IsNullOrEmpty(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_IdSucursalAlternativa)
                    && resultado.FirstOrDefault(x => x.sucursal == ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_IdSucursalAlternativa) == null)
                {
                    cTipoEnvioClienteFront obj = new cTipoEnvioClienteFront();
                    obj.sucursal = ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_IdSucursalAlternativa;
                    obj.tipoEnvio = null;
                    obj.lista = new List<cTiposEnvios>();
                    cTiposEnvios objTipoEnvioR = new cTiposEnvios();
                    objTipoEnvioR.env_codigo = "R";
                    objTipoEnvioR.env_nombre = "Reparto";
                    obj.lista.Add(objTipoEnvioR);
                    resultado.Add(obj);
                }


            }
            return resultado;
        }
        //////////////////////////////////////////////////////////////////////////////////////////////
        public static List<int> CargarProductoCantidadDependiendoTransfer(cProductosGenerico pProducto, int pCantidad)
        {
            List<int> resultado = new List<int>();
            bool isPasarDirectamente = false;
            int cantidadCarritoTransfer = 0;
            int cantidadCarritoComun = 0;

            if (pProducto.isProductoFacturacionDirecta)
            { // facturacion directa
              // Combiene transfer o promocion                      
                decimal precioConDescuentoDependiendoCantidad = ObtenerPrecioUnitarioConDescuentoOfertaSiLlegaConLaCantidad(pProducto, pCantidad);
                if (precioConDescuentoDependiendoCantidad > pProducto.PrecioFinalTransfer)
                {
                    var isSumarTransfer = false;
                    if (pProducto.tde_muluni != null && pProducto.tde_unidadesbonificadas != null)
                    {
                        /// UNIDAD MULTIPLO Y BONIFICADA
                        if ((pCantidad >= (int)pProducto.tde_muluni) && (pCantidad <= ((int)pProducto.tde_muluni + (int)pProducto.tde_unidadesbonificadas)))
                        {
                            // es multiplo
                            isSumarTransfer = true;
                            cantidadCarritoTransfer = (int)pProducto.tde_muluni + (int)pProducto.tde_unidadesbonificadas;
                        }
                        else if (pCantidad > ((int)pProducto.tde_muluni + (int)pProducto.tde_unidadesbonificadas))
                        {
                            isSumarTransfer = true;
                            int cantidadMultiplicar = Convert.ToInt32(Math.Truncate(Convert.ToDouble(pCantidad) / Convert.ToDouble(pProducto.tde_muluni)));
                            cantidadCarritoTransfer = cantidadMultiplicar * ((int)pProducto.tde_muluni + (int)pProducto.tde_unidadesbonificadas);
                            //
                            for (int iCantMulti = 0; iCantMulti < cantidadMultiplicar; iCantMulti++)
                            {
                                int cantTemp = iCantMulti * ((int)pProducto.tde_muluni + (int)pProducto.tde_unidadesbonificadas);
                                if (cantTemp >= pCantidad)
                                {
                                    cantidadCarritoTransfer = cantTemp;
                                    break;
                                }
                            }
                            //
                            if (cantidadCarritoTransfer == pCantidad)
                            {

                            }
                            else
                            {
                                if (pCantidad < cantidadCarritoTransfer)
                                {
                                    cantidadCarritoComun = 0;
                                }
                                else
                                {
                                    cantidadCarritoComun = pCantidad - cantidadCarritoTransfer;
                                }
                                if ((cantidadCarritoComun >= (int)pProducto.tde_muluni) && (cantidadCarritoComun <= ((int)pProducto.tde_muluni + (int)pProducto.tde_unidadesbonificadas)))
                                {
                                    cantidadCarritoTransfer += (int)pProducto.tde_muluni + (int)pProducto.tde_unidadesbonificadas;
                                    cantidadCarritoComun = 0;
                                }
                            }
                        }
                        if (isSumarTransfer)
                        {

                        }
                        else
                        {
                            isPasarDirectamente = true;
                        }
                        /// FIN UNIDAD MULTIPLO Y BONIFICADA
                    } // fin if (listaProductosBuscados[pFila].tde_muluni != null && listaProductosBuscados[pFila].tde_unidadesbonificadas != null){
                    else if (pProducto.tde_fijuni != null)
                    {
                        // UNIDAD FIJA
                        if (pCantidad == (int)pProducto.tde_fijuni)
                        {
                            isSumarTransfer = true;
                            cantidadCarritoTransfer = (int)pProducto.tde_fijuni;
                        }
                        else if (pCantidad > (int)pProducto.tde_fijuni)
                        {
                            isSumarTransfer = true;
                            cantidadCarritoTransfer = (int)pProducto.tde_fijuni;
                            cantidadCarritoComun = pCantidad - (int)pProducto.tde_fijuni;
                        }
                        if (isSumarTransfer)
                        {

                        }
                        else
                        {
                            isPasarDirectamente = true;
                        }
                        // FIN UNIDAD FIJA
                    }
                    else if (pProducto.tde_minuni != null && pProducto.tde_maxuni != null)
                    {
                        // UNIDAD MAXIMA Y MINIMA
                        if ((int)pProducto.tde_minuni <= pCantidad && (int)pProducto.tde_maxuni >= pCantidad)
                        {
                            isSumarTransfer = true;
                            cantidadCarritoTransfer = pCantidad;
                        }
                        else if ((int)pProducto.tde_maxuni < pCantidad)
                        {
                            isSumarTransfer = true;
                            cantidadCarritoTransfer = (int)pProducto.tde_maxuni;
                            cantidadCarritoComun = pCantidad - (int)pProducto.tde_maxuni;
                        }
                        if (isSumarTransfer)
                        {

                        }
                        else
                        {
                            isPasarDirectamente = true;
                        }
                        // FIN UNIDAD MAXIMA Y MINIMA
                    }
                    else if (pProducto.tde_minuni != null)
                    {
                        // UNIDAD MINIMA
                        if ((int)pProducto.tde_minuni <= pCantidad)
                        {
                            isSumarTransfer = true;
                            cantidadCarritoTransfer = pCantidad;
                        }
                        if (isSumarTransfer)
                        {

                        }
                        else
                        {
                            isPasarDirectamente = true;
                        }
                        // FIN UNIDAD MINIMA
                    }
                } // fin if (listaProductosBuscados[pFila].PrecioConDescuentoOferta > listaProductosBuscados[pFila].PrecioFinalTransfer){
                else
                {
                    isPasarDirectamente = true;
                }
            }
            else
            {
                isPasarDirectamente = true;
            }
            if (isPasarDirectamente)
            {
                cantidadCarritoComun = pCantidad;
            }
            resultado.Add(cantidadCarritoComun);
            resultado.Add(cantidadCarritoTransfer);
            return resultado;
        }
        public static decimal ObtenerPrecioUnitarioConDescuentoOfertaSiLlegaConLaCantidad(cProductosGenerico pProductos, int pCantidad)
        {
            decimal resultado = Convert.ToDecimal(0);
            bool isClienteTomaOferta = false;
            if (HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                isClienteTomaOferta = ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_tomaOfertas;
            }
            if (isClienteTomaOferta)
            {
                if (pProductos.pro_ofeunidades == 0 || pProductos.pro_ofeporcentaje == 0)
                {
                    resultado = pProductos.PrecioFinal;
                }
                else
                {
                    if (pProductos.pro_ofeunidades > pCantidad)
                    {
                        resultado = pProductos.PrecioFinal;
                    }
                    else
                    {
                        resultado = pProductos.PrecioFinal * (Convert.ToDecimal(1) - (pProductos.pro_ofeporcentaje / Convert.ToDecimal(100)));
                    }
                }
            }
            else
            {
                // Cliente si permiso para tomar oferta
                resultado = pProductos.PrecioFinal;
            }
            return resultado;
        }

        public static string obtenerNombreMes(int pValue)
        {
            string result = string.Empty;
            switch (pValue)
            {
                case 1:
                    result = "Enero";
                    break;
                case 2:
                    result = "Febrero";
                    break;
                case 3:
                    result = "Marzo";
                    break;
                case 4:
                    result = "Abril";
                    break;
                case 5:
                    result = "Mayo";
                    break;
                case 6:
                    result = "Junio";
                    break;
                case 7:
                    result = "Julio";
                    break;
                case 8:
                    result = "Agosto";
                    break;
                case 9:
                    result = "Septiembre";
                    break;
                case 10:
                    result = "Octubre";
                    break;
                case 11:
                    result = "Noviembre";
                    break;
                case 12:
                    result = "Diciembre";
                    break;
                default:
                    break;
            }
            return result;
        }
        public static cjSonBuscadorProductos RecuperarProductosGeneralSubirPedidos(List<cProductosGenerico> pListaProveedor)
        {
            cjSonBuscadorProductos resultado = null;
            if (HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {

                List<cProductosGenerico> listaProductosBuscador = pListaProveedor;
                // fin Si el cliente no toma perfumeria
                for (int iPrecioFinal = 0; iPrecioFinal < listaProductosBuscador.Count; iPrecioFinal++)
                {
                    listaProductosBuscador[iPrecioFinal].PrecioFinal = DKbase.web.FuncionesPersonalizadas_base.ObtenerPrecioFinal(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]), listaProductosBuscador[iPrecioFinal]);
                    listaProductosBuscador[iPrecioFinal].PrecioConDescuentoOferta = DKbase.web.FuncionesPersonalizadas_base.ObtenerPrecioUnitarioConDescuentoOferta(listaProductosBuscador[iPrecioFinal].PrecioFinal, listaProductosBuscador[iPrecioFinal]);
                }
                // Inicio 17/02/2016
                List<string> ListaSucursal = RecuperarSucursalesParaBuscadorDeCliente();
                listaProductosBuscador = ActualizarStockListaProductos_SubirArchico(ListaSucursal, listaProductosBuscador, HttpContext.Current.Session["subirpedido_SucursalEleginda"].ToString());
                // Fin 17/02/2016
                cjSonBuscadorProductos ResultadoObj = new cjSonBuscadorProductos();
                ResultadoObj.listaSucursal = ListaSucursal;
                ResultadoObj.listaProductos = listaProductosBuscador;
                resultado = ResultadoObj;
            }
            return resultado;
        }
    }

}