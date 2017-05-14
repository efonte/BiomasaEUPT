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
    public class UsuarioValidationRule : ValidationRule
    {
        private int minNombre = 4;
        private int maxNombre = 10;
        private string regexNombre = "^[a-z]+$";
        private int minEmail = 10;
        private int maxEmail = 254;
        private string regexEmail = "^[a-zA-Z][\\w\\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\\w\\.-]*[a-zA-Z0-9]\\.[a-zA-Z][a-zA-Z\\.]*[a-zA-Z]$";

        private string errorObligatorio = "El campo {0} es obligatorio.";
        private string errorMin = "La longitud del campo {0} es menor de {1} carácteres.";
        private string errorMax = "La longitud del campo {0} es mayor de {1} carácteres.";
        private string errorUnico = "El campo {0} debe ser único.";
        private string errorRegex = "El campo {0} no tiene formato válido{1}.";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            usuarios usuario = (value as BindingGroup).Items[0] as usuarios;

            // Nombre
            if (string.IsNullOrWhiteSpace(usuario.nombre))
                return new ValidationResult(false, String.Format(errorObligatorio, "nombre"));

            if (usuario.nombre.Length < minNombre)
                return new ValidationResult(false, String.Format(errorMin, "nombre", minNombre));

            if (usuario.nombre.Length > maxNombre)
                return new ValidationResult(false, String.Format(errorMax, "nombre", maxNombre));

            if (!Regex.IsMatch(usuario.nombre, regexNombre))
                return new ValidationResult(false, "El campo nombre sólo acepta letras minúsculas.");


            // Email
            if (string.IsNullOrWhiteSpace(usuario.email))
                return new ValidationResult(false, String.Format(errorObligatorio, "email"));

            if (usuario.email.Length < minEmail)
                return new ValidationResult(false, String.Format(errorMin, "email", minEmail));

            if (usuario.email.Length > maxEmail)
                return new ValidationResult(false, String.Format(errorMax, "email", maxEmail));

            if (!Regex.IsMatch(usuario.email, regexEmail))
                return new ValidationResult(false, String.Format(errorRegex, "email"));

            // Valores únicos
            foreach (var u in BaseDeDatos.Instancia.biomasaEUPTEntidades.usuarios.Local)
            {
                if (u.GetHashCode() != usuario.GetHashCode())
                {
                    if (u.nombre == usuario.nombre)
                        return new ValidationResult(false, String.Format(errorUnico, "nombre"));

                    if (u.email == usuario.email)
                        return new ValidationResult(false, String.Format(errorUnico, "email"));
                }
            }

            return ValidationResult.ValidResult;
        }
    }
}
