using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using DKintranet.Codigo.clases.Generales;

namespace DKintranet.Codigo.clases
{
    /// <summary>
    /// Descripción breve de cThumbnail
    /// </summary>
    public class cThumbnail
    {
        public static System.Drawing.Image obtenerImagen(string ruta, string nombre, string strAncho, string strAlto, string strColor, bool boolAlto)
        {

            return DKbase.web.cThumbnail_base.obtenerImagen(ruta, nombre, strAncho, strAlto, strColor, boolAlto);
        }
        public static System.Drawing.Image RedimencionarImagen(System.Drawing.Image srcImage, int newWidth, int newHeight, int pAnchoOriginal, int pAltoOriginal, Color pColor)
        {
            return DKbase.web.cThumbnail_base.RedimencionarImagen(srcImage, newWidth, newHeight, pAnchoOriginal, pAltoOriginal, pColor);
        }
        public static System.Drawing.Image RedimencionarImagen(System.Drawing.Image srcImage, int newWidth, int newHeight)
        {
            return DKbase.web.cThumbnail_base.RedimencionarImagen(srcImage, newWidth, newHeight);
        }
    }
}