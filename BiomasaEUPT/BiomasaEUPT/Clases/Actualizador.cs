using BiomasaEUPT.Vistas;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
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
        private readonly string URL_RELEASES = "https://api.github.com/repos/F0NT3/BiomasaEUPT/releases";

        public string VersionOnline { get; set; }
        public string UrlDescarga { get; set; }

        public Actualizador()
        {

        }

        public bool ComprobarActualizacionPrograma()
        {
#if (DEBUG)
            return false;
#endif
            var wc = new WebClient();
            wc.Headers.Add("User-Agent", "Nothing");

            var contenido = "";
            try { contenido = wc.DownloadString(URL_RELEASES); }
            catch (WebException ex) { Console.WriteLine(ex.Message); }

            var o = JArray.Parse(contenido)[0];
            string tag_name = (string)o["tag_name"];
            bool prerelease = (bool)o["prerelease"];
            UrlDescarga = (string)o.SelectToken("assets[0].browser_download_url");

            // Se obtiene la versión sin la "v". Ejemplo v1.2.3 -> 1.2.3
            VersionOnline = tag_name.StartsWith("v") ? tag_name.Substring(1) : tag_name;
            //VersionOnline = "1";
            return !VersionOnline.Equals(Assembly.GetExecutingAssembly().GetName().Version.ToString());
        }

        public void ActualizarPrograma()
        {
            // Se descarga el zip que contiene la nueva versión
            var wc = new WebClient();
            wc.DownloadFile(UrlDescarga, "BiomasaEUPT.zip");
            if (Directory.Exists("actualizacion"))
            {
                Directory.Delete("actualizacion", true);
            }
            // Se extrae en la carpeta actualización
            ZipFile.ExtractToDirectory("BiomasaEUPT.zip", "actualizacion");

            // Se hace un backup de todos los ficheros menos los relativos al desinstalador
            var directorioRaiz = new DirectoryInfo(".");
            directorioRaiz.GetFiles("*", SearchOption.AllDirectories)
                .Where(f => !f.DirectoryName.Contains("actualizacion") && !f.Name.Contains("unins"))
                .ToList()
                .ForEach(f => File.Move(f.FullName, f.FullName + ".bak"));

            // Se mueven los nuevos ficheros
            var rutaEjecutable = Directory.GetFiles("actualizacion", "BiomasaEUPT.exe", SearchOption.AllDirectories).FirstOrDefault();
            var subdirectorio = Path.GetDirectoryName(rutaEjecutable);
            var directorioActualizacion = new DirectoryInfo(subdirectorio);
            directorioActualizacion.GetFiles("*", SearchOption.AllDirectories).ToList()
              .ForEach(f => File.Copy(f.FullName, Path.Combine(".", f.Name)));
        }
    }
}
