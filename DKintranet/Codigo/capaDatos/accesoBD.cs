using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DKintranet.Codigo.capaDatos
{
    public class accesoBD
    {
        public static string ObtenerConexión()
        {
            string strConexión;
            strConexión = ConfigurationManager.ConnectionStrings["db_conexion"].ConnectionString;
            return strConexión;
        }
    }
}