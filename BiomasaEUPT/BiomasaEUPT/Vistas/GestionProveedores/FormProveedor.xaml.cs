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
        private FormProveedorViewModel viewModel;

        public FormProveedor()
        {
            InitializeComponent();
            viewModel = new FormProveedorViewModel();
            DataContext = viewModel;
        }

        public FormProveedor(Proveedor proveedor) : this()
        {
            viewModel.FormTitulo = "Editar Proveedor";
            viewModel.RazonSocial = proveedor.RazonSocial;
            vUnicoRazonSocial.NombreActual = viewModel.RazonSocial;
            viewModel.Nif = proveedor.Nif;
            vUnicoNif.NombreActual = viewModel.Nif;
            viewModel.Email = proveedor.Email;
            vUnicoEmail.NombreActual = viewModel.Email;
            viewModel.TipoProveedorSeleccionado = viewModel.Context.TiposProveedores.Single(tp => tp.TipoProveedorId == proveedor.TipoId);

            var municipio = viewModel.Context.Municipios.Single(m => m.MunicipioId == proveedor.Municipio.MunicipioId);
            var provincia = viewModel.Context.Provincias.Single(p => p.ProvinciaId == proveedor.Municipio.ProvinciaId);
            var comunidad = viewModel.Context.Comunidades.Single(c => c.ComunidadId == provincia.ComunidadId);
            var pais = viewModel.Context.Paises.Single(p => p.PaisId == comunidad.PaisId);
            viewModel.PaisSeleccionado = pais;
            viewModel.ComunidadSeleccionada = comunidad;
            viewModel.ProvinciaSeleccionada = provincia;
            viewModel.MunicipioSeleccionado = municipio;
            viewModel.Calle = proveedor.Calle;
            viewModel.Observaciones = proveedor.Observaciones;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

    }
}
