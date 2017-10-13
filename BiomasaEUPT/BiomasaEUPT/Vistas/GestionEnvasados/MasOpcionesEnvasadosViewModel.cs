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

namespace BiomasaEUPT.Vistas.GestionEnvasados
{
    public class MasOpcionesEnvasadosViewModel : ViewModelBase
    {
        public ObservableCollection<GrupoProductoEnvasado> GruposProductosEnvasados { get; set; }
        public IList<GrupoProductoEnvasado> GruposProductosEnvasadosSeleccionados { get; set; }

        private GrupoProductoEnvasado _grupoProductoEnvasadoSeleccionado;
        public GrupoProductoEnvasado GrupoProductoEnvasadoSeleccionado
        {
            get => _grupoProductoEnvasadoSeleccionado;
            set
            {
                _grupoProductoEnvasadoSeleccionado = value;
                CargarTiposProductosEnvasados();
            }
        }

        public ObservableCollection<TipoProductoEnvasado> TiposProductosEnvasados { get; set; }
        public IList<TipoProductoEnvasado> TiposProductosEnvasadosSeleccionados { get; set; }
        public TipoProductoEnvasado TipoProductoEnvasadoSeleccionado { get; set; }

        public ObservableCollection<Picking> Picking { get; set; }
        public IList<Picking> PickingSeleccionados { get; set; }
        public Picking PickingSeleccionado { get; set; }


        private ICommand _anadirGrupoProductoEnvasadoComando;
        private ICommand _modificarGrupoProductoEnvasadoComando;
        private ICommand _borrarGrupoProductoEnvasadoComando;
        private ICommand _refrescarGruposProductosEnvasadosComando;

        private ICommand _anadirTipoProductoEnvasadoComando;
        private ICommand _modificarTipoProductoEnvasadoComando;
        private ICommand _borrarTipoProductoEnvasadoComando;
        private ICommand _refrescarTiposProductosEnvasadosComando;

        private ICommand _anadirPickingComando;
        private ICommand _modificarPickingComando;
        private ICommand _borrarPickingComando;
        private ICommand _refrescarPickingComando;


        private BiomasaEUPTContext context;

        public MasOpcionesEnvasadosViewModel()
        {

        }


        public override void Inicializar()
        {
            using (new CursorEspera())
            {
                context = new BiomasaEUPTContext();
                CargarGruposProductosEnvasados();
                CargarPicking();
            }
        }


        private void CargarGruposProductosEnvasados()
        {
            GruposProductosEnvasados = new ObservableCollection<GrupoProductoEnvasado>(context.GruposProductosEnvasados.ToList());
            GrupoProductoEnvasadoSeleccionado = null;
        }

        public void CargarTiposProductosEnvasados()
        {
            TiposProductosEnvasados = (GrupoProductoEnvasadoSeleccionado == null)
                ? new ObservableCollection<TipoProductoEnvasado>()
                : new ObservableCollection<TipoProductoEnvasado>(
                    context.TiposProductosEnvasados
                    .Where(tpe => tpe.GrupoId == GrupoProductoEnvasadoSeleccionado.GrupoProductoEnvasadoId)
                    .ToList());
            TipoProductoEnvasadoSeleccionado = null;
        }

        private void CargarPicking()
        {
            Picking = new ObservableCollection<Picking>(context.Picking.ToList());
            PickingSeleccionado = null;
        }


        private void RefrescarContext()
        {
            // Hay que volver a instanciar un nuevo context ya que sino no se pueden refrescar
            // los datos debido a que se guardan en una cache.
            context.Dispose();
            context = new BiomasaEUPTContext();
        }


        #region Añadir Grupo Producto Envasado
        public ICommand AnadirGrupoProductoEnvasadoComando => _anadirGrupoProductoEnvasadoComando ??
           (_anadirGrupoProductoEnvasadoComando = new RelayCommand(
               param => AnadirGrupoProductoEnvasado()
           ));

        private async void AnadirGrupoProductoEnvasado()
        {
            var formTipo = new FormTipo("Nuevo Grupo PE");
            formTipo.vNombreUnico.Atributo = "Nombre";
            formTipo.vNombreUnico.Tipo = "GrupoProductoEnvasado";
            formTipo.vNombreLongitud.Min = Constantes.LONG_MIN_NOMBRE_GRUPO;
            formTipo.vNombreLongitud.Max = Constantes.LONG_MAX_NOMBRE_GRUPO;
            formTipo.vDescripcionLongitud.Min = Constantes.LONG_MIN_DESCRIPCION_GRUPO;
            formTipo.vDescripcionLongitud.Max = Constantes.LONG_MAX_DESCRIPCION_GRUPO;

            if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
            {
                context.GruposProductosEnvasados.Add(new GrupoProductoEnvasado()
                {
                    Nombre = formTipo.Nombre,
                    Descripcion = formTipo.Descripcion
                });

                context.SaveChanges();
                CargarGruposProductosEnvasados();
            }
        }
        #endregion


