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

namespace BiomasaEUPT.Vistas.GestionElaboraciones
{
    public class MasOpcionesElaboracionesViewModel : ViewModelBase
    {
        public ObservableCollection<GrupoProductoTerminado> GruposProductosTerminados { get; set; }
        public IList<GrupoProductoTerminado> GruposProductosTerminadosSeleccionados { get; set; }

        private GrupoProductoTerminado _grupoProductoTerminadoSeleccionado;
        public GrupoProductoTerminado GrupoProductoTerminadoSeleccionado
        {
            get => _grupoProductoTerminadoSeleccionado;
            set
            {
                _grupoProductoTerminadoSeleccionado = value;
                CargarTiposProductosTerminados();
            }
        }

        public ObservableCollection<TipoProductoEnvasado> TiposProductosEnvasados { get; set; }
        public IList<TipoProductoEnvasado> TiposProductosEnvasadosSeleccionados { get; set; }
        public TipoProductoEnvasado TipoProductoEnvasadoSeleccionado { get; set; }

        public ObservableCollection<TipoProductoTerminado> TiposProductosTerminados { get; set; }
        public IList<TipoProductoTerminado> TiposProductosTerminadosSeleccionados { get; set; }
        public TipoProductoTerminado TipoProductoTerminadoSeleccionado { get; set; }


        public ObservableCollection<SitioAlmacenaje> SitiosAlmacenajes { get; set; }
        public IList<SitioAlmacenaje> SitiosAlmacenajesSeleccionados { get; set; }

        private SitioAlmacenaje _sitioAlmacenajeSeleccionado;
        public SitioAlmacenaje SitioAlmacenajeSeleccionado
        {
            get => _sitioAlmacenajeSeleccionado;
            set
            {
                _sitioAlmacenajeSeleccionado = value;
                CargarHuecosAlmacenajes();
            }
        }

        public ObservableCollection<HuecoAlmacenaje> HuecosAlmacenajes { get; set; }
        public IList<HuecoAlmacenaje> HuecosAlmacenajesSeleccionados { get; set; }
        public HuecoAlmacenaje HuecoAlmacenajeSeleccionado { get; set; }


        private ICommand _anadirGrupoProductoTerminadoComando;
        private ICommand _modificarGrupoProductoTerminadoComando;
        private ICommand _borrarGrupoProductoTerminadoComando;
        private ICommand _refrescarGruposProductosTerminadosComando;

        private ICommand _anadirTipoProductoTerminadoComando;
        private ICommand _modificarTipoProductoTerminadoComando;
        private ICommand _borrarTipoProductoTerminadoComando;
        private ICommand _refrescarTiposProductosTerminadosComando;

        private ICommand _anadirTipoProductoEnvasadoComando;
        private ICommand _modificarTipoProductoEnvasadoComando;
        private ICommand _borrarTipoProductoEnvasadoComando;
        private ICommand _refrescarTiposProductosEnvasadosComando;

        private ICommand _anadirSitioAlmacenajeComando;
        private ICommand _modificarSitioAlmacenajeComando;
        private ICommand _borrarSitioAlmacenajeComando;
        private ICommand _refrescarSitiosAlmacenajesComando;

        private ICommand _anadirHuecoAlmacenajeComando;
        private ICommand _modificarHuecoAlmacenajeComando;
        private ICommand _borrarHuecoAlmacenajeComando;
        private ICommand _refrescarHuecosAlmacenajesComando;

        private BiomasaEUPTContext context;

        public MasOpcionesElaboracionesViewModel()
        {

        }

        public override void Inicializar()
        {
            using (new CursorEspera())
            {
                context = new BiomasaEUPTContext();
                CargarGruposProductosTerminados();
                CargarSitiosAlmacenajes();
            }
        }


        private void CargarGruposProductosTerminados()
        {
            GruposProductosTerminados = new ObservableCollection<GrupoProductoTerminado>(context.GruposProductosTerminados.ToList());
            GrupoProductoTerminadoSeleccionado = null;
        }

        public void CargarTiposProductosTerminados()
        {
            TiposProductosTerminados = (GrupoProductoTerminadoSeleccionado == null)
                ? new ObservableCollection<TipoProductoTerminado>()
                : new ObservableCollection<TipoProductoTerminado>(
                    context.TiposProductosTerminados
                    .Where(tpt => tpt.GrupoId == GrupoProductoTerminadoSeleccionado.GrupoProductoTerminadoId)
                    .ToList());
            TipoProductoTerminadoSeleccionado = null;
        }

