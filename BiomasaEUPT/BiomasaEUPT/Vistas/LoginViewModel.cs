using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Vistas
{
    public class LoginViewModel : INotifyPropertyChanged, IDataErrorInfo
    {

        private string _usuario;
        public string Usuario
        {
            get { return _usuario; }
            set
            {
                _usuario = value;
                OnPropertyChanged();
            }
        }

        private SecureString _contrasena;
        public SecureString Contrasena
        {
            get { return _contrasena; }
            set
            {
                _contrasena = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public LoginViewModel()
        {

        }

        public Usuario IniciarSesion(String usuario, String hashContrasena)
        {
            using (var context = new BiomasaEUPTContext())
            {
                return context.Usuarios.FirstOrDefault(u => u.Nombre == usuario && u.Contrasena == hashContrasena
                                                            && u.Baneado == false);
            }
        }


        #region Validación Contraseña
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
