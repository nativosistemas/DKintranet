﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;

namespace DKintranet.Codigo.clases.Generales
{
    /// <summary>
    /// Summary description for Texto
    /// </summary>
    public class Texto
    {

        public static string CortarBajada(string pTexto, int pCorte)
        {
            if (pTexto.Length > pCorte)
            {
                string texAux = pTexto.Substring(pCorte, pTexto.Length - pCorte);
                int Posicion = -1;
                if (texAux.Contains(" "))
                {
                    Posicion = texAux.IndexOf(" ");
                }
                if (Posicion == -1)
                {
                    return pTexto.Substring(0, pCorte) + texAux;
                }
                else
                {
                    return pTexto.Substring(0, pCorte) + texAux.Substring(0, Posicion) + "&nbsp;... ";
                }
            }
            else
            {
                return pTexto;
            }
        }
        public static string CambiarComillaSimplePorDoble(string pTexto)
        {
            return pTexto.Replace("'", "\"");
        }

        private const string consignos = "áàäéèëíìïóòöúùuñÁÀÄÉÈËÍÌÏÓÒÖÚÙÜÑçÇÑ";
        private const string sinsignos = "aaaeeeiiiooouuunAAAEEEIIIOOOUUUNcCN";
        public static string limpiarNombreArchivo(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
        }
        public static string removerSignosAcentos(String texto)
        {
            StringBuilder textoSinAcentos = new StringBuilder(texto.Length);
            int indexConAcento;
            foreach (char caracter in texto)
            {
                indexConAcento = consignos.IndexOf(caracter);
                if (indexConAcento > -1)
                {
                    textoSinAcentos.Append(sinsignos.Substring(indexConAcento, 1));
                }
                else
                {
                    textoSinAcentos.Append(caracter);
                }
            }
            return textoSinAcentos.ToString();
        }
        //CompareInfo comp = CultureInfo.InvariantCulture.CompareInfo;
        //comp = CompareInfo.GetCompareInfo("es-AR");
        //CompareOptions compOpt = CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase;
        //int  ic = comp.Compare("s1"," s2", compOpt);
        public static string ConvertToStringHTML(string pValor)
        {
            string resultado = string.Empty;
            resultado = HttpUtility.HtmlEncode(pValor);
            return resultado;
        }
        public static string SacarTagHtml(string pTexto)
        {
            return Regex.Replace(pTexto, @"<(.|\n)*?>", string.Empty);
        }
        /// <summary>
        ///&ntilde; ñ
        ///&Ntilde; Ñ
        ///&aacute; á
        ///&Aacute; Á
        ///&eacute; é
        ///&Eacute; É
        ///&iacute; í
        ///&Iacute; Í
        ///&oacute; ó
        ///&Oacute; Ó
        ///&uacute; ú
        ///&Uacute; Ú
        /// </summary>
        /// <param name="pTexto"></param>
        /// <returns></returns>
        //public static string ConvertirStringHTML(string pTexto)
        //{
        //    string strResultado = pTexto;
        //    strResultado = strResultado.Replace("\n", "<br/>"); // Enter = nueva linea -- "\n" = chr(10)
        //    strResultado = strResultado.Replace("\r", "<br/>"); // Enter = nueva linea --     "\r" = chr(13)
        //    strResultado = strResultado.Replace("ñ", "&ntilde;");
        //    strResultado = strResultado.Replace("Ñ", "&Ntilde;");
        //    strResultado = strResultado.Replace("á", "&aacute;");
        //    strResultado = strResultado.Replace("Á", "&Aacute;");
        //    strResultado = strResultado.Replace("é", "&eacute;");
        //    strResultado = strResultado.Replace("É", "&Eacute;");
        //    strResultado = strResultado.Replace("í", "&iacute;");
        //    strResultado = strResultado.Replace("Í", "&Iacute;");
        //    strResultado = strResultado.Replace("ó", "&oacute;");
        //    strResultado = strResultado.Replace("Ó", "&Oacute;");
        //    strResultado = strResultado.Replace("ú", "&uacute;");
        //    strResultado = strResultado.Replace("Ú", "&Uacute;");
        //    return strResultado;
        //}
    }
}