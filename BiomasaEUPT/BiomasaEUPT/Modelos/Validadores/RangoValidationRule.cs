using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BiomasaEUPT.Modelos.Validadores
{
    public class RangoValidationRule : ValidationRule
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
            if (!float.TryParse(value.ToString().Replace('.', ','), out float floatValue))
                return new ValidationResult(false, "El " + campo + " no es numérico.");

            if (floatValue < Min)
            {
                return new ValidationResult(false, "El " + campo + " no puede que ser menor de " + Min + ".");
            }
            if (floatValue > Max)
            {
                return new ValidationResult(false, "El " + campo + " no puede ser mayor de " + Max + " .");
            }
            return ValidationResult.ValidResult;
        }
    }
}
