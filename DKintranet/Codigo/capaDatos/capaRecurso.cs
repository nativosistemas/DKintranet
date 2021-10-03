using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DKintranet.Codigo.clases;

namespace DKintranet.Codigo.capaDatos
{

    public class cArchivo : DKbase.web.capaDatos.cArchivo
    {

    }
    public class cNombreArchivos : DKbase.web.capaDatos.cNombreArchivos
    {
    }
    public class capaRecurso
    {
        public static DataSet GestiónArchivo(int? arc_codRecurso, int? arc_codRelacion, string arc_galeria, string arc_tipo, string arc_mime, string arc_nombre, string arc_titulo, string arc_descripcion, string arc_hash, int? arc_codUsuarioUltMov, int? arc_estado, int? arc_accion, string filtro, string accion)
        {
            return DKbase.web.capaDatos.capaRecurso.GestiónArchivo(arc_codRecurso, arc_codRelacion, arc_galeria, arc_tipo, arc_mime, arc_nombre, arc_titulo, arc_descripcion, arc_hash, arc_codUsuarioUltMov, arc_estado, arc_accion, filtro, accion);
        }
        public static string obtenerExtencion(string pNombreArchivo)
        {
            string resultado = string.Empty;
            if (!string.IsNullOrEmpty(pNombreArchivo))
            {
                string[] listaNombre = pNombreArchivo.Split('.');
                resultado = listaNombre[listaNombre.Length - 1].Trim().ToLower();
            }
            return resultado;
        }
        public static string obtenerMIME(string pExtencion)
        {
            string resultado = string.Empty;

            switch (pExtencion.Trim().ToLower())
            {
                case "jpe":
                    return Constantes.cMIME_jpe;
                //break;
                case "jpeg":
                    return Constantes.cMIME_jpeg;
                //break;
                case "jpg":
                    return Constantes.cMIME_jpg;
                //break;                
                case "doc":
                    return Constantes.cMIME_doc;
                //break;
                case "docx":
                    return Constantes.cMIME_docx;
                //break;
                case "xls":
                    return Constantes.cMIME_xls;
                //break;
                case "xlsx":
                    return Constantes.cMIME_xlsx;
                case "gif":
                    return Constantes.cMIME_gif;
                //break;
                case "png":
                    return Constantes.cMIME_png;
                //break;
                case "ai":
                    return Constantes.cMIME_ai;
                //break;
                case "pdf":
                    return Constantes.cMIME_pdf;
                //break;
                case "psd":
                    return Constantes.cMIME_psd;
                //break;
                case "vsd":
                    return Constantes.cMIME_vsd;
                //break;
                default:
                    break;
            }
            return resultado;
        }
        public static string nombreArchivoSinRepetir(string pPath, string pNombreArchivo)
        {
            string resultado = string.Empty;
            string[] listaNombre = pNombreArchivo.Split('.');
            string NombreArchivo = string.Empty;
            string ExtencionArchivo = string.Empty;
            for (int i = 0; i < listaNombre.Length - 1; i++)
            {
                NombreArchivo += listaNombre[i];
            }
            NombreArchivo = remplazarCaracteresEspeciales(NombreArchivo);
            ExtencionArchivo = listaNombre[listaNombre.Length - 1];
            int contNombre = -1;
            string NombreTemporal = NombreArchivo + "." + ExtencionArchivo;
            while (System.IO.File.Exists(pPath + "\\" + NombreTemporal))
            {
                contNombre++;
                NombreTemporal = NombreArchivo + "_" + contNombre.ToString() + "." + ExtencionArchivo;
            }
            if (contNombre == -1)
            {
                resultado = NombreArchivo + "." + ExtencionArchivo;
            }
            else
            {
                resultado = NombreTemporal;
            }
            return resultado;
        }
        public static string remplazarCaracteresEspeciales(string pStr)
        {
            //pStr = Encoding.UTF8.GetString(pStr.GetEnumerator());
            const string pStrOriginal = "áéíóúàèìòùâêîôûäëïöüãõñÁÉÍÓÚÀÈÌÒÙÂÊÎÔÛÄËÏÖÜÑçÇ";
            const string pStrARemplazar = "aeiouaeiouaeiouaeiouaonaeiouaeiouaeiouaeiouncc";
            if (pStr != null)
            {
                string resultado = string.Empty;
                for (int i = 0; i < pStrOriginal.Length; i++)
                {
                    pStr.Replace(pStrOriginal[i], pStrARemplazar[i]);
                }

                for (int i = 0; i < pStr.Length; i++)
                {
                    char[] CharEspeciales = new char[] { '\r', '\n', '\t' }; ;
                    if (pStr[i] == CharEspeciales[0] || pStr[i] == CharEspeciales[1] || pStr[i] == CharEspeciales[2])
                    {
                    }
                    else
                    {
                        resultado += pStr[i];
                    }

                }
                return resultado;
            }
            else
            {
                return null;
            }
        }
    }
}