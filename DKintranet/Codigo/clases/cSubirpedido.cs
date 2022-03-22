using DKbase.web.capaDatos;
using DKintranet.Codigo.capaDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace DKintranet.Codigo.clases
{
    public class cSubirpedido
    {
        const int longFilaArchivoS = 22;
        const int longFilaArchivoF = 24;

        public static Boolean? LeerArchivoPedido(HttpPostedFileBase pFileUpload, string pSucursal)
        {
            Boolean? resultado = false;
            if (HttpContext.Current.Session["clientesDefault_Usuario"] != null && HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                string nombreCompletoOriginal = pFileUpload.FileName;
                if (nombreCompletoOriginal != string.Empty)
                {
                    //isRedireccionar = true;
                    string rutaTemporal = Constantes.cRaizArchivos + @"\archivos\ArchivosPedidos\";
                    DirectoryInfo DIR = new DirectoryInfo(rutaTemporal);
                    if (!DIR.Exists)
                    {
                        DIR.Create();
                    }
                    string[] listaNombre = pFileUpload.FileName.Split('.');
                    string NombreArchivo = string.Empty;
                    string ExtencionArchivo = string.Empty;
                    for (int i = 0; i < listaNombre.Length - 1; i++)
                    {
                        NombreArchivo += listaNombre[i];
                    }
                    ExtencionArchivo = listaNombre[listaNombre.Length - 1];
                    int count = 0;
                    string SegundaParteNombre = string.Empty;
                    //
                    bool isNombreRepetido = false;
                    List<cHistorialArchivoSubir> listaHistorialArchivoSubir = WebService.RecuperarHistorialSubirArchivoPorNombreArchivoOriginal(nombreCompletoOriginal);
                    if (listaHistorialArchivoSubir != null)
                    {
                        if (listaHistorialArchivoSubir.Count > 0)
                        {
                            isNombreRepetido = true;
                        }
                    }
                    while (System.IO.File.Exists(rutaTemporal + NombreArchivo + SegundaParteNombre + "." + ExtencionArchivo))
                    {
                        if (count > 0)
                        {
                            SegundaParteNombre = "_" + count.ToString();
                        }
                        count++;
                    }
                    string nombreCompleto = NombreArchivo + SegundaParteNombre + "." + ExtencionArchivo;
                    pFileUpload.SaveAs(rutaTemporal + nombreCompleto);
                    resultado = LeerArchivoPedido_Generica(nombreCompleto, pSucursal, nombreCompletoOriginal, isNombreRepetido);
                }
            }
            return resultado;
        }


        private static Boolean? LeerArchivoPedido_Generica(string pNombreArchivo, string pSucursal, string pNombreArchivoOriginal, Boolean? pIsNombreRepetido)
        {
            Boolean? resultado = false;
            if (!string.IsNullOrWhiteSpace(pNombreArchivo))
            {
                string rutaTemporal = Constantes.cRaizArchivos + @"\archivos\ArchivosPedidos\";
                string rutaTemporalAndNombreArchivo = rutaTemporal + pNombreArchivo;
                if (System.IO.File.Exists(rutaTemporalAndNombreArchivo))
                {
                    resultado = true;
                    string sLine = string.Empty;
                    StreamReader objReader = new StreamReader(rutaTemporalAndNombreArchivo);
                    string ext = Path.GetExtension(rutaTemporalAndNombreArchivo);
                    if (ext == null)
                        ext = string.Empty;
                    ext = ext.Replace(".", "").ToUpper();
                    bool isArchivoErroneo = false;
                    string TipoArchivo = string.Empty;
                    if (pNombreArchivo.Length > 0)
                    {
                        TipoArchivo = pNombreArchivo[0].ToString();
                    }
                    if (ext == "TXT" || ext == "ASC")
                    {
                        TipoArchivo = "S";
                    }
                    DataTable tablaArchivoPedidos = FuncionesPersonalizadas.ObtenerDataTableProductosCarritoArchivosPedidos();

                    while (sLine != null)
                    {
                        sLine = objReader.ReadLine();
                        if (!string.IsNullOrEmpty(sLine))
                        {
                            if (ext == "TXT")
                            {
                                DataRow r = LeerRenglonArchivoTXT(tablaArchivoPedidos, sLine);
                                if (r != null)
                                    tablaArchivoPedidos.Rows.Add(r);
                            }
                            else if (ext == "ASC")
                            {
                                DataRow r = LeerRenglonArchivoASC(tablaArchivoPedidos, sLine);
                                if (r != null)
                                    tablaArchivoPedidos.Rows.Add(r);
                            }
                            else
                            {
                                // Leer Archivo
                                switch (TipoArchivo)
                                {
                                    case "S":
                                        if (sLine.Length >= longFilaArchivoS) //22
                                        {
                                            DataRow r = LeerRenglonArchivoS(tablaArchivoPedidos, sLine);
                                            if (r != null)
                                                tablaArchivoPedidos.Rows.Add(r);
                                        }
                                        break;
                                    case "F":
                                        if (sLine.Length >= longFilaArchivoF)//24
                                        {
                                            DataRow r = LeerRenglonArchivoF(tablaArchivoPedidos, sLine);
                                            if (r != null)
                                                tablaArchivoPedidos.Rows.Add(r);
                                        }
                                        break;
                                    default:
                                        isArchivoErroneo = true;
                                        break;
                                }
                            }
                        }
                        if (isArchivoErroneo)
                        {
                            resultado = null;
                            break;
                        }
                    }
                    objReader.Close();
                    if (!isArchivoErroneo)
                    {
                        string sucElegida = pSucursal;// HiddenFieldSucursalEleginda.Value;
                        HttpContext.Current.Session["subirpedido_SucursalEleginda"] = sucElegida;
                        HttpContext.Current.Session["subirpedido_ListaProductos"] = WebService.AgregarProductoAlCarritoDesdeArchivoPedidosV5(sucElegida, ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codsuc, tablaArchivoPedidos, TipoArchivo, ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo, ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codprov, ((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_isGLN, ((capaDatos.Usuario)HttpContext.Current.Session["clientesDefault_Usuario"]).id);


                        //if (pIsNombreRepetido == null || pIsNombreRepetido.Value)
                        //{
                            HttpContext.Current.Session["subirpedido_nombreArchivoCompleto"] = pNombreArchivo;
                            HttpContext.Current.Session["subirpedido_nombreArchivoCompletoOriginal"] = pNombreArchivoOriginal;
                        //}
                        //else
                        //{
                            WebService.AgregarHistorialSubirArchivo(((cClientes)HttpContext.Current.Session["clientesDefault_Cliente"]).cli_codigo, sucElegida, pNombreArchivo, pNombreArchivoOriginal, DateTime.Now);
                        //}
                    }
                }
            }
            return resultado;
        }
        public static DataRow LeerRenglonArchivoTXT(DataTable pTabla, string pRenglon)
        {
            //DataRow r = null;
            //Primer columna numerica?
            try
            {
                int n;
                if (int.TryParse(pRenglon[0].ToString(), out n))
                {
                    string[] partSplit = pRenglon.Split('\t');
                    string strCodBarra = partSplit[5];
                    string strCantidad = partSplit[2];
                    return FuncionesPersonalizadas.ConvertProductosCarritoArchivosPedidosToDataRow(pTabla, null, Convert.ToInt32(strCantidad), strCodBarra, string.Empty, string.Empty, "S");
                }
            }
            catch (Exception ex)
            {
                FuncionesPersonalizadas.grabarLog(MethodBase.GetCurrentMethod(), ex, DateTime.Now, pTabla, pRenglon);
            }
            return null;
        }
        public static DataRow LeerRenglonArchivoASC(DataTable pTabla, string pRenglon)
        {
            try
            {
                string[] partSplit = pRenglon.Split(',');
                string strCodBarra = partSplit[4];
                string strCantidad = partSplit[2];
                return FuncionesPersonalizadas.ConvertProductosCarritoArchivosPedidosToDataRow(pTabla, null, Convert.ToInt32(strCantidad), strCodBarra, string.Empty, string.Empty, "S");
            }
            catch (Exception ex)
            {
                FuncionesPersonalizadas.grabarLog(MethodBase.GetCurrentMethod(), ex, DateTime.Now, pTabla, pRenglon);
            }
            return null;
        }
        public static DataRow LeerRenglonArchivoS(DataTable pTabla, string pRenglon)
        {
            try
            {
                //Cantidad: 5 dígitos
                //Alfa-Beta: 10 dígitos
                //Troquel: 10 dígitos
                //Cod. Barra: 13 dígitos
                //Característica: 1 dígito (ya no se usa)
                string strCantidad = string.Empty;
                for (int i = 0; i < 5; i++)
                {
                    strCantidad += pRenglon[i].ToString();
                }
                string strAlfaBeta = string.Empty;
                for (int i = 5; i < 15; i++)
                {
                    strAlfaBeta += pRenglon[i].ToString();
                }
                string strTroquel = string.Empty;
                for (int i = 15; i < 25; i++)
                {
                    strTroquel += pRenglon[i].ToString();
                }
                string strCodBarra = string.Empty;
                for (int i = 25; i < 38; i++)
                {
                    strCodBarra += pRenglon[i].ToString();
                }
                return FuncionesPersonalizadas.ConvertProductosCarritoArchivosPedidosToDataRow(pTabla, null, Convert.ToInt32(strCantidad), strCodBarra, strAlfaBeta, strTroquel, "S");
            }
            catch (Exception ex)
            {
                FuncionesPersonalizadas.grabarLog(MethodBase.GetCurrentMethod(), ex, DateTime.Now, pTabla, pRenglon);
            }
            return null;
        }
        public static DataRow LeerRenglonArchivoF(DataTable pTabla, string pRenglon)
        {
            try
            {
                //Nro Cliente: 4 dígitos
                //Cantidad: 5 dígitos (0 a la izquierda)
                //Nro de Producto:13 dígitos
                string strNroCliente = string.Empty;
                for (int i = 0; i < 4; i++)
                {
                    strNroCliente += pRenglon[i].ToString();
                }
                string strCantidad = string.Empty;
                for (int i = 4; i < 9; i++)
                {
                    strCantidad += pRenglon[i].ToString();
                }
                string strNroProducto = string.Empty;
                for (int i = 9; i < 22; i++)
                {
                    strNroProducto += pRenglon[i].ToString();
                }
                return FuncionesPersonalizadas.ConvertProductosCarritoArchivosPedidosToDataRow(pTabla, strNroProducto, Convert.ToInt32(strCantidad), null, null, null, "F");
            }
            catch (Exception ex)
            {
                FuncionesPersonalizadas.grabarLog(MethodBase.GetCurrentMethod(), ex, DateTime.Now, pTabla, pRenglon);
            }
            return null;
        }
        public static Boolean? CargarArchivoPedidoDeNuevo(int has_id)
        {
            Boolean? isRedireccionar = false;
            if (System.Web.HttpContext.Current.Session["clientesDefault_Usuario"] != null && System.Web.HttpContext.Current.Session["clientesDefault_Cliente"] != null)
            {
                cHistorialArchivoSubir objHistorialArchivoSubir = WebService.RecuperarHistorialSubirArchivoPorId(has_id);
                if (objHistorialArchivoSubir != null)
                    isRedireccionar = LeerArchivoPedido_Generica(objHistorialArchivoSubir.has_NombreArchivo, objHistorialArchivoSubir.has_sucursal, objHistorialArchivoSubir.has_NombreArchivoOriginal, null);
            }
            return isRedireccionar;
        }
    }
}