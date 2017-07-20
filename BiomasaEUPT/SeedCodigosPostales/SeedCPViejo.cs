using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SeedCodigosPostales
{
    class SeedCPViejo
    {
        string URL_DESCARGA = "http://download.geonames.org/export/zip/{0}.zip";
        string NOMBRE_FICHERO = "SeedCodigosPostales.txt";

        private string[] codigosPaises = { "ES", "FR" };
        private List<string> seedCP;
        private List<string> datosCP;

        public SeedCPViejo()
        {

        }

        public SeedCPViejo(string[] codigosPaises) : base()
        {
            this.codigosPaises = codigosPaises;
        }

        public void Generar()
        {
            datosCP = new List<string>();
            foreach (var p in codigosPaises)
            {
                datosCP = datosCP.Concat(ObtenerListaDatosCP(p)).ToList();
            }

            seedCP = ObtenerListaSeedCP(datosCP);

            File.WriteAllLines(NOMBRE_FICHERO, seedCP);
            Console.WriteLine("\nFichero \"" + NOMBRE_FICHERO + "\" generado correctamente.");
        }

        private List<string> ObtenerListaDatosCP(string codigoPais)
        {
            List<string> lineas = new List<string>();
            using (ZipFile zip = ZipFile.Read(new MemoryStream(new WebClient().DownloadData(string.Format(URL_DESCARGA, codigoPais)))))
            {
                MemoryStream memoryStream = new MemoryStream();
                zip[codigoPais + ".txt"].Extract(memoryStream);
                //string datosPais = Encoding.UTF8.GetString(memoryStream.ToArray());

                memoryStream.Position = 0;
                using (var reader = new StreamReader(memoryStream, Encoding.UTF8))
                {
                    while (!reader.EndOfStream)
                    {
                        var linea = reader.ReadLine();
                        lineas.Add(linea);
                    }
                }

                return lineas;
            }
        }

        private List<string> ObtenerListaSeedCP(List<string> datosCP)
        {
            List<string> lineas = new List<string>();
            //var datos = from linea in lineas select (linea.Split('\t')).ToArray();
            //Console.WriteLine(datos.Select(d => d[1]).Count());
            var codigosPostales = datosCP.Select(l => new
            {
                CodigoPais = l.Split('\t').ElementAt(0),
                CodidoPostal = l.Split('\t').ElementAt(1),
                Municipio = l.Split('\t').ElementAt(2),
                Comunidad = l.Split('\t').ElementAt(3),
                CodigoComunidad = l.Split('\t').ElementAt(4),
                Provincia = l.Split('\t').ElementAt(5),
                CodigoProvincia = l.Split('\t').ElementAt(6),
                Latitud = l.Split('\t').ElementAt(9),
                Longitud = l.Split('\t').ElementAt(10)
            });

            lineas.Add("context.Paises.AddOrUpdate( c => c.Codigo,");
            var paises = codigosPostales.Select(c => new { c.CodigoPais }).Distinct().OrderBy(c => c.CodigoPais).ToList();
            for (int i = 0; i < paises.Count(); i++)
            {
                lineas.Add("    new Pais() { Codigo = \"" + paises[i].CodigoPais + "\", Nombre = \"" + new RegionInfo(paises[i].CodigoPais).DisplayName + "\" }" + (i != paises.Count() - 1 ? "," : ""));
                Console.Write("\rParseando Países {0,3}%", i * 100 / paises.Count());
            }
            Console.WriteLine("\rParseando Países 100%");
            lineas.Add(");\n\ncontext.SaveChanges();\n\n");

            lineas.Add("context.Comunidades.AddOrUpdate( c => c.Codigo,");
            var comunidades = codigosPostales.Select(c => new { c.CodigoPais, c.CodigoComunidad, c.Comunidad }).Distinct().OrderBy(c => c.Comunidad).ToList();
            for (int i = 0; i < comunidades.Count(); i++)
            {
                if (comunidades[i].CodigoComunidad != "") // El fichero de Francia no está bien
                {
                    lineas.Add("    new Comunidad() { Codigo = \"" + comunidades[i].CodigoPais + "-" + comunidades[i].CodigoComunidad + "\", Nombre = \"" + comunidades[i].Comunidad + "\", PaisId = context.Paises.FirstOrDefault(x => x.Codigo == \"" + comunidades[i].CodigoPais + "\").PaisId }" + (i != comunidades.Count() - 1 ? "," : ""));
                }
                Console.Write("\rParseando Comunidades {0,3}%", i * 100 / comunidades.Count());
            }
            Console.WriteLine("\rParseando Comunidades 100%");
            lineas.Add(");\n\ncontext.SaveChanges();\n\n");

            lineas.Add("context.Provincias.AddOrUpdate( c => c.Codigo,");
            var provincias = codigosPostales.Select(c => new { c.CodigoPais, c.CodigoComunidad, c.CodigoProvincia, c.Provincia }).Distinct().OrderBy(c => c.Provincia).ToList();
            for (int i = 0; i < provincias.Count(); i++)
            {
                if (provincias[i].CodigoProvincia != "") // El fichero de Francia no está bien
                {
                    lineas.Add("    new Provincia() { Codigo = \"" + provincias[i].CodigoPais + "-" + provincias[i].CodigoProvincia + "\", Nombre = \"" + provincias[i].Provincia + "\", ComunidadId = context.Comunidades.FirstOrDefault(x => x.Codigo == \"" + provincias[i].CodigoPais + "-" + provincias[i].CodigoComunidad + "\").ComunidadId }" + (i != provincias.Count() - 1 ? "," : ""));
                }
                Console.Write("\rParseando Provincias {0,3}%", i + 1 * 100 / provincias.Count());
            }
            Console.WriteLine("\rParseando Provincias 100%");
            lineas.Add(");\n\ncontext.SaveChanges();\n\n");

            lineas.Add("context.Municipios.AddOrUpdate( c => c.MunicipioId,");
            var municipios = codigosPostales.Select(c => new { c.CodigoPais, c.CodigoProvincia, c.Municipio, c.Latitud, c.Longitud }).Distinct().OrderBy(c => c.Municipio).ToList();
            for (int i = 0; i < municipios.Count(); i++)
            {
                if (municipios[i].CodigoProvincia != "") // El fichero de Francia no está bien
                {
                    lineas.Add("    new Municipio() { Nombre = \"" + municipios[i].Municipio + "\", Latitud = " + municipios[i].Latitud + ", Longitud = " + municipios[i].Longitud + ", ProvinciaId = context.Provincias.FirstOrDefault(x => x.Codigo == \"" + municipios[i].CodigoPais + "-" + municipios[i].CodigoProvincia + "\").ProvinciaId }" + (i != municipios.Count() - 1 ? "," : ""));
                }
                Console.Write("\rParseando Municipios {0,3}%", i * 100 / municipios.Count());
            }
            Console.WriteLine("\rParseando Municipios 100%");
            lineas.Add(");\n\ncontext.SaveChanges();\n\n");

            return lineas;
        }
    }
}
