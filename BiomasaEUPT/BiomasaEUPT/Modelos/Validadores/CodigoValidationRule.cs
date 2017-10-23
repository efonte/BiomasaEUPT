using BiomasaEUPT.Clases;
using BiomasaEUPT.Modelos.Tablas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace BiomasaEUPT.Modelos.Validadores
{
    public class CodigoValidationRule : ValidationRule
    {
        private int minNumeroCodigo = 10;
        private int maxNumeroCodigo = 10;
        private string regexCodigo = "^[3][0-9]{1,9}$";

        private string errorObligatorio = "El campo {0} es obligatorio.";
        private string errorMin = "La longitud del campo {0} es menor de {1} carácteres.";
        private string errorMax = "La longitud del campo {0} es mayor de {1} carácteres.";
        private string errorUnico = "El campo {0} debe ser único.";
        private string errorRegex = "El campo {0} no tiene formato válido{1}.";


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ProductoEnvasado productoEnvasado = (value as BindingGroup).Items[0] as ProductoEnvasado;

            // Número albarán
            if (string.IsNullOrWhiteSpace(productoEnvasado.Codigo))
                return new ValidationResult(false, String.Format(errorObligatorio, "código"));

            if (productoEnvasado.Codigo.Length < minNumeroCodigo)
                return new ValidationResult(false, String.Format(errorMin, "código", minNumeroCodigo));

            if (productoEnvasado.Codigo.Length > maxNumeroCodigo)
                return new ValidationResult(false, String.Format(errorMax, "código", maxNumeroCodigo));

            if (!Regex.IsMatch(productoEnvasado.Codigo, regexCodigo))
                return new ValidationResult(false, String.Format(errorRegex, "código", " (Ej: 2000000001, 2000000020)"));


            using (var context = new BiomasaEUPTContext())
            {
                // Valores únicos
                foreach (var c in context.ProductosEnvasados.Where(pe => pe.ProductoEnvasadoId == productoEnvasado.ProductoEnvasadoId).ToList())
                {
                    if (c.GetHashCode() != productoEnvasado.GetHashCode())
                    {
                        if (c.Codigo == productoEnvasado.Codigo)
                            return new ValidationResult(false, String.Format(errorUnico, "código"));

                    }
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
