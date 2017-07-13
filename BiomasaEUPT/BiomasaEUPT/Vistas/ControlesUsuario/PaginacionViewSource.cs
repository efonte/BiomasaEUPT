using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos;
using BiomasaEUPT.Vistas.GestionElaboraciones;
using BiomasaEUPT.Vistas.GestionRecepciones;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BiomasaEUPT.Vistas.ControlesUsuario
{
    public class PaginacionViewSource : INotifyPropertyChanged
    {
        public ObservableCollection<int> ItemsPorPaginaDisponibles { get; set; }
        public int ItemsPorPagina { get; set; }
        public int ItemsTotales { get; set; } = 0;

        public int PaginaSeleccionada { get; set; }
        public int PaginasTotales { get; set; }

        public UserControl ParentUC { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public PaginacionViewSource()
        {
            ItemsPorPaginaDisponibles = new ObservableCollection<int>() { 10, 20, 30, 50, 100 };
            ItemsPorPagina = 10;
            CalcularItemsTotales();
            PaginaSeleccionada = 1;
        }

        private ICommand _paginacionComando;
        public ICommand PaginacionComando
        {
            get
            {
                if (_paginacionComando == null)
                {
                    _paginacionComando = new RelayComando(
                        param => PaginacionUC((string)param),
                        param => CanPaginacionUC((string)param)
                    );
                }
                return _paginacionComando;
            }
        }

        private void PaginacionUC(string parametro)
        {
            CalcularItemsTotales();
            switch (parametro)
            {
                case "<<":
                    PaginaSeleccionada = 1;
                    break;
                case "<":
                    PaginaSeleccionada -= 1;
                    PaginaSeleccionada = PaginaSeleccionada <= 0 ? 1 : PaginaSeleccionada;
                    break;
                case ">":
                    PaginaSeleccionada += 1;
                    PaginaSeleccionada = PaginaSeleccionada > PaginasTotales ? PaginasTotales : PaginaSeleccionada;
                    break;
                case ">>":
                    PaginaSeleccionada = PaginasTotales;
                    break;
            }
            CargarItems();
        }

        private bool CanPaginacionUC(string parametro)
        {
            switch (parametro)
            {
                case "<<":
                case "<":
                    return PaginaSeleccionada != 1;
                case ">":
                case ">>":
                    return PaginaSeleccionada != PaginasTotales;
            }
            return true;
        }


        public void CalcularItemsTotales()
        {
            using (var context = new BiomasaEUPTContext())
            {
                if (ParentUC is TabRecepciones)
                {
                    ItemsTotales = context.Recepciones.Count();
                }
                else if (ParentUC is TabElaboraciones)
                {
                    ItemsTotales = context.OrdenesElaboraciones.Count();
                }
            }
            PaginasTotales = (int)Math.Ceiling((double)ItemsTotales / ItemsPorPagina);
            PaginaSeleccionada = PaginasTotales < PaginaSeleccionada ? PaginasTotales : PaginaSeleccionada;
        }

        public void CargarItems()
        {
            // Hay que esperar a que se haya cargado la vista para que no salte excepción valor nulo
            if (ParentUC.IsLoaded)
            {
                if (ParentUC is TabRecepciones)
                {
                    //(ParentUC as TabRecepciones).CargarRecepciones(ItemsPorPagina, ItemsSaltados);
                    var tabRecepcionesViewModel = (ParentUC as TabRecepciones).DataContext as TabRecepcionesViewModel;
                    tabRecepcionesViewModel.CargarRecepciones(ItemsPorPagina, (PaginaSeleccionada - 1) * ItemsPorPagina);
                }
                else if (ParentUC is TabElaboraciones)
                {
                    //(ParentUC as TabElaboraciones).CargarElaboraciones(ItemsPorPagina, ItemsSaltados);
                }
            }
        }
    }
}
