using BiomasaEUPT.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Vistas.Ajustes
{
    public class WinAjustesViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public SecureString Contrasena { get; set; }
        public SecureString ContrasenaConfirmacion { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public WinAjustesViewModel()
        {

        }


        #region Validación Contraseñas
        string IDataErrorInfo.Error { get { return Validate(null); } }

        string IDataErrorInfo.this[string columnName] { get { return Validate(columnName); } }

        private string Validate(string memberName)
        {
            string error = null;

            if (memberName == "Contrasena" || memberName == null)
            {
                if (Contrasena == null || Contrasena.Length == 0)
                {
                    error = "El campo contraseña es obligatorio.";
                }
                else if (Contrasena != null &&ContrasenaConfirmacion != null && !ContrasenaHashing.SecureStringEqual(Contrasena, ContrasenaConfirmacion))
                {
                    error = "El campo contraseña y contraseña confirmación no son iguales.";
                }
            }

            if (memberName == "ContrasenaConfirmacion" || memberName == null)
            {
                if (ContrasenaConfirmacion == null || ContrasenaConfirmacion.Length == 0)
                {
                    error = "El campo contraseña confirmación es obligatorio.";
                }
                else
                {
                    // Fuerza a comprobar la validación de la porpiedad Contraseña para saber si son iguales
                    OnPropertyChanged("Contrasena");
                }
            }

            return error;
        }
        #endregion


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
