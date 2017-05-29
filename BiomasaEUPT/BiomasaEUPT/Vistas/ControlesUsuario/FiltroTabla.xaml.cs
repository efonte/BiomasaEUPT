using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos.Validadores;
using BiomasaEUPT.Vistas.GestionClientes;
using BiomasaEUPT.Vistas.GestionProveedores;
using BiomasaEUPT.Vistas.GestionMateriasPrimas;
using BiomasaEUPT.Vistas.GestionUsuarios;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BiomasaEUPT.Vistas.GestionRecepciones;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;

namespace BiomasaEUPT.Vistas.ControlesUsuario
{
    /// <summary>
    /// Lógica de interacción para FiltroTabla.xaml
    /// </summary>
    public partial class FiltroTabla : UserControl
    {
        private BiomasaEUPTContext context;
        private DependencyObject ucParent;
        private CollectionViewSource tiposClientesViewSource;
        private CollectionViewSource tiposProveedoresViewSource;
        private CollectionViewSource tiposMateriasPrimasViewSource;

        public FiltroTabla()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            context = BaseDeDatos.Instancia.biomasaEUPTContext;
            ucParent = Parent;
            while (!(ucParent is UserControl))
            {
                ucParent = LogicalTreeHelper.GetParent(ucParent);
            }

            // Pestaña Clientes
            if (ucParent.GetType().Equals(typeof(TabClientes)))
            {
                TabClientes tabClientes = (TabClientes)ucParent;
                tiposClientesViewSource = ((CollectionViewSource)(tabClientes.ucTablaClientes.FindResource("tiposClientesViewSource")));
                ccFiltro.Collection = tiposClientesViewSource.View;
                //   tabClientes.FiltrarTabla();
            }
            // Pestaña Proveedores
            if (ucParent.GetType().Equals(typeof(TabProveedores)))
            {
                TabProveedores tabProveedores = (TabProveedores)ucParent;
                tiposProveedoresViewSource = ((CollectionViewSource)(tabProveedores.ucTablaProveedores.FindResource("tiposProveedoresViewSource")));
                ccFiltro.Collection = tiposProveedoresViewSource.View;
                //   tabProveedores.FiltrarTabla();
            }
            // Pestaña MateriasPrimas
            if (ucParent.GetType().Equals(typeof(TabMateriasPrimas)))
            {
                TabMateriasPrimas tabMateriasPrimas = (TabMateriasPrimas)ucParent;
                tiposMateriasPrimasViewSource = ((CollectionViewSource)(tabMateriasPrimas.ucTablaMateriasPrimas.FindResource("tiposMateriasPrimasViewSource")));
                ccFiltro.Collection = tiposMateriasPrimasViewSource.View;
                //   tabMateriasPrimas.FiltrarTabla();
            }

            // Pestaña Recepciones
            if (ucParent.GetType().Equals(typeof(TabRecepciones)))
            {
                TabRecepciones tabRecepciones = (TabRecepciones)ucParent;
                tiposMateriasPrimasViewSource = ((CollectionViewSource)(tabRecepciones.ucTablaRecepciones.FindResource("tiposMateriasPrimasViewSource")));
                ccFiltro.Collection = tiposMateriasPrimasViewSource.View;
                //   tabMateriasPrimas.FiltrarTabla();
            }
        }


        private async void bAnadir_Click(object sender, RoutedEventArgs e)
        {
            var formTipo = new FormTipo();

            // Pestaña Clientes
            if (ucParent.GetType().Equals(typeof(TabClientes)))
            {
                TabClientes tabClientes = (TabClientes)ucParent;
                formTipo.vNombreUnico.Coleccion = tiposClientesViewSource;
                formTipo.vNombreUnico.Tipo = "tiposClientes";
                formTipo.vNombreUnico.Atributo = "nombre";
                if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
                {
                    context.TiposClientes.Add(new TipoCliente() { Nombre = formTipo.Nombre, Descripcion = formTipo.Descripcion });
                    context.GuardarCambios<TipoCliente>();
                }
            }

            // Pestaña Proveedores
            if (ucParent.GetType().Equals(typeof(TabProveedores)))
            {
                TabProveedores tabProveedores = (TabProveedores)ucParent;
                formTipo.vNombreUnico.Coleccion = tiposProveedoresViewSource;
                formTipo.vNombreUnico.Tipo = "tiposProveedores";
                formTipo.vNombreUnico.Atributo = "nombre";
                if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
                {
                    context.TiposProveedores.Add(new TipoProveedor() { Nombre = formTipo.Nombre, Descripcion = formTipo.Descripcion });
                    context.GuardarCambios<TipoProveedor>();
                }
            }

            // Pestaña Materias primas y Recepciones
            if (ucParent.GetType().Equals(typeof(TabMateriasPrimas)) || ucParent.GetType().Equals(typeof(TabRecepciones)))
            {
                //TabMateriasPrimas tabMateriasPrimas = (TabMateriasPrimas)ucParent;
                formTipo.vNombreUnico.Coleccion = tiposMateriasPrimasViewSource;
                formTipo.vNombreUnico.Tipo = "tiposMateriasPrimas";
                formTipo.vNombreUnico.Atributo = "nombre";
                if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
                {
                    context.TiposMateriasPrimas.Add(new TipoMateriaPrima() { Nombre = formTipo.Nombre, Descripcion = formTipo.Descripcion });
                    context.GuardarCambios<TipoMateriaPrima>();
                }
            }

        }

