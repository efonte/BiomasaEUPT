using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
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
        private FormClienteViewModel viewModel;

        public FormCliente()
        {
            InitializeComponent();
            viewModel = new FormClienteViewModel();
            DataContext = viewModel;
        }

        public FormCliente(Cliente cliente) : this()
        {
            viewModel.FormTitulo = "Editar Cliente";
            viewModel.RazonSocial = cliente.RazonSocial;
            vUnicoRazonSocial.NombreActual = viewModel.RazonSocial;
            viewModel.Nif = cliente.Nif;
            vUnicoNif.NombreActual = viewModel.Nif;
            viewModel.Email = cliente.Email;
            vUnicoEmail.NombreActual = viewModel.Email;
            viewModel.TipoClienteSeleccionado = viewModel.Context.TiposClientes.Single(tc => tc.TipoClienteId == cliente.TipoId);
            viewModel.GrupoClienteSeleccionado = viewModel.Context.GruposClientes.Single(gc => gc.GrupoClienteId == cliente.GrupoId);

            var municipio = viewModel.Context.Municipios.Single(m => m.MunicipioId == cliente.Municipio.MunicipioId);
            var provincia = viewModel.Context.Provincias.Single(p => p.ProvinciaId == cliente.Municipio.ProvinciaId);
            var comunidad = viewModel.Context.Comunidades.Single(c => c.ComunidadId == provincia.ComunidadId);
            var pais = viewModel.Context.Paises.Single(p => p.PaisId == comunidad.PaisId);
            viewModel.PaisSeleccionado = pais;
            viewModel.ComunidadSeleccionada = comunidad;
            viewModel.ProvinciaSeleccionada = provincia;
            viewModel.MunicipioSeleccionado = municipio;
            viewModel.Calle = cliente.Calle;
            viewModel.Observaciones = cliente.Observaciones;
        }
    }
}
