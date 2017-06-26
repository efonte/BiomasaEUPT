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

            if (cliente.RazonSocial.Length < Constantes.LONG_MIN_RAZON_SOCIAL)
                return new ValidationResult(false, String.Format(errorMin, "razón social", Constantes.LONG_MIN_RAZON_SOCIAL));

            if (cliente.RazonSocial.Length > Constantes.LONG_MAX_RAZON_SOCIAL)
                return new ValidationResult(false, String.Format(errorMax, "razón social", Constantes.LONG_MAX_RAZON_SOCIAL));

            if (!Regex.IsMatch(cliente.RazonSocial, Constantes.REGEX_RAZON_SOCIAL))
                return new ValidationResult(false, String.Format(errorRegex, "razón social", ""));


            // NIF
            if (string.IsNullOrWhiteSpace(cliente.Nif))
                return new ValidationResult(false, String.Format(errorObligatorio, "NIF"));

            if (cliente.Nif.Length < Constantes.LONG_MIN_NIF)
                return new ValidationResult(false, String.Format(errorMin, "NIF", Constantes.LONG_MIN_NIF));

            if (cliente.Nif.Length > Constantes.LONG_MAX_NIF)
                return new ValidationResult(false, String.Format(errorMax, "NIF", Constantes.LONG_MAX_NIF));

            if (!Regex.IsMatch(cliente.Nif, Constantes.REGEX_NIF))
                return new ValidationResult(false, String.Format(errorRegex, "NIF", " (L-NNNNNNN o NNNNNNN-L)"));


            // Email
            if (string.IsNullOrWhiteSpace(cliente.Email))
                return new ValidationResult(false, String.Format(errorObligatorio, "email"));

            if (cliente.Email.Length < Constantes.LONG_MIN_EMAIL)
                return new ValidationResult(false, String.Format(errorMin, "email", Constantes.LONG_MIN_EMAIL));

            if (cliente.Email.Length > Constantes.LONG_MAX_EMAIL)
                return new ValidationResult(false, String.Format(errorMax, "email", Constantes.LONG_MAX_EMAIL));

            if (!Regex.IsMatch(cliente.Email, Constantes.REGEX_EMAIL))
                return new ValidationResult(false, String.Format(errorRegex, "email", ""));


            // Calle
            if (string.IsNullOrWhiteSpace(cliente.Calle))
                return new ValidationResult(false, String.Format(errorObligatorio, "calle"));

            if (cliente.Calle.Length < Constantes.LONG_MIN_CALLE)
                return new ValidationResult(false, String.Format(errorMin, "calle", Constantes.LONG_MIN_CALLE));

            if (cliente.Calle.Length > Constantes.LONG_MAX_CALLE)
                return new ValidationResult(false, String.Format(errorMax, "calle", Constantes.LONG_MAX_CALLE));

            if (!Regex.IsMatch(cliente.Calle, Constantes.REGEX_CALLE))
                return new ValidationResult(false, String.Format(errorRegex, "calle", ""));

            // Valores únicos
            using (var context = new BiomasaEUPTContext())
            {
                foreach (var c in context.Clientes.Where(c => c.ClienteId != cliente.ClienteId).ToList())
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
            }
            return ValidationResult.ValidResult;
        }
    }
}
