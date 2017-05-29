using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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

            // Paises(context);
            // Comunidades(context);
            // Provincias(context);
            Municipios(context);
        }

        void Paises(BiomasaEUPTContext context)
        {
            string resourceName = "BiomasaEUPT.Migrations.DatosSeed.SeedPaises.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    csvReader.Configuration.Delimiter = ";";
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
                    csvReader.Configuration.Delimiter = ";";
                    while (csvReader.Read())
                    {
                        var comunidad = csvReader.GetRecord<Comunidad>();
                        var codigoPais = csvReader.GetField<string>("Codigo").Substring(0, 2);
                        comunidad.Pais = context.Paises.Local.Single(c => c.Codigo == codigoPais);
                        context.Comunidades.AddOrUpdate(p => p.Codigo, comunidad);
                    }
                }
            }
            context.SaveChanges();
        }

        void Provincias(BiomasaEUPTContext context)
        {
            context.Comunidades.Load();
            string resourceName = "BiomasaEUPT.Migrations.DatosSeed.SeedProvincias.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    csvReader.Configuration.Delimiter = ";";
                    while (csvReader.Read())
                    {
                        var provincia = csvReader.GetRecord<Provincia>();
                        var codigoComunidad = csvReader.GetField<string>("CodigoComunidad");
                        provincia.Comunidad = context.Comunidades.Local.Single(c => c.Codigo == codigoComunidad);
                        context.Provincias.AddOrUpdate(p => p.Codigo, provincia);
                    }
                }
            }
            context.SaveChanges();
        }

        void Municipios(BiomasaEUPTContext context)
        {
            context.Provincias.Load();
            string resourceName = "BiomasaEUPT.Migrations.DatosSeed.SeedMunicipios.csv";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    CsvReader csvReader = new CsvReader(reader);
                    csvReader.Configuration.WillThrowOnMissingField = false;
                    csvReader.Configuration.Delimiter = ";";
                    while (csvReader.Read())
                    {
                        var municipio = csvReader.GetRecord<Municipio>();
                        var codigoProvincia = csvReader.GetField<string>("CodigoProvincia");
                        municipio.Provincia = context.Provincias.Local.Single(c => c.Codigo == codigoProvincia);
                        context.Municipios.AddOrUpdate(p => p.MunicipioId, municipio);
                    }
                }
            }
            context.SaveChanges();
        }
    }
}