        private void CargarTiposProductosEnvasados()
        {
            TiposProductosEnvasados = new ObservableCollection<TipoProductoEnvasado>(context.TiposProductosEnvasados.ToList());
            TipoProductoEnvasadoSeleccionado = null;
        }

        private void CargarSitiosAlmacenajes()
        {
            SitiosAlmacenajes = new ObservableCollection<SitioAlmacenaje>(context.SitiosAlmacenajes.ToList());
            SitioAlmacenajeSeleccionado = null;
        }

        private void CargarHuecosAlmacenajes()
        {
            HuecosAlmacenajes = (_sitioAlmacenajeSeleccionado == null)
                ? new ObservableCollection<HuecoAlmacenaje>()
                : new ObservableCollection<HuecoAlmacenaje>(
                context.HuecosAlmacenajes
                .Where(ha => ha.SitioId == SitioAlmacenajeSeleccionado.SitioAlmacenajeId)
                .ToList());
            HuecoAlmacenajeSeleccionado = null;
        }

        private void RefrescarContext()
        {
            // Hay que volver a instanciar un nuevo context ya que sino no se pueden refrescar
            // los datos debido a que se guardan en una cache.
            context.Dispose();
            context = new BiomasaEUPTContext();
        }
        
        
        #region Añadir Grupo Producto Terminado
        public ICommand AnadirGrupoProductoTerminadoComando => _anadirGrupoProductoTerminadoComando ??
           (_anadirGrupoProductoTerminadoComando = new RelayCommand(
               param => AnadirGrupoProductoTerminado()
           ));

        private async void AnadirGrupoProductoTerminado()
        {
            var formTipo = new FormTipo("Nuevo Grupo PT");
            formTipo.vNombreUnico.Atributo = "Nombre";
            formTipo.vNombreUnico.Tipo = "GrupoProductoTerminado";
            formTipo.vNombreLongitud.Min = Constantes.LONG_MIN_NOMBRE_GRUPO;
            formTipo.vNombreLongitud.Max = Constantes.LONG_MAX_NOMBRE_GRUPO;
            formTipo.vDescripcionLongitud.Min = Constantes.LONG_MIN_DESCRIPCION_GRUPO;
            formTipo.vDescripcionLongitud.Max = Constantes.LONG_MAX_DESCRIPCION_GRUPO;

            if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
            {
                context.GruposProductosTerminados.Add(new GrupoProductoTerminado()
                {
                    Nombre = formTipo.Nombre,
                    Descripcion = formTipo.Descripcion
                });

                context.SaveChanges();
                CargarGruposProductosTerminados();
            }
        }
        #endregion


        #region Borrar Grupo Producto Terminado
        public ICommand BorrarGrupoProductoTerminadoComando => _borrarGrupoProductoTerminadoComando ??
          (_borrarGrupoProductoTerminadoComando = new RelayCommand(
              param => BorrarGrupoProductoTerminado(),
              param => GrupoProductoTerminadoSeleccionado != null
          ));

        private async void BorrarGrupoProductoTerminado()
        {
            var mensajeConf = new MensajeConfirmacion()
            {
                Mensaje = "¿Está seguro de que desea borrar el grupo " + GrupoProductoTerminadoSeleccionado.Nombre + "?"
            };
            if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
            {
                if (!context.ProductosTerminados.Any(pt => pt.TipoProductoTerminado.GrupoId == GrupoProductoTerminadoSeleccionado.GrupoProductoTerminadoId))
                {
                    context.GruposProductosTerminados.Remove(GrupoProductoTerminadoSeleccionado);
                    context.SaveChanges();
                    CargarGruposProductosTerminados();
                }
                else
                {
                    await DialogHost.Show(new MensajeInformacion("No puede borrar el grupo de producto terminado debido a que está en uso."), "RootDialog");
                }
            }
        }
        #endregion


        #region Modificar Grupo Producto Terminado 
        public ICommand ModificarGrupoProductoTerminadoComando => _modificarGrupoProductoTerminadoComando ??
          (_modificarGrupoProductoTerminadoComando = new RelayCommand(
              param => ModificarGrupoProductoTerminado(),
              param => GrupoProductoTerminadoSeleccionado != null
          ));

