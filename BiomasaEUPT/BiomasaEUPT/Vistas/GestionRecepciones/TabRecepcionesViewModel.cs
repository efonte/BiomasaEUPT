using BiomasaEUPT.Clases;
using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.ControlesUsuario;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace BiomasaEUPT.Vistas.GestionRecepciones
{
    public class TabRecepcionesViewModel : ViewModelBase
    {
        public ObservableCollection<Recepcion> Recepciones { get; set; }
        public CollectionView RecepcionesView { get; private set; }
        public IList<Recepcion> RecepcionesSeleccionadas { get; set; }
        public Recepcion RecepcionSeleccionada { get; set; }

        public ObservableCollection<MateriaPrima> MateriasPrimas { get; set; }
        public CollectionView MateriasPrimasView { get; private set; }
        public IList<MateriaPrima> MateriasPrimasSeleccionadas { get; set; }
        public MateriaPrima MateriaPrimaSeleccionada { get; set; }
        public bool ObservacionesMateriasPrimasEnEdicion { get; set; }

        // Checkbox Filtro Recepciones
        public bool FechaRecepcionSeleccionada { get; set; } = true;
        public bool NumeroAlbaranRecepcionSeleccionado { get; set; } = true;
        public bool ProveedorRecepcionSeleccionado { get; set; } = false;
        public bool EstadoRecepcionSeleccionado { get; set; } = false;

        private string _textoFiltroRecepciones;
        public string TextoFiltroRecepciones
        {
            get { return _textoFiltroRecepciones; }
            set
            {
                _textoFiltroRecepciones = value.ToLower();
                FiltrarRecepciones();
            }
        }

        // Checkbox Filtro Materias Primas
        public bool FechaBajaMateriaPrimaSeleccionada { get; set; } = false;
        public bool TipoMateriaPrimaSeleccionado { get; set; } = true;
        public bool GrupoMateriaPrimaSeleccionado { get; set; } = true;
        public bool VolUniMateriaPrimaSeleccionado { get; set; } = false;
        public bool ProcedenciaMateriaPrimaSeleccionada { get; set; } = false;

        private string _textoFiltroMateriasPrimas;
        public string TextoFiltroMateriasPrimas
        {
            get { return _textoFiltroMateriasPrimas; }
            set
            {
                _textoFiltroMateriasPrimas = value.ToLower();
                FiltrarMateriasPrimas();
            }
        }

        private ICommand _anadirRecepcionComando;
        private ICommand _modificarRecepcionComando;
        private ICommand _borrarRecepcionComando;
        private ICommand _refrescarRecepcionesComando;
        private ICommand _filtrarRecepcionesComando;
        private ICommand _dgRecepciones_SelectionChangedComando;

        private ICommand _anadirMateriaPrimaComando;
        private ICommand _modificarMateriaPrimaComando;
        private ICommand _borrarMateriaPrimaComando;
        private ICommand _refrescarMateriasPrimasComando;
        private ICommand _filtrarMateriasPrimasComando;
        private ICommand _modificarObservacionesMateriaPrimaComando;

        private BiomasaEUPTContext context;
        public PaginacionViewModel PaginacionViewModel { get; set; }


        public TabRecepcionesViewModel()
        {
            PaginacionViewModel = new PaginacionViewModel();
        }

        public override void Inicializar()
        {
            context = new BiomasaEUPTContext();
            CargarRecepciones();
            PaginacionViewModel.GetItemsTotales = () => { return context.Recepciones.Count(); };
            PaginacionViewModel.ActualizarContadores();
            PaginacionViewModel.CargarItems = CargarRecepciones;
        }

        public void CargarRecepciones(int cantidad = 10, int saltar = 0)
        {
            using (new CursorEspera())
            {
                Recepciones = new ObservableCollection<Recepcion>(
                    context.Recepciones
                    .Include(r => r.EstadoRecepcion).Include(r => r.Proveedor)
                    .OrderBy(r => r.NumeroAlbaran).Skip(saltar).Take(cantidad).ToList());
                RecepcionesView = (CollectionView)CollectionViewSource.GetDefaultView(Recepciones);
                // ClientesView.Filter = OnFilterMovie;

                // Por defecto no está seleccionada ninguna fila del datagrid recepciones 
                RecepcionSeleccionada = null;

            }
        }

        public void CargarMateriasPrimas()
        {
            if (RecepcionSeleccionada != null)
            {
                // ucTablaMateriasPrimas.bAnadirMateriaPrima.IsEnabled = recepcion.EstadoId == 1; // Disponible
                using (new CursorEspera())
                {
                    MateriasPrimas = new ObservableCollection<MateriaPrima>(
                          context.MateriasPrimas.
                          Where(mp => mp.RecepcionId == RecepcionSeleccionada.RecepcionId)
                          .Include(m => m.HistorialHuecosRecepciones)
                          .Include(m => m.Procedencia)
                          .Include(m => m.TipoMateriaPrima.GrupoMateriaPrima)
                          .ToList());
                    MateriasPrimasView = (CollectionView)CollectionViewSource.GetDefaultView(MateriasPrimas);

                    MateriaPrimaSeleccionada = null;
                }
            }
            else
            {
                // ucTablaMateriasPrimas.bAnadirMateriaPrima.IsEnabled = false;
                MateriasPrimas = null;
            }
        }

        // Asigna el valor de RecepcionesSeleccinodas ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGRecepciones_SelectionChangedComando => _dgRecepciones_SelectionChangedComando ??
            (_dgRecepciones_SelectionChangedComando = new RelayCommand2<IList<object>>(
                param => DGRecepciones_SelectionChanged(param)
            ));

        private void DGRecepciones_SelectionChanged(IList<object> recepcionesSeleccionadas)
        {
            RecepcionesSeleccionadas = recepcionesSeleccionadas.Cast<Recepcion>().ToList();
            CargarMateriasPrimas();
        }

        // Asigna el valor de MateriasPrimasSeleccinodas ya que no se puede crear un Binding de SelectedItems desde el XAML
        public ICommand DGMateriasPrimas_SelectionChangedComando => new RelayCommand2<IList<object>>(param => MateriasPrimasSeleccionadas = param.Cast<MateriaPrima>().ToList());


        #region Añadir Recepción
        public ICommand AnadirRecepcionComando => _anadirRecepcionComando ??
            (_anadirRecepcionComando = new RelayComando(
                param => AnadirRecepcion()
            ));

        private async void AnadirRecepcion()
        {
            var formRecepcion = new FormRecepcion(context);

            if ((bool)await DialogHost.Show(formRecepcion, "RootDialog"))
            {
                context.Recepciones.Add(new Recepcion()
                {
                    NumeroAlbaran = formRecepcion.NumeroAlbaran,
                    FechaRecepcion = new DateTime(formRecepcion.Fecha.Year, formRecepcion.Fecha.Month, formRecepcion.Fecha.Day, formRecepcion.Hora.Hour, formRecepcion.Hora.Minute, formRecepcion.Hora.Second),
                    ProveedorId = (formRecepcion.cbProveedores.SelectedItem as Proveedor).ProveedorId,
                    EstadoId = 1
                });
                context.SaveChanges();
            }
        }
        #endregion


        #region Borrar Recepción
        public ICommand BorrarRecepcionComando => _borrarRecepcionComando ??
            (_borrarRecepcionComando = new RelayCommand2<IList<object>>(
                param => BorrarRecepcion(),
                param => RecepcionSeleccionada != null
            ));

        private async void BorrarRecepcion()
        {
            string pregunta = RecepcionesSeleccionadas.Count == 1
                   ? "¿Está seguro de que desea borrar la recepción " + RecepcionSeleccionada.NumeroAlbaran + "?"
                   : "¿Está seguro de que desea borrar las recepciones seleccionadas?";

            if ((bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog"))
            {
                var recepcionesABorrar = new List<Recepcion>();
                //var recepcionesSeleccionadas = ucTablaRecepciones.dgRecepciones.SelectedItems.Cast<Recepcion>().ToList();

                foreach (var recepcion in RecepcionesSeleccionadas)
                {
                    if (!context.MateriasPrimas.Any(mp => mp.RecepcionId == recepcion.RecepcionId))
                    {
                        recepcionesABorrar.Add(recepcion);
                    }
                }
                context.Recepciones.RemoveRange(recepcionesABorrar);
                context.SaveChanges();

                if (RecepcionesSeleccionadas.Count != recepcionesABorrar.Count)
                {
                    string mensaje = RecepcionesSeleccionadas.Count == 1
                           ? "No se ha podido borrar la recepción seleccionada."
                           : "No se han podido borrar todas las recepciones seleccionadas.";
                    mensaje += "\n\nAsegurese de no que no exista ninguna materia prima asociada a dicha recepción.";
                    await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
                }
                PaginacionViewModel.Refrescar();
            }
        }
        #endregion


        #region Modificar Recepción
        public ICommand ModificarRecepcionComando => _modificarRecepcionComando ??
            (_modificarRecepcionComando = new RelayComando(
                param => ModificarRecepcion(),
                param => RecepcionSeleccionada != null
             ));

        public async void ModificarRecepcion()
        {

            var formRecepcion = new FormRecepcion(context, "Editar Recepción")
            {
                NumeroAlbaran = RecepcionSeleccionada.NumeroAlbaran,
                Fecha = RecepcionSeleccionada.FechaRecepcion,
                Hora = RecepcionSeleccionada.FechaRecepcion
            };
            formRecepcion.cbEstadosRecepciones.Visibility = Visibility.Visible;
            var albaranViejo = formRecepcion.NumeroAlbaran;
            formRecepcion.vAlbaranUnico.NombreActual = RecepcionSeleccionada.NumeroAlbaran;
            formRecepcion.cbEstadosRecepciones.SelectedValue = RecepcionSeleccionada.EstadoRecepcion.EstadoRecepcionId;
            formRecepcion.cbProveedores.SelectedValue = RecepcionSeleccionada.Proveedor.ProveedorId;
            if ((bool)await DialogHost.Show(formRecepcion, "RootDialog"))
            {
                RecepcionSeleccionada.NumeroAlbaran = formRecepcion.NumeroAlbaran;
                RecepcionSeleccionada.FechaRecepcion = new DateTime(formRecepcion.Fecha.Year, formRecepcion.Fecha.Month, formRecepcion.Fecha.Day, formRecepcion.Hora.Hour, formRecepcion.Hora.Minute, formRecepcion.Hora.Second);
                RecepcionSeleccionada.ProveedorId = (formRecepcion.cbProveedores.SelectedItem as Proveedor).ProveedorId;
                RecepcionSeleccionada.EstadoId = (formRecepcion.cbEstadosRecepciones.SelectedItem as EstadoRecepcion).EstadoRecepcionId;
                context.SaveChanges();
                RecepcionesView.Refresh();
            }
        }
        #endregion


        #region Refrescar Recepciones
        public ICommand RefrescarRecepcionesComando => _refrescarRecepcionesComando ??
            (_refrescarRecepcionesComando = new RelayComando(
                param => RefrescarRecepciones()
             ));

        public void RefrescarRecepciones()
        {
            PaginacionViewModel.Refrescar();
            RecepcionSeleccionada = null;
        }
        #endregion


        #region Filtro Recepciones
        public ICommand FiltrarRecepcionesComando => _filtrarRecepcionesComando ??
           (_filtrarRecepcionesComando = new RelayComando(
                param => FiltrarRecepciones()
           ));

        public void FiltrarRecepciones()
        {
            RecepcionesView.Filter = FiltroRecepciones;
            RecepcionesView.Refresh();
        }

        private bool FiltroRecepciones(object item)
        {
            var recepcion = item as Recepcion;
            string fechaRecepcion = recepcion.FechaRecepcion.ToString();
            string numeroAlbaran = recepcion.NumeroAlbaran.ToLower();
            string proveedor = recepcion.Proveedor.RazonSocial.ToLower();
            string estado = recepcion.EstadoRecepcion.Nombre.ToLower();

            return (FechaRecepcionSeleccionada == true ? fechaRecepcion.Contains(TextoFiltroRecepciones) : false)
                || (NumeroAlbaranRecepcionSeleccionado == true ? numeroAlbaran.Contains(TextoFiltroRecepciones) : false)
                || (ProveedorRecepcionSeleccionado == true ? proveedor.Contains(TextoFiltroRecepciones) : false)
                || (EstadoRecepcionSeleccionado == true ? estado.Contains(TextoFiltroRecepciones) : false);
        }
        #endregion


        #region Añadir Materia Prima
        public ICommand AnadirMateriaPrimaComando => _anadirMateriaPrimaComando ??
            (_anadirMateriaPrimaComando = new RelayComando(
                param => AnadirMateriaPrima(),
                param => CanAnadirMateriaPrima()
            ));

        private bool CanAnadirMateriaPrima()
        {
            if (RecepcionSeleccionada != null)
            {
                return RecepcionSeleccionada.EstadoId == 1; // Disponible
            }
            return false;
        }

        private async void AnadirMateriaPrima()
        {
            var formMateriaPrima = new FormMateriaPrima(context);

            if ((bool)await DialogHost.Show(formMateriaPrima, "RootDialog"))
            {
                var formMateriaPrimaDataContext = formMateriaPrima.DataContext as FormMateriaPrimaViewModel;
                var materiaPrima = new MateriaPrima()
                {
                    RecepcionId = RecepcionSeleccionada.RecepcionId,
                    Observaciones = formMateriaPrimaDataContext.Observaciones,
                    //Codigo = formMateriaPrima.Codigo,
                    ProcedenciaId = (formMateriaPrima.cbProcedencias.SelectedItem as Procedencia).ProcedenciaId,
                    TipoId = (formMateriaPrima.cbTiposMateriasPrimas.SelectedItem as TipoMateriaPrima).TipoMateriaPrimaId,
                    Volumen = formMateriaPrimaDataContext.Volumen,
                    Unidades = formMateriaPrimaDataContext.Unidades
                };

                if (formMateriaPrimaDataContext.FechaBaja != null)
                {
                    materiaPrima.FechaBaja = new DateTime(
                        formMateriaPrimaDataContext.FechaBaja.Value.Year,
                        formMateriaPrimaDataContext.FechaBaja.Value.Month,
                        formMateriaPrimaDataContext.FechaBaja.Value.Day,
                        formMateriaPrimaDataContext.HoraBaja.Value.Hour,
                        formMateriaPrimaDataContext.HoraBaja.Value.Minute,
                        formMateriaPrimaDataContext.HoraBaja.Value.Second);
                }
                context.MateriasPrimas.Add(materiaPrima);
                var huecosMateriasPrimas = new List<HistorialHuecoRecepcion>();
                foreach (var hmp in formMateriaPrimaDataContext.HistorialHuecosRecepciones)
                {
                    var hrId = hmp.HuecoRecepcion.HuecoRecepcionId;
                    // Los huecos que no se ha añadido ninguna cantidad no se añaden
                    if (hmp.Unidades != 0 && hmp.Volumen != 0)
                    {
                        hmp.HuecoRecepcion = null;
                        hmp.HuecoRecepcionId = hrId;
                        hmp.MateriaPrima = materiaPrima;
                        huecosMateriasPrimas.Add(hmp);
                    }
                }
                context.HistorialHuecosRecepciones.AddRange(huecosMateriasPrimas);
                context.SaveChanges();

                CargarMateriasPrimas();
            }
        }
        #endregion


        #region Borrar Materia Prima    
        public ICommand BorrarMateriaPrimaComando => _borrarMateriaPrimaComando ??
            (_borrarMateriaPrimaComando = new RelayCommand2<IList<object>>(
                param => BorrarMateriaPrima(),
                param => MateriaPrimaSeleccionada != null
            ));

        private async void BorrarMateriaPrima()
        {
            string pregunta = MateriasPrimasSeleccionadas.Count == 1
                ? "¿Está seguro de que desea borrar la materia prima con código " + MateriaPrimaSeleccionada.Codigo + "?"
                : "¿Está seguro de que desea borrar las materias primas seleccionadas?";

            if ((bool)await DialogHost.Show(new MensajeConfirmacion(pregunta), "RootDialog"))
            {
                List<MateriaPrima> materiasPrimasABorrar = new List<MateriaPrima>();
                // var materiasPrimasSeleccionadas = ucTablaMateriasPrimas.dgMateriasPrimas.SelectedItems.Cast<MateriaPrima>().ToList();

                foreach (var mp in MateriasPrimasSeleccionadas)
                {
                    if (!context.ProductosTerminadosComposiciones.Any(ptc => ptc.HistorialHuecoRecepcion.MateriaPrimaId == mp.MateriaPrimaId))
                    {
                        materiasPrimasABorrar.Add(mp);
                    }
                }
                context.MateriasPrimas.RemoveRange(materiasPrimasABorrar);
                context.SaveChanges();
                CargarMateriasPrimas();

                // materiasPrimasViewSource.View.Refresh();
                if (MateriasPrimasSeleccionadas.Count != materiasPrimasABorrar.Count)
                {
                    string mensaje = MateriasPrimasSeleccionadas.Count == 1
                           ? "No se ha podido borrar la materia prima seleccionada."
                           : "No se han podido borrar todas las materias primas seleccionadas.";
                    mensaje += "\n\nAsegurese de no que no exista ningun producto terminado elaborado con dicha materia prima.";
                    await DialogHost.Show(new MensajeInformacion(mensaje) { Width = 380 }, "RootDialog");
                }
            }
        }
        #endregion


        #region Modificar Materia Prima
        public ICommand ModificarMateriaPrimaComando => _modificarMateriaPrimaComando ??
            (_modificarMateriaPrimaComando = new RelayComando(
                param => ModificarMateriaPrima(),
                param => MateriaPrimaSeleccionada != null
             ));

        public async void ModificarMateriaPrima()
        {
            // var materiaPrimaSeleccionada = ucTablaMateriasPrimas.dgMateriasPrimas.SelectedItem as MateriaPrima;
            var formMateriaPrima = new FormMateriaPrima(context, MateriaPrimaSeleccionada);
            var formMateriaPrimaDataContext = formMateriaPrima.DataContext as FormMateriaPrimaViewModel;
            var historialHuecosRecepionesIniciales = formMateriaPrimaDataContext.HistorialHuecosRecepciones.ToList();
            if ((bool)await DialogHost.Show(formMateriaPrima, "RootDialog"))
            {
                MateriaPrimaSeleccionada.TipoId = formMateriaPrimaDataContext.TipoMateriaPrima.TipoMateriaPrimaId;
                MateriaPrimaSeleccionada.ProcedenciaId = (formMateriaPrima.cbProcedencias.SelectedItem as Procedencia).ProcedenciaId;
                MateriaPrimaSeleccionada.Unidades = formMateriaPrimaDataContext.Unidades;
                MateriaPrimaSeleccionada.Volumen = formMateriaPrimaDataContext.Volumen;
                MateriaPrimaSeleccionada.Observaciones = formMateriaPrimaDataContext.Observaciones;
                if (formMateriaPrimaDataContext.FechaBaja != null)
                {
                    MateriaPrimaSeleccionada.FechaBaja = new DateTime(
                        formMateriaPrimaDataContext.FechaBaja.Value.Year,
                        formMateriaPrimaDataContext.FechaBaja.Value.Month,
                        formMateriaPrimaDataContext.FechaBaja.Value.Day,
                        formMateriaPrimaDataContext.HoraBaja.Value.Hour,
                        formMateriaPrimaDataContext.HoraBaja.Value.Minute,
                        formMateriaPrimaDataContext.HoraBaja.Value.Second);
                }
                else
                {
                    MateriaPrimaSeleccionada.FechaBaja = null;
                }
                if (!context.ProductosTerminadosComposiciones.Any(ptc => ptc.HistorialHuecoRecepcion.MateriaPrimaId == MateriaPrimaSeleccionada.MateriaPrimaId))
                {
                    // Se borran todos los historiales huecos recepciones antiguos y se añaden los nuevos
                    context.HistorialHuecosRecepciones.RemoveRange(historialHuecosRecepionesIniciales);
                    var huecosMateriasPrimas = new List<HistorialHuecoRecepcion>();
                    foreach (var hhr in formMateriaPrimaDataContext.HistorialHuecosRecepciones)
                    {
                        var hrId = hhr.HuecoRecepcion.HuecoRecepcionId;
                        // Los huecos que no se ha añadido ninguna cantidad no se añaden
                        if (hhr.Unidades != 0 && hhr.Volumen != 0)
                        {
                            hhr.HuecoRecepcion = null;
                            hhr.HuecoRecepcionId = hrId;
                            hhr.MateriaPrima = MateriaPrimaSeleccionada;
                            huecosMateriasPrimas.Add(hhr);
                        }
                    }
                    context.HistorialHuecosRecepciones.AddRange(huecosMateriasPrimas);
                }

                context.SaveChanges();
                MateriasPrimasView.Refresh();
                // CargarMateriasPrimas();
            }
        }
        #endregion


        #region Refrescar Materias Primas
        public ICommand RefrescarMateriasPrimasComando => _refrescarMateriasPrimasComando ??
            (_refrescarMateriasPrimasComando = new RelayComando(
                param => CargarMateriasPrimas(),
                param => RecepcionSeleccionada != null
             ));
        #endregion


        #region Modificar Observaciones Materia Prima
        public ICommand ModificarObservacionesMateriaPrimaComando => _modificarObservacionesMateriaPrimaComando ??
            (_modificarObservacionesMateriaPrimaComando = new RelayComando(
                param => ModificarObservacionesMateriaPrima(),
                param => MateriaPrimaSeleccionada != null
             ));

        private void ModificarObservacionesMateriaPrima()
        {
            /*var materiaPrima = context.MateriasPrimas.Single(mp => mp.MateriaPrimaId == MateriaPrimaSeleccionada.MateriaPrimaId);
            materiaPrima.Observaciones = MateriaPrimaSeleccionada.Observaciones;*/
            context.SaveChanges();

            ObservacionesMateriasPrimasEnEdicion = false;
        }
        #endregion


        #region Filtro Materias Primas
        public ICommand FiltrarMateriasPrimasComando => _filtrarMateriasPrimasComando ??
           (_filtrarMateriasPrimasComando = new RelayComando(
                param => FiltrarMateriasPrimas()
           ));

        public void FiltrarMateriasPrimas()
        {
            MateriasPrimasView.Filter = FiltroMateriasPrimas;
            MateriasPrimasView.Refresh();
        }

        private bool FiltroMateriasPrimas(object item)
        {
            var materiaPrima = item as MateriaPrima;
            string tipo = materiaPrima.TipoMateriaPrima.Nombre.ToLower();
            string grupo = materiaPrima.TipoMateriaPrima.GrupoMateriaPrima.Nombre.ToLower();
            string volumen = materiaPrima.Volumen.ToString();
            string unidades = materiaPrima.Unidades.ToString();
            string procedencia = materiaPrima.Procedencia.Nombre.ToLower();
            string fechaBaja = materiaPrima.FechaBaja.ToString();

            return (FechaBajaMateriaPrimaSeleccionada == true ? fechaBaja.Contains(TextoFiltroMateriasPrimas) : false)
                || (TipoMateriaPrimaSeleccionado == true ? tipo.Contains(TextoFiltroMateriasPrimas) : false)
                || (GrupoMateriaPrimaSeleccionado == true ? grupo.Contains(TextoFiltroMateriasPrimas) : false)
                || (VolUniMateriaPrimaSeleccionado == true ? (volumen.Contains(TextoFiltroMateriasPrimas) || unidades.Contains(TextoFiltroMateriasPrimas)) : false)
                || (ProcedenciaMateriaPrimaSeleccionada == true ? procedencia.Contains(TextoFiltroMateriasPrimas) : false);

        }
        #endregion
    }
}
