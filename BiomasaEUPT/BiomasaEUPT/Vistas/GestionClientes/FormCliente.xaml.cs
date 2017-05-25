using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
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
        private BiomasaEUPTContext context;
        private CollectionViewSource tiposClientesViewSource;
        private CollectionViewSource gruposClientesViewSource;
        private CollectionViewSource direccionesPaisViewSource;
        private CollectionViewSource direccionesComunidadViewSource;
        private CollectionViewSource direccionesProvinciaViewSource;
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
            context = BaseDeDatos.Instancia.biomasaEUPTContext;
            tiposClientesViewSource = ((CollectionViewSource)(FindResource("tiposClientesViewSource")));
            gruposClientesViewSource = ((CollectionViewSource)(FindResource("gruposClientesViewSource")));
            direccionesPaisViewSource = ((CollectionViewSource)(FindResource("direccionesPaisViewSource")));
            direccionesComunidadViewSource = ((CollectionViewSource)(FindResource("direccionesComunidadViewSource")));
            direccionesProvinciaViewSource = ((CollectionViewSource)(FindResource("direccionesProvinciaViewSource")));
            direccionesCodigoPostalViewSource = ((CollectionViewSource)(FindResource("direccionesCodigoPostalViewSource")));
            context.TiposClientes.Load();
            context.GruposClientes.Load();
            //context.Direcciones.Load();
            tiposClientesViewSource.Source = context.TiposClientes.Local;
            gruposClientesViewSource.Source = context.GruposClientes.Local;
            //direccionesPaisViewSource.Source = context.Direcciones.Local.Select(d => d.Pais).Distinct();
            direccionesPaisViewSource.Source = context.Direcciones.Select(d => d.Pais).Distinct().ToList();
        }

        private void cbPaisesDirecciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //direccionesComunidadViewSource.Source = context.Direcciones.Local.Where(d => d.Pais == cbPaisesDirecciones.SelectedItem as string).Distinct();
            //direccionesCodigoPostalViewSource.Source = context.Direcciones.Local.Where(d => d.Pais == cbPaisesDirecciones.SelectedItem as string).Distinct();
            direccionesComunidadViewSource.Source = context.Direcciones.Where(d => d.Pais == (string)cbPaisesDirecciones.SelectedItem).OrderBy(d => d.Pais).Select(d => d.Comunidad).Distinct().ToList();
        }

        private void cbComunidadesDirecciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            direccionesProvinciaViewSource.Source = context.Direcciones.Where(d => d.Pais == (string)cbPaisesDirecciones.SelectedItem && d.Comunidad == (string)cbComunidadesDirecciones.SelectedItem).OrderBy(d => d.Provincia).Select(d => d.Provincia).Distinct().ToList();
        }

        private void cbProvinciasDirecciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            direccionesCodigoPostalViewSource.Source = context.Direcciones.Where(d => d.Pais == (string)cbPaisesDirecciones.SelectedItem && d.Comunidad == (string)cbComunidadesDirecciones.SelectedItem && d.Provincia == (string)cbProvinciasDirecciones.SelectedItem).OrderBy(d => d.CodigoPostal).Distinct().ToList();
        }

    }
}