        private async void ModificarGrupoProductoTerminado()
        {
            var formTipo = new FormTipo("Editar Grupo PT");
            formTipo.vNombreUnico.Atributo = "Nombre";
            formTipo.vNombreUnico.Tipo = "GrupoProductoTerminado";
            formTipo.vNombreUnico.NombreActual = GrupoProductoTerminadoSeleccionado.Nombre;
            formTipo.vNombreLongitud.Min = Constantes.LONG_MIN_NOMBRE_GRUPO;
            formTipo.vNombreLongitud.Max = Constantes.LONG_MAX_NOMBRE_GRUPO;
            formTipo.vDescripcionLongitud.Min = Constantes.LONG_MIN_DESCRIPCION_GRUPO;
            formTipo.vDescripcionLongitud.Max = Constantes.LONG_MAX_DESCRIPCION_GRUPO;

            formTipo.Nombre = GrupoProductoTerminadoSeleccionado.Nombre;
            formTipo.Descripcion = GrupoProductoTerminadoSeleccionado.Descripcion;

            if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
            {
                GrupoProductoTerminadoSeleccionado.Nombre = formTipo.Nombre;
                GrupoProductoTerminadoSeleccionado.Descripcion = formTipo.Descripcion;
                context.SaveChanges();
                CargarGruposProductosTerminados();
            }
        }
        #endregion


        #region Añadir Tipo Producto Terminado
        public ICommand AnadirTipoProductoTerminadoComando => _anadirTipoProductoTerminadoComando ??
           (_anadirTipoProductoTerminadoComando = new RelayCommand(
               param => AnadirTipoProductoTerminado(),
               param => GrupoProductoTerminadoSeleccionado != null
           ));

        private async void AnadirTipoProductoTerminado()
        {
            var formTipoPT = new FormTipoProductoTerminado();
            formTipoPT.vNombreUnico.Atributo = "Nombre";
            formTipoPT.vNombreUnico.Tipo = "TipoProductoTerminado";

            if ((bool)await DialogHost.Show(formTipoPT, "RootDialog"))
            {
                context.TiposProductosTerminados.Add(new TipoProductoTerminado()
                {
                    Nombre = formTipoPT.Nombre,
                    //Descripcion = formTipoPT.Descripcion,
                    //Humedad = formTipoPT.Humedad,
                    //Tamano = formTipoPT.Tamano,
                    MedidoEnVolumen = formTipoPT.lbMedido.SelectedIndex == 0,
                    MedidoEnUnidades = formTipoPT.lbMedido.SelectedIndex == 1,
                    GrupoId = GrupoProductoTerminadoSeleccionado.GrupoProductoTerminadoId
                });

                context.SaveChanges();
                CargarTiposProductosTerminados();
            }
        }
        #endregion


        #region Borrar Tipo Producto Terminado
        public ICommand BorrarTipoProductoTerminadoComando => _borrarTipoProductoTerminadoComando ??
          (_borrarTipoProductoTerminadoComando = new RelayCommand(
              param => BorrarTipoProductoTerminado(),
              param => TipoProductoTerminadoSeleccionado != null
          ));

        private async void BorrarTipoProductoTerminado()
        {
            var mensajeConf = new MensajeConfirmacion()
            {
                Mensaje = "¿Está seguro de que desea borrar el Tipo " + TipoProductoTerminadoSeleccionado.Nombre + "?"
            };
            if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
            {
                if (!context.ProductosTerminados.Any(pt => pt.TipoId == TipoProductoTerminadoSeleccionado.TipoProductoTerminadoId))
                {
                    context.TiposProductosTerminados.Remove(TipoProductoTerminadoSeleccionado);
                    context.SaveChanges();
                    CargarTiposProductosTerminados();
                }
                else
                {
                    await DialogHost.Show(new MensajeInformacion("No puede borrar el tipo de producto terminado debido a que está en uso."), "RootDialog");
                }
            }
        }
        #endregion


        #region Modificar Tipo Producto Terminado
        public ICommand ModificarTipoProductoTerminadoComando => _modificarTipoProductoTerminadoComando ??
          (_modificarTipoProductoTerminadoComando = new RelayCommand(
              param => ModificarTipoProductoTerminado(),
              param => TipoProductoTerminadoSeleccionado != null
          ));

