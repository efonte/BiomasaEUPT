using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using System;
using System.Collections.Generic;
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
        private CollectionViewSource direccionesPaisViewSource;
        private CollectionViewSource direccionesComunidadViewSource;
        private CollectionViewSource direccionesProvinciaViewSource;
        private CollectionViewSource direccionesCodigoPostalViewSource;

        public FormDireccion()
        {
            InitializeComponent();
            DataContext = this;
            context = BaseDeDatos.Instancia.biomasaEUPTContext;
            direccionesPaisViewSource = ((CollectionViewSource)(FindResource("direccionesPaisViewSource")));
            direccionesComunidadViewSource = ((CollectionViewSource)(FindResource("direccionesComunidadViewSource")));
            direccionesProvinciaViewSource = ((CollectionViewSource)(FindResource("direccionesProvinciaViewSource")));
            direccionesCodigoPostalViewSource = ((CollectionViewSource)(FindResource("direccionesCodigoPostalViewSource")));
            direccionesPaisViewSource.Source = context.Direcciones.Select(d => d.Pais).Distinct().ToList();
            Console.WriteLine("------------------------------------------------------------");
        }

        private void cbPaisesDirecciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
