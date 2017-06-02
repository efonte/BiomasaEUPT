using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace BiomasaEUPT.Modelos.Validadores
{
    public class FechaValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            /*return DateTime.TryParse((value ?? "").ToString(),
                CultureInfo.CurrentCulture,
                DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
                out DateTime dateTime)
                ? ValidationResult.ValidResult
                : new ValidationResult(false, "Fecha incorrecta.");*/
            return DateTime.TryParse((value ?? "").ToString(), out DateTime dateTime)
            ? ValidationResult.ValidResult
            : new ValidationResult(false, "Fecha incorrecta.");

        }
    }
}
