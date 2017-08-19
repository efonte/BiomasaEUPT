using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace BiomasaEUPT
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string mensaje = "";
            //var excepcion = e.Exception.InnerException;
            var excepcion = e.Exception;

            if (excepcion is DbEntityValidationException)
            {
                var mensajesError = (excepcion as DbEntityValidationException).EntityValidationErrors
                      .SelectMany(x => x.ValidationErrors)
                      .Select(x => x.ErrorMessage);

                mensaje = "No pueden guardar los cambios:\n\n" + string.Join("\n\n", mensajesError);
            }
            else if (excepcion is DbUpdateException ex1)
            {
                if (ExceptionHelper.IsUniqueConstraintViolation(ex1))
                {
                    mensaje = "No se ha podido modificar el campo.";
                    foreach (var entry in ex1.Entries)
                    {
                        if (entry.State == EntityState.Modified)
                        {
                            if (entry.Entity is Usuario)
                            {
                                if (entry.State == EntityState.Added)
                                {
                                    mensaje = "No se ha podido añadir el usuario.";
                                }
                                else if (entry.State == EntityState.Modified)
                                {
                                    mensaje = "No se ha podido modificar el usuario.";
                                }
                                mensaje += "\n\nAsegurese que el nombre de usuario y el email son únicos.";
                            }
                            else if (entry.Entity is Cliente)
                            {
                                if (entry.State == EntityState.Added)
                                {
                                    mensaje = "No se ha podido añadir el cliente.";
                                }
                                else if (entry.State == EntityState.Modified)
                                {
                                    mensaje = "No se ha podido modificar el cliente.";
                                }
                                mensaje += "\n\nAsegurese que la razón social, el NIF y el email son únicos.";
                            }
                            else if (entry.Entity is Proveedor)
                            {
                                if (entry.State == EntityState.Added)
                                {
                                    mensaje = "No se ha podido añadir el proveedor.";
                                }
                                else if (entry.State == EntityState.Modified)
                                {
                                    mensaje = "No se ha podido modificar el proveedor.";
                                }
                                mensaje += "\n\nAsegurese que la razón social, el NIF y el email son únicos.";
                            }
                            break;
                        }
                    }
                }
            }
            else if (excepcion is SqlException ex2)
            {
                if (ex2.Number == -2) // System.Data.SqlClient.TdsEnums.TIMEOUT_EXPIRED (No es accesible)
                {
                    mensaje = "No se ha podido conectar con la Base de Datos. Tiempo de espera excedido.";
                }
            }
            else if (excepcion is EntityCommandExecutionException ex3)
            {
                // if (ex3.InnerException is SqlException ex3Inner)
                // {
                //     if (ex3Inner.Number == -2146232060) // No se puede utilizar la conexión física
                //     {
                mensaje = "Se ha perdido la conexión con la Base de Datos.";
                //     }
                // }
            }
            else if (excepcion is EntityException ex4)
            {
                /* if (ex4.Number == -2) // System.Data.SqlClient.TdsEnums.TIMEOUT_EXPIRED (No es accesible)
                 {
                     mensaje = "No se ha podido conectar con la Base de Datos. Tiempo de espera excedido.";
                 }*/
            }


            // Si ocurre otra excepción, se muestr el mensaje de la excepción más interna.
            if (string.IsNullOrEmpty(mensaje))
            {
                var excep = e.Exception;
                while (excep.InnerException != null) excep = excep.InnerException;

                mensaje = excep.Message;
            }

            var mensajeInformacion = new MensajeInformacion()
            {
                Width = 350,
                Mensaje = mensaje
            };
            DialogHost.Show(mensajeInformacion, "RootDialog");
            e.Handled = true;

            // Process.GetCurrentProcess().Kill();
        }
    }

    public static class ExceptionHelper
    {
        private static Exception GetInnermostException(Exception ex)
        {
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex;
        }

        public static bool IsUniqueConstraintViolation(Exception ex)
        {
            var innermost = GetInnermostException(ex);
            var sqlException = innermost as SqlException;

            return sqlException != null && sqlException.Class == 14 && (sqlException.Number == 2601 || sqlException.Number == 2627);
        }
    }
}
