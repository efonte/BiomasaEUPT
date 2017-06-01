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
    public class RecepcionValidationRule : ValidationRule
    {
        private int minNumeroAlbaran = 3;
        private int maxNumeroAlbaran = 10;
        private string regexNumeroAlbaran = "^([A-Z]-[\\dA-Z]{1,8})$";

        private string errorObligatorio = "El campo {0} es obligatorio.";
        private string errorMin = "La longitud del campo {0} es menor de {1} carácteres.";
        private string errorMax = "La longitud del campo {0} es mayor de {1} carácteres.";
        private string errorUnico = "El campo {0} debe ser único.";
        private string errorRegex = "El campo {0} no tiene formato válido{1}.";


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Recepcion recepcion = (value as BindingGroup).Items[0] as Recepcion;

            // Número albarán
            if (string.IsNullOrWhiteSpace(recepcion.NumeroAlbaran))
                return new ValidationResult(false, String.Format(errorObligatorio, "número albarán"));

            if (recepcion.NumeroAlbaran.Length < minNumeroAlbaran)
                return new ValidationResult(false, String.Format(errorMin, "número albarán", minNumeroAlbaran));

            if (recepcion.NumeroAlbaran.Length > maxNumeroAlbaran)
                return new ValidationResult(false, String.Format(errorMax, "número albarán", maxNumeroAlbaran));

            if (!Regex.IsMatch(recepcion.NumeroAlbaran, regexNumeroAlbaran))
                return new ValidationResult(false, String.Format(errorRegex, "número albarán"," (Ej: A-13, B-15A25870)"));


            using (var context = new BiomasaEUPTContext())
            {
                // Valores únicos
                foreach (var c in context.Recepciones.Where(r => r.ProveedorId == recepcion.ProveedorId).ToList())
                {
                    if (c.GetHashCode() != recepcion.GetHashCode())
                    {
                        if (c.NumeroAlbaran == recepcion.NumeroAlbaran)
                            return new ValidationResult(false, String.Format(errorUnico, "número albarán"));

                    }
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}

