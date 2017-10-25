using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace BiomasaEUPT.Vistas.GestionVentas
{
    public class FormPedidoDetalleViewModel : INotifyPropertyChanged, IDataErrorInfo
    {
        public TipoProductoEnvasado TipoProductoEnvasado { get; set; }
        public ProductoEnvasado ProductoEnvasado { get; set; }
        public ObservableCollection<TipoProductoEnvasado> TiposProductosEnvasadosDisponibles { get; set; }
        public ObservableCollection<PedidoDetalle> PedidosDetalles { get; set; }
        public ObservableCollection<ProductoEnvasado> ProductosEnvasados { get; set; }

        public int? Unidades { get; set; }
        public double? Volumen { get; set; }
        public string CantidadHint { get; set; }
        public double Cantidad { get; set; }
        public string Codigo { get; set; }

        public bool QuedaCantidadPorAlmacenar { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public FormPedidoDetalleViewModel()
        {
            TiposProductosEnvasadosDisponibles = new ObservableCollection<TipoProductoEnvasado>();
            PedidosDetalles = new ObservableCollection<PedidoDetalle>();
            ProductosEnvasados = new ObservableCollection<ProductoEnvasado>();
        }

        #region Validación Códigos
        string IDataErrorInfo.Error { get { return Validate(null); } }

        string IDataErrorInfo.this[string columnName] { get { return Validate(columnName); } }

        private string Validate(string memberName)
        {
            string error = null;

            if (memberName == "Codigo" || memberName == null)
            {
                if (Codigo == null || Codigo.Length == 0)
                {
                    error = "El campo código es obligatorio.";
                }
                else if (Codigo != null)
                {
                    error = "El campo contraseña y contraseña confirmación no son iguales.";
                }
            }

            /*if (memberName == "ContrasenaConfirmacion" || memberName == null)
            {
                if (ContrasenaConfirmacion == null || ContrasenaConfirmacion.Length == 0)
                {
                    error = "El campo contraseña confirmación es obligatorio.";
                }
                else
                {
                    // Fuerza a comprobar la validación de la propiedad Contraseña para saber si son iguales
                    OnPropertyChanged("Contrasena");
                }
            }*/

            return error;
        }
        #endregion


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
