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
        public Pais PaisSeleccionado { get; set; }

        public ObservableCollection<Comunidad> Comunidades { get; set; }
        public Comunidad ComunidadSeleccionada { get; set; }

        public ObservableCollection<Provincia> Provincias { get; set; }
        public Provincia ProvinciaSeleccionada { get; set; }

        public ObservableCollection<Municipio> Municipios { get; set; }
        public Municipio MunicipioSeleccionado { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand CBPaises_SelectionChangedComando => new RelayComando(param => CargarComunidades());
        public ICommand CBComunidades_SelectionChangedComando => new RelayComando(param => CargarProvincias());
        public ICommand CBProvincias_SelectionChangedComando => new RelayComando(param => CargarMunicipios());

        public FormDireccionViewModel()
        {
            CargarPaises();
            /* Binding b = new Binding("DataContext.Municipio.Provincia.Comunidad.Pais.Codig");
             b.RelativeSource = new RelativeSource(RelativeSourceMode.FindAncestor, typeof(DataGridRow), 1);
             textBlock.SetBinding(TextBlock.TextProperty, b);*/
        }

        private async void CargarPaises()
        {
            using (var context = new BiomasaEUPTContext())
            {
                Paises = new ObservableCollection<Pais>(context.Paises.ToList());
            }
        }

        private void CargarComunidades()
        {
            if (PaisSeleccionado != null)
            {
                Console.WriteLine(PaisSeleccionado.Nombre);
                using (var context = new BiomasaEUPTContext())
                {
                    Comunidades = new ObservableCollection<Comunidad>(context.Comunidades.Where(c => c.PaisId == PaisSeleccionado.PaisId).ToList());
                }
            }
        }

        private void CargarProvincias()
        {
            if (ComunidadSeleccionada != null)
            {
                Console.WriteLine(ComunidadSeleccionada.Nombre);
                using (var context = new BiomasaEUPTContext())
                {
                    Provincias = new ObservableCollection<Provincia>(context.Provincias.Where(p => p.ComunidadId == ComunidadSeleccionada.ComunidadId).ToList());
                }
            }
        }

        private void CargarMunicipios()
        {
            if (ProvinciaSeleccionada != null)
            {
                Console.WriteLine(ProvinciaSeleccionada.Nombre);
                using (var context = new BiomasaEUPTContext())
                {
                    Municipios = new ObservableCollection<Municipio>(context.Municipios.Where(m => m.ProvinciaId == ProvinciaSeleccionada.ProvinciaId).Include(m => m.Provincia.Comunidad.Pais).ToList());
                }
            }
        }
    }
}
