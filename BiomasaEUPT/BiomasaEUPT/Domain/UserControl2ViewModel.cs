using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Domain
{
    class UserControl2ViewModel : INotifyPropertyChanged
    {

        private BiomasaEUPTEntities db = null;


        public UserControl2ViewModel()
        {
            db = new BiomasaEUPTEntities();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.AutoDetectChangesEnabled = false;
            UsuariosColeccion = new ObservableCollection<usuarios>(db.usuarios);
            TiposUsuariosColeccion = new ObservableCollection<tipos_usuarios>(db.tipos_usuarios);
            // OrdersCollection = new ObservableCollection<Orders>();
            //  ProductCollection = new ObservableCollection<Order_Details>();
            AColeccion = new ObservableCollection<string>();
            AColeccion.Add("Location 1");
            AColeccion.Add("Location 2");
            AColeccion.Add("Location 3");
            AColeccion.Add("Location 4");
        }

        private ObservableCollection<string> _aColeccion;
        public ObservableCollection<string> AColeccion
        {
            get
            {
                return _aColeccion;
            }
            set
            {
                _aColeccion = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<usuarios> _usuariosColeccion;
        public ObservableCollection<usuarios> UsuariosColeccion
        {
            get
            {
                return _usuariosColeccion;
            }
            set
            {
                _usuariosColeccion = value;
                NotifyPropertyChanged();
            }
        }

        private ObservableCollection<tipos_usuarios> _tiposUsuariosColeccion;
        public ObservableCollection<tipos_usuarios> TiposUsuariosColeccion
        {
            get
            {
                return _tiposUsuariosColeccion;
            }
            set
            {
                _tiposUsuariosColeccion = value;
                NotifyPropertyChanged();
            }
        }


        private bool? _estanTodosUsuariosSeleccionados;
        public bool? EstanTodosUsuariosSeleccionados
        {
            get { return _estanTodosUsuariosSeleccionados; }
            set
            {
                if (_estanTodosUsuariosSeleccionados == value) return;

                _estanTodosUsuariosSeleccionados = value;

                if (_estanTodosUsuariosSeleccionados.HasValue)
                {
                    foreach (var model in UsuariosColeccion)
                    {
                        //  model.EstaSeleccionado = _estanTodosUsuariosSeleccionados.Value;
                    }
                }
                //OnPropertyChanged();
            }
        }

        usuarios _usuarioSeleccionado = null;
        public usuarios UsuarioSeleccionado
        {
            get { return _usuarioSeleccionado; }
            set
            {
                _usuarioSeleccionado = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
