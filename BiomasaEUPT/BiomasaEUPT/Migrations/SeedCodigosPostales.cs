using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Migrations
{
    class SeedCodigosPostales
    {
        private BiomasaEUPTContext context;
        private Assembly assembly;
        public SeedCodigosPostales(BiomasaEUPTContext context)
        {
            this.context = context;
            assembly = Assembly.GetExecutingAssembly();

            Paises(context);
            Comunidades(context);
        }

        void Paises(BiomasaEUPTContext context)
        {
            string resourceName = "BiomasaEUPT.Migrations.DatosSeed.SeedPaises.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.Delimiter = ";";
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    var paises = csvReader.GetRecords<Pais>().ToArray();
                    context.Paises.AddOrUpdate(c => c.Codigo, paises);
                }
            }
            context.SaveChanges();
        }

        void Comunidades(BiomasaEUPTContext context)
        {
            context.Paises.Load();
            string resourceName = "BiomasaEUPT.Migrations.DatosSeed.SeedComunidades.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    while (csvReader.Read())
                    {
                        var comunidad = csvReader.GetRecord<Comunidad>();
                        var codigoPais = csvReader.GetField<string>("Codigo").Substring(0, 2);
                        Console.WriteLine(codigoPais);
                        Debug.WriteLine("Debug Test");

                        comunidad.PaisId = context.Paises.Local.Single(c => c.Codigo == codigoPais).PaisId;
                        context.Comunidades.AddOrUpdate(p => p.Codigo, comunidad);
                    }
                }
            }
            context.SaveChanges();
        }
    }
}
