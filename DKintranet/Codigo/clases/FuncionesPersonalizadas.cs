using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.IO;
using DKintranet.Codigo.capaDatos;
using System.Reflection;

namespace DKintranet.Codigo.clases
{
    public class FuncionesPersonalizadas
    {
        public static List<cProductosGenerico> cargarProductosBuscadorArchivos(DataTable tablaProductos, DataTable tablaSucursalStocks, List<cTransferDetalle> listaTransferDetalle, Constantes.CargarProductosBuscador pCargarProductosBuscador, string pSucursalElejida)
        {
            List<cProductosGenerico> resultado = new List<cProductosGenerico>();
            foreach (DataRow item in tablaProductos.Rows)
            {
                if (item["pro_codigo"] != DBNull.Value)
                {
                    List<cSucursalStocks> tempListaSucursalStocks = new List<cSucursalStocks>();
                    tempListaSucursalStocks = (from r in tablaSucursalStocks.Select("stk_codpro = '" + item["pro_codigo"].ToString() + "'").AsEnumerable()
                                               select new cSucursalStocks { stk_codpro = r["stk_codpro"].ToString(), stk_codsuc = r["stk_codsuc"].ToString(), stk_stock = r["stk_stock"].ToString() }).ToList();

                    if (tempListaSucursalStocks.Count > 0)
                    {
                        cProductosGenerico obj = new cProductosGenerico();
                        obj.listaSucursalStocks = tempListaSucursalStocks;
                        obj.pro_codigo = item["pro_codigo"].ToString();
                        if (item["pro_nombre"] != DBNull.Value)
                        {
                            obj.pro_nombre = item["pro_nombre"].ToString();
                        }
                        if (item["pro_precio"] != DBNull.Value)
                        {
                            obj.pro_precio = Convert.ToDecimal(item["pro_precio"]);
                        }
                        if (item["pro_preciofarmacia"] != DBNull.Value)
                        {
                            obj.pro_preciofarmacia = Convert.ToDecimal(item["pro_preciofarmacia"]);
                        }
                        if (item["pro_ofeunidades"] != DBNull.Value)
                        {
                            obj.pro_ofeunidades = Convert.ToInt32(item["pro_ofeunidades"]);
                        }
                        if (item["pro_ofeporcentaje"] != DBNull.Value)
                        {
                            obj.pro_ofeporcentaje = Convert.ToDecimal(item["pro_ofeporcentaje"]);
                        }
                        if (item["pro_neto"] != DBNull.Value)
                        {
                            obj.pro_neto = Convert.ToBoolean(item["pro_neto"]);
                        }
                        if (item["pro_codtpopro"] != DBNull.Value)
                        {
                            obj.pro_codtpopro = item["pro_codtpopro"].ToString().ToUpper();
                        }
                        if (item["pro_descuentoweb"] != DBNull.Value)
                        {
                            obj.pro_descuentoweb = Convert.ToDecimal(item["pro_descuentoweb"]);
                        }
                        if (item["pro_laboratorio"] != DBNull.Value)
                        {
                            obj.pro_laboratorio = item["pro_laboratorio"].ToString();
                        }
                        if (item["pro_monodroga"] != DBNull.Value)
                        {
                            obj.pro_monodroga = item["pro_monodroga"].ToString();
                        }
                        if (item["pro_codigobarra"] != DBNull.Value)
                        {
                            obj.pro_codigobarra = item["pro_codigobarra"].ToString();
                        }
                        if (item["pro_codigoalfabeta"] != DBNull.Value)
                        {
                            obj.pro_codigoalfabeta = item["pro_codigoalfabeta"].ToString();
                        }
                        if (item["pro_troquel"] != DBNull.Value)
                        {
                            obj.pro_troquel = item["pro_troquel"].ToString();
                        }
                        if (item.Table.Columns.Contains("pro_Ranking") && item["pro_Ranking"] != DBNull.Value)
                        {
                            obj.pro_Ranking = Convert.ToInt32(item["pro_Ranking"]);
                        }
                        if (item["pro_codtpovta"] != DBNull.Value)
                        {
                            obj.pro_codtpovta = Convert.ToString(item["pro_codtpovta"]);
                        }
                        if (item["isTransfer"] == DBNull.Value)
                        {
                            obj.isTieneTransfer = false;
                        }
                        else
                        {
                            obj.isTieneTransfer = true;
                        }
                        if (item["pro_isTrazable"] != DBNull.Value)
                        {
                            obj.pro_isTrazable = Convert.ToBoolean(item["pro_isTrazable"]);
                        }
                        if (item["pro_isCadenaFrio"] != DBNull.Value)
                        {
                            obj.pro_isCadenaFrio = Convert.ToBoolean(item["pro_isCadenaFrio"]);
                        }
                        if (item["pro_AceptaVencidos"] != DBNull.Value)
                        {
                            obj.pro_AceptaVencidos = Convert.ToBoolean(item["pro_AceptaVencidos"]);
                        }
                        if (item["pro_ProductoRequiereLote"] != DBNull.Value)
                        {
                            obj.pro_ProductoRequiereLote = Convert.ToBoolean(item["pro_ProductoRequiereLote"]);
                        }
                        if (item["pro_canmaxima"] != DBNull.Value)
                        {
                            obj.pro_canmaxima = Convert.ToInt32(item["pro_canmaxima"]);
                        }
                        // Default = false
                        if (item["pro_entransfer"] != DBNull.Value)
                        {
                            obj.pro_entransfer = Convert.ToBoolean(item["pro_entransfer"]);
                        }
                        if (item["pro_vtasolotransfer"] != DBNull.Value)
                        {
                            obj.pro_vtasolotransfer = Convert.ToBoolean(item["pro_vtasolotransfer"]);
                        }
                        if (item["RequiereVale"] != DBNull.Value)
                        {
                            obj.isValePsicotropicos = Convert.ToBoolean(item["RequiereVale"]);
                        }
                        if (item.Table.Columns.Contains("cantidad"))
                        {
                            if (item["cantidad"] != DBNull.Value)
                            {
                                obj.cantidad = Convert.ToInt32(item["cantidad"]);
                            }
                        }
                        if (item.Table.Columns.Contains("nroordenamiento"))
                        {
                            if (item["nroordenamiento"] != DBNull.Value)
                            {
                                obj.nroordenamiento = Convert.ToInt32(item["nroordenamiento"]);
                            }
                        }
                        if (item.Table.Columns.Contains("pro_Familia"))
                        {
                            if (item["pro_Familia"] != DBNull.Value)
                            {
                                obj.pro_Familia = Convert.ToString(item["pro_Familia"]);
                            }
                        }
                        if (item.Table.Columns.Contains("pro_PackDeVenta"))
                        {
                            if (item["pro_PackDeVenta"] != DBNull.Value)
                            {
                                obj.pro_PackDeVenta = Convert.ToInt32(item["pro_PackDeVenta"]);
                            }
                        }
                        if (item.Table.Columns.Contains("pro_PrecioBase") && item["pro_PrecioBase"] != DBNull.Value)
                        {
                            obj.pro_PrecioBase = Convert.ToDecimal(item["pro_PrecioBase"]);
                        }
                        if (item.Table.Columns.Contains("pro_PorcARestarDelDtoDeCliente") && item["pro_PorcARestarDelDtoDeCliente"] != DBNull.Value)
                        {
                            obj.pro_PorcARestarDelDtoDeCliente = Convert.ToDecimal(item["pro_PorcARestarDelDtoDeCliente"]);
                        }
                        if (item.Table.Columns.Contains("pro_AltoCosto") && item["pro_AltoCosto"] != DBNull.Value)
                        {
                            obj.pro_AltoCosto = Convert.ToBoolean(item["pro_AltoCosto"]);
                        }
                        obj.isProductoFacturacionDirecta = false;
                        if (item.Table.Columns.Contains("pro_NoTransfersEnClientesPerf") && item["pro_NoTransfersEnClientesPerf"] != DBNull.Value)
                        {
                            obj.pro_NoTransfersEnClientesPerf = Convert.ToBoolean(item["pro_NoTransfersEnClientesPerf"]);
                            if (HttpContext.Current.Session["clientesDefault_Cliente"] != null && ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_tipo == "P" && obj.pro_NoTransfersEnClientesPerf)
                                obj.isMostrarTransfersEnClientesPerf = false;
                        }
                        if ((listaTransferDetalle != null && obj.isMostrarTransfersEnClientesPerf && pCargarProductosBuscador == Constantes.CargarProductosBuscador.isDesdeBuscador) ||
                        (listaTransferDetalle != null && (pCargarProductosBuscador == Constantes.CargarProductosBuscador.isDesdeBuscador_OfertaTransfer || pCargarProductosBuscador == Constantes.CargarProductosBuscador.isSubirArchivo || pCargarProductosBuscador == Constantes.CargarProductosBuscador.isDesdeTabla || pCargarProductosBuscador == Constantes.CargarProductosBuscador.isRecuperadorFaltaCredito)))
                        {
                            List<cTransferDetalle> listaAUXtransferDetalle = listaTransferDetalle.Where(x => x.tde_codpro == obj.pro_nombre).ToList();
                            if (listaAUXtransferDetalle.Count > 0)
                            {
                                int indexTransfer = 0;
                                if (listaAUXtransferDetalle.Count > 1)
                                {
                                    for (int iTablaTransfersClientes = 0; iTablaTransfersClientes < listaAUXtransferDetalle.Count; iTablaTransfersClientes++)
                                    {
                                        if (listaAUXtransferDetalle[iTablaTransfersClientes].isTablaTransfersClientes)
                                        {
                                            indexTransfer = iTablaTransfersClientes;
                                        }
                                    }

                                }
                                obj.isProductoFacturacionDirecta = true;
                                obj.CargarTransferYTransferDetalle(listaAUXtransferDetalle[indexTransfer]);
                                if (HttpContext.Current.Session["clientesDefault_Cliente"] != null)
                                {
                                    obj.PrecioFinalTransfer = FuncionesPersonalizadas.ObtenerPrecioFinalTransferBase((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"], obj.tfr_deshab, obj.tfr_pordesadi, obj.pro_neto, obj.pro_codtpopro, obj.pro_descuentoweb, obj.tde_predescuento == null ? 0 : (decimal)obj.tde_predescuento, obj.tde_PrecioConDescuentoDirecto, obj.tde_PorcARestarDelDtoDeCliente);
                                }
                            }
                        }

                        if (pCargarProductosBuscador == Constantes.CargarProductosBuscador.isDesdeTabla || pCargarProductosBuscador == Constantes.CargarProductosBuscador.isRecuperadorFaltaCredito)
                        {
                            obj.PrecioFinal = FuncionesPersonalizadas.ObtenerPrecioFinal(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]), obj);
                            obj.PrecioConDescuentoOferta = FuncionesPersonalizadas.ObtenerPrecioUnitarioConDescuentoOferta(obj.PrecioFinal, obj);
                        }
                        if (pCargarProductosBuscador == Constantes.CargarProductosBuscador.isRecuperadorFaltaCredito)
                        {
                            if (item.Table.Columns.Contains("fpc_cantidad") && item["fpc_cantidad"] != DBNull.Value)
                                obj.fpc_cantidad = Convert.ToInt32(item["fpc_cantidad"]);
                            if (item.Table.Columns.Contains("fpc_nombreProducto") && item["fpc_nombreProducto"] != DBNull.Value)
                                obj.fpc_nombreProducto = Convert.ToString(item["fpc_nombreProducto"]);
                            if (item.Table.Columns.Contains("stk_stock") && item["stk_stock"] != DBNull.Value)
                                obj.stk_stock = Convert.ToString(item["stk_stock"]);

                            obj.PrecioFinalRecuperador = obj.PrecioFinal;
                            if (obj.PrecioFinalTransfer > 0 && obj.PrecioFinalTransfer < obj.PrecioFinalRecuperador)
                            {
                                obj.PrecioFinalRecuperador = obj.PrecioFinalTransfer;
                            }
                            if (obj.PrecioConDescuentoOferta > 0 && obj.PrecioConDescuentoOferta < obj.PrecioFinalRecuperador)
                            {
                                obj.PrecioFinalRecuperador = obj.PrecioConDescuentoOferta;
                            }
                        }
                        if (pCargarProductosBuscador == Constantes.CargarProductosBuscador.isSubirArchivo)
                        {
                            for (int iSucursalStocks = 0; iSucursalStocks < obj.listaSucursalStocks.Count; iSucursalStocks++)
                            {
                                if (pSucursalElejida == obj.listaSucursalStocks[iSucursalStocks].stk_codsuc)
                                {
                                    obj.listaSucursalStocks[iSucursalStocks].cantidadSucursal = obj.cantidad;
                                    break;
                                }
                            }

                        }
                        resultado.Add(obj);
                    }
                }

            }
            return resultado;
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
        public static string GenerarWhereLikeConColumna(string pTxtBuscador, string pColumna)
        {
            string where = string.Empty;
            string[] palabras = pTxtBuscador.Split(new char[] { ' ' });
            bool isPrimerWhere = true;
            foreach (string item in palabras)
            {
                if (item != string.Empty)
                {
                    if (isPrimerWhere)
                    {
                        isPrimerWhere = false;
                    }
                    else
                    {
                        where += " AND ";
                    }
                    where += " " + pColumna + " collate SQL_Latin1_General_Cp1_CI_AI like '%" + item + "%' ";
                }
            }
            return where;
        }
        public static string GenerarWhereLikeConColumna_EmpiezaCon(string pTxtBuscador, string pColumna)
        {
            string where = string.Empty;
            string[] palabras = pTxtBuscador.Split(new char[] { ' ' });
            //bool isPrimerWhere = true;
            foreach (string item in palabras)
            {
                if (item != string.Empty)
                {
                    where += " " + pColumna + " collate SQL_Latin1_General_Cp1_CI_AI like '" + item + "%' ";
                    break;
                }
            }
            return where;
        }
        public static string GenerarWhereLikeConVariasColumnas(string pTxtBuscador, List<string> pListaColumna)
        {
            string where = string.Empty;
            string[] palabras = pTxtBuscador.Split(new char[] { ' ' });
            bool isPrimerWhere = true;
            foreach (string item in palabras)
            {
                if (item != string.Empty)
                {
                    if (isPrimerWhere)
                    {
                        isPrimerWhere = false;
                        where += " ( ";
                    }
                    else
                    {
                        where += " AND ( ";
                    }
                    for (int i = 0; i < pListaColumna.Count; i++)
                    {
                        if (i != 0)
                        {
                            where += " OR ";
                        }
                        where += " " + pListaColumna[i] + " collate SQL_Latin1_General_Cp1_CI_AI like '%" + item + "%' ";
                    }
                    where += " ) ";
                }
            }
            return where;
        }
        public static List<string> RecuperarPalabrasYaBuscadaSinRepetir(int? pIdUsuario, string pNombreTabla)
        {
            var listaPalabras = WebService.RecuperarTodasPalabrasYaBuscada(pIdUsuario, pNombreTabla);
            var listaStrPalabrasSinRepetir = (from ob in listaPalabras
                                              select ob.hbp_Palabra.ToUpper()).Distinct().ToList();
            return listaStrPalabrasSinRepetir;
        }
        public static decimal ObtenerPrecioFinal(cClientes pClientes, cProductos pProductos)
        {
            decimal resultado = new decimal(0.0);
            if (pProductos.pro_neto)
            {
                switch (pProductos.pro_codtpopro)
                {
                    case "M": // medicamento
                        resultado = getPrecioBaseConDescuento(pProductos, pClientes.cli_pordesbetmed);
                        break;
                    case "P": // Perfumeria
                    case "A": // Accesorio
                    case "V": // Accesorio
                        resultado = getPrecioBaseConDescuento(pProductos, pClientes.cli_pordesnetos);
                        break;
                    default:
                        break;
                }
            }
            else
            {  // No neto   
                resultado = getPrecioBaseConDescuento(pProductos, pClientes.cli_PorcentajeDescuentoDeEspecialidadesMedicinalesDirecto);
            }
            return resultado;
        }
        private static decimal getPrecioBaseConDescuento(cProductos pProductos, decimal? pDescuentoRestar)
        {
            decimal descuentoRestarTemp = pDescuentoRestar == null ? 0 : pDescuentoRestar.Value;
            decimal descuento = descuentoRestarTemp - pProductos.pro_PorcARestarDelDtoDeCliente;
            if (descuento < 0)
                descuento = 0;
            decimal resultado = pProductos.pro_PrecioBase;
            resultado = resultado * (Convert.ToDecimal(1) - (descuento / Convert.ToDecimal(100)));
            return resultado;
        }
        public static decimal ObtenerPrecioUnitarioConDescuentoOferta(decimal pPrecioFinal, cProductos pProductos)
        {
            decimal resultado = Convert.ToDecimal(0);
            if (pProductos.pro_ofeporcentaje == 0 || pProductos.pro_ofeunidades == 0)
            {
                resultado = pPrecioFinal;
            }
            else
            {
                resultado = (pPrecioFinal * (Convert.ToDecimal(1) - (pProductos.pro_ofeporcentaje / Convert.ToDecimal(100))));
            }
            return resultado;
        }
        public static decimal ObtenerPrecioFinalTransfer(cClientes pClientes, cTransfer pTransfer, cTransferDetalle pTransferDetalle)
        {
            return ObtenerPrecioFinalTransferBase(pClientes, pTransfer.tfr_deshab, pTransfer.tfr_pordesadi, pTransferDetalle.pro_neto, pTransferDetalle.pro_codtpopro, pTransferDetalle.pro_descuentoweb, (decimal)pTransferDetalle.tde_predescuento, pTransferDetalle.tde_PrecioConDescuentoDirecto, pTransferDetalle.tde_PorcARestarDelDtoDeCliente);
        }
        public static decimal ObtenerPrecioFinalTransferBase(cClientes pClientes, bool pTfr_deshab, decimal? pTfr_pordesadi, bool pPro_neto, string pPro_codtpopro, decimal pPro_descuentoweb, decimal pTde_predescuento, decimal pTde_PrecioConDescuentoDirecto, decimal pTde_PorcARestarDelDtoDeCliente)
        {
            decimal resultado = new decimal(0.0);
            resultado = pTde_predescuento;
            if (pTfr_deshab)
            {
                if (pPro_neto)
                {   // Neto        
                    switch (pPro_codtpopro)
                    {
                        case "M": // medicamento
                            if (pClientes.cli_PorcentajeDescuentoDeEspecialidadesMedicinalesDirecto == null)
                            {
                                resultado = resultado * (Convert.ToDecimal(1) - (pClientes.cli_pordesbetmed / Convert.ToDecimal(100)));
                            }
                            else
                            {
                                resultado = getPrecioConDescuentoTransfer(pTde_PrecioConDescuentoDirecto, pTde_PorcARestarDelDtoDeCliente, pClientes.cli_pordesbetmed);
                            }
                            break;
                        case "P": // Perfumeria
                        case "A": // Accesorio
                        case "V": // Accesorio
                            if (pClientes.cli_PorcentajeDescuentoDeEspecialidadesMedicinalesDirecto == null)
                            {
                                resultado = resultado * (Convert.ToDecimal(1) - (pClientes.cli_pordesnetos / Convert.ToDecimal(100)));
                            }
                            else
                            {
                                resultado = getPrecioConDescuentoTransfer(pTde_PrecioConDescuentoDirecto, pTde_PorcARestarDelDtoDeCliente, pClientes.cli_pordesnetos);
                            }
                            break;
                        default:
                            break;
                    }
                }
                else
                {  // No neto   
                    if (pClientes.cli_PorcentajeDescuentoDeEspecialidadesMedicinalesDirecto == null)
                    {
                        resultado = resultado * (Convert.ToDecimal(1) - (pClientes.cli_pordesespmed / Convert.ToDecimal(100)));
                    }
                    else
                    {
                        resultado = getPrecioConDescuentoTransfer(pTde_PrecioConDescuentoDirecto, pTde_PorcARestarDelDtoDeCliente, pClientes.cli_PorcentajeDescuentoDeEspecialidadesMedicinalesDirecto.Value);
                    }
                }
            }
            if (pPro_neto)
            {   // Neto        
                switch (pPro_codtpopro)
                {
                    case "M": // medicamento
                        if (pClientes.cli_deswebnetmed)
                        {
                            resultado = resultado * (Convert.ToDecimal(1) - (pPro_descuentoweb / Convert.ToDecimal(100)));
                        }
                        break;
                    case "P": // Perfumeria
                        if (pClientes.cli_deswebnetperpropio)
                        {
                            resultado = resultado * (Convert.ToDecimal(1) - (pPro_descuentoweb / Convert.ToDecimal(100)));
                        }
                        break;
                    case "A": // Accesorio
                        if (pClientes.cli_deswebnetacc)
                        {
                        }
                        break;
                    case "V": // Accesorio
                        if (pClientes.cli_deswebnetacc)
                        {
                            resultado = resultado * (Convert.ToDecimal(1) - (pPro_descuentoweb / Convert.ToDecimal(100)));
                        }
                        break;
                    case "F": // Perfumería Cuenta y Orden
                        if (pClientes.cli_deswebnetpercyo)
                        {
                            resultado = resultado * (Convert.ToDecimal(1) - (pPro_descuentoweb / Convert.ToDecimal(100)));
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {  // No neto      
                if (pClientes.cli_deswebespmed)
                {
                    resultado = resultado * (Convert.ToDecimal(1) - (pPro_descuentoweb / Convert.ToDecimal(100)));
                }
            }
            if (pTfr_pordesadi != null)
            {
                resultado = resultado * (Convert.ToDecimal(1) - ((decimal)pTfr_pordesadi / Convert.ToDecimal(100)));
            }
            return resultado;
        }
        private static decimal getPrecioConDescuentoTransfer(decimal pTde_PrecioConDescuentoDirecto, decimal pTde_PorcARestarDelDtoDeCliente, decimal pDescuentoRestar)
        {
            decimal descuento = pDescuentoRestar - pTde_PorcARestarDelDtoDeCliente;
            if (descuento < 0)
                descuento = 0;
            decimal resultado = pTde_PrecioConDescuentoDirecto;
            resultado = resultado * (Convert.ToDecimal(1) - (descuento / Convert.ToDecimal(100)));
            return resultado;
        }
        public static string LimpiarStringErrorPedido(string pValor)
        {
            string resultado = pValor;
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
            return resultado;
        }
        public static DataTable ConvertProductosAndCantidadToDataTable(List<cProductosAndCantidad> pListaProductosMasCantidad)
        {

            DataTable pTablaDetalle = new DataTable();
            pTablaDetalle.Columns.Add(new DataColumn("codProducto", System.Type.GetType("System.String")));
            pTablaDetalle.Columns.Add(new DataColumn("cantidad", System.Type.GetType("System.Int32")));
            foreach (cProductosAndCantidad item in pListaProductosMasCantidad)
            {
                DataRow fila = pTablaDetalle.NewRow();
                fila["codProducto"] = item.codProductoNombre;
                fila["cantidad"] = item.cantidad;
                pTablaDetalle.Rows.Add(fila);
            }
            return pTablaDetalle;
        }
        public static DataTable ConvertNombresSeccionToDataTable(List<string> pListaNombreSeccion)
        {
            DataTable pTablaDetalle = new DataTable();
            pTablaDetalle.Columns.Add(new DataColumn("NombreSeccion", System.Type.GetType("System.String")));
            if (pListaNombreSeccion != null)
            {
                foreach (string item in pListaNombreSeccion)
                {
                    DataRow fila = pTablaDetalle.NewRow();
                    fila["NombreSeccion"] = item;
                    pTablaDetalle.Rows.Add(fila);
                }
            }
            return pTablaDetalle;
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
        public static void CargarRecuperadorFaltaActualizado(int pIdCliente)
        {
            bool isAgregar = true;
            if (HttpContext.Current.Session["clientesDefault_CantRecuperadorFaltaFechaHora"] != null)
            {
                if (((DateTime)HttpContext.Current.Session["clientesDefault_CantRecuperadorFaltaFechaHora"]) < DateTime.Now.AddMinutes(-5))
                {
                    isAgregar = false;
                }
            }
            if (isAgregar && System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                List<cFaltantesConProblemasCrediticiosPadre> listaRecuperador = WebService.RecuperarFaltasProblemasCrediticios(pIdCliente, 1, 14, ((cClientes)System.Web.HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc); ;
                if (listaRecuperador != null)
                {
                    HttpContext.Current.Session["clientesDefault_CantRecuperadorFalta"] = listaRecuperador.Count;
                    HttpContext.Current.Session["clientesDefault_CantRecuperadorFaltaFechaHora"] = DateTime.Now;
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
                        listaProductosBuscador[iPrecioFinal].PrecioFinal = FuncionesPersonalizadas.ObtenerPrecioFinal(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]), listaProductosBuscador[iPrecioFinal]);
                        listaProductosBuscador[iPrecioFinal].PrecioConDescuentoOferta = FuncionesPersonalizadas.ObtenerPrecioUnitarioConDescuentoOferta(listaProductosBuscador[iPrecioFinal].PrecioFinal, listaProductosBuscador[iPrecioFinal]);
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
            // Optimizar
            List<string> ListaSucursal = new List<string>();
            ListaSucursal.Add(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc);

            if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codrep == "S7")//((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codtpoenv == "R" && 
            {
                if (!ListaSucursal.Contains("SF"))
                    ListaSucursal.Add("SF");
                if (!ListaSucursal.Contains("CC"))
                    ListaSucursal.Add("CC");
            }
            //else if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc == "CD" && ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codprov != "ENTRE RIOS")// cli_codsuc == "CD" concordia y de la provincia de entre rios
            //{
            //    List<cSucursal> listaSucursalesAUX = WebService.RecuperarTodasSucursalesDependientes().Where(x => x.sde_sucursal == ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc).ToList();
            //    foreach (cSucursal itemSucursalesAUX in listaSucursalesAUX)
            //    {
            //        if (itemSucursalesAUX.sde_sucursalDependiente != ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc && itemSucursalesAUX.sde_sucursalDependiente != "CO") // CO = Concepción del Uruguay
            //        {
            //            ListaSucursal.Add(itemSucursalesAUX.sde_sucursalDependiente);
            //        }
            //    }
            //}
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
            return ListaSucursal;
            // Fin Optimizar
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
                DataTable table = capaProductos.RecuperarStockPorProductosAndSucursal(ConvertNombresSeccionToDataTable(pListaSucursal), ConvertProductosAndCantidadToDataTable(listaProductos));
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
            List<DKintranet.Codigo.capaDatos.cSucursal> listaSucursal = WebService.RecuperarTodasSucursales();
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
                DataTable table = capaProductos.RecuperarStockPorProductosAndSucursal(ConvertNombresSeccionToDataTable(ListaSucursal), ConvertProductosAndCantidadToDataTable(listaProductos));
                if (table != null)
                    result = (from r in table.Select("stk_codpro = '" + pro_codigo + "'").AsEnumerable()
                              select new cSucursalStocks { stk_codpro = r["stk_codpro"].ToString(), stk_codsuc = r["stk_codsuc"].ToString(), stk_stock = r["stk_stock"].ToString() }).ToList();

            }
            return result;
        }

        //
        //




        public static cjSonBuscadorProductos RecuperarProductosGeneral_V3(int? pIdOferta, string pTxtBuscador, List<string> pListaColumna, bool pIsOrfeta, bool pIsTransfer)
        {
            cjSonBuscadorProductos resultado = null;
            if (HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {

                List<cProductosGenerico> listaProductosBuscador = WebService.RecuperarTodosProductosDesdeBuscadorV3(pIdOferta, pTxtBuscador, pListaColumna, ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc, ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo, pIsOrfeta, pIsTransfer, ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codprov);
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
                        listaProductosBuscador[iPrecioFinal].PrecioFinal = FuncionesPersonalizadas.ObtenerPrecioFinal(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]), listaProductosBuscador[iPrecioFinal]);
                        listaProductosBuscador[iPrecioFinal].PrecioConDescuentoOferta = FuncionesPersonalizadas.ObtenerPrecioUnitarioConDescuentoOferta(listaProductosBuscador[iPrecioFinal].PrecioFinal, listaProductosBuscador[iPrecioFinal]);
                    }

                    List<cProductos> listaProductosConImagen = WebService.ObtenerProductosImagenes();
                    for (int iImagen = 0; iImagen < listaProductosBuscador.Count; iImagen++)
                    {
                        cProductos objImagen = listaProductosConImagen.Where(x => x.pro_codigo == listaProductosBuscador[iImagen].pro_codigo).FirstOrDefault();
                        if (objImagen != null)
                        {
                            listaProductosBuscador[iImagen].pri_nombreArchivo = objImagen.pri_nombreArchivo;
                            listaProductosBuscador[iImagen].pri_ancho_ampliar = objImagen.pri_ancho_ampliar;
                            listaProductosBuscador[iImagen].pri_alto_ampliar = objImagen.pri_alto_ampliar;
                            listaProductosBuscador[iImagen].pri_ancho_ampliar_original = objImagen.pri_ancho_ampliar_original;
                            listaProductosBuscador[iImagen].pri_alto_ampliar_original = objImagen.pri_alto_ampliar_original;
                        }
                    }

                    // Inicio 17/02/2016
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
        public static cjSonBuscadorProductos RecuperarProductosGeneralSubirPedidos(List<cProductosGenerico> pListaProveedor)
        {
            cjSonBuscadorProductos resultado = null;
            if (HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {

                List<cProductosGenerico> listaProductosBuscador = pListaProveedor;
                // fin Si el cliente no toma perfumeria
                for (int iPrecioFinal = 0; iPrecioFinal < listaProductosBuscador.Count; iPrecioFinal++)
                {
                    listaProductosBuscador[iPrecioFinal].PrecioFinal = FuncionesPersonalizadas.ObtenerPrecioFinal(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]), listaProductosBuscador[iPrecioFinal]);
                    listaProductosBuscador[iPrecioFinal].PrecioConDescuentoOferta = FuncionesPersonalizadas.ObtenerPrecioUnitarioConDescuentoOferta(listaProductosBuscador[iPrecioFinal].PrecioFinal, listaProductosBuscador[iPrecioFinal]);
                }
                //// Optimizar
                //List<string> ListaSucursal = new List<string>();
                //ListaSucursal.Add(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc);
                //List<cSucursal> listaSucursalesAUX = WebService.RecuperarTodasSucursalesDependientes().Where(x => x.sde_sucursal == ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc).ToList();
                //foreach (cSucursal itemSucursalesAUX in listaSucursalesAUX)
                //{
                //    if (itemSucursalesAUX.sde_sucursalDependiente != ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc)
                //    {
                //        ListaSucursal.Add(itemSucursalesAUX.sde_sucursalDependiente);
                //    }
                //}
                //// Fin Optimizar
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
        //public static cjSonBuscadorProductos RecuperarProductosBase(string pTxtBuscador, List<string> pListaColumna)
        //{
        //    cjSonBuscadorProductos resultado = null;
        //    if (!string.IsNullOrEmpty(pTxtBuscador))
        //    {
        //        if (pTxtBuscador.Trim() != string.Empty)
        //        {
        //            if (HttpContext.Current.Session["clientesDefault_Usuario"] != null)
        //            {
        //                HttpContext.Current.Session["clientesPages_Buscador"] = WebService.InsertarPalabraBuscada(pTxtBuscador.ToUpper(), ((capaDatos.Usuario)HttpContext.Current.Session["clientesDefault_Usuario"]).id, Constantes.cTABLA_PRODUCTO);
        //            }
        //            resultado = FuncionesPersonalizadas.RecuperarProductosGeneral(pTxtBuscador, pListaColumna, false, false);
        //        }
        //    }
        //    return resultado;
        //}
        public static cjSonBuscadorProductos RecuperarProductosBase_V3(int? pIdOferta, string pTxtBuscador, List<string> pListaColumna, bool pIsBuscarConOferta, bool pIsBuscarConTransfer)
        {
            cjSonBuscadorProductos resultado = null;
            if (!string.IsNullOrEmpty(pTxtBuscador) || pIdOferta != null)
            {
                if (!string.IsNullOrEmpty(pTxtBuscador) && pTxtBuscador.Trim() != string.Empty && HttpContext.Current.Session["clientesDefault_Usuario"] != null)
                    HttpContext.Current.Session["clientesPages_Buscador"] = WebService.InsertarPalabraBuscada(pTxtBuscador.ToUpper(), ((capaDatos.Usuario)HttpContext.Current.Session["clientesDefault_Usuario"]).id, Constantes.cTABLA_PRODUCTO);

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
        //public static cjSonBuscadorProductos RecuperarProductosBase(string pTxtBuscador, List<string> pListaColumna, bool pIsBuscarConOferta, bool pIsBuscarConTransfer)
        //{
        //    cjSonBuscadorProductos resultado = null;
        //    if (!string.IsNullOrEmpty(pTxtBuscador))
        //    {
        //        if (pTxtBuscador.Trim() != string.Empty)
        //        {
        //            if (HttpContext.Current.Session["clientesDefault_Usuario"] != null)
        //            {
        //                HttpContext.Current.Session["clientesPages_Buscador"] = WebService.InsertarPalabraBuscada(pTxtBuscador.ToUpper(), ((capaDatos.Usuario)HttpContext.Current.Session["clientesDefault_Usuario"]).id, Constantes.cTABLA_PRODUCTO);
        //            }
        //            resultado = FuncionesPersonalizadas.RecuperarProductosGeneral(pTxtBuscador, pListaColumna, false, false);
        //            if (pIsBuscarConOferta || pIsBuscarConTransfer)
        //            {
        //                if (resultado != null)
        //                {
        //                    if (pIsBuscarConOferta && pIsBuscarConTransfer)
        //                    {
        //                        resultado.listaProductos = resultado.listaProductos.Where(x => x.pro_ofeporcentaje > 0 || x.isTieneTransfer).ToList();
        //                    }
        //                    else
        //                    {
        //                        if (pIsBuscarConOferta)
        //                        {
        //                            resultado.listaProductos = resultado.listaProductos.Where(x => x.pro_ofeporcentaje > 0).ToList();
        //                        }
        //                        else if (pIsBuscarConTransfer)
        //                        {
        //                            resultado.listaProductos = resultado.listaProductos.Where(x => x.isTieneTransfer).ToList();
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return resultado;
        //}

        public static string ObtenerHorarioCierre_base(string pSucursal, string pSucursalDependiente, string pCodigoReparto, DateTime fechaActual)
        {
            string resultado = null;
            List<cHorariosSucursal> listaHorariosSucursal = WebService.RecuperarTodosHorariosSucursalDependiente().Where(x => x.sdh_sucursal == pSucursal && x.sdh_sucursalDependiente == pSucursalDependiente && x.sdh_codReparto == pCodigoReparto).ToList();
            string day = string.Empty;
            bool isEncontroEnFechaHoy = false;
            switch (fechaActual.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    day = Constantes.cDIASEMANA_Lunes;
                    break;
                case DayOfWeek.Tuesday:
                    day = Constantes.cDIASEMANA_Martes;
                    break;
                case DayOfWeek.Wednesday:
                    day = Constantes.cDIASEMANA_Miercoles;
                    break;
                case DayOfWeek.Thursday:
                    day = Constantes.cDIASEMANA_Jueves;
                    break;
                case DayOfWeek.Friday:
                    day = Constantes.cDIASEMANA_Viernes;
                    break;
                case DayOfWeek.Saturday:
                    day = Constantes.cDIASEMANA_Sabado;
                    break;
                case DayOfWeek.Sunday:
                    day = Constantes.cDIASEMANA_Domingo;
                    break;
                default:
                    break;
            }
            if (day != string.Empty)
            {
                foreach (cHorariosSucursal itemHorariosSucursal in listaHorariosSucursal)
                {
                    if (itemHorariosSucursal.sdh_diaSemana == day)
                    {
                        if (itemHorariosSucursal.listaHorarios != null)
                        {
                            foreach (string itemHorarios in itemHorariosSucursal.listaHorarios)
                            {
                                string[] tiempo = itemHorarios.Split(':');
                                if (tiempo.Count() > 1)
                                {
                                    DateTime fechaCierre = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day, Convert.ToInt32(tiempo[0]), Convert.ToInt32(tiempo[1]), 30);
                                    if (fechaCierre > fechaActual)
                                    {
                                        isEncontroEnFechaHoy = true;
                                        resultado = itemHorarios + " hs. ";
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    }
                }

            }
            int SumaDia = 0;
            while (!isEncontroEnFechaHoy)
            {
                day = string.Empty;
                SumaDia += 1;
                //DateTime fechaOtroDia = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day + SumaDia, fechaActual.Hour, fechaActual.Minute, fechaActual.Second);
                DateTime fechaOtroDia = fechaActual.AddDays(SumaDia);
                switch (fechaOtroDia.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        day = Constantes.cDIASEMANA_Lunes;
                        break;
                    case DayOfWeek.Tuesday:
                        day = Constantes.cDIASEMANA_Martes;
                        break;
                    case DayOfWeek.Wednesday:
                        day = Constantes.cDIASEMANA_Miercoles;
                        break;
                    case DayOfWeek.Thursday:
                        day = Constantes.cDIASEMANA_Jueves;
                        break;
                    case DayOfWeek.Friday:
                        day = Constantes.cDIASEMANA_Viernes;
                        break;
                    case DayOfWeek.Saturday:
                        day = Constantes.cDIASEMANA_Sabado;
                        break;
                    case DayOfWeek.Sunday:
                        day = Constantes.cDIASEMANA_Domingo;
                        break;
                    default:
                        break;
                }
                if (day != string.Empty)
                {
                    foreach (cHorariosSucursal itemHorariosSucursal in listaHorariosSucursal)
                    {
                        if (itemHorariosSucursal.sdh_diaSemana == day)
                        {
                            if (itemHorariosSucursal.listaHorarios != null)
                            {
                                foreach (string itemHorarios in itemHorariosSucursal.listaHorarios)
                                {
                                    string[] tiempo = itemHorarios.Split(':');
                                    if (tiempo.Count() > 1)
                                    {
                                        DateTime fechaCierre = new DateTime(fechaOtroDia.Year, fechaOtroDia.Month, fechaOtroDia.Day, Convert.ToInt32(tiempo[0]), Convert.ToInt32(tiempo[1]), 30);
                                        if (fechaCierre > fechaActual)
                                        {
                                            isEncontroEnFechaHoy = true;
                                            resultado = itemHorarios + " hs. " + day;
                                            break;
                                        }
                                    }
                                }
                            }

                            break;
                        }
                    }
                }
                if (SumaDia > 7)
                {
                    isEncontroEnFechaHoy = true;
                }
            } // fin while (!isEncontroEnFechaHoy)

            // Inicio S7
            if (((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codrep == "S7" && pSucursalDependiente == "SF")
            {
                DateTime fechaCierre = new DateTime(fechaActual.Year, fechaActual.Month, fechaActual.Day, 22, 15, 30);
                resultado = "22:15" + " hs. ";
                switch (fechaActual.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        if (fechaCierre < fechaActual)
                            resultado = "22:15" + " hs. " + "MA";
                        break;
                    case DayOfWeek.Tuesday:
                        if (fechaCierre < fechaActual)
                            resultado = "22:15" + " hs. " + "MI";
                        break;
                    case DayOfWeek.Wednesday:
                        if (fechaCierre < fechaActual)
                            resultado = "22:15" + " hs. " + "JU";
                        break;
                    case DayOfWeek.Thursday:
                        if (fechaCierre < fechaActual)
                            resultado = "22:15" + " hs. " + "VI";
                        break;
                    case DayOfWeek.Friday:
                        if (fechaCierre < fechaActual)
                            resultado = "22:15" + " hs. " + "LU";
                        break;
                    case DayOfWeek.Saturday:
                        resultado = "22:15" + " hs. " + "LU";
                        break;
                    case DayOfWeek.Sunday:
                        resultado = "22:15" + " hs. " + "LU";
                        break;
                    default:
                        break;
                }
            }
            // Fin S7
            return resultado;
        }

        public static string ObtenerHorarioCierre(string pSucursal, string pSucursalDependiente, string pCodigoReparto)
        {
            return ObtenerHorarioCierre_base(pSucursal, pSucursalDependiente, pCodigoReparto, DateTime.Now);
        }
        public static string ObtenerHorarioCierreAnterior(string pSucursal, string pSucursalDependiente, string pCodigoReparto, string pHorarioCierre)
        {
            if (string.IsNullOrEmpty(pHorarioCierre))
                return string.Empty;
            string result = string.Empty;
            try
            {


                DateTime fechaCuentaRegresiva;
                DateTime hoy = DateTime.Now;
                if (pHorarioCierre.Length == 12)
                {
                    var diaSemana = pHorarioCierre.Substring(10, 2);
                    var diaSemanaNro = -1;
                    //Note: Sunday is 0, Monday is 1, and so on || from 0 to 6
                    // LU = 1
                    // MA = 2
                    // MI = 3
                    // JU = 4
                    // VI = 5
                    // SA = 6
                    // DO = 0
                    switch (diaSemana)
                    {
                        case "LU":
                            diaSemanaNro = 1;
                            break;
                        case "MA":
                            diaSemanaNro = 2;
                            break;
                        case "MI":
                            diaSemanaNro = 3;
                            break;
                        case "JU":
                            diaSemanaNro = 4;
                            break;
                        case "VI":
                            diaSemanaNro = 5;
                            break;
                        case "SA":
                            diaSemanaNro = 6;
                            break;
                        case "DO":
                            diaSemanaNro = 0;
                            break;
                        default:
                            break;
                    }
                    pHorarioCierre = pHorarioCierre.Replace(" hs. " + diaSemana, "");
                    var values = pHorarioCierre.Split(':');
                    var d = new DateTime(hoy.Year, hoy.Month, hoy.Day, Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), 0);// mes 0 = enero
                                                                                                                                  //var n = d.DayOfWeek;
                    var sumaDia = 0;
                    while ((int)d.DayOfWeek != diaSemanaNro)
                    {
                        sumaDia++;
                        d = new DateTime(hoy.Year, hoy.Month, hoy.Day + sumaDia, Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), 50);// mes 0 = enero
                        if (sumaDia > 7 || (int)d.DayOfWeek == diaSemanaNro)
                            break;
                    }
                    fechaCuentaRegresiva = d;

                }
                else
                {
                    pHorarioCierre = pHorarioCierre.Replace(" hs.", "");
                    var values = pHorarioCierre.Split(':');
                    fechaCuentaRegresiva = new DateTime(hoy.Year, hoy.Month, hoy.Day, Convert.ToInt32(values[0]), Convert.ToInt32(values[1]), 50);// mes 0 = enero
                }
                result = ObtenerHorarioCierre_base(pSucursal, pSucursalDependiente, pCodigoReparto, fechaCuentaRegresiva);
            }
            catch (Exception ex)
            {

                var oo = 1;
            }
            return result;
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
                Codigo.capaDatos.Usuario user = (Codigo.capaDatos.Usuario)System.Web.HttpContext.Current.Session["clientesDefault_Usuario"];
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
        public static bool isUserTomaPedidoIntranet(Codigo.capaDatos.Usuario pUser)
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
    }

}