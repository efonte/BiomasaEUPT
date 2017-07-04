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
            string mensaje = e.Exception.Message;
            //var excepcion = e.Exception.InnerException;
            var excepcion = e.Exception;

            if (excepcion is DbEntityValidationException)
            {
                var mensajesError = (excepcion as DbEntityValidationException).EntityValidationErrors
                      .SelectMany(x => x.ValidationErrors)
                      .Select(x => x.ErrorMessage);

                mensaje = "No pueden guardar los cambios:\n\n" + string.Join("\n\n", mensajesError);
            }
            else if (excepcion is DbUpdateException ex)
            {
                if (ExceptionHelper.IsUniqueConstraintViolation(ex))
                {
                    mensaje = "No se ha podido modificar el campo.";
                    foreach (var entry in ex.Entries)
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

            var mensajeInformacion = new MensajeInformacion(mensaje);
            mensajeInformacion.Width = 350;

            DialogHost.Show(mensajeInformacion, "RootDialog");
            e.Handled = true;
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
