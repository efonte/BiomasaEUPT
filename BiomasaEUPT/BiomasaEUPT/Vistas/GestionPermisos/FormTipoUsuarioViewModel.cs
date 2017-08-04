using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace BiomasaEUPT.Vistas.GestionPermisos
{
    public class FormTipoUsuarioViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Tab> Tabs { get; set; }
        public CollectionView TabsView { get; private set; }
        public ObservableCollection<Permiso> Permisos { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Titulo { get; set; } = "Nuevo Tipo de Usuario";

        private ICommand _quitarPermisoComando;
        private ICommand _wpPermisos_DropComando;


        public event PropertyChangedEventHandler PropertyChanged;

        public FormTipoUsuarioViewModel()
        {
            Tabs = new ObservableCollection<Tab>();
            Permisos = new ObservableCollection<Permiso>();
            CargarTabs();
        }

        public FormTipoUsuarioViewModel(TipoUsuario tipoUsuario, BiomasaEUPTContext context) : this()
        {
            Titulo = "Editar Tipo de Usuario";
            Nombre = tipoUsuario.Nombre;
            Descripcion = tipoUsuario.Descripcion;
            Permisos = new ObservableCollection<Permiso>(tipoUsuario.Permisos.ToList());
            TabsView.Filter = FiltroTabs;
        }

        private void CargarTabs()
        {
            Tabs = new ObservableCollection<Tab>()
            {
                Tab.Permisos,
                Tab.Usuarios,
                Tab.Clientes,
                Tab.Proveedores,
                Tab.Recepciones,
                Tab.Elaboraciones,
                Tab.Ventas,
                Tab.Trazabilidad
            };
            TabsView = (CollectionView)CollectionViewSource.GetDefaultView(Tabs);
        }

        #region Drop Permiso
        public ICommand WPPermisos_DropComando => _wpPermisos_DropComando ??
            (_wpPermisos_DropComando = new RelayCommandGenerico<DragEventArgs>(
                 param => SoltarPermiso(param)
            ));

        private void SoltarPermiso(DragEventArgs e)
        {
            var tab = (Tab)e.Data.GetData("Tab");
            var permiso = new Permiso()
            {
                Tab = tab
            };
            Permisos.Add(permiso);
            TabsView.Filter = FiltroTabs;
        }
        #endregion


        #region Quitar Permiso
        public ICommand QuitarPermisoComando => _quitarPermisoComando ??
            (_quitarPermisoComando = new RelayCommandGenerico<Tab>(
                param => QuitarPermiso(param)
            ));

        private void QuitarPermiso(Tab tab)
        {
            var permiso = Permisos.Single(p => p.Tab == tab);
            Permisos.Remove(permiso);
            TabsView.Filter = FiltroTabs;
        }
        #endregion

        private bool FiltroTabs(object item)
        {
            var tab = (Tab)item;
            return !Permisos.Select(p => p.Tab).Contains(tab);
        }
    }
}
