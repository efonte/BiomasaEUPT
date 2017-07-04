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

namespace BiomasaEUPT.Vistas.GestionProveedores
{
    /// <summary>
    /// Lógica de interacción para FormProveedor.xaml
    /// </summary>
    public partial class FormProveedor : UserControl
    {
        private BiomasaEUPTContext context;
        private CollectionViewSource tiposProveedoresViewSource;
        private CollectionViewSource paisesViewSource;
        private CollectionViewSource comunidadesViewSource;
        private CollectionViewSource provinciasViewSource;
        private CollectionViewSource municipiosViewSource;
        public String RazonSocial { get; set; }
        public String Nif { get; set; }
        public String Email { get; set; }
        public String Calle { get; set; }
        public String Observaciones { get; set; }

        public FormProveedor()
        {
            InitializeComponent();
            DataContext = this;
            context = new BiomasaEUPTContext();
            tiposProveedoresViewSource = ((CollectionViewSource)(FindResource("tiposProveedoresViewSource")));
            paisesViewSource = ((CollectionViewSource)(FindResource("paisesViewSource")));
            comunidadesViewSource = ((CollectionViewSource)(FindResource("comunidadesViewSource")));
            provinciasViewSource = ((CollectionViewSource)(FindResource("provinciasViewSource")));
            municipiosViewSource = ((CollectionViewSource)(FindResource("municipiosViewSource")));
        }

        public FormProveedor(Proveedor proveedor) : this()
        {
            gbTitulo.Header = "Editar Proveedor";

            RazonSocial = proveedor.RazonSocial;
            vUnicoRazonSocial.NombreActual = RazonSocial;
            Nif = proveedor.Nif;
            vUnicoNif.NombreActual = Nif;
            Email = proveedor.Email;
            vUnicoEmail.NombreActual = Email;
            cbTiposProveedores.SelectedValue = proveedor.TipoId;
            var provincia = context.Provincias.Single(p => p.ProvinciaId == proveedor.Municipio.ProvinciaId);
            var comunidad = context.Comunidades.Single(c => c.ComunidadId == provincia.ComunidadId);
            var pais = context.Paises.Single(p => p.PaisId == comunidad.PaisId);
            cbPaises.SelectedItem = pais;
            cbComunidades.SelectedItem = comunidad;
            cbProvincias.SelectedItem = provincia;
            cbMunicipios.SelectedItem = proveedor.Municipio;
            Calle = proveedor.Calle;
            Observaciones = proveedor.Observaciones;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (new CursorEspera())
            {
                context.TiposProveedores.Load();
                context.Paises.Load();
                tiposProveedoresViewSource.Source = context.TiposProveedores.Local;
                paisesViewSource.Source = context.Paises.Local;
            }
        }

        private void cbPaises_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (new CursorEspera())
            {
                comunidadesViewSource.Source = context.Comunidades.Where(d => d.PaisId == ((Pais)cbPaises.SelectedItem).PaisId).ToList();
            }
        }

        private void cbComunidades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (new CursorEspera())
            {
                provinciasViewSource.Source = context.Provincias.Where(d => d.ComunidadId == ((Comunidad)cbComunidades.SelectedItem).ComunidadId).ToList();
            }
        }

        private void cbProvincias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (new CursorEspera())
            {
                municipiosViewSource.Source = context.Municipios.Where(d => d.ProvinciaId == ((Provincia)cbProvincias.SelectedItem).ProvinciaId).ToList();
            }
        }
    }
}
