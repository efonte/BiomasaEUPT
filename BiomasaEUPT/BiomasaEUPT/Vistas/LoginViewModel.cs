using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

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

        public bool RecordarContrasena { get; set; }

        private ICommand _iniciarSesionComando;

        public event PropertyChangedEventHandler PropertyChanged;


        public LoginViewModel()
        {

        }

        public Usuario IniciarSesion(String usuario, String hashContrasena)
        {
            using (var context = new BiomasaEUPTContext())
            {
                return context.Usuarios
                    .Include("TipoUsuario.Permisos")
                    .FirstOrDefault(u => u.Nombre == usuario && u.Contrasena == hashContrasena
                                                            && u.Baneado == false);
            }
        }


        #region Iniciar Sesión
        public ICommand IniciarSesionComando => _iniciarSesionComando ??
            (_iniciarSesionComando = new RelayCommand(
                param => IniciarSesion(),
                param => Validate("Usuario") == null && Validate("Contrasena") == null
            ));

        private void IniciarSesion()
        {
            Console.WriteLine(Usuario);
            String hashContrasena = "";
            if (Contrasena != null)
            {
                hashContrasena = ContrasenaHashing.ObtenerHashSHA256(ContrasenaHashing.SecureStringToString(Contrasena));
            }

            var usuario = IniciarSesion(Usuario, hashContrasena);
            if (usuario != null)
            {
                Properties.Settings.Default.contrasena = RecordarContrasena == true ? hashContrasena : "";
                Properties.Settings.Default.usuario = Usuario;
                Properties.Settings.Default.Save();
                CargarVistaMain(usuario);
            }
            else
            {
                MensajeLoginIncorrecto();
            }
        }
        #endregion      

        public async void MensajeLoginIncorrecto()
        {
            var mensaje = new MensajeInformacion()
            {
                Titulo = "Login incorrecto",
                Mensaje = "El usuario y/o la contraseña son incorrectos."
            };
            var resultado = await DialogHost.Show(mensaje, "RootDialog");
        }

        private void CargarVistaMain(Usuario usuario)
        {
            MainWindow mainWindows = new MainWindow();
            (mainWindows.DataContext as MainWindowViewModel).Usuario = usuario;
            Application.Current.Windows.OfType<Window>().SingleOrDefault(x => x.IsActive).Close();
            mainWindows.Show();
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
