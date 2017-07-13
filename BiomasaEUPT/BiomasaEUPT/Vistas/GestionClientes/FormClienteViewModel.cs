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

        public Pais PaisSeleccionado { get; set; }
        public Comunidad ComunidadSeleccionada { get; set; }
        public Provincia ProvinciaSeleccionada { get; set; }
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
            TiposClientes = new ObservableCollection<TipoCliente>(Context.TiposClientes.ToList());
            GruposClientes = new ObservableCollection<GrupoCliente>(Context.GruposClientes.ToList());

            CargarPaises();
        }

        public void CargarPaises()
        {
            using (new CursorEspera())
            {
                Paises = new ObservableCollection<Pais>(Context.Paises.ToList());
                PaisSeleccionado = Paises.First();
            }
        }

        public void CargarComunidades()
        {
            if (PaisSeleccionado != null)
            {
                using (new CursorEspera())
                {
                    Comunidades = new ObservableCollection<Comunidad>(Context.Comunidades.Where(d => d.PaisId == PaisSeleccionado.PaisId).ToList());

                    ComunidadSeleccionada = Comunidades.First();
                }
            }
        }

        public void CargarProvincias()
        {
            if (ComunidadSeleccionada != null)
            {
                using (new CursorEspera())
                {
                    Provincias = new ObservableCollection<Provincia>(Context.Provincias.Where(d => d.ComunidadId == ComunidadSeleccionada.ComunidadId).ToList());

                    ProvinciaSeleccionada = Provincias.First();
                }
            }
        }

        public void CargarMunicipios()
        {
            if (ProvinciaSeleccionada != null)
            {
                using (new CursorEspera())
                {
                    Municipios = new ObservableCollection<Municipio>(Context.Municipios.Where(d => d.ProvinciaId == ProvinciaSeleccionada.ProvinciaId).ToList());

                    MunicipioSeleccionado = Municipios.First();
                }
            }
        }
    }
}
