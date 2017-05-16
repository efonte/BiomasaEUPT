using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace BiomasaEUPT.Modelos.Validadores
{
    public class UnicoValidationRule : ValidationRule
    {
        private string _tipo;
        public string Tipo
        {
            get { return _tipo; }
            set { _tipo = value; }
        }

        private string _atributo;
        public string Atributo
        {
            get { return _atributo; }
            set { _atributo = value; }
        }

        public CollectionViewSource Coleccion { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string mensajeError = "El campo {0} debe ser único.";
            string valor = (string)value;
            if (Tipo == "usuarios")
            {
                foreach (var item in (ObservableCollection<usuarios>)Coleccion.Source)
                {
                    if (Atributo == "nombre" && item.nombre == valor)
                        return new ValidationResult(false, String.Format(mensajeError, "nombre"));

                    if (Atributo == "email" && item.email == valor)
                        return new ValidationResult(false, String.Format(mensajeError, "email"));

                }
            }
            else if (Tipo == "clientes")
            {
                foreach (var item in (ObservableCollection<clientes>)Coleccion.Source)
                {
                    if (Atributo == "razon_social" && item.razon_social == valor)
                        return new ValidationResult(false, String.Format(mensajeError, "razón social"));

                    if (Atributo == "nif" && item.nif == valor)
                        return new ValidationResult(false, String.Format(mensajeError, "NIF"));

                    if (Atributo == "email" && item.email == valor)
                        return new ValidationResult(false, String.Format(mensajeError, "email"));

                }
            }
            else if (Tipo == "tiposClientes")
            {
                foreach (var item in (ObservableCollection<tipos_clientes>)Coleccion.Source)
                {
                    if (Atributo == "nombre" && /*item.nombre.GetHashCode() != valor.GetHashCode() &&*/ item.nombre == valor)
                        return new ValidationResult(false, String.Format(mensajeError, "nombre"));

                }
            }
            else if (Tipo == "gruposClientes")
            {
                foreach (var item in (ObservableCollection<grupos_clientes>)Coleccion.Source)
                {
                    if (Atributo == "nombre" && item.nombre == valor)
                        return new ValidationResult(false, String.Format(mensajeError, "nombre"));

                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
