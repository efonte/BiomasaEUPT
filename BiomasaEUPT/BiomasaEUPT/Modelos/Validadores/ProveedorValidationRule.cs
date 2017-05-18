using BiomasaEUPT.Clases;
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
            proveedores proveedor = (value as BindingGroup).Items[0] as proveedores;

            // Razón Social
            if (string.IsNullOrWhiteSpace(proveedor.razon_social))
                return new ValidationResult(false, String.Format(errorObligatorio, "razón social"));

            if (proveedor.razon_social.Length < minRazonSocial)
                return new ValidationResult(false, String.Format(errorMin, "razón social", minRazonSocial));

            if (proveedor.razon_social.Length > maxRazonSocial)
                return new ValidationResult(false, String.Format(errorMax, "razón social", maxRazonSocial));

            if (!Regex.IsMatch(proveedor.razon_social, regexRazonSocial))
                return new ValidationResult(false, String.Format(errorRegex, "razón social"));


            // NIF
            if (string.IsNullOrWhiteSpace(proveedor.nif))
                return new ValidationResult(false, String.Format(errorObligatorio, "NIF"));

            if (proveedor.nif.Length < minNif)
                return new ValidationResult(false, String.Format(errorMin, "NIF", minRazonSocial));

            if (proveedor.nif.Length > maxNif)
                return new ValidationResult(false, String.Format(errorMax, "NIF", maxRazonSocial));

            if (!Regex.IsMatch(proveedor.nif, regexNif))
                return new ValidationResult(false, String.Format(errorRegex, "NIF", " (L-NNNNNNN o NNNNNNN-L)"));


            // Email
            if (string.IsNullOrWhiteSpace(proveedor.email))
                return new ValidationResult(false, String.Format(errorObligatorio, "email"));

            if (proveedor.email.Length < minEmail)
                return new ValidationResult(false, String.Format(errorMin, "email", minEmail));

            if (proveedor.email.Length > maxEmail)
                return new ValidationResult(false, String.Format(errorMax, "email", maxEmail));

            if (!Regex.IsMatch(proveedor.email, regexEmail))
                return new ValidationResult(false, String.Format(errorRegex, "email"));


            // Calle
            if (string.IsNullOrWhiteSpace(proveedor.calle))
                return new ValidationResult(false, String.Format(errorObligatorio, "calle"));

            if (proveedor.calle.Length < minCalle)
                return new ValidationResult(false, String.Format(errorMin, "calle", minCalle));

            if (proveedor.calle.Length > maxCalle)
                return new ValidationResult(false, String.Format(errorMax, "calle", maxCalle));

            if (!Regex.IsMatch(proveedor.calle, regexCalle))
                return new ValidationResult(false, String.Format(errorRegex, "calle"));


            // Valores únicos
            foreach (var c in BaseDeDatos.Instancia.biomasaEUPTEntidades.proveedores.Local)
            {
                if (c.GetHashCode() != proveedor.GetHashCode())
                {
                    if (c.razon_social == proveedor.razon_social)
                        return new ValidationResult(false, String.Format(errorUnico, "razón social"));

                    if (c.nif == proveedor.nif)
                        return new ValidationResult(false, String.Format(errorUnico, "NIF"));

                    if (c.email == proveedor.email)
                        return new ValidationResult(false, String.Format(errorUnico, "email"));
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
