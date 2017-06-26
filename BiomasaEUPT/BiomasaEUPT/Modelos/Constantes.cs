using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomasaEUPT.Modelos
{
    public static class Constantes
    {
        public const string CODIGO_MATERIAS_PRIMAS = "1";
        public const string CODIGO_ELABORACIONES = "2";
        public const string CODIGO_VENTAS = "3";



        public const int LONG_MIN_RAZON_SOCIAL = 5;
        public const int LONG_MAX_RAZON_SOCIAL = 40;

        public const int LONG_MIN_NIF = 10;
        public const int LONG_MAX_NIF = 10;

        public const int LONG_MIN_EMAIL = 5;
        public const int LONG_MAX_EMAIL = 254;

        public const int LONG_MIN_CALLE = 5;
        public const int LONG_MAX_CALLE = 60;

        public const int LONG_MIN_NOMBRE_USUARIO = 3;
        public const int LONG_MAX_NOMBRE_USUARIO = 10;

        public const int LONG_MIN_HASH_CONTRASENA = 64;
        public const int LONG_MAX_HASH_CONTRASENA = 64;



        public const string REGEX_RAZON_SOCIAL = @"^(?!\s)(?!.*\s$)[\p{L}0-9\s'~?!\.,@]+$";
        public const string REGEX_NIF = @"^([A-Z]-\d{7})|(\d{7}-[A-Z])$";
        public const string REGEX_EMAIL = @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$";
        public const string REGEX_CALLE = @"^(?!\s)(?!.*\s$)[\p{L}0-9\s'~?!\.,\/]+$";
        public const string REGEX_NOMBRE_USUARIO = @"^[a-z]*$";
    }
}
