using BiomasaEUPT.Modelos;
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
        public ProductoEnvasado ProductoEnvasado { get; set; }

        public int? Unidades { get; set; }
        public double? Volumen { get; set; }

        public string CantidadHint { get; set; }
        private double _cantidad;
        public double Cantidad
        {
            get { return _cantidad; }
            set
            {
                _cantidad = value;
                if (ProductoEnvasado != null && ProductoEnvasado.TipoProductoEnvasado.MedidoEnUnidades == true)
                {
                    Unidades = Convert.ToInt32(Cantidad);
                }
                else
                {
                    Volumen = Cantidad;

                }
            }
        }
        public string Codigo { get; set; }

        private BiomasaEUPTContext context;


        public event PropertyChangedEventHandler PropertyChanged;

        public FormPedidoDetalleViewModel()
        {
            context = new BiomasaEUPTContext();
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
                else if ((ProductoEnvasado = context.ProductosEnvasados.FirstOrDefault(pe => pe.Codigo == Codigo)) == null)
                {
                    error = "El código no existe.";
                }/*else if (ProductoEnvasado.TipoProductoEnvasado==aaa.TipoProductoEnvasado) {

                }*/
                else {
                    OnPropertyChanged("Cantidad");
                }
            }
         /*   else if (memberName == "Cantidad" || memberName == null)
            {
            if (ProductoEnvasado.)
                {
                    error = "El campo código no existe.";
                }
                else
                {
                    OnPropertyChanged("Cantidad");
                }
            }
            */


            return error;
        }
        #endregion


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
