using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mosqueral
{
    class Actualizador
    {
        private readonly string URL_PROGRAMA = "https://github.com/F0NT3/Mosqueral";
        private readonly string URL_ULTIMA_VERSION = "https://github.com/F0NT3/Mosqueral/releases";

        public Actualizador()
        {

        }

        public bool ComprobarActualizacionPrograma()
        {
            Log.Information("ACTUALIZADOR: Buscando una nueva actualización del programa...");

            string version;
            using (var wc = new WebClient())
            {
                version = wc.DownloadString(URL_ULTIMA_VERSION);
            }

            if (!version.Equals(Assembly.GetExecutingAssembly().GetName().Version.ToString()))
            {
                Log.Information("ACTUALIZADOR: Actualización encontrada.");
            }
            else
            {
                Log.Information("ACTUALIZADOR: No se ha encontrado ninguna actualización.");
            }
            return !version.Equals(Assembly.GetExecutingAssembly().GetName().Version.ToString());

        }

        public void actualizarPrograma()
        {
            Log.Information("ACTUALIZADOR: Actualizando Mosqueral...");

            WebClient wc = new WebClient();

            try
            {
                wc.DownloadFile(URL_PROGRAMA, "Mosqueral.exe");
                Log.Information("UPDATER: Actualización completada.");
            }
            catch (WebException ex)
            {
                Log.Error("ACTUALIZADOR: Actualización fallida.", ex.Message);
            }

        }
    }
}
