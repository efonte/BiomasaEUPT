using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Vistas.GestionClientes
{
    public class FormClienteViewModel : INotifyPropertyChanged
    {
        public string FormTitulo { get; set; }

        public ObservableCollection<TipoCliente> TiposClientes { get; set; }
        public ObservableCollection<GrupoCliente> GruposClientes { get; set; }
        public ObservableCollection<Pais> Paises { get; set; }
        public ObservableCollection<Comunidad> Comunidades { get; set; }
        public ObservableCollection<Provincia> Provincias { get; set; }
        public ObservableCollection<Municipio> Municipios { get; set; }

        public TipoCliente TipoClienteSeleccionado { get; set; }
        public GrupoCliente GrupoClienteSeleccionado { get; set; }

        private Pais _paisSeleccionado;
        public Pais PaisSeleccionado
        {
            get { return _paisSeleccionado; }
            set
            {
                _paisSeleccionado = value;
                CargarComunidades();
            }
        }

        private Comunidad _comunidadSeleccionada;
        public Comunidad ComunidadSeleccionada
        {
            get { return _comunidadSeleccionada; }
            set
            {
                _comunidadSeleccionada = value;
                CargarProvincias();
            }
        }

        private Provincia _provinciaSeleccionada;
        public Provincia ProvinciaSeleccionada
        {
            get { return _provinciaSeleccionada; }
            set
            {
                _provinciaSeleccionada = value;
                CargarMunicipios();
            }
        }

        public Municipio MunicipioSeleccionado { get; set; }

        public String RazonSocial { get; set; }
        public String Nif { get; set; }
        public String Email { get; set; }
        public String Calle { get; set; }
        public String Observaciones { get; set; }

        public BiomasaEUPTContext Context { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        public FormClienteViewModel()
        {
            FormTitulo = "Nuevo Cliente";
            Context = new BiomasaEUPTContext();
            CargarTipos();
            CargarGrupos();
            CargarPaises();
        }

        private void CargarTipos()
        {
            TiposClientes = new ObservableCollection<TipoCliente>(Context.TiposClientes.ToList());
            TipoClienteSeleccionado = TipoClienteSeleccionado ?? TiposClientes.First();
        }

        private void CargarGrupos()
        {
            GruposClientes = new ObservableCollection<GrupoCliente>(Context.GruposClientes.ToList());
            GrupoClienteSeleccionado = GrupoClienteSeleccionado ?? GruposClientes.First();
        }

        private void CargarPaises()
        {
            using (new CursorEspera())
            {
                Paises = new ObservableCollection<Pais>(Context.Paises.ToList());
                PaisSeleccionado = PaisSeleccionado ?? Paises.First();
            }
        }

        private void CargarComunidades()
        {
            if (PaisSeleccionado != null)
            {
                using (new CursorEspera())
                {
                    Comunidades = new ObservableCollection<Comunidad>(Context.Comunidades.Where(d => d.PaisId == PaisSeleccionado.PaisId).ToList());
                    ComunidadSeleccionada = ComunidadSeleccionada ?? Comunidades.First();
                }
            }
        }

        private void CargarProvincias()
        {
            if (ComunidadSeleccionada != null)
            {
                using (new CursorEspera())
                {
                    Provincias = new ObservableCollection<Provincia>(Context.Provincias.Where(d => d.ComunidadId == ComunidadSeleccionada.ComunidadId).ToList());
                    ProvinciaSeleccionada = ProvinciaSeleccionada ?? Provincias.First();
                }
            }
        }

        private void CargarMunicipios()
        {
            if (ProvinciaSeleccionada != null)
            {
                using (new CursorEspera())
                {
                    Municipios = new ObservableCollection<Municipio>(Context.Municipios.Where(d => d.ProvinciaId == ProvinciaSeleccionada.ProvinciaId).ToList());
                    MunicipioSeleccionado = MunicipioSeleccionado ?? Municipios.First();
                }
            }
        }
    }
}
