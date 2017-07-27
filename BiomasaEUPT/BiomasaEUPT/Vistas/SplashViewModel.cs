using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Vistas
{
    public class SplashViewModel : INotifyPropertyChanged
    {
        private string _mensajeInformacion;
        public string MensajeInformacion
        {
            get { return _mensajeInformacion; }
            set
            {
                _mensajeInformacion = value;
                OnPropertyChanged();
            }
        }

        private float _progreso;
        public float Progreso
        {
            get { return _progreso; }
            set
            {
                _progreso = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public SplashViewModel()
        {

        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
