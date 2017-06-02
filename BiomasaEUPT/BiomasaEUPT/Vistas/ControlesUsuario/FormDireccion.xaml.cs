using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
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

namespace BiomasaEUPT.Vistas.ControlesUsuario
{
    /// <summary>
    /// Lógica de interacción para FormDireccion.xaml
    /// </summary>
    public partial class FormDireccion : UserControl
    {
        private BiomasaEUPTContext context;
        private CollectionViewSource paisesViewSource;
        private CollectionViewSource comunidadesViewSource;
        private CollectionViewSource provinciasViewSource;
        private CollectionViewSource municipiosViewSource;

        public FormDireccion()
        {
            InitializeComponent();
            DataContext = this;
            context = new BiomasaEUPTContext();
            paisesViewSource = ((CollectionViewSource)(FindResource("paisesViewSource")));
            comunidadesViewSource = ((CollectionViewSource)(FindResource("comunidadesViewSource")));
            provinciasViewSource = ((CollectionViewSource)(FindResource("provinciasViewSource")));
            municipiosViewSource = ((CollectionViewSource)(FindResource("municipiosViewSource")));
            context.Paises.Load();
            paisesViewSource.Source = context.Paises.Local;
        }

        private void cbPaises_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comunidadesViewSource.Source = context.Comunidades.Where(d => d.PaisId == ((Pais)cbPaises.SelectedItem).PaisId).ToList();
        }

        private void cbComunidades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            provinciasViewSource.Source = context.Provincias.Where(d => d.ComunidadId == ((Comunidad)cbComunidades.SelectedItem).ComunidadId).ToList();
        }

        private void cbProvincias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            municipiosViewSource.Source = context.Municipios.Where(d => d.ProvinciaId == ((Provincia)cbProvincias.SelectedItem).ProvinciaId).ToList();
        }
    }
}
