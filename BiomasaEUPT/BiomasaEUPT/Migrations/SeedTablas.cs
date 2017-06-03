using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Migrations
{
    public class SeedTablas
    {
        private BiomasaEUPTContext context;
        private Assembly assembly;
        private string NOMBRE_CSV = "BiomasaEUPT.Migrations.DatosSeed.{0}.csv";

        private sealed class ComunidadMap : CsvClassMap<Comunidad>
        {
            public ComunidadMap()
            {
                Map(m => m.ComunidadId);
                Map(m => m.Codigo);
                Map(m => m.Nombre);
                Map(m => m.PaisId);
            }
        }

        private sealed class ProvinciaMap : CsvClassMap<Provincia>
        {
            public ProvinciaMap()
            {
                Map(m => m.ProvinciaId);
                Map(m => m.Codigo);
                Map(m => m.Nombre);
                Map(m => m.ComunidadId);
            }
        }
        public SeedTablas(BiomasaEUPTContext context)
        {
            this.context = context;
            assembly = Assembly.GetExecutingAssembly();

            CsvConfiguration csvConfig = new CsvConfiguration()
            {
                WillThrowOnMissingField = false,
                Delimiter = ";"
            };

            string resourceName = String.Format(NOMBRE_CSV, "TiposUsuarios");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var tiposUsuarios = csvReader.GetRecords<TipoUsuario>().ToArray();
                    context.TiposUsuarios.AddOrUpdate(tu => tu.Nombre, tiposUsuarios);
                }
            }
            context.SaveChanges();

            //context.TiposUsuarios.Load();
            resourceName = String.Format(NOMBRE_CSV, "Usuarios");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var usuarios = csvReader.GetRecords<Usuario>().ToArray();
                    context.Usuarios.AddOrUpdate(u => u.Nombre, usuarios);
                }
            }
            context.SaveChanges();

            resourceName = String.Format(NOMBRE_CSV, "Paises");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var paises = csvReader.GetRecords<Pais>().ToArray();
                    context.Paises.AddOrUpdate(p => p.Codigo, paises);
                }
            }
            context.SaveChanges();

            //context.Paises.Load();
            resourceName = String.Format(NOMBRE_CSV, "Comunidades");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    // Hay que mapear cada columna ya que la columna Codigo
                    // piensa que es la de la tabla Pais
                    csvReader.Configuration.RegisterClassMap<ComunidadMap>();
                    var comunidades = csvReader.GetRecords<Comunidad>().ToArray();
                    context.Comunidades.AddOrUpdate(p => p.Codigo, comunidades);
                }
            }
            context.SaveChanges();

            //context.Comunidades.Load();
            resourceName = String.Format(NOMBRE_CSV, "Provincias");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    csvReader.Configuration.RegisterClassMap<ProvinciaMap>();
                    var provincias = csvReader.GetRecords<Provincia>().ToArray();
                    context.Provincias.AddOrUpdate(p => p.Codigo, provincias);
                }
            }
            context.SaveChanges();

            /*
            resourceName = String.Format(NOMBRE_CSV, "Municipios");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    while (csvReader.Read())
                    {
                        var municipio = csvReader.GetRecord<Municipio>();
                        var codigoProvincia = csvReader.GetField<string>("CodigoProvincia");
                        municipio.Provincia = context.Provincias.Local.Single(c => c.Codigo == codigoProvincia);
                        context.Municipios.AddOrUpdate(p => p.MunicipioId, municipio);
                    }
                }
            }
            /*
            resourceName = String.Format(NOMBRE_CSV, "TiposClientes");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var tiposClientes = csvReader.GetRecords<TipoCliente>().ToArray();
                    context.TiposClientes.AddOrUpdate(c => c.Nombre, tiposClientes);
                }
            }

            resourceName = String.Format(NOMBRE_CSV, "GruposClientes");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var gruposClientes = csvReader.GetRecords<GrupoCliente>().ToArray();
                    context.GruposClientes.AddOrUpdate(c => c.Nombre, gruposClientes);
                }
            }

            resourceName = String.Format(NOMBRE_CSV, "Clientes");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    while (csvReader.Read())
                    {
                        var cliente = csvReader.GetRecord<Cliente>();
                        var tipoCliente = csvReader.GetField<string>("TipoCliente");
                        cliente.TipoCliente = context.TiposClientes.Local.Single(c => c.Nombre == tipoCliente);
                        var grupoCliente = csvReader.GetField<string>("GrupoCliente");
                        cliente.GrupoCliente = context.GruposClientes.Local.Single(c => c.Nombre == grupoCliente);
                        var municipio = csvReader.GetField<string>("Municipio");
                        cliente.Municipio = context.Municipios.Local.Single(c => c.CodigoPostal == municipio);
                        context.Clientes.AddOrUpdate(p => p.RazonSocial, cliente);
                    }
                }
            }


            resourceName = String.Format(NOMBRE_CSV, "EstadosRecepciones");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var estadosRecepciones = csvReader.GetRecords<EstadoRecepcion>().ToArray();
                    context.EstadosRecepciones.AddOrUpdate(c => c.Nombre, estadosRecepciones);
                }
            }

            resourceName = String.Format(NOMBRE_CSV, "EstadosElaboraciones");
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    CsvReader csvReader = new CsvReader(reader, csvConfig);
                    var estadosElaboraciones = csvReader.GetRecords<EstadoElaboracion>().ToArray();
                    context.EstadosElaboraciones.AddOrUpdate(c => c.Nombre, estadosElaboraciones);
                }
            }*/

            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }
}
