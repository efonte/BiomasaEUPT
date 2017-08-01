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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BiomasaEUPT.Vistas.GestionRecepciones
{
    public class MasOpcionesRecepcionesViewModel : ViewModelBase
    {
        public ObservableCollection<Procedencia> Procedencias { get; set; }
        public IList<Procedencia> ProcedenciasSeleccionadas { get; set; }
        public Procedencia ProcedenciaSeleccionada { get; set; }
        public ObservableCollection<GrupoMateriaPrima> GruposMateriasPrimas { get; set; }
        public IList<GrupoMateriaPrima> GruposMateriasPrimasSeleccionadas { get; set; }

        private GrupoMateriaPrima _grupoMateriaPrimaSeleccionada;
        public GrupoMateriaPrima GrupoMateriaPrimaSeleccionada
        {
            get => _grupoMateriaPrimaSeleccionada;
            set
            {
                _grupoMateriaPrimaSeleccionada = value;
                CargarTiposMateriasPrimas();
            }
        }

        public ObservableCollection<TipoMateriaPrima> TiposMateriasPrimas { get; set; }
        public IList<TipoMateriaPrima> TiposMateriasPrimasSeleccionadas { get; set; }
        public TipoMateriaPrima TipoMateriaPrimaSeleccionada { get; set; }
        public ObservableCollection<SitioRecepcion> SitiosRecepciones { get; set; }
        public IList<SitioRecepcion> SitiosRecepcionesSeleccionados { get; set; }

        private SitioRecepcion _sitioRecepcionSeleccionado;
        public SitioRecepcion SitioRecepcionSeleccionado
        {
            get => _sitioRecepcionSeleccionado;
            set
            {
                _sitioRecepcionSeleccionado = value;
                CargarHuecosRecepciones();
            }
        }

        public ObservableCollection<HuecoRecepcion> HuecosRecepciones { get; set; }
        public IList<HuecoRecepcion> HuecosRecepcionesSeleccionados { get; set; }
        public HuecoRecepcion HuecoRecepcionSeleccionado { get; set; }

        private ICommand _anadirProcedenciaComando;
        private ICommand _modificarProcedenciaComando;
        private ICommand _borrarProcedenciaComando;
        private ICommand _refrescarProcedenciasComando;

        private ICommand _anadirGrupoMateriaPrimaComando;
        private ICommand _modificarGrupoMateriaPrimaComando;
        private ICommand _borrarGrupoMateriaPrimaComando;
        private ICommand _refrescarGruposMateriasPrimasComando;

        private ICommand _anadirTipoMateriaPrimaComando;
        private ICommand _modificarTipoMateriaPrimaComando;
        private ICommand _borrarTipoMateriaPrimaComando;
        private ICommand _refrescarTiposMateriasPrimasComando;

        private ICommand _anadirSitioRecepcionComando;
        private ICommand _modificarSitioRecepcionComando;
        private ICommand _borrarSitioRecepcionComando;
        private ICommand _refrescarSitiosRecepcionesComando;

        private ICommand _anadirHuecoRecepcionComando;
        private ICommand _modificarHuecoRecepcionComando;
        private ICommand _borrarHuecoRecepcionComando;
        private ICommand _refrescarHuecoRecepcionesComando;

        private BiomasaEUPTContext context;

        public MasOpcionesRecepcionesViewModel()
        {

        }

        public override void Inicializar()
        {
            using (new CursorEspera())
            {
                context = new BiomasaEUPTContext();
                CargarProcedencias();
                CargarGruposMateriasPrimas();
                CargarSitiosRecepciones();
            }
        }

        private void CargarProcedencias()
        {
            Procedencias = new ObservableCollection<Procedencia>(context.Procedencias.ToList());
            ProcedenciaSeleccionada = null;
        }

        private void CargarGruposMateriasPrimas()
        {
            GruposMateriasPrimas = new ObservableCollection<GrupoMateriaPrima>(context.GruposMateriasPrimas.ToList());
            GrupoMateriaPrimaSeleccionada = null;
        }

        public void CargarTiposMateriasPrimas()
        {
            TiposMateriasPrimas = (GrupoMateriaPrimaSeleccionada == null)
                ? new ObservableCollection<TipoMateriaPrima>()
                : new ObservableCollection<TipoMateriaPrima>(
                    context.TiposMateriasPrimas
                    .Where(tmp => tmp.GrupoId == GrupoMateriaPrimaSeleccionada.GrupoMateriaPrimaId)
                    .ToList());
            TipoMateriaPrimaSeleccionada = null;
        }

        private void CargarSitiosRecepciones()
        {
            SitiosRecepciones = new ObservableCollection<SitioRecepcion>(context.SitiosRecepciones.ToList());
            SitioRecepcionSeleccionado = null;
        }

        private void CargarHuecosRecepciones()
        {
            HuecosRecepciones = (_sitioRecepcionSeleccionado == null)
                ? new ObservableCollection<HuecoRecepcion>()
                : new ObservableCollection<HuecoRecepcion>(
                context.HuecosRecepciones
                .Where(hr => hr.SitioId == SitioRecepcionSeleccionado.SitioRecepcionId)
                .ToList());
            HuecoRecepcionSeleccionado = null;
        }

        private void RefrescarContext()
        {
            // Hay que volver a instanciar un nuevo context ya que sino no se pueden refrescar
            // los datos debido a que se guardan en una cache.
            context.Dispose();
            context = new BiomasaEUPTContext();
        }


        #region Añadir Procedencia
        public ICommand AnadirProcedenciaComando => _anadirProcedenciaComando ??
           (_anadirProcedenciaComando = new RelayCommand(
               param => AnadirProcedencia()
           ));

        private async void AnadirProcedencia()
        {
            var formTipo = new FormTipo("Nueva Procedencia");
            formTipo.vNombreUnico.Atributo = "Nombre";
            formTipo.vNombreUnico.Tipo = "Procedencia";

            if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
            {
                context.Procedencias.Add(new Procedencia()
                {
                    Nombre = formTipo.Nombre,
                    Descripcion = formTipo.Descripcion
                });

                context.SaveChanges();
                CargarProcedencias();
            }
        }
        #endregion


        #region Borrar Procedencia
        public ICommand BorrarProcedenciaComando => _borrarProcedenciaComando ??
          (_borrarProcedenciaComando = new RelayCommand(
              param => BorrarProcedencia(),
              param => ProcedenciaSeleccionada != null
          ));

        private async void BorrarProcedencia()
        {
            var mensajeConf = new MensajeConfirmacion()
            {
                Mensaje = "¿Está seguro de que desea borrarla procedencia " + ProcedenciaSeleccionada.Nombre + "?"
            };
            if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
            {
                if (!context.MateriasPrimas.Any(mp => mp.ProcedenciaId == ProcedenciaSeleccionada.ProcedenciaId))
                {
                    context.Procedencias.Remove(ProcedenciaSeleccionada);
                    context.SaveChanges();
                    CargarProcedencias();
                }
                else
                {
                    await DialogHost.Show(new MensajeInformacion("No puede borrar la procedencia debido a que está en uso."), "RootDialog");
                }
            }
        }
        #endregion


        #region Modificar Procedencia
        public ICommand ModificarProcedenciaComando => _modificarProcedenciaComando ??
          (_modificarProcedenciaComando = new RelayCommand(
              param => ModificarProcedencia(),
              param => ProcedenciaSeleccionada != null
          ));

        private async void ModificarProcedencia()
        {
            var formTipo = new FormTipo("Editar Procedencia");
            formTipo.vNombreUnico.Atributo = "Nombre";
            formTipo.vNombreUnico.Tipo = "Procedencia";
            formTipo.vNombreUnico.NombreActual = ProcedenciaSeleccionada.Nombre;

            formTipo.Nombre = ProcedenciaSeleccionada.Nombre;
            formTipo.Descripcion = ProcedenciaSeleccionada.Descripcion;

            if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
            {
                ProcedenciaSeleccionada.Nombre = formTipo.Nombre;
                ProcedenciaSeleccionada.Descripcion = formTipo.Descripcion;
                context.SaveChanges();
                CargarProcedencias();
            }
        }
        #endregion

        #region Añadir Grupo Materia Prima
        public ICommand AnadirGrupoMateriaPrimaComando => _anadirGrupoMateriaPrimaComando ??
           (_anadirGrupoMateriaPrimaComando = new RelayCommand(
               param => AnadirGrupoMateriaPrima()
           ));

        private async void AnadirGrupoMateriaPrima()
        {
            var formTipo = new FormTipo("Nuevo Grupo MP");
            formTipo.vNombreUnico.Atributo = "Nombre";
            formTipo.vNombreUnico.Tipo = "GrupoMateriaPrima";
            formTipo.vNombreLongitud.Min = Constantes.LONG_MIN_NOMBRE_GRUPO;
            formTipo.vNombreLongitud.Max = Constantes.LONG_MAX_NOMBRE_GRUPO;
            formTipo.vDescripcionLongitud.Min = Constantes.LONG_MIN_DESCRIPCION_GRUPO;
            formTipo.vDescripcionLongitud.Max = Constantes.LONG_MAX_DESCRIPCION_GRUPO;

            if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
            {
                context.GruposMateriasPrimas.Add(new GrupoMateriaPrima()
                {
                    Nombre = formTipo.Nombre,
                    Descripcion = formTipo.Descripcion
                });

                context.SaveChanges();
                CargarGruposMateriasPrimas();
            }
        }
        #endregion


        #region Borrar Grupo Materia Prima
        public ICommand BorrarGrupoMateriaPrimaComando => _borrarGrupoMateriaPrimaComando ??
          (_borrarGrupoMateriaPrimaComando = new RelayCommand(
              param => BorrarGrupoMateriaPrima(),
              param => GrupoMateriaPrimaSeleccionada != null
          ));

        private async void BorrarGrupoMateriaPrima()
        {
            var mensajeConf = new MensajeConfirmacion()
            {
                Mensaje = "¿Está seguro de que desea borrar el grupo " + GrupoMateriaPrimaSeleccionada.Nombre + "?"
            };
            if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
            {
                if (!context.MateriasPrimas.Any(mp => mp.TipoMateriaPrima.GrupoId == GrupoMateriaPrimaSeleccionada.GrupoMateriaPrimaId))
                {
                    context.GruposMateriasPrimas.Remove(GrupoMateriaPrimaSeleccionada);
                    context.SaveChanges();
                    CargarGruposMateriasPrimas();
                }
                else
                {
                    await DialogHost.Show(new MensajeInformacion("No puede borrar el grupo de materia prima debido a que está en uso."), "RootDialog");
                }
            }
        }
        #endregion


        #region Modificar Grupo Materia Prima
        public ICommand ModificarGrupoMateriaPrimaComando => _modificarGrupoMateriaPrimaComando ??
          (_modificarGrupoMateriaPrimaComando = new RelayCommand(
              param => ModificarGrupoMateriaPrima(),
              param => GrupoMateriaPrimaSeleccionada != null
          ));

        private async void ModificarGrupoMateriaPrima()
        {
            var formTipo = new FormTipo("Editar Grupo MP");
            formTipo.vNombreUnico.Atributo = "Nombre";
            formTipo.vNombreUnico.Tipo = "GrupoMateriaPrima";
            formTipo.vNombreUnico.NombreActual = GrupoMateriaPrimaSeleccionada.Nombre;
            formTipo.vNombreLongitud.Min = Constantes.LONG_MIN_NOMBRE_GRUPO;
            formTipo.vNombreLongitud.Max = Constantes.LONG_MAX_NOMBRE_GRUPO;
            formTipo.vDescripcionLongitud.Min = Constantes.LONG_MIN_DESCRIPCION_GRUPO;
            formTipo.vDescripcionLongitud.Max = Constantes.LONG_MAX_DESCRIPCION_GRUPO;

            formTipo.Nombre = GrupoMateriaPrimaSeleccionada.Nombre;
            formTipo.Descripcion = GrupoMateriaPrimaSeleccionada.Descripcion;

            if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
            {
                GrupoMateriaPrimaSeleccionada.Nombre = formTipo.Nombre;
                GrupoMateriaPrimaSeleccionada.Descripcion = formTipo.Descripcion;
                context.SaveChanges();
                CargarGruposMateriasPrimas();
            }
        }
        #endregion


        #region Añadir Tipo Materia Prima
        public ICommand AnadirTipoMateriaPrimaComando => _anadirTipoMateriaPrimaComando ??
           (_anadirTipoMateriaPrimaComando = new RelayCommand(
               param => AnadirTipoMateriaPrima(),
               param => GrupoMateriaPrimaSeleccionada != null
           ));

        private async void AnadirTipoMateriaPrima()
        {
            var formTipoMP = new FormTipoMateriaPrima();
            formTipoMP.vNombreUnico.Atributo = "Nombre";
            formTipoMP.vNombreUnico.Tipo = "TipoMateriaPrima";

            if ((bool)await DialogHost.Show(formTipoMP, "RootDialog"))
            {
                context.TiposMateriasPrimas.Add(new TipoMateriaPrima()
                {
                    Nombre = formTipoMP.Nombre,
                    Descripcion = formTipoMP.Descripcion,
                    MedidoEnVolumen = formTipoMP.lbMedido.SelectedIndex == 0,
                    MedidoEnUnidades = formTipoMP.lbMedido.SelectedIndex == 1,
                    GrupoId = GrupoMateriaPrimaSeleccionada.GrupoMateriaPrimaId
                });

                context.SaveChanges();
                CargarTiposMateriasPrimas();
            }
        }
        #endregion


        #region Borrar Tipo Materia Prima
        public ICommand BorrarTipoMateriaPrimaComando => _borrarTipoMateriaPrimaComando ??
          (_borrarTipoMateriaPrimaComando = new RelayCommand(
              param => BorrarTipoMateriaPrima(),
              param => TipoMateriaPrimaSeleccionada != null
          ));

        private async void BorrarTipoMateriaPrima()
        {
            var mensajeConf = new MensajeConfirmacion()
            {
                Mensaje = "¿Está seguro de que desea borrar el Tipo " + TipoMateriaPrimaSeleccionada.Nombre + "?"
            };
            if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
            {
                if (!context.MateriasPrimas.Any(mp => mp.TipoId == TipoMateriaPrimaSeleccionada.TipoMateriaPrimaId))
                {
                    context.TiposMateriasPrimas.Remove(TipoMateriaPrimaSeleccionada);
                    context.SaveChanges();
                    CargarTiposMateriasPrimas();
                }
                else
                {
                    await DialogHost.Show(new MensajeInformacion("No puede borrar el tipo de materia prima debido a que está en uso."), "RootDialog");
                }
            }
        }
        #endregion


        #region Modificar Tipo Materia Prima
        public ICommand ModificarTipoMateriaPrimaComando => _modificarTipoMateriaPrimaComando ??
          (_modificarTipoMateriaPrimaComando = new RelayCommand(
              param => ModificarTipoMateriaPrima(),
              param => TipoMateriaPrimaSeleccionada != null
          ));

        private async void ModificarTipoMateriaPrima()
        {
            var formTipoMP = new FormTipoMateriaPrima("Editar Tipo M. Prima");
            formTipoMP.vNombreUnico.Atributo = "Nombre";
            formTipoMP.vNombreUnico.Tipo = "TipoMateriaPrima";
            formTipoMP.vNombreUnico.NombreActual = TipoMateriaPrimaSeleccionada.Nombre;

            formTipoMP.Nombre = TipoMateriaPrimaSeleccionada.Nombre;
            formTipoMP.Descripcion = TipoMateriaPrimaSeleccionada.Descripcion;
            if (TipoMateriaPrimaSeleccionada.MedidoEnVolumen == true)
                formTipoMP.lbMedido.SelectedIndex = 0;
            else
                formTipoMP.lbMedido.SelectedIndex = 1;

            if ((bool)await DialogHost.Show(formTipoMP, "RootDialog"))
            {
                TipoMateriaPrimaSeleccionada.Nombre = formTipoMP.Nombre;
                TipoMateriaPrimaSeleccionada.Descripcion = formTipoMP.Descripcion;
                TipoMateriaPrimaSeleccionada.MedidoEnVolumen = formTipoMP.lbMedido.SelectedIndex == 0;
                TipoMateriaPrimaSeleccionada.MedidoEnUnidades = formTipoMP.lbMedido.SelectedIndex == 1;
                context.SaveChanges();
                CargarTiposMateriasPrimas();
            }
        }
        #endregion


        #region Añadir Sitio Recepción
        public ICommand AnadirSitioRecepcionComando => _anadirSitioRecepcionComando ??
           (_anadirSitioRecepcionComando = new RelayCommand(
               param => AnadirSitioRecepcion()
           ));

        private async void AnadirSitioRecepcion()
        {
            var formTipo = new FormTipo("Nuevo Sitio de Recepción");
            formTipo.vNombreUnico.Atributo = "Nombre";
            formTipo.vNombreUnico.Tipo = "SitioRecepcion";

            if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
            {
                context.SitiosRecepciones.Add(new SitioRecepcion()
                {
                    Nombre = formTipo.Nombre,
                    Descripcion = formTipo.Descripcion,
                });

                context.SaveChanges();
                CargarTiposMateriasPrimas();
            }
        }
        #endregion


        #region Borrar Sitio Recepción
        public ICommand BorrarSitioRecepcionComando => _borrarSitioRecepcionComando ??
          (_borrarSitioRecepcionComando = new RelayCommand(
              param => BorrarSitioRecepcion(),
              param => SitioRecepcionSeleccionado != null
          ));

        private async void BorrarSitioRecepcion()
        {
            var mensajeConf = new MensajeConfirmacion()
            {
                Mensaje = "¿Está seguro de que desea borrar el sitio de recepción " + SitioRecepcionSeleccionado.Nombre + "?"
            };
            if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
            {
                if (!context.HuecosRecepciones.Any(hr => hr.SitioId == SitioRecepcionSeleccionado.SitioRecepcionId))
                {
                    context.SitiosRecepciones.Remove(SitioRecepcionSeleccionado);
                    context.SaveChanges();
                    CargarTiposMateriasPrimas();
                }
                else
                {
                    await DialogHost.Show(new MensajeInformacion("No puede borrar el sitio recepción debido a que está en uso."), "RootDialog");
                }
            }
        }
        #endregion


        #region Modificar Sitio Recepción
        public ICommand ModificarSitioRecepcionComando => _modificarSitioRecepcionComando ??
          (_modificarSitioRecepcionComando = new RelayCommand(
              param => ModificarSitioRecepcion(),
              param => SitioRecepcionSeleccionado != null
          ));

        private async void ModificarSitioRecepcion()
        {
            var formTipo = new FormTipo("Editar Sitio de Recepcion");
            formTipo.vNombreUnico.Atributo = "Nombre";
            formTipo.vNombreUnico.Tipo = "SitioRecepcion";
            formTipo.vNombreUnico.NombreActual = SitioRecepcionSeleccionado.Nombre;

            formTipo.Nombre = SitioRecepcionSeleccionado.Nombre;
            formTipo.Descripcion = SitioRecepcionSeleccionado.Descripcion;

            if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
            {
                SitioRecepcionSeleccionado.Nombre = formTipo.Nombre;
                SitioRecepcionSeleccionado.Descripcion = formTipo.Descripcion;
                context.SaveChanges();
                CargarSitiosRecepciones();
            }
        }
        #endregion
    }
}
