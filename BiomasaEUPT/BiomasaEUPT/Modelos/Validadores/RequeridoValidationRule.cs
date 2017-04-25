using System;
using System.Globalization;
using System.Windows.Controls;

namespace BiomasaEUPT.Modelos.Validadores
{
    public class RequeridoValidationRule : ValidationRule
    {
        private string _nombreCampo;
        public string NombreCampo { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            /* return string.IsNullOrWhiteSpace((value ?? "").ToString())
                 ? new ValidationResult(false, "El campo es requerido.")
                 : ValidationResult.ValidResult;*/

            if (string.IsNullOrEmpty((string)value))
            {
                if (string.IsNullOrEmpty(NombreCampo))
                {
                    _nombreCampo = "campo";
                }
                else
                {
                    _nombreCampo = "campo " + NombreCampo;
                }
                return new ValidationResult(false, "El " + _nombreCampo + " es obligatorio.");
            }
            return ValidationResult.ValidResult;
        }
    }
}