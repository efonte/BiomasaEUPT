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
    public class ClienteValidationRule : ValidationRule
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
            Cliente cliente = (value as BindingGroup).Items[0] as Cliente;

            // Razón Social
            if (string.IsNullOrWhiteSpace(cliente.RazonSocial))
                return new ValidationResult(false, String.Format(errorObligatorio, "razón social"));

            if (cliente.RazonSocial.Length < minRazonSocial)
                return new ValidationResult(false, String.Format(errorMin, "razón social", minRazonSocial));

            if (cliente.RazonSocial.Length > maxRazonSocial)
                return new ValidationResult(false, String.Format(errorMax, "razón social", maxRazonSocial));

            if (!Regex.IsMatch(cliente.RazonSocial, regexRazonSocial))
                return new ValidationResult(false, String.Format(errorRegex, "razón social"));


            // NIF
            if (string.IsNullOrWhiteSpace(cliente.Nif))
                return new ValidationResult(false, String.Format(errorObligatorio, "NIF"));

            if (cliente.Nif.Length < minNif)
                return new ValidationResult(false, String.Format(errorMin, "NIF", minRazonSocial));

            if (cliente.Nif.Length > maxNif)
                return new ValidationResult(false, String.Format(errorMax, "NIF", maxRazonSocial));

            if (!Regex.IsMatch(cliente.Nif, regexNif))
                return new ValidationResult(false, String.Format(errorRegex, "NIF", " (L-NNNNNNN o NNNNNNN-L)"));


            // Email
            if (string.IsNullOrWhiteSpace(cliente.Email))
                return new ValidationResult(false, String.Format(errorObligatorio, "email"));

            if (cliente.Email.Length < minEmail)
                return new ValidationResult(false, String.Format(errorMin, "email", minEmail));

            if (cliente.Email.Length > maxEmail)
                return new ValidationResult(false, String.Format(errorMax, "email", maxEmail));

            if (!Regex.IsMatch(cliente.Email, regexEmail))
                return new ValidationResult(false, String.Format(errorRegex, "email"));


            // Calle
            if (string.IsNullOrWhiteSpace(cliente.Calle))
                return new ValidationResult(false, String.Format(errorObligatorio, "calle"));

            if (cliente.Calle.Length < minCalle)
                return new ValidationResult(false, String.Format(errorMin, "calle", minCalle));

            if (cliente.Calle.Length > maxCalle)
                return new ValidationResult(false, String.Format(errorMax, "calle", maxCalle));

            if (!Regex.IsMatch(cliente.Calle, regexCalle))
                return new ValidationResult(false, String.Format(errorRegex, "calle"));


            // Valores únicos
            foreach (var c in BaseDeDatos.Instancia.biomasaEUPTContext.Clientes.Local)
            {
                if (c.GetHashCode() != cliente.GetHashCode())
                {
                    if (c.RazonSocial == cliente.RazonSocial)
                        return new ValidationResult(false, String.Format(errorUnico, "razón social"));

                    if (c.Nif == cliente.Nif)
                        return new ValidationResult(false, String.Format(errorUnico, "NIF"));

                    if (c.Email == cliente.Email)
                        return new ValidationResult(false, String.Format(errorUnico, "email"));
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