        private async void ModificarTipoProductoTerminado()
        {
            var formTipoPT = new FormTipoProductoTerminado("Editar Tipo P. Terminado");
            formTipoPT.vNombreUnico.Atributo = "Nombre";
            formTipoPT.vNombreUnico.Tipo = "TipoProductoTerminado";
            formTipoPT.vNombreUnico.NombreActual = TipoProductoTerminadoSeleccionado.Nombre;

            formTipoPT.Nombre = TipoProductoTerminadoSeleccionado.Nombre;
            //formTipoMP.Descripcion = TipoProductoTerminadoSeleccionado.Descripcion;
            if (TipoProductoTerminadoSeleccionado.MedidoEnVolumen == true)
                formTipoPT.lbMedido.SelectedIndex = 0;
            else
                formTipoPT.lbMedido.SelectedIndex = 1;

            if ((bool)await DialogHost.Show(formTipoPT, "RootDialog"))
            {
                TipoProductoTerminadoSeleccionado.Nombre = formTipoPT.Nombre;
                //TipoProductoTerminadoSeleccionado.Descripcion = formTipoPT.Descripcion;
                //TipoProductoTerminadoSeleccionado.Humedad = formTipoPT.Humedad;
                //TipoProductoTerminadoSeleccionado.Tamano = formTipoPT.Tamano;
                TipoProductoTerminadoSeleccionado.MedidoEnVolumen = formTipoPT.lbMedido.SelectedIndex == 0;
                TipoProductoTerminadoSeleccionado.MedidoEnUnidades = formTipoPT.lbMedido.SelectedIndex == 1;
                context.SaveChanges();
                CargarTiposProductosTerminados();
            }
        }
        #endregion

        #region Añadir Tipo Producto Envasado
        public ICommand AnadirTipoProductoEnvasadoComando => _anadirTipoProductoEnvasadoComando ??
           (_anadirTipoProductoEnvasadoComando = new RelayCommand(
               param => AnadirTipoProductoEnvasado()
           ));

        private async void AnadirTipoProductoEnvasado()
        {
            var formTipoPE = new FormTipoProductoEnvasado("Nuevo Tipo de Producto Envasado");
            formTipoPE.vNombreUnico.Atributo = "Nombre";
            formTipoPE.vNombreUnico.Tipo = "TipoProductoEnvasado";

            if ((bool)await DialogHost.Show(formTipoPE, "RootDialog"))
            {
                context.TiposProductosEnvasados.Add(new TipoProductoEnvasado()
                {
                    Nombre = formTipoPE.Nombre,
                    Descripcion = formTipoPE.Descripcion,
                    MedidoEnVolumen = formTipoPE.lbMedido.SelectedIndex == 0,
                    MedidoEnUnidades = formTipoPE.lbMedido.SelectedIndex == 1,
                });

                context.SaveChanges();
                CargarTiposProductosEnvasados();
            }
        }
        #endregion


        #region Borrar Tipo Producto Envasado
        public ICommand BorrarTipoProductoEnvasadoComando => _borrarTipoProductoEnvasadoComando ??
          (_borrarTipoProductoEnvasadoComando = new RelayCommand(
              param => BorrarTipoProductoEnvasado(),
              param => TipoProductoEnvasadoSeleccionado != null
          ));

        private async void BorrarTipoProductoEnvasado()
        {
            var mensajeConf = new MensajeConfirmacion()
            {
                Mensaje = "¿Está seguro de que desea borrar el tipo de producto envasado " + TipoProductoEnvasadoSeleccionado.Nombre + "?"
            };
            if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
            {
                if (!context.TiposProductosEnvasados.Any(tpe => tpe.TipoProductoEnvasadoId == TipoProductoEnvasadoSeleccionado.TipoProductoEnvasadoId))
                {
                    context.TiposProductosEnvasados.Remove(TipoProductoEnvasadoSeleccionado);
                    context.SaveChanges();
                    CargarTiposProductosEnvasados();
                }
                else
                {
                    await DialogHost.Show(new MensajeInformacion("No puede borrar el tipo de producto envasado debido a que está en uso."), "RootDialog");
                }
            }
        }
        #endregion


