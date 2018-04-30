using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace ISSSTE.Tramites2016.QuejasMedicas.Helpers
{
    public class ConfiguracionHelper
    {

        public static string ObtenerConfiguracion(string IdConfiguracion)
        {
            return  HostingEnvironment.MapPath(ConfigurationManager.AppSettings[IdConfiguracion]);
        }

        public static String LeerTextoRuta(String RutaArchivo)
        {

            return  File.ReadAllText(RutaArchivo, Encoding.Default);
        }

    }
}