        #region Borrar Grupo Producto Envasado
        public ICommand BorrarGrupoProductoEnvasadoComando => _borrarGrupoProductoEnvasadoComando ??
          (_borrarGrupoProductoEnvasadoComando = new RelayCommand(
              param => BorrarGrupoProductoEnvasado(),
              param => GrupoProductoEnvasadoSeleccionado != null
          ));

        private async void BorrarGrupoProductoEnvasado()
        {
            var mensajeConf = new MensajeConfirmacion()
            {
                Mensaje = "¿Está seguro de que desea borrar el grupo " + GrupoProductoEnvasadoSeleccionado.Nombre + "?"
            };
            if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
            {
                if (!context.ProductosEnvasados.Any(pt => pt.TipoProductoEnvasado.GrupoId == GrupoProductoEnvasadoSeleccionado.GrupoProductoEnvasadoId))
                {
                    context.GruposProductosEnvasados.Remove(GrupoProductoEnvasadoSeleccionado);
                    context.SaveChanges();
                    CargarGruposProductosEnvasados();
                }
                else
                {
                    await DialogHost.Show(new MensajeInformacion("No puede borrar el grupo de producto envasado debido a que está en uso."), "RootDialog");
                }
            }
        }
        #endregion


        #region Modificar Grupo Producto Envasado
        public ICommand ModificarGrupoProductoEnvasadoComando => _modificarGrupoProductoEnvasadoComando ??
          (_modificarGrupoProductoEnvasadoComando = new RelayCommand(
              param => ModificarGrupoProductoEnvasado(),
              param => GrupoProductoEnvasadoSeleccionado != null
          ));

        private async void ModificarGrupoProductoEnvasado()
        {
            var formTipo = new FormTipo("Editar Grupo PT");
            formTipo.vNombreUnico.Atributo = "Nombre";
            formTipo.vNombreUnico.Tipo = "GrupoProductoTerminado";
            formTipo.vNombreUnico.NombreActual = GrupoProductoEnvasadoSeleccionado.Nombre;
            formTipo.vNombreLongitud.Min = Constantes.LONG_MIN_NOMBRE_GRUPO;
            formTipo.vNombreLongitud.Max = Constantes.LONG_MAX_NOMBRE_GRUPO;
            formTipo.vDescripcionLongitud.Min = Constantes.LONG_MIN_DESCRIPCION_GRUPO;
            formTipo.vDescripcionLongitud.Max = Constantes.LONG_MAX_DESCRIPCION_GRUPO;

            formTipo.Nombre = GrupoProductoEnvasadoSeleccionado.Nombre;
            formTipo.Descripcion = GrupoProductoEnvasadoSeleccionado.Descripcion;

            if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
            {
                GrupoProductoEnvasadoSeleccionado.Nombre = formTipo.Nombre;
                GrupoProductoEnvasadoSeleccionado.Descripcion = formTipo.Descripcion;
                context.SaveChanges();
                CargarGruposProductosEnvasados();
            }
        }
        #endregion


        #region Añadir Tipo Producto Envasado
        public ICommand AnadirTipoProductoEnvasadoComando => _anadirTipoProductoEnvasadoComando ??
           (_anadirTipoProductoEnvasadoComando = new RelayCommand(
               param => AnadirTipoProductoEnvasado(),
               param => GrupoProductoEnvasadoSeleccionado != null
           ));

