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
    public class ProveedorValidationRule : ValidationRule
    {
        private int minRazonSocial = 5;
        private int maxRazonSocial = 40;
        private string regexRazonSocial = "^(?!\\s)(?!.*\\s$)[\\p{L}0-9\\s'~?!\\.,@]+$";
        private int minNif = 9;
        private int maxNif = 9;
        private string regexNif = "^([A-Z]-\\d{7})|(\\d{7}-[A-Z])$";
        private int minEmail = 5;
        private int maxEmail = 254;
        private string regexEmail = "^[a-zA-Z][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$";
        private int minCalle = 5;
        private int maxCalle = 60;
        private string regexCalle = "^(?!\\s)(?!.*\\s$)[\\p{L}0-9\\s'~?!\\.,\\/]+$";
        //private int minObservaciones;
        //private int maxObservaciones;

        private string errorObligatorio = "El campo {0} es obligatorio.";
        private string errorMin = "La longitud del campo {0} es menor de {1} carácteres.";
        private string errorMax = "La longitud del campo {0} es mayor de {1} carácteres.";
        private string errorUnico = "El campo {0} debe ser único.";
        private string errorRegex = "El campo {0} no tiene formato válido{1}.";


        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Proveedor proveedor = (value as BindingGroup).Items[0] as Proveedor;

            // Razón Social
            if (string.IsNullOrWhiteSpace(proveedor.RazonSocial))
                return new ValidationResult(false, String.Format(errorObligatorio, "razón social"));

            if (proveedor.RazonSocial.Length < minRazonSocial)
                return new ValidationResult(false, String.Format(errorMin, "razón social", minRazonSocial));

            if (proveedor.RazonSocial.Length > maxRazonSocial)
                return new ValidationResult(false, String.Format(errorMax, "razón social", maxRazonSocial));

            if (!Regex.IsMatch(proveedor.RazonSocial, regexRazonSocial))
                return new ValidationResult(false, String.Format(errorRegex, "razón social"));


            // NIF
            if (string.IsNullOrWhiteSpace(proveedor.Nif))
                return new ValidationResult(false, String.Format(errorObligatorio, "NIF"));

            if (proveedor.Nif.Length < minNif)
                return new ValidationResult(false, String.Format(errorMin, "NIF", minRazonSocial));

            if (proveedor.Nif.Length > maxNif)
                return new ValidationResult(false, String.Format(errorMax, "NIF", maxRazonSocial));

            if (!Regex.IsMatch(proveedor.Nif, regexNif))
                return new ValidationResult(false, String.Format(errorRegex, "NIF", " (L-NNNNNNN o NNNNNNN-L)"));


            // Email
            if (string.IsNullOrWhiteSpace(proveedor.Email))
                return new ValidationResult(false, String.Format(errorObligatorio, "email"));

            if (proveedor.Email.Length < minEmail)
                return new ValidationResult(false, String.Format(errorMin, "email", minEmail));

            if (proveedor.Email.Length > maxEmail)
                return new ValidationResult(false, String.Format(errorMax, "email", maxEmail));

            if (!Regex.IsMatch(proveedor.Email, regexEmail))
                return new ValidationResult(false, String.Format(errorRegex, "email"));


            // Calle
            if (string.IsNullOrWhiteSpace(proveedor.Calle))
                return new ValidationResult(false, String.Format(errorObligatorio, "calle"));

            if (proveedor.Calle.Length < minCalle)
                return new ValidationResult(false, String.Format(errorMin, "calle", minCalle));

            if (proveedor.Calle.Length > maxCalle)
                return new ValidationResult(false, String.Format(errorMax, "calle", maxCalle));

            if (!Regex.IsMatch(proveedor.Calle, regexCalle))
                return new ValidationResult(false, String.Format(errorRegex, "calle"));


            // Valores únicos
            using (var context = new BiomasaEUPTContext())
            {
                foreach (var c in context.Proveedores.ToList())
                {
                    if (c.GetHashCode() != proveedor.GetHashCode())
                    {
                        if (c.RazonSocial == proveedor.RazonSocial)
                            return new ValidationResult(false, String.Format(errorUnico, "razón social"));

                        if (c.Nif == proveedor.Nif)
                            return new ValidationResult(false, String.Format(errorUnico, "NIF"));

                        if (c.Email == proveedor.Email)
                            return new ValidationResult(false, String.Format(errorUnico, "email"));
                    }
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
