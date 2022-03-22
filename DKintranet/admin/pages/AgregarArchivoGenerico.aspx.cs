using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.Services;
using DKintranet.Codigo;
using DKintranet.Codigo.clases;
using DKintranet.Codigo.capaDatos;
using DKbase.web.capaDatos;

namespace DKintranet.admin.pages
{
    public partial class AgregarArchivoGenerico : cBaseAdmin
    {
        public const string consPalabraClave = "gestionhome";
        protected void Page_Load(object sender, EventArgs e)
        {
            Seguridad(consPalabraClave);
            if (!IsPostBack)
            {
                htmlArchivo obj = null;
                if (Request.QueryString.AllKeys.Contains("id") && Request.QueryString.AllKeys.Contains("t"))
                {
                    //
                    obj = new htmlArchivo();
                    obj.id = Convert.ToInt32(Request.QueryString.Get("id"));
                    obj.tipo = Request.QueryString.Get("t");
                    if (obj.tipo != "popup")
                    {
                        List<cArchivo> listaArchivo = WebService.RecuperarTodosArchivos(obj.id, obj.tipo, string.Empty);
                        if (listaArchivo != null)
                        {
                            cArchivo o = listaArchivo.FirstOrDefault();
                            if (o != null)
                            {
                                obj.arc_nombre = o.arc_nombre;
                                obj.codRecurso = o.arc_codRecurso;
                                obj.titulo = o.arc_titulo;
                                obj.descr = o.arc_descripcion;
                                obj.objArchivo = o;
                            }
                        }
                    }
                }
                else if (Request.QueryString.AllKeys.Contains("idRecurso"))
                {
                    cArchivo o = WebService.RecuperarArchivoPorId(Convert.ToInt32(Request.QueryString.Get("idRecurso")));
                    if (o != null)
                    {
                        obj = new htmlArchivo();
                        obj.id = o.arc_codRelacion;
                        obj.tipo = o.arc_galeria;
                        obj.arc_nombre = o.arc_nombre;
                        obj.codRecurso = o.arc_codRecurso;
                        obj.titulo = o.arc_titulo;
                        obj.descr = o.arc_descripcion;
                        obj.objArchivo = o;
                    }
                }
                if (obj != null)
                    HttpContext.Current.Session["AgregarArchivoGenerico_obj"] = obj;
            }
            else if (HttpContext.Current.Session["AgregarArchivoGenerico_obj"] != null)
            {
                htmlArchivo obj = (htmlArchivo)HttpContext.Current.Session["AgregarArchivoGenerico_obj"];
                Label1.Text = "";
                Boolean fileOK = false;
                String path = Constantes.cRaizArchivos + @"\archivos\" + obj.tipo + @"\";
                if (Directory.Exists(path) == false)
                {
                    Directory.CreateDirectory(path);
                }
                if (FileUpload1.HasFile)
                {
                    fileOK = true;
                }

                if (fileOK)
                {
                    try
                    {
                        string nombre = FileUpload1.FileName;
                        nombre = DKintranet.Codigo.clases.Generales.Texto.limpiarNombreArchivo(nombre);
                        nombre = "a" + nombre;
                        string[] listaParteNombre = nombre.Split('.');
                        string CacheNombreArchivo = string.Empty;
                        string CacheExtencionArchivo = string.Empty;
                        for (int i = 0; i < listaParteNombre.Length - 1; i++)
                        {
                            CacheNombreArchivo += listaParteNombre[i];
                        }
                        CacheExtencionArchivo = listaParteNombre[listaParteNombre.Length - 1];
                        int cont = -1;
                        string parteNueva = string.Empty;
                        string nombreFinal = CacheNombreArchivo + parteNueva + "." + CacheExtencionArchivo;
                        while (System.IO.File.Exists(path + nombreFinal))
                        {
                            cont++;
                            parteNueva = cont.ToString();
                            nombreFinal = CacheNombreArchivo + parteNueva + "." + CacheExtencionArchivo;
                        }

                        FileUpload1.PostedFile.SaveAs(path + nombreFinal);
                        string titulo = String.Format("{0}", Request.Form["txt_titulo"]);
                        // string descr = String.Format("{0}", Request.Form["txt_descr"]);
                        string descr_aux = String.Format("{0}", Request.Form["hiddenSucursales"]);
                        string descr = string.Empty;
                        if (!string.IsNullOrWhiteSpace(descr_aux))
                        {
                            string[] words = descr_aux.Split('-');
                            foreach (string word in words)
                            {
                                descr += "<" + word + ">";
                            }
                        }

                        obj.codRecurso = WebService.InsertarActualizarArchivo(obj.codRecurso, obj.id, obj.tipo, CacheExtencionArchivo, FileUpload1.PostedFile.ContentType, nombreFinal, titulo, descr, string.Empty, Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["codigoUsuarioSinRegistrar"]));
                        HttpContext.Current.Session["AgregarArchivoGenerico_obj"] = null;




                        switch (obj.tipo)
                        {
                            case "ofertaspdf":
                                Response.Redirect("GestionOferta.aspx?isVolver=1");
                                break;
                            case "recallpdf":
                                Response.Redirect("GestionReCall.aspx");
                                break;
                            case "popup":
                                //WebService.ActualizarImagenHomeSlide(obj.id, obj.codRecurso, obj.ancho == 700 ? 2 : 1);
                                cThumbnail.obtenerImagen(obj.tipo, nombreFinal, "1024", "768", "", false);
                                Response.Redirect("GestionPopUp.aspx");
                                break;
                            default:
                                break;
                        }
                        //}
                    }
                    catch (Exception ex)
                    {
                        //Label1.Text = "File could not be uploaded.";
                    }
                }
                else if (!fileOK && obj.objArchivo != null && obj.tipo == "popup")
                {
                    string titulo = String.Format("{0}", Request.Form["txt_titulo"]);
                    //string descr = String.Format("{0}", Request.Form["txt_descr"]);
                    string descr_aux = String.Format("{0}", Request.Form["hiddenSucursales"]);
                    string descr = string.Empty;
                    if (!string.IsNullOrWhiteSpace(descr_aux))
                    {
                        string[] words = descr_aux.Split('-');
                        foreach (string word in words)
                        {
                            descr += "<" + word + ">";
                        }
                    }

                    obj.codRecurso = WebService.InsertarActualizarArchivo(obj.codRecurso, obj.id, obj.tipo, obj.objArchivo.arc_tipo, obj.objArchivo.arc_mime, obj.objArchivo.arc_nombre, titulo, descr, string.Empty, Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["codigoUsuarioSinRegistrar"]));
                    HttpContext.Current.Session["AgregarArchivoGenerico_obj"] = null;

                    switch (obj.tipo)
                    {
                        case "ofertaspdf":
                            Response.Redirect("GestionOferta.aspx?isVolver=1");
                            break;
                        case "recallpdf":
                            Response.Redirect("GestionReCall.aspx");
                            break;
                        case "popup":
                            // WebService.ActualizarImagenHomeSlide(obj.id, obj.codRecurso, obj.ancho == 700 ? 2 : 1);
                            Response.Redirect("GestionPopUp.aspx");
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    Label1.Text = "Error: No se subió ningún archivo o archivo incorrecto.";
                }

            }
        }
        [WebMethod(EnableSession = true)]
        public static bool EliminarArchivoPorId(int pArc_codRecurso)
        {
            WebService.EliminarArchivoPorId(pArc_codRecurso);
            //HttpContext.Current.Session["AgregarArchivoGenerico_obj"] = null;
            return true;
        }
        public void AgregarHtmlOculto()
        {
            if (HttpContext.Current.Session["AgregarArchivoGenerico_obj"] != null)
            {
                string resultado = string.Empty;
                htmlArchivo obj = (htmlArchivo)HttpContext.Current.Session["AgregarArchivoGenerico_obj"];
                resultado += "<input type=\"hidden\" id=\"hiddenFile\" value=\"" + Server.HtmlEncode(DKintranet.Codigo.clases.Generales.Serializador.SerializarAJson(obj)) + "\" />";
                resultado += "<input type=\"hidden\" id=\"hiddenTodasSucursales\" value=\"" + Server.HtmlEncode(DKintranet.Codigo.clases.Generales.Serializador.SerializarAJson(WebService.RecuperarTodasSucursales())) + "\" />";
                Response.Write(resultado);
            }
        }
    }
}