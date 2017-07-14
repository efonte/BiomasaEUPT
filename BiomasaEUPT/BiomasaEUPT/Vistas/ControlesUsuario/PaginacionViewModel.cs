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
    public class PaginacionViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<int> ItemsPorPaginaDisponibles { get; set; }
        public int ItemsPorPagina { get; set; }

        private int _itemsTotales;
        public int ItemsTotales
        {
            get { return _itemsTotales; }
            set
            {
                _itemsTotales = value;
                // PaginasTotales = (int)Math.Ceiling((double)_itemsTotales / ItemsPorPagina);
            }
        }

        private int _paginasTotales;
        public int PaginasTotales
        {
            get { return _paginasTotales; }
            set
            {
                _paginasTotales = value;
                // PaginaSeleccionada = _paginasTotales < PaginaSeleccionada ? _paginasTotales : PaginaSeleccionada;
            }
        }

        public int PaginaSeleccionada { get; set; }

        public Func<int> GetItemsTotales { get; set; }
        public Action<int, int> CargarItems { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public PaginacionViewModel()
        {
            ItemsTotales = 0;
            ItemsPorPaginaDisponibles = new ObservableCollection<int>() { 10, 20, 30, 50, 100 };
            ItemsPorPagina = 10;
            // ActualizarContadores();
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
            ActualizarContadores();
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
            CargarItems(ItemsPorPagina, (PaginaSeleccionada - 1) * ItemsPorPagina);
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

        public void ActualizarContadores()
        {
            ItemsTotales = GetItemsTotales();
            PaginasTotales = (int)Math.Ceiling((double)ItemsTotales / ItemsPorPagina);
            PaginaSeleccionada = PaginasTotales < PaginaSeleccionada ? PaginasTotales : PaginaSeleccionada;
        }

        public void Refrescar()
        {
            ActualizarContadores();
            CargarItems(ItemsPorPagina, (PaginaSeleccionada - 1) * ItemsPorPagina);
        }
    }
}
