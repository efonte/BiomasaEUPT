using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BiomasaEUPT.Modelos.Validadores
{
    public class RegexValidationRule : ValidationRule
    {
        private string _expReg;
        public string ExpReg
        {
            get { return _expReg; }
            set { _expReg = value; }
        }

        private string _mensajeFormato;
        public string MensajeFormato
        {
            get { return _mensajeFormato; }
            set { _mensajeFormato = value; }
        }

        private string _nombreCampo;
        public string NombreCampo
        {
            get { return _nombreCampo; }
            set { _nombreCampo = value; }
        }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!string.IsNullOrEmpty(ExpReg))
            {
                if (!Regex.IsMatch((string)value, ExpReg))
                {
                    return new ValidationResult(false, String.Format("El campo{0} no tiene formato válido{1}.", " " + NombreCampo, " " + MensajeFormato));
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
