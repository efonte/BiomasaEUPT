using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Vistas.GestionUsuarios
{
    public class FormUsuarioViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public string FormTitulo { get; set; }

        public ObservableCollection<TipoUsuario> TiposUsuarios { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public bool Baneado { get; set; }
        public SecureString Contrasena { get; set; }
        public SecureString ContrasenaConfirmacion { get; set; }

        public TipoUsuario TipoUsuarioSeleccionado { get; set; }

        public BiomasaEUPTContext Context { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        public FormUsuarioViewModel()
        {
            FormTitulo = "Nuevo Usuario";
            Context = new BiomasaEUPTContext();
            CargarTipos();
        }

        private void CargarTipos()
        {
            // TiposUsuarios = new ObservableCollection<TipoUsuario>(Context.TiposUsuarios.ToList());
            var usuarioLogeado = Context.Usuarios.Single(u => u.Nombre == Properties.Settings.Default.usuario);
            var tiposUsuarios = Context.TiposUsuarios.ToList();

            // Si el usuario no es Super Administrador no puede seleccionar dicho tipo
            if (usuarioLogeado.TipoId != 1)
            {
                tiposUsuarios = tiposUsuarios.Where(tu => tu.TipoUsuarioId != 1).ToList();
            }

            // Si el usuario no tiene la gestión de permisos no puede seleccionar los tipos de usuarios con dicho permiso
            if (!usuarioLogeado.TipoUsuario.Permisos.Select(p => p.Tab).Contains(Tab.Permisos))
            {
                var tiposUsuariosConPermisos = Context.TiposUsuarios.Where(tu => tu.Permisos.Any(p => p.Tab == Tab.Permisos)).ToList();
                tiposUsuarios = tiposUsuarios.Where(tui => !tiposUsuariosConPermisos.Any(tue => tui.TipoUsuarioId == tue.TipoUsuarioId)).ToList();

            }

            TiposUsuarios = new ObservableCollection<TipoUsuario>(tiposUsuarios);
            TipoUsuarioSeleccionado = TipoUsuarioSeleccionado ?? TiposUsuarios.First();
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
                else if (Contrasena != null && ContrasenaConfirmacion != null && !ContrasenaHashing.SecureStringEqual(Contrasena, ContrasenaConfirmacion))
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
                    // Fuerza a comprobar la validación de la propiedad Contraseña para saber si son iguales
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
