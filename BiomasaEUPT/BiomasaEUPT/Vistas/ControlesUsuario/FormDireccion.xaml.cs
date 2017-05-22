using BiomasaEUPT.Clases;
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
        private BiomasaEUPTEntidades context;
        private CollectionViewSource direccionesPaisViewSource;
        private CollectionViewSource direccionesComunidadViewSource;
        private CollectionViewSource direccionesProvinciaViewSource;
        private CollectionViewSource direccionesCodigoPostalViewSource;

        public FormDireccion()
        {
            InitializeComponent();
            DataContext = this;
            context = BaseDeDatos.Instancia.biomasaEUPTEntidades;
            direccionesPaisViewSource = ((CollectionViewSource)(FindResource("direccionesPaisViewSource")));
            direccionesComunidadViewSource = ((CollectionViewSource)(FindResource("direccionesComunidadViewSource")));
            direccionesProvinciaViewSource = ((CollectionViewSource)(FindResource("direccionesProvinciaViewSource")));
            direccionesCodigoPostalViewSource = ((CollectionViewSource)(FindResource("direccionesCodigoPostalViewSource")));
            direccionesPaisViewSource.Source = context.direcciones.Select(d => d.pais).Distinct().ToList();
            Console.WriteLine("------------------------------------------------------------");
        }

        private void cbPaisesDirecciones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
