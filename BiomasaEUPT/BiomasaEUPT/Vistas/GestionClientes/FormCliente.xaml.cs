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
            context = BaseDeDatos.Instancia.biomasaEUPTEntidades;
            tiposClientesViewSource = ((CollectionViewSource)(FindResource("tiposClientesViewSource")));
            gruposClientesViewSource = ((CollectionViewSource)(FindResource("gruposClientesViewSource")));
            direccionesPaisViewSource = ((CollectionViewSource)(FindResource("direccionesPaisViewSource")));
            direccionesComunidadViewSource = ((CollectionViewSource)(FindResource("direccionesComunidadViewSource")));
            direccionesProvinciaViewSource = ((CollectionViewSource)(FindResource("direccionesProvinciaViewSource")));
            direccionesCodigoPostalViewSource = ((CollectionViewSource)(FindResource("direccionesCodigoPostalViewSource")));
            context.tipos_clientes.Load();
            context.grupos_clientes.Load();
            //context.direcciones.Load();
            tiposClientesViewSource.Source = context.tipos_clientes.Local;
            gruposClientesViewSource.Source = context.grupos_clientes.Local;
            //direccionesPaisViewSource.Source = context.direcciones.Local.Select(d => d.pais).Distinct();
            direccionesPaisViewSource.Source = context.direcciones.Select(d => d.pais).Distinct().ToList();
        }

        private void cbPaisesDirecciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //direccionesComunidadViewSource.Source = context.direcciones.Local.Where(d => d.pais == cbPaisesDirecciones.SelectedItem as string).Distinct();
            //direccionesCodigoPostalViewSource.Source = context.direcciones.Local.Where(d => d.pais == cbPaisesDirecciones.SelectedItem as string).Distinct();
            direccionesComunidadViewSource.Source = context.direcciones.Where(d => d.pais == (string)cbPaisesDirecciones.SelectedItem).OrderBy(d => d.pais).Select(d => d.comunidad).Distinct().ToList();
        }

        private void cbComunidadesDirecciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            direccionesProvinciaViewSource.Source = context.direcciones.Where(d => d.pais == (string)cbPaisesDirecciones.SelectedItem && d.comunidad == (string)cbComunidadesDirecciones.SelectedItem).OrderBy(d => d.provincia).Select(d => d.provincia).Distinct().ToList();
        }

        private void cbProvinciasDirecciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            direccionesCodigoPostalViewSource.Source = context.direcciones.Where(d => d.pais == (string)cbPaisesDirecciones.SelectedItem && d.comunidad == (string)cbComunidadesDirecciones.SelectedItem && d.provincia == (string)cbProvinciasDirecciones.SelectedItem).OrderBy(d => d.codigo_postal).Distinct().ToList();
        }




    }
}
