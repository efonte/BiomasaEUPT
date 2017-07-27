using BiomasaEUPT.Vistas;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BiomasaEUPT
{
    public class Actualizador
    {
        private readonly string URL_PROGRAMA = "https://api.github.com/repos/F0NT3/BiomasaEUPT/releases/latest";
        //private readonly string URL_ULTIMA_VERSION = "https://github.com/F0NT3/BiomasaEUPT/releases";

        public SplashViewModel SplashViewModel { get; set; }

        public Actualizador()
        {

        }

        public bool ComprobarActualizacionPrograma()
        {
            SplashViewModel.MensajeInformacion = "Buscando actualizaciones...";
            SplashViewModel.Progreso = 10;

            string version = "";

            var wc = new WebClient();
            wc.Headers.Add("User-Agent", "Nothing");

            try
            {
                var content = wc.DownloadString(URL_PROGRAMA);
                var serializer = new DataContractJsonSerializer(typeof(List<DatosGitHub>));
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(content)))
                {
                    var datos = (List<DatosGitHub>)serializer.ReadObject(ms);
                    datos.ForEach(Console.WriteLine);
                }
            }
            catch (WebException ex)
            {
                Console.WriteLine(ex.Message);
            }

            /*using (var wc = new WebClient())
            {
                version = wc.DownloadString(URL_ULTIMA_VERSION);
            }*/

            if (!version.Equals(Assembly.GetExecutingAssembly().GetName().Version.ToString()))
            {
                SplashViewModel.MensajeInformacion = "Actualización encontrada.";
            }
            else
            {
                SplashViewModel.MensajeInformacion = "No se ha encontrado ninguna actualización.";
            }
            return !version.Equals(Assembly.GetExecutingAssembly().GetName().Version.ToString());

        }

        public void ActualizarPrograma()
        {
            SplashViewModel.MensajeInformacion = "Actualizando...";

            var wc = new WebClient();

            try
            {
                wc.DownloadFile(URL_PROGRAMA, "BiomasaEUPT.zip");
                SplashViewModel.MensajeInformacion = "Actualización completada.";
            }
            catch (WebException ex)
            {
                SplashViewModel.MensajeInformacion = "Actualización fallida. " + ex.Message;
            }
        }

        internal class DatosGitHub
        {
            public string tag_name { get; set; }

        }
    }
}
