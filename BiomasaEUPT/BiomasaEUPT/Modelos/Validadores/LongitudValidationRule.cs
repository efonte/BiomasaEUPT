using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BiomasaEUPT.Modelos.Validadores
{
    public class LongitudValidationRule : ValidationRule
    {
        private int _min;
        public int Min
        {
            get { return _min; }
            set { _min = value; }
        }

        private int _max;
        public int Max
        {
            get { return _max; }
            set { _max = value; }
        }

        private string _nombreCampo;
        public string NombreCampo
        {
            get { return _nombreCampo; }
            set { _nombreCampo = value; }
        }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string campo = string.IsNullOrEmpty(NombreCampo) ? "campo" : "campo " + NombreCampo;
            string cadena = (string)value ?? string.Empty;

            if (cadena.Length < Min)
            {
                return new ValidationResult(false, "La longitud del " + campo + " es menor de " + Min + " carácteres.");
            }
            if (cadena.Length > Max)
            {
                return new ValidationResult(false, "La longitud del " + campo + " es mayor de " + Max + " carácteres.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
