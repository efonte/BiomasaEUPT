using BiomasaEUPT.Clases;
using BiomasaEUPT.Domain;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace BiomasaEUPT.Vistas.ControlesUsuario
{
    public class FormDireccionViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Pais> Paises { get; set; }
        public ObservableCollection<Comunidad> Comunidades { get; set; }
        public ObservableCollection<Provincia> Provincias { get; set; }
        public ObservableCollection<Municipio> Municipios { get; set; }

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

        //public Municipio MunicipioSeleccionado { get; set; }

        public BiomasaEUPTContext Context;

        public event PropertyChangedEventHandler PropertyChanged;

        // public ICommand CBPaises_SelectionChangedComando => new RelayCommand(param => CargarComunidades());
        // public ICommand CBComunidades_SelectionChangedComando => new RelayCommand(param => CargarProvincias());
        // public ICommand CBProvincias_SelectionChangedComando => new RelayCommand(param => CargarMunicipios());

        public FormDireccionViewModel()
        {

        }

        public void CargarPaises()
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

                    //MunicipioSeleccionado = MunicipioSeleccionado ?? Municipios.First();
                }
            }
        }
    }
}
