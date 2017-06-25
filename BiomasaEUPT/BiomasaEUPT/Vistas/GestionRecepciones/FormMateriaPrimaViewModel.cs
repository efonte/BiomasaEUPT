using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Vistas.GestionRecepciones
{
    public class FormMateriaPrimaViewModel : INotifyPropertyChanged
    {
        public TipoMateriaPrima TipoMateriaPrima { get; set; }
        public ObservableCollection<HuecoRecepcion> HuecosRecepcionesDisponibles { get; set; }
        public ObservableCollection<HistorialHuecoRecepcion> HistorialHuecosRecepciones { get; set; }
        public int? Unidades { get; set; }
        public double? Volumen { get; set; }
        public string CantidadHint { get; set; }
        public double Cantidad { get; set; }
        //public String Codigo { get; set; }
        public String Observaciones { get; set; }
        public DateTime? FechaBaja { get; set; }
        public DateTime? HoraBaja { get; set; }
        public bool QuedaCantidadPorAlmacenar { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public FormMateriaPrimaViewModel()
        {
            HuecosRecepcionesDisponibles = new ObservableCollection<HuecoRecepcion>();
            HistorialHuecosRecepciones = new ObservableCollection<HistorialHuecoRecepcion>();
        }

    }
}
