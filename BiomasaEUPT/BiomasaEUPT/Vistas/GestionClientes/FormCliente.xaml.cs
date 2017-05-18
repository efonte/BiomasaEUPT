using BiomasaEUPT.Clases;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
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
    /// Lógica de interacción para FormCliente.xaml
    /// </summary>
    public partial class FormCliente : UserControl
    {
        private BiomasaEUPTEntidades context;
        private CollectionViewSource tiposClientesViewSource;
        private CollectionViewSource gruposClientesViewSource;
        private CollectionViewSource direccionesPaisViewSource;
        private CollectionViewSource direccionesCodigoPostalViewSource;
        public String RazonSocial { get; set; }
        public String Nif { get; set; }
        public String Email { get; set; }
        public String Tipo { get; set; }
        public String Grupo { get; set; }
        public String Calle { get; set; }
        public String Observaciones { get; set; }


        public FormCliente()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            context = BaseDeDatos.Instancia.biomasaEUPTEntidades;
            tiposClientesViewSource = ((CollectionViewSource)(FindResource("tiposClientesViewSource")));
            gruposClientesViewSource = ((CollectionViewSource)(FindResource("gruposClientesViewSource")));
            direccionesPaisViewSource = ((CollectionViewSource)(FindResource("direccionesPaisViewSource")));
            direccionesCodigoPostalViewSource = ((CollectionViewSource)(FindResource("direccionesCodigoPostalViewSource")));
            context.tipos_clientes.Load();
            context.grupos_clientes.Load();
            context.direcciones.Load();
            tiposClientesViewSource.Source = context.tipos_clientes.Local;
            gruposClientesViewSource.Source = context.grupos_clientes.Local;
            direccionesPaisViewSource.Source = context.direcciones.Local.Select(d => d.pais).Distinct();
        }

        private void cbPaisesDirecciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            direccionesCodigoPostalViewSource.Source = context.direcciones.Local.Where(d => d.pais == cbPaisesDirecciones.SelectedItem as string).Distinct();
        }
    }
}
