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
    public class UsuarioValidationRule : ValidationRule
    {
        private int minNombre = 3;
        private int maxNombre = 10;
        private string regexNombre = "^[a-z]+$";
        private int minEmail = 5;
        private int maxEmail = 254;
        private string regexEmail = "^[a-zA-Z][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$";

        private string errorObligatorio = "El campo {0} es obligatorio.";
        private string errorMin = "La longitud del campo {0} es menor de {1} carácteres.";
        private string errorMax = "La longitud del campo {0} es mayor de {1} carácteres.";
        private string errorUnico = "El campo {0} debe ser único.";
        private string errorRegex = "El campo {0} no tiene formato válido{1}.";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            Usuario usuario = (value as BindingGroup).Items[0] as Usuario;

            // Nombre
            if (string.IsNullOrWhiteSpace(usuario.Nombre))
                return new ValidationResult(false, String.Format(errorObligatorio, "nombre"));

            if (usuario.Nombre.Length < minNombre)
                return new ValidationResult(false, String.Format(errorMin, "nombre", minNombre));

            if (usuario.Nombre.Length > maxNombre)
                return new ValidationResult(false, String.Format(errorMax, "nombre", maxNombre));

            if (!Regex.IsMatch(usuario.Nombre, regexNombre))
                return new ValidationResult(false, "El campo nombre sólo acepta letras minúsculas.");


            // Email
            if (string.IsNullOrWhiteSpace(usuario.Email))
                return new ValidationResult(false, String.Format(errorObligatorio, "email"));

            if (usuario.Email.Length < minEmail)
                return new ValidationResult(false, String.Format(errorMin, "email", minEmail));

            if (usuario.Email.Length > maxEmail)
                return new ValidationResult(false, String.Format(errorMax, "email", maxEmail));

            if (!Regex.IsMatch(usuario.Email, regexEmail))
                return new ValidationResult(false, String.Format(errorRegex, "email"));

            // Valores únicos
            using (var context = new BiomasaEUPTContext())
            {
                foreach (var u in context.Usuarios.ToList())
                {
                    if (u.GetHashCode() != usuario.GetHashCode())
                    {
                        if (u.Nombre == usuario.Nombre)
                            return new ValidationResult(false, String.Format(errorUnico, "nombre"));

                        if (u.Email == usuario.Email)
                            return new ValidationResult(false, String.Format(errorUnico, "email"));
                    }
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