        #region Modificar Tipo Producto Envasado
        public ICommand ModificarTipoProductoEnvasadoComando => _modificarTipoProductoEnvasadoComando ??
          (_modificarTipoProductoEnvasadoComando = new RelayCommand(
              param => ModificarTipoProductoEnvasado(),
              param => TipoProductoEnvasadoSeleccionado != null
          ));

        private async void ModificarTipoProductoEnvasado()
        {
            var formTipoPE = new FormTipoProductoEnvasado("Editar Tipo de Producto Envasado");
            formTipoPE.vNombreUnico.Atributo = "Nombre";
            formTipoPE.vNombreUnico.Tipo = "TipoProductoEnvasado";
            formTipoPE.vNombreUnico.NombreActual = TipoProductoEnvasadoSeleccionado.Nombre;

            formTipoPE.Nombre = TipoProductoEnvasadoSeleccionado.Nombre;
            formTipoPE.Descripcion = TipoProductoEnvasadoSeleccionado.Descripcion;

            if ((bool)await DialogHost.Show(formTipoPE, "RootDialog"))
            {
                TipoProductoEnvasadoSeleccionado.Nombre = formTipoPE.Nombre;
                TipoProductoEnvasadoSeleccionado.Descripcion = formTipoPE.Descripcion;
                TipoProductoEnvasadoSeleccionado.MedidoEnVolumen = formTipoPE.lbMedido.SelectedIndex == 0;
                TipoProductoEnvasadoSeleccionado.MedidoEnUnidades = formTipoPE.lbMedido.SelectedIndex == 1;
                context.SaveChanges();
                CargarTiposProductosEnvasados();
            }
        }
        #endregion


        #region Añadir Sitio Almacenaje
        public ICommand AnadirSitioAlmacenajeComando => _anadirSitioAlmacenajeComando ??
           (_anadirSitioAlmacenajeComando = new RelayCommand(
               param => AnadirSitioAlmacenaje()
           ));

        private async void AnadirSitioAlmacenaje()
        {
            var formTipo = new FormTipo("Nuevo Sitio de Almacenaje");
            formTipo.vNombreUnico.Atributo = "Nombre";
            formTipo.vNombreUnico.Tipo = "SitioAlmacenaje";

            if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
            {
                context.SitiosAlmacenajes.Add(new SitioAlmacenaje()
                {
                    Nombre = formTipo.Nombre,
                    Descripcion = formTipo.Descripcion,
                });

                context.SaveChanges();
                CargarSitiosAlmacenajes();
            }
        }
        #endregion


        #region Borrar Sitio Almacenaje
        public ICommand BorrarSitioAlmacenajeComando => _borrarSitioAlmacenajeComando ??
          (_borrarSitioAlmacenajeComando = new RelayCommand(
              param => BorrarSitioAlmacenaje(),
              param => SitioAlmacenajeSeleccionado != null
          ));

        private async void BorrarSitioAlmacenaje()
        {
            var mensajeConf = new MensajeConfirmacion()
            {
                Mensaje = "¿Está seguro de que desea borrar el sitio de almacenaje " + SitioAlmacenajeSeleccionado.Nombre + "?"
            };
            if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
            {
                if (!context.HuecosAlmacenajes.Any(ha => ha.SitioId == SitioAlmacenajeSeleccionado.SitioAlmacenajeId))
                {
                    context.SitiosAlmacenajes.Remove(SitioAlmacenajeSeleccionado);
                    context.SaveChanges();
                    CargarSitiosAlmacenajes();
                }
                else
                {
                    await DialogHost.Show(new MensajeInformacion("No puede borrar el sitio de almacenaje debido a que está en uso."), "RootDialog");
                }
            }
        }
        #endregion


        #region Modificar Sitio Almacenaje
        public ICommand ModificarSitioAlmacenajeComando => _modificarSitioAlmacenajeComando ??
          (_modificarSitioAlmacenajeComando = new RelayCommand(
              param => ModificarSitioAlmacenaje(),
              param => SitioAlmacenajeSeleccionado != null
          ));

