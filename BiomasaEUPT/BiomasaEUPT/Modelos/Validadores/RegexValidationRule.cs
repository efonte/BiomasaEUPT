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
        private string _mensaje;

        public string ExpReg
        {
            get { return _expReg; }
            set { _expReg = value; }
        }
        public string Mensaje
        {
            get { return _mensaje; }
            set { _mensaje = value; }
        }


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (!string.IsNullOrEmpty(ExpReg))
            {
                if (!Regex.IsMatch((string)value, ExpReg))
                {
                    return new ValidationResult(false, Mensaje);
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
