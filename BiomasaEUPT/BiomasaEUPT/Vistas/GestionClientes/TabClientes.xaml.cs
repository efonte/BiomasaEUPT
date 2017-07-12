using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using BiomasaEUPT.Vistas.ControlesUsuario;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
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

namespace BiomasaEUPT.Vistas.GestionClientes
{
    /// <summary>
    /// Lógica de interacción para TabClientes.xaml
    /// </summary>
    public partial class TabClientes : UserControl
    {
        private TabClientesViewModel viewModel;

        public TabClientes()
        {
            InitializeComponent();

            viewModel = new TabClientesViewModel();
            DataContext = viewModel;

            ucFiltroTabla.DataContext = viewModel.FiltroTablaViewModel;

            ucFiltroTabla.lbFiltroTipo.SelectionChanged += (s, e1) => { FiltrarTabla(); };
            ucTablaClientes.cbRazonSocial.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaClientes.cbRazonSocial.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaClientes.cbNif.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaClientes.cbNif.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaClientes.cbEmail.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaClientes.cbEmail.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaClientes.cbCalle.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaClientes.cbCalle.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaClientes.cbCodigoPostal.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaClientes.cbCodigoPostal.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaClientes.cbMunicipio.Checked += (s, e1) => { FiltrarTabla(); };
            ucTablaClientes.cbMunicipio.Unchecked += (s, e1) => { FiltrarTabla(); };
            ucTablaClientes.bRefrescar.Click += (s, e1) => { viewModel.CargarClientes(); };
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // CargarClientes();
        }

        #region FiltroTabla
        public void FiltrarTabla()
        {
            // clientesViewSource.Filter += new FilterEventHandler(FiltroTabla);
        }

        private void FiltroTabla(object sender, FilterEventArgs e)
        {
            string textoBuscado = ucTablaClientes.tbBuscar.Text.ToLower();
            var cliente = e.Item as Cliente;
            string razonSocial = cliente.RazonSocial.ToLower();
            string nif = cliente.Nif.ToLower();
            string email = cliente.Email.ToLower();
            string calle = cliente.Calle.ToLower();
            string codigoPostal = cliente.Municipio.CodigoPostal.ToLower();
            string municipio = cliente.Municipio.Nombre.ToLower();
            string tipo = cliente.TipoCliente.Nombre.ToLower();

            var condicion = (ucTablaClientes.cbRazonSocial.IsChecked == true ? razonSocial.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbNif.IsChecked == true ? nif.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbEmail.IsChecked == true ? email.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbCalle.IsChecked == true ? calle.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbCodigoPostal.IsChecked == true ? codigoPostal.Contains(textoBuscado) : false) ||
                             (ucTablaClientes.cbMunicipio.IsChecked == true ? municipio.Contains(textoBuscado) : false);

            // Filtra todos
            if (ucFiltroTabla.lbFiltroTipo.SelectedItems.Count == 0)
            {
                e.Accepted = condicion;
            }
            else
            {
                foreach (TipoCliente tipoCliente in ucFiltroTabla.lbFiltroTipo.SelectedItems)
                {
                    if (tipoCliente.Nombre.ToLower().Equals(tipo))
                    {
                        // Si lo encuentra en el ListBox del filtro no hace falta que siga haciendo el foreach
                        e.Accepted = condicion;
                        break;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                }
            }
        }
        #endregion    

    }
}