        private async void bEditar_Click(object sender, RoutedEventArgs e)
        {
            var formTipo = new FormTipo();

            // Pestaña Clientes
            if (ucParent.GetType().Equals(typeof(TabClientes)))
            {
                var tipoSeleccionado = lbFiltro.SelectedItem as TipoCliente;
                formTipo.Nombre = tipoSeleccionado.Nombre;
                formTipo.Descripcion = tipoSeleccionado.Descripcion;
                formTipo.vNombreUnico.Coleccion = tiposClientesViewSource;
                formTipo.vNombreUnico.Tipo = "tiposClientes";
                formTipo.vNombreUnico.Atributo = "nombre";
                formTipo.vNombreUnico.NombreActual = (lbFiltro.SelectedItem as TipoCliente).Nombre;
                if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
                {
                    tipoSeleccionado.Nombre = formTipo.Nombre;
                    tipoSeleccionado.Descripcion = formTipo.Descripcion;
                    tiposClientesViewSource.View.Refresh();
                    context.GuardarCambios<TipoCliente>();
                }
            }

            // Pestaña Proveedores
            if (ucParent.GetType().Equals(typeof(TabProveedores)))
            {
                var tipoSeleccionado = lbFiltro.SelectedItem as TipoProveedor;
                formTipo.Nombre = tipoSeleccionado.Nombre;
                formTipo.Descripcion = tipoSeleccionado.Descripcion;
                formTipo.vNombreUnico.Coleccion = tiposClientesViewSource;
                formTipo.vNombreUnico.Tipo = "tiposProveedores";
                formTipo.vNombreUnico.Atributo = "nombre";
                formTipo.vNombreUnico.NombreActual = (lbFiltro.SelectedItem as TipoProveedor).Nombre;
                if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
                {
                    tipoSeleccionado.Nombre = formTipo.Nombre;
                    tipoSeleccionado.Descripcion = formTipo.Descripcion;
                    tiposClientesViewSource.View.Refresh();
                    context.GuardarCambios<TipoProveedor>();
                }
            }

            // Pestaña Materias primas y Recepciones
            if (ucParent.GetType().Equals(typeof(TabMateriasPrimas)) || ucParent.GetType().Equals(typeof(TabRecepciones)))
            {
                var tipoSeleccionado = lbFiltro.SelectedItem as TipoMateriaPrima;
                formTipo.Nombre = tipoSeleccionado.Nombre;
                formTipo.Descripcion = tipoSeleccionado.Descripcion;
                formTipo.vNombreUnico.Coleccion = tiposMateriasPrimasViewSource;
                formTipo.vNombreUnico.Tipo = "tiposMateriasPrimas";
                formTipo.vNombreUnico.Atributo = "nombre";
                formTipo.vNombreUnico.NombreActual = (lbFiltro.SelectedItem as TipoMateriaPrima).Nombre;
                if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
                {
                    tipoSeleccionado.Nombre = formTipo.Nombre;
                    tipoSeleccionado.Descripcion = formTipo.Descripcion;
                    tiposClientesViewSource.View.Refresh();
                    context.GuardarCambios<TipoMateriaPrima>();
                }
            }

        }

        private async void bBorrar_Click(object sender, RoutedEventArgs e)
        {
            var mensajeConf = new MensajeConfirmacion();

            // Pestaña Clientes
            if (ucParent.GetType().Equals(typeof(TabClientes)))
            {
                var tipoSeleccionado = lbFiltro.SelectedItem as TipoCliente;
                mensajeConf.Mensaje = "¿Está seguro de que desea borrar el tipo " + tipoSeleccionado.Nombre + "?";
                if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
                {
                    if (context.Clientes.Where(t => t.TipoId == tipoSeleccionado.TipoClienteId).Count() == 0)
                    {
                        context.TiposClientes.Remove(tipoSeleccionado);
                        context.GuardarCambios<TipoCliente>();
                    }
                    else
                    {
                        await DialogHost.Show(new MensajeInformacion("No puede borrar el tipo debido a que está en uso"), "RootDialog");
                    }
                }
            }

            // Pestaña Proveedores
            if (ucParent.GetType().Equals(typeof(TabProveedores)))
            {
                var tipoSeleccionado = lbFiltro.SelectedItem as TipoProveedor;
                mensajeConf.Mensaje = "¿Está seguro de que desea borrar el tipo " + tipoSeleccionado.Nombre + "?";
                if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
                {
                    if (context.Clientes.Where(t => t.TipoId == tipoSeleccionado.TipoProveedorId).Count() == 0)
                    {
                        context.TiposProveedores.Remove(tipoSeleccionado);
                        context.GuardarCambios<TipoProveedor>();
                    }
                    else
                    {
                        await DialogHost.Show(new MensajeInformacion("No puede borrar el tipo debido a que está en uso"), "RootDialog");
                    }
                }
            }

            // Pestaña Materias primas y Recepciones
            if (ucParent.GetType().Equals(typeof(TabMateriasPrimas)) || ucParent.GetType().Equals(typeof(TabRecepciones)))
            {
                var tipoSeleccionado = lbFiltro.SelectedItem as TipoMateriaPrima;
                mensajeConf.Mensaje = "¿Está seguro de que desea borrar el tipo " + tipoSeleccionado.Nombre + "?";
                if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
                {
                    if (context.Clientes.Where(t => t.TipoId == tipoSeleccionado.TipoMateriaPrimaId).Count() == 0)
                    {
                        context.TiposMateriasPrimas.Remove(tipoSeleccionado);
                        context.GuardarCambios<TipoMateriaPrima>();
                    }
                    else
                    {
                        await DialogHost.Show(new MensajeInformacion("No puede borrar el tipo debido a que está en uso"), "RootDialog");
                    }
                }
            }

        }

    }
}
