﻿using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.GestionClientes;
using BiomasaEUPT.Vistas.GestionProveedores;
using BiomasaEUPT.Vistas.GestionUsuarios;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace BiomasaEUPT.Vistas.ControlesUsuario
{
    public class FiltroTablaViewModel : ViewModelBase
    {
        public CollectionView TiposView { get; private set; }
        public object TipoSeleccionado { get; set; }
        public CollectionView GruposView { get; private set; }
        public object GrupoSeleccionado { get; set; }
        public DependencyObject UCParent { get; set; }
        public ViewModelBase ViewModel { get; set; }

        private ICommand _anadirTipoComando;
        private ICommand _modificarTipoComando;
        private ICommand _borrarTipoComando;
        private ICommand _anadirGrupoComando;
        private ICommand _modificarGrupoComando;
        private ICommand _borrarGrupoComando;

        public FiltroTablaViewModel()
        {

        }

        public override void Inicializar()
        {

        }


        public void CargarFiltro()
        {
            // Pestaña Usuarios
            if (ViewModel is TabUsuariosViewModel)
            {
                var tabUsuariosViewModel = ViewModel as TabUsuariosViewModel;
                TiposView = (CollectionView)CollectionViewSource.GetDefaultView(tabUsuariosViewModel.TiposUsuarios);
            }

            // Pestaña Clientes
            else if (ViewModel is TabClientesViewModel)
            {
                var tabClientesViewModel = ViewModel as TabClientesViewModel;
                tabClientesViewModel.CargarClientes();
                TiposView = (CollectionView)CollectionViewSource.GetDefaultView(tabClientesViewModel.TiposClientes);
                GruposView = (CollectionView)CollectionViewSource.GetDefaultView(tabClientesViewModel.GruposClientes);
            }

            // Pestaña Proveedores
            /*  else if (ViewModel is TabProveedoresViewModel)
                {
                    var tabProveedoresViewModel = ViewModel as TabProveedoresViewModel;
                    tabProveedoresViewModel.CargarProveedores();
                    TiposView = (CollectionView)CollectionViewSource.GetDefaultView(tabProveedoresViewModel.TiposProveedores);
                    GgruposView = (CollectionView)CollectionViewSource.GetDefaultView(tabProveedoresViewModel.GruposClientes);
                }*/
        }



        #region AñadirTipo
        public ICommand AnadirTipoComando => _anadirTipoComando ??
           (_anadirTipoComando = new RelayComando(
               param => AnadirTipo()
           ));

        private async void AnadirTipo()
        {
            var formTipo = new FormTipo();
            formTipo.vNombreUnico.Atributo = "Nombre";

            // Pestaña Clientes
            if (ViewModel is TabClientesViewModel)
            {
                // formTipo.vNombreUnico.Coleccion = tiposClientesViewSource;
                // formTipo.vNombreUnico.Tipo = "TipoCliente";
            }

            // Pestaña Proveedores
            /*  else if (ViewModel is TabProveedoresViewModel)
              {
                  formTipo.vNombreUnico.Coleccion = tiposProveedoresViewSource;
                  formTipo.vNombreUnico.Tipo = "TiposProveedor";
              }*/

            if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
            {
                using (var context = new BiomasaEUPTContext())
                {
                    if (ViewModel is TabClientesViewModel)
                    {
                        context.TiposClientes.Add(new TipoCliente()
                        {
                            Nombre = formTipo.Nombre,
                            Descripcion = formTipo.Descripcion
                        });
                        var tabClientesViewModel = ViewModel as TabClientesViewModel;
                        tabClientesViewModel.CargarClientes();
                    }
                    /*  if (ViewModel is TabProveedoresViewModel)
                      {
                          context.TiposProveedores.Add(new TipoProveedor() { Nombre = formTipo.Nombre, Descripcion = formTipo.Descripcion });
                          var tabProveedoresViewModel = ViewModel as TabProveedoresViewModel;
                      }*/
                    context.SaveChanges();
                }
                CargarFiltro();
            }
        }
        #endregion

        #region AñadirGrupo
        public ICommand AnadirGrupoComando => _anadirGrupoComando ??
           (_anadirGrupoComando = new RelayComando(
               param => AnadirGrupo()
           ));

        private async void AnadirGrupo()
        {
            var formGrupo = new FormTipo("Nuevo Grupo");
            formGrupo.vNombreUnico.Atributo = "Nombre";

            // Pestaña Clientes
            if (ViewModel is TabClientesViewModel)
            {
                // formGrupo.vNombreUnico.Coleccion = gruposClientesViewSource;
                formGrupo.vNombreUnico.Tipo = "GrupoCliente";
            }

            if ((bool)await DialogHost.Show(formGrupo, "RootDialog"))
            {
                using (var context = new BiomasaEUPTContext())
                {
                    if (ViewModel is TabClientesViewModel)
                        context.GruposClientes.Add(new GrupoCliente() { Nombre = formGrupo.Nombre, Descripcion = formGrupo.Descripcion });

                    context.SaveChanges();
                }
                CargarFiltro();
            }
        }
        #endregion

        #region BorrarTipo
        public ICommand BorrarTipoComando => _borrarTipoComando ??
          (_borrarTipoComando = new RelayComando(
              param => BorrarTipo(),
              param => TipoSeleccionado != null
          ));

        private async void BorrarTipo()
        {
            var mensajeConf = new MensajeConfirmacion();

            // Pestaña Clientes
            if (ViewModel is TabClientesViewModel)
            {
                var tipoSeleccionado = TipoSeleccionado as TipoCliente;
                mensajeConf.Mensaje = "¿Está seguro de que desea borrar el tipo " + tipoSeleccionado.Nombre + "?";
                if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
                {
                    using (var context = new BiomasaEUPTContext())
                    {
                        if (!context.Clientes.Any(t => t.TipoId == tipoSeleccionado.TipoClienteId))
                        {
                            var tipoABorrar = context.TiposClientes.Single(tc => tc.TipoClienteId == tipoSeleccionado.TipoClienteId);
                            context.TiposClientes.Remove(tipoABorrar);
                            context.SaveChanges();
                            CargarFiltro();
                        }
                        else
                        {
                            await DialogHost.Show(new MensajeInformacion("No puede borrar el tipo debido a que está en uso"), "RootDialog");
                        }
                    }                   
                }
            }

            // Pestaña Proveedores
            /* else if (ViewModel is TabProveedoresViewModel)
             {
                 var tipoSeleccionado = TipoSeleccionado as TipoProveedor;
                 mensajeConf.Mensaje = "¿Está seguro de que desea borrar el tipo " + tipoSeleccionado.Nombre + "?";
                 if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
                 {
                     using (var context = new BiomasaEUPTContext())
                     {
                         if (!context.Proveedores.Any(t => t.TipoId == tipoSeleccionado.TipoProveedorId))
                         {
                             var tipoABorrar = context.TiposProveedores.Single(tc => tc.TipoProveedorId == tipoSeleccionado.TipoProveedorId);
                             context.TiposProveedores.Remove(tipoABorrar);
                             context.SaveChanges();
                              CargarFiltro();
                         }
                         else
                         {
                             await DialogHost.Show(new MensajeInformacion("No puede borrar el tipo debido a que está en uso"), "RootDialog");
                         }
                     }
                 }
             }*/
        }
        #endregion


        #region EditarTipo
        public ICommand ModificarTipoComando
        {
            get
            {
                if (_modificarTipoComando == null)
                {
                    _modificarTipoComando = new RelayComando(
                        param => ModificarTipo(),
                        param => TipoSeleccionado != null
                    );
                }
                return _modificarTipoComando;
            }
        }

        private async void ModificarTipo()
        {
            var formTipo = new FormTipo("Editar Tipo");
            formTipo.vNombreUnico.Atributo = "Nombre";

            // Pestaña Clientes
            if (ViewModel is TabClientesViewModel)
            {
                var tipoSeleccionado = TipoSeleccionado as TipoCliente;
                formTipo.Nombre = tipoSeleccionado.Nombre;
                formTipo.Descripcion = tipoSeleccionado.Descripcion;
                var nombreViejo = formTipo.Nombre;
                formTipo.vNombreUnico.Tipo = "TipoCliente";
                formTipo.vNombreUnico.NombreActual = tipoSeleccionado.Nombre;
                if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
                {
                    tipoSeleccionado.Nombre = formTipo.Nombre;
                    tipoSeleccionado.Descripcion = formTipo.Descripcion;
                    using (var context = new BiomasaEUPTContext())
                    {
                        var tipoCliente = context.TiposClientes.Single(tc => tc.Nombre == nombreViejo);
                        tipoCliente.Nombre = formTipo.Nombre;
                        tipoCliente.Descripcion = formTipo.Descripcion;
                        context.SaveChanges();
                    }
                    CargarFiltro();
                }
            }

            // Pestaña Proveedores
            /* else if (ViewModel is TabProveedoresViewModel)
             {
                 var tipoSeleccionado = lbFiltroTipo.SelectedItem as TipoProveedor;
                 formTipo.Nombre = tipoSeleccionado.Nombre;
                 formTipo.Descripcion = tipoSeleccionado.Descripcion;
                 var nombreViejo = formTipo.Nombre;
                 formTipo.vNombreUnico.Tipo = "TipoProveedor";
                 formTipo.vNombreUnico.NombreActual = tipoSeleccionado.Nombre;
                 if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
                 {
                     tipoSeleccionado.Nombre = formTipo.Nombre;
                     tipoSeleccionado.Descripcion = formTipo.Descripcion;
                     tiposClientesViewSource.View.Refresh();
                     using (var context = new BiomasaEUPTContext())
                     {
                         var tipoProveedor = context.TiposProveedores.Single(tc => tc.Nombre == nombreViejo);
                         tipoProveedor.Nombre = formTipo.Nombre;
                         tipoProveedor.Descripcion = formTipo.Descripcion;
                         context.SaveChanges();
                     }
                     CargarFiltro();
                 }
             }*/
        }
        #endregion

        #region BorrarGrupo
        public ICommand BorrarGrupoComando
        {
            get
            {
                if (_borrarGrupoComando == null)
                {
                    _borrarGrupoComando = new RelayComando(
                        param => BorrarGrupo(),
                        param => GrupoSeleccionado != null
                    );
                }
                return _borrarGrupoComando;
            }
        }

        private async void BorrarGrupo()
        {
            var mensajeConf = new MensajeConfirmacion();

            // Pestaña Clientes
            if (ViewModel is TabClientesViewModel)
            {
                var grupoSeleccionado = GrupoSeleccionado as GrupoCliente;
                mensajeConf.Mensaje = "¿Está seguro de que desea borrar el grupo " + grupoSeleccionado.Nombre + "?";
                if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
                {
                    using (var context = new BiomasaEUPTContext())
                    {
                        if (!context.GruposClientes.Any(gc => gc.GrupoClienteId == grupoSeleccionado.GrupoClienteId))
                        {
                            var grupo = context.GruposClientes.Where(gc => gc.GrupoClienteId == grupoSeleccionado.GrupoClienteId).First();
                            context.GruposClientes.Remove(grupo);
                            context.SaveChanges();
                            CargarFiltro();
                        }
                        else
                        {
                            await DialogHost.Show(new MensajeInformacion("No puede borrar el grupo debido a que está en uso"), "RootDialog");
                        }
                    }
                }
            }
        }
        #endregion

        #region EditarGrupo
        public ICommand ModificarGrupoComando
        {
            get
            {
                if (_modificarGrupoComando == null)
                {
                    _modificarGrupoComando = new RelayComando(
                        param => ModificarGrupo(),
                        param => GrupoSeleccionado != null
                    );
                }
                return _modificarGrupoComando;
            }
        }

        private async void ModificarGrupo()
        {
            var formGrupo = new FormTipo("Editar Grupo");
            formGrupo.vNombreUnico.Atributo = "Nombre";

            // Pestaña Clientes
            if (ViewModel is TabClientesViewModel)
            {
                var grupoSeleccionado = GrupoSeleccionado as GrupoCliente;
                formGrupo.Nombre = grupoSeleccionado.Nombre;
                formGrupo.Descripcion = grupoSeleccionado.Descripcion;
                var nombreViejo = formGrupo.Nombre;
                formGrupo.vNombreUnico.Tipo = "GrupoCliente";
                formGrupo.vNombreUnico.NombreActual = grupoSeleccionado.Nombre;
                if ((bool)await DialogHost.Show(formGrupo, "RootDialog"))
                {
                    grupoSeleccionado.Nombre = formGrupo.Nombre;
                    grupoSeleccionado.Descripcion = formGrupo.Descripcion;
                    using (var context = new BiomasaEUPTContext())
                    {
                        var grupoCliente = context.GruposClientes.Single(gc => gc.Nombre == nombreViejo);
                        grupoCliente.Nombre = formGrupo.Nombre;
                        grupoCliente.Descripcion = formGrupo.Descripcion;
                        context.SaveChanges();
                    }
                    CargarFiltro();
                }
            }
        }
        #endregion

    }
}