        private async void AnadirTipoProductoEnvasado()
        {
            var formTipoProducto = new FormTipoProducto();
            formTipoProducto.vNombreUnico.Atributo = "Nombre";
            formTipoProducto.vNombreUnico.Tipo = "TipoProductoTerminado";

            if ((bool)await DialogHost.Show(formTipoProducto, "RootDialog"))
            {
                context.TiposProductosEnvasados.Add(new TipoProductoEnvasado()
                {
                    Nombre = formTipoProducto.Nombre,
                    Descripcion = formTipoProducto.Descripcion,
                    MedidoEnVolumen = formTipoProducto.lbMedido.SelectedIndex == 0,
                    MedidoEnUnidades = formTipoProducto.lbMedido.SelectedIndex == 1,
                    GrupoId = GrupoProductoEnvasadoSeleccionado.GrupoProductoEnvasadoId
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
                Mensaje = "¿Está seguro de que desea borrar el Tipo " + TipoProductoEnvasadoSeleccionado.Nombre + "?"
            };
            if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
            {
                if (!context.ProductosEnvasados.Any(pe => pe.TipoProductoEnvasadoId == TipoProductoEnvasadoSeleccionado.TipoProductoEnvasadoId))
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
            var formTipoProducto = new FormTipoProducto("Editar Tipo P. Envasado");
            formTipoProducto.vNombreUnico.Atributo = "Nombre";
            formTipoProducto.vNombreUnico.Tipo = "TipoProductoEnvasado";
            formTipoProducto.vNombreUnico.NombreActual = TipoProductoEnvasadoSeleccionado.Nombre;

            formTipoProducto.Nombre = TipoProductoEnvasadoSeleccionado.Nombre;
            formTipoProducto.Descripcion = TipoProductoEnvasadoSeleccionado.Descripcion;
            if (TipoProductoEnvasadoSeleccionado.MedidoEnVolumen == true)
                formTipoProducto.lbMedido.SelectedIndex = 0;
            else
                formTipoProducto.lbMedido.SelectedIndex = 1;

            if ((bool)await DialogHost.Show(formTipoProducto, "RootDialog"))
            {
                TipoProductoEnvasadoSeleccionado.Nombre = formTipoProducto.Nombre;
                TipoProductoEnvasadoSeleccionado.Descripcion = formTipoProducto.Descripcion;
                TipoProductoEnvasadoSeleccionado.MedidoEnVolumen = formTipoProducto.lbMedido.SelectedIndex == 0;
                TipoProductoEnvasadoSeleccionado.MedidoEnUnidades = formTipoProducto.lbMedido.SelectedIndex == 1;
                context.SaveChanges();
                CargarTiposProductosEnvasados();
            }
        }
        #endregion

        #region Añadir Picking
        public ICommand AnadirPickingComando => _anadirPickingComando ??
           (_anadirPickingComando = new RelayCommand(
               param => AnadirPicking()
           ));

        private async void AnadirPicking()
        {
            var formPicking = new FormPicking("Nuevo Picking");
            formPicking.vNombreUnico.Atributo = "Nombre";
            formPicking.vNombreUnico.Tipo = "Picking";

            if ((bool)await DialogHost.Show(formPicking, "RootDialog"))
            {
                context.Picking.Add(new Picking()
                {
                    Nombre = formPicking.Nombre,
                    VolumenTotal = formPicking.VolumenTotal,
                    VolumenRestante = formPicking.VolumenRestante,
                    UnidadesTotales = formPicking.UnidadesTotales,
                    UnidadesRestantes = formPicking.UnidadesRestantes

                });

                context.SaveChanges();
                CargarPicking();
            }
        }
        #endregion


        #region Borrar Picking
        public ICommand BorrarPickingComando => _borrarPickingComando ??
          (_borrarPickingComando = new RelayCommand(
              param => BorrarPicking(),
              param => PickingSeleccionado != null
          ));

        private async void BorrarPicking()
        {
            var mensajeConf = new MensajeConfirmacion()
            {
                Mensaje = "¿Está seguro de que desea borrar el picking " + PickingSeleccionado.Nombre + "?"
            };
            if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
            {
                /*if (!context.ProductosEnvasados.Any(pe => pe.PickingId == PickingSeleccionado.PickingId))
                {
                    context.Picking.Remove(PickingSeleccionado);
                    context.SaveChanges();
                    CargarPicking();
                }
                else
                {
                    await DialogHost.Show(new MensajeInformacion("No puede borrar el picking debido a que está en uso."), "RootDialog");
                }*/
            }
        }
        #endregion


        #region Modificar Picking
        public ICommand ModificarPickingComando => _modificarPickingComando ??
          (_modificarPickingComando = new RelayCommand(
              param => ModificarPicking(),
              param => PickingSeleccionado != null
          ));

        private async void ModificarPicking()
        {
            var formPicking = new FormPicking("Editar Picking");
            formPicking.vNombreUnico.Atributo = "Nombre";
            formPicking.vNombreUnico.Tipo = "Picking";
            formPicking.vNombreUnico.NombreActual = PickingSeleccionado.Nombre;

            formPicking.Nombre = PickingSeleccionado.Nombre;
            formPicking.VolumenTotal = PickingSeleccionado.VolumenTotal;
            formPicking.VolumenRestante = PickingSeleccionado.VolumenRestante;
            formPicking.UnidadesTotales = PickingSeleccionado.UnidadesTotales;
            formPicking.UnidadesRestantes = PickingSeleccionado.UnidadesRestantes;

            if ((bool)await DialogHost.Show(formPicking, "RootDialog"))
            {
                PickingSeleccionado.Nombre = formPicking.Nombre;
                PickingSeleccionado.VolumenTotal = formPicking.VolumenTotal;
                PickingSeleccionado.VolumenRestante = formPicking.VolumenRestante;
                PickingSeleccionado.UnidadesTotales = formPicking.UnidadesTotales;
                PickingSeleccionado.UnidadesRestantes = formPicking.UnidadesRestantes;
                context.SaveChanges();
                CargarPicking();
            }
        }
        #endregion

    }
}
