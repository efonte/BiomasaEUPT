using BiomasaEUPT.Vistas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core;
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
            string mensaje = "";
            var excepcion = e.Exception.InnerException;

            if (excepcion is DbEntityValidationException)
            {
                var mensajesError = (excepcion as DbEntityValidationException).EntityValidationErrors
                      .SelectMany(x => x.ValidationErrors)
                      .Select(x => x.ErrorMessage);

                mensaje = "No pueden guardar los cambios:\n\n" + string.Join("\n", mensajesError);
            }

            var mensajeInformacion = new MensajeInformacion(mensaje);
            mensajeInformacion.Width = 350;

            DialogHost.Show(mensajeInformacion, "RootDialog");
            e.Handled = true;
        }
    }
}
