using BiomasaEUPT.Clases;
using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.ControlesUsuario;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace BiomasaEUPT.Vistas.GestionPermisos
{
    public class TabPermisosViewModel : ViewModelBase
    {
        public ObservableCollection<TipoUsuario> TiposUsuarios { get; set; }
        public CollectionView TiposUsuariosView { get; private set; }
        public IList<TipoUsuario> TiposUsuariosSeleccionados { get; set; }
        public TipoUsuario TipoUsuarioSeleccionado { get; set; }
        public ContadorViewModel<TipoUsuario> ContadorViewModel { get; set; }
        public OpcionesViewModel OpcionesViewModel { get; set; }

        // Checkbox Filtro Tipos de Usuarios
        public bool NombreSeleccionado { get; set; } = true;
        public bool PermisosSeleccionado { get; set; } = true;

        private string _textoFiltroTiposUsuarios = "";
        public string TextoFiltroTiposUsuarios
        {
            get { return _textoFiltroTiposUsuarios; }
            set
            {
                _textoFiltroTiposUsuarios = value.ToLower();
                FiltrarTiposUsuarios();
            }
        }

        private ICommand _anadirTipoUsuarioComando;
        private ICommand _modificarTipoUsuarioComando;
        private ICommand _borrarTipoUsuarioComando;
        private ICommand _refrescarTiposUsuariosComando;
        private ICommand _filtrarTiposUsuariosComando;
        private ICommand _dgTiposUsuarios_CellEditEndingComando;

        private BiomasaEUPTContext context;

        public TabPermisosViewModel()
        {
            ContadorViewModel = new ContadorViewModel<TipoUsuario>();
            OpcionesViewModel = new OpcionesViewModel()
            {
                AnadirComando = AnadirTipoUsuarioComando,
                BorrarComando = BorrarTipoUsuarioComando,
                ModificarComando = ModificarTipoUsuarioComando,
                RefrescarComando = RefrescarTiposUsuariosComando
            };
        }

        public override void Inicializar()
        {
            context = new BiomasaEUPTContext();
            CargarTiposUsuarios();
        }

        public void CargarTiposUsuarios()
        {
            using (new CursorEspera())
            {
                TiposUsuarios = new ObservableCollection<TipoUsuario>(context.TiposUsuarios.Include(tu => tu.Permisos).ToList());
                TiposUsuariosView = (CollectionView)CollectionViewSource.GetDefaultView(TiposUsuarios);
                //ContadorViewModel.Tipos = TiposUsuarios; // Se filtran también, no puede usarse
                ContadorViewModel.Tipos = new ObservableCollection<TipoUsuario>(context.TiposUsuarios.ToList());

                // Por defecto no está seleccionada ninguna fila del datagrid tipos de usuarios
                TipoUsuarioSeleccionado = null;
            }
        }

        // Asigna el valor de TiposUsuariosSeleccionados ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGTiposUsuarios_SelectionChangedComando => new RelayCommandGenerico<IList<object>>(param => TiposUsuariosSeleccionados = param.Cast<TipoUsuario>().ToList());

        #region Editar Celda
        public ICommand DGTiposUsuarios_CellEditEndingComando => _dgTiposUsuarios_CellEditEndingComando ??
            (_dgTiposUsuarios_CellEditEndingComando = new RelayCommandGenerico<DataGridCellEditEndingEventArgs>(
                 param => EditarCeldaTipoUsuario(param)
            ));

        private async void EditarCeldaTipoUsuario(DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                context.SaveChanges();
            }
        }
        #endregion


        #region Añadir Tipo de Usuario
        public ICommand AnadirTipoUsuarioComando => _anadirTipoUsuarioComando ??
            (_anadirTipoUsuarioComando = new RelayCommand(
                param => AnadirTipoUsuario()
            ));

        private async void AnadirTipoUsuario()
        {
            var formTipoUsuarioViewModel = new FormTipoUsuarioViewModel();
            var formTipoUsuario = new FormTipoUsuario() { DataContext = formTipoUsuarioViewModel };

            if ((bool)await DialogHost.Show(formTipoUsuario, "RootDialog"))
            {
                var tipoUsuario = new TipoUsuario()
                {
                    Nombre = formTipoUsuarioViewModel.Nombre,
                    Descripcion = formTipoUsuarioViewModel.Descripcion
                };
                context.TiposUsuarios.Add(tipoUsuario);

                // A cada permiso se le asigna el tipo de usuario
                var permisos = formTipoUsuarioViewModel.Permisos.ToList();
                permisos.ForEach(p => p.TipoUsuario = tipoUsuario);
                context.Permisos.AddRange(permisos);

                context.SaveChanges();
                CargarTiposUsuarios();
            }
        }
        #endregion


        #region Borrar Tipo de Usuario    
        public ICommand BorrarTipoUsuarioComando => _borrarTipoUsuarioComando ??
            (_borrarTipoUsuarioComando = new RelayCommandGenerico<IList<object>>(
                param => BorrarTipoUsuario(),
                param => TipoUsuarioSeleccionado != null
            ));

        private async void BorrarTipoUsuario()
        {
            if (TipoUsuarioSeleccionado.TipoUsuarioId == 1)
            {
                await DialogHost.Show(new MensajeInformacion()
                {
                    Mensaje = "Este tipo de usuario no puede borrarse."
                }, "RootDialog");
            }
            else
            {
                string pregunta = TiposUsuariosSeleccionados.Count == 1
                    ? "¿Está seguro de que desea borrar el tipo de usuario " + TipoUsuarioSeleccionado.Nombre + "?"
                    : "¿Está seguro de que desea borrar los tipos de usuarios seleccionados?";

                if ((bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog"))
                {
                    var tiposUsuariosABorrar = new List<TipoUsuario>();

                    foreach (var tu in TiposUsuariosSeleccionados)
                    {
                        if (!context.Usuarios.Any(u => u.TipoId == tu.TipoUsuarioId))
                        {
                            tiposUsuariosABorrar.Add(tu);
                        }
                    }
                    context.TiposUsuarios.RemoveRange(tiposUsuariosABorrar);
                    context.SaveChanges();

                    if (TiposUsuariosSeleccionados.Count != tiposUsuariosABorrar.Count)
                    {
                        string mensaje = TiposUsuariosSeleccionados.Count == 1
                               ? "No se ha podido borrar el tipo de usuario seleccionado."
                               : "No se han podido borrar todos los tipos de usuarios seleccionados.";
                        mensaje += "\n\nAsegurese de no que no exista ningún usuario asociado a dicho tipo.";
                        await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
                    }
                    CargarTiposUsuarios();
                }
            }
        }
        #endregion


        #region Modificar Tipo de Usuario
        public ICommand ModificarTipoUsuarioComando => _modificarTipoUsuarioComando ??
            (_modificarTipoUsuarioComando = new RelayCommand(
                param => ModificarTipoUsuario(),
                param => TipoUsuarioSeleccionado != null
             ));

        private async void ModificarTipoUsuario()
        {
            if (TipoUsuarioSeleccionado.TipoUsuarioId == 1)
            {
                await DialogHost.Show(new MensajeInformacion()
                {
                    Mensaje = "Este tipo de usuario no puede modificarse.",
                }, "RootDialog");
            }
            else
            {
                var formTipoUsuarioViewModel = new FormTipoUsuarioViewModel(TipoUsuarioSeleccionado, context);
                var formTipoUsuario = new FormTipoUsuario() { DataContext = formTipoUsuarioViewModel };
                if ((bool)await DialogHost.Show(formTipoUsuario, "RootDialog"))
                {
                    TipoUsuarioSeleccionado.Nombre = formTipoUsuarioViewModel.Nombre;
                    TipoUsuarioSeleccionado.Descripcion = formTipoUsuarioViewModel.Descripcion;

                    // A cada permiso se le asigna el tipo de usuario
                    var permisos = formTipoUsuarioViewModel.Permisos.ToList();
                    permisos.ForEach(p => p.TipoUsuario = TipoUsuarioSeleccionado);

                    var permisosAntiguos = TipoUsuarioSeleccionado.Permisos.ToList();
                    var permisosAnadidos = permisos.Except(permisosAntiguos).ToList();
                    var permisosBorrados = permisosAntiguos.Except(permisos).ToList();

                    permisosBorrados.ForEach(x => context.Entry(x).State = EntityState.Deleted);
                    permisosAnadidos.ForEach(x => context.Entry(x).State = EntityState.Added);

                    context.SaveChanges();
                    CargarTiposUsuarios();
                }
            }
        }
        #endregion


        #region Refrescar Tipos de Usuarios
        public ICommand RefrescarTiposUsuariosComando => _refrescarTiposUsuariosComando ??
            (_refrescarTiposUsuariosComando = new RelayCommand(
                param => RefrescarTiposUsuarios()
             ));

        private void RefrescarTiposUsuarios()
        {
            // Hay que volver a instanciar un nuevo context ya que sino no se pueden refrescar los datos
            // debido a que se guardardan en una cache
            context.Dispose();
            context = new BiomasaEUPTContext();
            CargarTiposUsuarios();
        }
        #endregion


        #region Filtro Tipos de Usuarios
        public ICommand FiltrarTiposUsuariosComando => _filtrarTiposUsuariosComando ??
           (_filtrarTiposUsuariosComando = new RelayCommand(
                param => FiltrarTiposUsuarios()
           ));

        public void FiltrarTiposUsuarios()
        {
            TiposUsuariosView.Filter = FiltroTiposUsuarios;
            TiposUsuariosView.Refresh();
        }

        private bool FiltroTiposUsuarios(object item)
        {
            var tipoUsuario = item as TipoUsuario;
            string nombre = tipoUsuario.Nombre.ToLower();

            return (NombreSeleccionado == true ? nombre.Contains(TextoFiltroTiposUsuarios) : false)
                || (PermisosSeleccionado == true ? tipoUsuario.Permisos.Any(p => p.PermisoId.ToString().ToLower().Contains(TextoFiltroTiposUsuarios)) : false);
        }
        #endregion
    }
}
