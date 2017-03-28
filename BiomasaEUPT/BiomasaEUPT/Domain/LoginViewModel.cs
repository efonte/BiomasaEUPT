using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BiomasaEUPT.Domain
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _usuario;
        //private int? _selectedValueOne;
        //private string _selectedTextTwo;

        public LoginViewModel()
        {
            //LongListToTestComboVirtualization = new List<int>(Enumerable.Range(0, 1000));

            //SelectedValueOne = LongListToTestComboVirtualization.Skip(2).First();
            //SelectedTextTwo = null;
        }

        public string Usuario
        {
            get { return _usuario; }
            set
            {
                //ValidateProperty("Usuario");
                _usuario = value;
                ValidateProperty("Usuario");
                NotifyPropertyChanged("Usuario");
                //this.MutateVerbose(ref _usuario, value, RaisePropertyChanged());
            }
        }

        /*  public int? SelectedValueOne
          {
              get { return _selectedValueOne; }
              set
              {
                  this.MutateVerbose(ref _selectedValueOne, value, RaisePropertyChanged());
              }
          }

          public string SelectedTextTwo
          {
              get { return _selectedTextTwo; }
              set
              {
                  this.MutateVerbose(ref _selectedTextTwo, value, RaisePropertyChanged());
              }
          }*/

        //public IList<int> LongListToTestComboVirtualization { get; }

        //public DemoItem DemoItem => new DemoItem("Mr. Test", null, Enumerable.Empty<DocumentationLink>());

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }

        //public ICommand IniciarSesionComando;

        //private readonly ICommand _downloadCommand = new RelayCommand(OnDownload, CanDownload);
        //public ICommand DownloadCommand { get { return _downloadCommand; } }


        public ICommand IniciarSesionComando { get { return IniciarSesionCmd; } }
        public ICommand IniciarSesionCmd = null;

        /* private void OnDownload(object parameter) {
         }
         private bool CanDownload(object parameter) {
             return HasErrors == false;
         }*/
      /*  protected Dictionary<string, List<ValidationError>> errors = new Dictionary<string, List<ValidationError>>();

        public int ErrorCount
        {
            get { return errors.Count; }
        }*/


        public bool PuedeIniciarSesion(object z)
        {
            return !string.IsNullOrEmpty(Usuario);
           // && ErrorCount == 0;
        }

        public virtual void ValidateProperty(string propertyName)
        {
            // We don't use this when relying on ValidationRules or IDataErrorInfo.
        }
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
