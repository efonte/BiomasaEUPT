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

namespace BiomasaEUPT.Vistas.GestionProveedores
{
    public class FormProveedorViewModel : INotifyPropertyChanged
    {
        public string FormTitulo { get; set; }

        public ObservableCollection<TipoProveedor> TiposProveedores { get; set; }
        public ObservableCollection<Pais> Paises { get; set; }
        public ObservableCollection<Comunidad> Comunidades { get; set; }
        public ObservableCollection<Provincia> Provincias { get; set; }
        public ObservableCollection<Municipio> Municipios { get; set; }

        public TipoProveedor TipoProveedorSeleccionado { get; set; }

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

        public string RazonSocial { get; set; }
        public string Nif { get; set; }
        public string Email { get; set; }
        public string Calle { get; set; }

        private string _observaciones;
        public string Observaciones
        {
            get => _observaciones;
            set
            {
                // Si las observaciones es cadena vacía hay que asignarle el valor null
                _observaciones = value == "" ? null : value;
            }
        }

        public BiomasaEUPTContext Context { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;


        public FormProveedorViewModel()
        {
            FormTitulo = "Nuevo Proveedor";
            Context = new BiomasaEUPTContext();
            CargarTipos();
            CargarPaises();
        }

        private void CargarTipos()
        {
            TiposProveedores = new ObservableCollection<TipoProveedor>(Context.TiposProveedores.ToList());
            TipoProveedorSeleccionado = TipoProveedorSeleccionado ?? TiposProveedores.First();
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
