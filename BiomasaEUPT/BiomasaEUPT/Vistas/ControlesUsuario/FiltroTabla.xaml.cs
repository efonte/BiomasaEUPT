using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos.Validadores;
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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BiomasaEUPT.Vistas.ControlesUsuario
{
    /// <summary>
    /// Lógica de interacción para FiltroTabla.xaml
    /// </summary>
    public partial class FiltroTabla : UserControl
    {
        private BiomasaEUPTEntidades context;
        private DependencyObject ucParent;
        private CollectionViewSource tiposClientesViewSource;
        private CollectionViewSource tiposProveedoresViewSource;

        public FiltroTabla()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            context = BaseDeDatos.Instancia.biomasaEUPTEntidades;
            ucParent = Parent;
            while (!(ucParent is UserControl))
            {
                ucParent = LogicalTreeHelper.GetParent(ucParent);
            }

            // Pestaña Clientes
            if (ucParent.GetType().Equals(typeof(TabClientes)))
            {
                TabClientes tabClientes = (TabClientes)ucParent;
                tiposClientesViewSource = ((CollectionViewSource)(tabClientes.ucTablaClientes.FindResource("tipos_clientesViewSource")));
                ccFiltro.Collection = tiposClientesViewSource.View;
                //   tabClientes.FiltrarTabla();
            }
            // Pestaña Proveedores
            if (ucParent.GetType().Equals(typeof(TabProveedores)))
            {
                TabProveedores tabProveedores = (TabProveedores)ucParent;
                tiposProveedoresViewSource = ((CollectionViewSource)(tabProveedores.ucTablaProveedores.FindResource("tipos_proveedoresViewSource")));
                ccFiltro.Collection = tiposProveedoresViewSource.View;
                //   tabProveedores.FiltrarTabla();
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
                    context.tipos_clientes.Add(new tipos_clientes() { nombre = formTipo.Nombre, descripcion = formTipo.Descripcion });
                    context.GuardarCambios<tipos_clientes>();
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
                    context.tipos_proveedores.Add(new tipos_proveedores() { nombre = formTipo.Nombre, descripcion = formTipo.Descripcion });
                    context.GuardarCambios<tipos_proveedores>();
                }
            }


        }

        private async void bEditar_Click(object sender, RoutedEventArgs e)
        {
            var formTipo = new FormTipo();

            // Pestaña Clientes
            if (ucParent.GetType().Equals(typeof(TabClientes)))
            {
                var tipoSeleccionado = lbFiltro.SelectedItem as tipos_clientes;
                formTipo.Nombre = tipoSeleccionado.nombre;
                formTipo.Descripcion = tipoSeleccionado.descripcion;
                formTipo.vNombreUnico.Coleccion = tiposClientesViewSource;
                formTipo.vNombreUnico.Tipo = "tiposClientes";
                formTipo.vNombreUnico.Atributo = "nombre";
                if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
                {
                    tipoSeleccionado.nombre = formTipo.Nombre;
                    tipoSeleccionado.descripcion = formTipo.Descripcion;
                    tiposClientesViewSource.View.Refresh();
                    context.GuardarCambios<tipos_clientes>();
                }
            }

            // Pestaña Proveedores
            if (ucParent.GetType().Equals(typeof(TabProveedores)))
            {
                var tipoSeleccionado = lbFiltro.SelectedItem as tipos_proveedores;
                formTipo.Nombre = tipoSeleccionado.nombre;
                formTipo.Descripcion = tipoSeleccionado.descripcion;
                formTipo.vNombreUnico.Coleccion = tiposClientesViewSource;
                formTipo.vNombreUnico.Tipo = "tiposProveedores";
                formTipo.vNombreUnico.Atributo = "nombre";
                if ((bool)await DialogHost.Show(formTipo, "RootDialog"))
                {
                    tipoSeleccionado.nombre = formTipo.Nombre;
                    tipoSeleccionado.descripcion = formTipo.Descripcion;
                    tiposClientesViewSource.View.Refresh();
                    context.GuardarCambios<tipos_proveedores>();
                }
            }

        }

        private async void bBorrar_Click(object sender, RoutedEventArgs e)
        {
            var mensajeConf = new MensajeConfirmacion();

            // Pestaña Clientes
            if (ucParent.GetType().Equals(typeof(TabClientes)))
            {
                var tipoSeleccionado = lbFiltro.SelectedItem as tipos_clientes;
                mensajeConf.Mensaje = "¿Está seguro de que desea borrar el tipo " + tipoSeleccionado.nombre + "?";
                if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
                {
                    if (context.clientes.Where(t => t.tipo_id == tipoSeleccionado.id_tipo_cliente).Count() == 0)
                    {
                        context.tipos_clientes.Remove(tipoSeleccionado);
                        context.GuardarCambios<tipos_clientes>();
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
                var tipoSeleccionado = lbFiltro.SelectedItem as tipos_proveedores;
                mensajeConf.Mensaje = "¿Está seguro de que desea borrar el tipo " + tipoSeleccionado.nombre + "?";
                if ((bool)await DialogHost.Show(mensajeConf, "RootDialog"))
                {
                    if (context.clientes.Where(t => t.tipo_id == tipoSeleccionado.id_tipo_proveedor).Count() == 0)
                    {
                        context.tipos_proveedores.Remove(tipoSeleccionado);
                        context.GuardarCambios<tipos_clientes>();
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
