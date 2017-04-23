using BiomasaEUPT.Clases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BiomasaEUPT.Vistas
{
    public class GestionUsuariosViewModel : INotifyPropertyChanged
    {

        public GestionUsuariosViewModel()
        {
            BorrarUsuarioComando = new Comando(BorrarUsuario);
            AceptarComando = new Comando(AceptarBorrarUsuario);
            CancelarComando = new Comando(CancelarBorrarUsuario);
        }

       /* private ObservableCollection<usuarios> _usuariosColeccion;
        public ObservableCollection<usuarios> UsuariosColeccion
        {
            get { return _usuariosColeccion; }
            set
            {
                _usuariosColeccion = value;
                OnPropertyChanged();
            }
        }*/

        public ICommand BorrarUsuarioComando { get; }
        public ICommand AceptarComando { get; }
        public ICommand CancelarComando { get; }


        private bool _estaMensajeBorrarUsuario;
        public bool EstaMensajeBorrarUsuario
        {
            get { return _estaMensajeBorrarUsuario; }
            set
            {
                if (_estaMensajeBorrarUsuario == value) return;
                _estaMensajeBorrarUsuario = value;
                OnPropertyChanged();
            }
        }

        private object _mensajeConfirmacion;
        public object MensajeConfirmacion
        {
            get { return _mensajeConfirmacion; }
            set
            {
                if (_mensajeConfirmacion == value) return;
                _mensajeConfirmacion = value;
                OnPropertyChanged();
            }
        }


        private void BorrarUsuario(object obj)
        {
            MensajeConfirmacion = new MensajeConfirmacion("¿Está seguro de querer borrar al usuario aaaa?");
            EstaMensajeBorrarUsuario = true;
            Console.WriteLine("sfafwefe");
        }




        private void CancelarBorrarUsuario(object obj)
        {
            EstaMensajeBorrarUsuario = false;
        }

        private void AceptarBorrarUsuario(object obj)
        {
            //pretend to do something for 3 seconds, then close
            //Sample4Content = new SampleProgressDialog();
            //Task.Delay(TimeSpan.FromSeconds(3))
            //    .ContinueWith((t, _) => EstaMensajeBorrarUsuario = false, null,
            //        TaskScheduler.FromCurrentSynchronizationContext());
            //UsuariosColeccion.Remove();
            EstaMensajeBorrarUsuario = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