        private async void ModificarSitioAlmacenaje()
        {
            var formTipo = new FormTipo("Editar Sitio de Almacenaje");
            formTipo.vNombreUnico.Atributo = "Nombre";
            formTipo.vNombreUnico.Tipo = "SitioAlmacenaje";
            formTipo.vNombreUnico.NombreActual = SitioAlmacenajeSeleccionado.Nombre;

            formTipo.Nombre = SitioAlmacenajeSeleccionado.Nombre;
            formTipo.Descripcion = SitioAlmacenajeSeleccionado.Descripcion;

            if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
            {
                SitioAlmacenajeSeleccionado.Nombre = formTipo.Nombre;
                SitioAlmacenajeSeleccionado.Descripcion = formTipo.Descripcion;
                context.SaveChanges();
                CargarSitiosAlmacenajes();
            }
        }
        #endregion


        #region Añadir Hueco Almacenaje
        public ICommand AnadirHuecoAlmacenajeComando => _anadirHuecoAlmacenajeComando ??
           (_anadirHuecoAlmacenajeComando = new RelayCommand(
               param => AnadirHuecoAlmacenaje()
           ));

        private async void AnadirHuecoAlmacenaje()
        {
            var formHueco = new FormHueco("Nuevo Hueco de Almacenaje");
            formHueco.vNombreUnico.Atributo = "Nombre";
            formHueco.vNombreUnico.Tipo = "HuecoAlmacenaje";

            if ((bool)await DialogHost.Show(formHueco, "RootDialog"))
            {
                context.HuecosAlmacenajes.Add(new HuecoAlmacenaje()
                {
                    Nombre = formHueco.Nombre,
                    UnidadesTotales = formHueco.Unidades,
                    VolumenTotal = formHueco.Volumen,
                    SitioId = SitioAlmacenajeSeleccionado.SitioAlmacenajeId
                });

                context.SaveChanges();
                CargarHuecosAlmacenajes();
            }
        }
        #endregion


        #region Borrar Hueco Almacenaje
        public ICommand BorrarHuecoAlmacenajeComando => _borrarHuecoAlmacenajeComando ??
          (_borrarHuecoAlmacenajeComando = new RelayCommand(
              param => BorrarHuecoAlmacenaje(),
              param => HuecoAlmacenajeSeleccionado != null
          ));

        private async void BorrarHuecoAlmacenaje()
        {
            var mensajeConf = new MensajeConfirmacion()
            {
                Mensaje = "¿Está seguro de que desea borrar el hueco de almacenaje " + HuecoAlmacenajeSeleccionado.Nombre + "?"
            };
            if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
            {
                if (!context.HistorialHuecosAlmacenajes.Any(hha => hha.HuecoAlmacenajeId == HuecoAlmacenajeSeleccionado.HuecoAlmacenajeId))
                {
                    context.HuecosAlmacenajes.Remove(HuecoAlmacenajeSeleccionado);
                    context.SaveChanges();
                    CargarHuecosAlmacenajes();
                }
                else
                {
                    await DialogHost.Show(new MensajeInformacion("No puede borrar el hueco de almacenaje debido a que está en uso."), "RootDialog");
                }
            }
        }
        #endregion


        #region Modificar Hueco Almacenaje
        public ICommand ModificarHuecoAlmacenajeComando => _modificarHuecoAlmacenajeComando ??
          (_modificarHuecoAlmacenajeComando = new RelayCommand(
              param => ModificarHuecoAlmacenaje(),
              param => HuecoAlmacenajeSeleccionado != null
          ));

        private async void ModificarHuecoAlmacenaje()
        {
            var formHueco = new FormHueco("Editar Hueco de Almacenaje");
            formHueco.vNombreUnico.Atributo = "Nombre";
            formHueco.vNombreUnico.Tipo = "HuecoAlmacenaje";
            formHueco.vNombreUnico.NombreActual = HuecoAlmacenajeSeleccionado.Nombre;

            formHueco.Nombre = HuecoAlmacenajeSeleccionado.Nombre;
            formHueco.Unidades = HuecoAlmacenajeSeleccionado.UnidadesTotales;
            formHueco.Volumen = HuecoAlmacenajeSeleccionado.VolumenTotal;

            if ((bool)await DialogHost.Show(formHueco, "RootDialog"))
            {
                HuecoAlmacenajeSeleccionado.Nombre = formHueco.Nombre;
                HuecoAlmacenajeSeleccionado.UnidadesTotales = formHueco.Unidades;
                HuecoAlmacenajeSeleccionado.VolumenTotal = formHueco.Volumen;
                context.SaveChanges();
                CargarHuecosAlmacenajes();
            }
        }
        #endregion
    }
}
