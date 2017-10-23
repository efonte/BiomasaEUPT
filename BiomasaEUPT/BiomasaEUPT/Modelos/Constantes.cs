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


        public const string REGEX_RAZON_SOCIAL = @"^(?!\s)(?!.*\s$)[\p{L}0-9\s'~?!\.,@]+$";
        public const string REGEX_NIF = @"^([A-Z]-\d{8})|(\d{8}-[A-Z])$";
        public const string REGEX_EMAIL = @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$";
        public const string REGEX_CALLE = @"^(?!\s)(?!.*\s$)[\p{L}0-9\s'~?!\.,\/]+$";
        public const string REGEX_NOMBRE_USUARIO = @"^[a-z]*$";
        public const string REGEX_CANTIDAD = @"^\d+(\.\d+)?$";
        public const string REGEX_UNIDADES = @"^\d+$";
        public const string REGEX_VOLUMEN = @"^\d+(\.\d+)?$";
        public const string REGEX_NUMERO_ALBARAN = @"^([A-Z]-[\dA-Z]{1,8})$";
        public const string REGEX_HUECO = @"^(HA|HR)-[A-Z]\d{2}$";
        public const string REGEX_CODIGO = @"^[3][0-9]{1,9}$";


        public const int LONG_MIN_RAZON_SOCIAL = 5;
        public const int LONG_MAX_RAZON_SOCIAL = 40;

        public const int LONG_MIN_NIF = 10;
        public const int LONG_MAX_NIF = 10;

        public const int LONG_MIN_EMAIL = 5;
        public const int LONG_MAX_EMAIL = 254;

        public const int LONG_MIN_CALLE = 5;
        public const int LONG_MAX_CALLE = 60;

        public const int LONG_MIN_NOMBRE_TIPO_USUARIO = 3;
        public const int LONG_MAX_NOMBRE_TIPO_USUARIO = 25;

        public const int LONG_MIN_NOMBRE_USUARIO = 3;
        public const int LONG_MAX_NOMBRE_USUARIO = 10;

        public const int LONG_MIN_HASH_CONTRASENA = 64;
        public const int LONG_MAX_HASH_CONTRASENA = 64;

        public const int LONG_MIN_CODIGO_COMUNIDAD = 4;
        public const int LONG_MAX_CODIGO_COMUNIDAD = 5;

        public const int LONG_MIN_NOMBRE_COMUNIDAD = 3;
        public const int LONG_MAX_NOMBRE_COMUNIDAD = 30;

        public const int LONG_MIN_NOMBRE_ESTADO_ELABORACION = 3;
        public const int LONG_MAX_NOMBRE_ESTADO_ELABORACION = 20;

        public const int LONG_MIN_DESCRIPCION_ESTADO_ELABORACION = 5;
        public const int LONG_MAX_DESCRIPCION_ESTADO_ELABORACION = 80;

        public const int LONG_MIN_NOMBRE_ESTADO_PEDIDO = 3;
        public const int LONG_MAX_NOMBRE_ESTADO_PEDIDO = 10;

        public const int LONG_MIN_DESCRIPCION_ESTADO_PEDIDO = 5;
        public const int LONG_MAX_DESCRIPCION_ESTADO_PEDIDO = 80;

        public const int LONG_MIN_NOMBRE_ESTADO_RECEPCION = 3;
        public const int LONG_MAX_NOMBRE_ESTADO_RECEPCION = 20;

        public const int LONG_MIN_DESCRIPCION_ESTADO_RECEPCION = 5;
        public const int LONG_MAX_DESCRIPCION_ESTADO_RECEPCION = 80;

        public const int LONG_MIN_NOMBRE_ESTADO_ENVASADO = 3;
        public const int LONG_MAX_NOMBRE_ESTADO_ENVASADO = 20;

        public const int LONG_MIN_DESCRIPCION_ESTADO_ENVASADO = 5;
        public const int LONG_MAX_DESCRIPCION_ESTADO_ENVASADO = 80;

        public const int LONG_MIN_NOMBRE_GRUPO = 3;
        public const int LONG_MAX_NOMBRE_GRUPO = 25;

        public const int LONG_MIN_DESCRIPCION_GRUPO = 5;
        public const int LONG_MAX_DESCRIPCION_GRUPO = 80;

        public const int LONG_MIN_NOMBRE_HUECO = 6;
        public const int LONG_MAX_NOMBRE_HUECO = 6;

        public const int LONG_MIN_OBSERVACIONES = 3;
        public const int LONG_MAX_OBSERVACIONES = 60;

        public const int LONG_MIN_CODIGO = 10;
        public const int LONG_MAX_CODIGO = 10;

        public const int LONG_MIN_NOMBRE_MUNICIPIO = 1;
        public const int LONG_MAX_NOMBRE_MUNICIPIO = 80;

        public const int LONG_MIN_NOMBRE_PROVINCIA = 3;
        public const int LONG_MAX_NOMBRE_PROVINCIA = 50;

        public const int LONG_MIN_CODIGO_POSTAL = 5;
        public const int LONG_MAX_CODIGO_POSTAL = 15;

        public const int LONG_MIN_LATITUD = 10;
        public const int LONG_MAX_LATITUD = 10;

        public const int LONG_MIN_LONGITUD = 10;
        public const int LONG_MAX_LONGITUD = 10;

        public const int LONG_MIN_DESCRIPCION = 5;
        public const int LONG_MAX_DESCRIPCION = 80;

        public const int LONG_MIN_CODIGO_PAIS = 2;
        public const int LONG_MAX_CODIGO_PAIS = 2;

        public const int LONG_MIN_CODIGO_PROVINCIA = 4;
        public const int LONG_MAX_CODIGO_PROVINCIA = 5;

        public const int LONG_MIN_NOMBRE_PAIS = 3;
        public const int LONG_MAX_NOMBRE_PAIS = 20;

        public const int LONG_MIN_NOMBRE = 3;
        public const int LONG_MAX_NOMBRE = 30;

        public const int LONG_MIN_NUMERO_ALBARAN = 3;
        public const int LONG_MAX_NUMERO_ALBARAN = 10;

        public const int LONG_MIN_CANTIDAD = 1;
        public const int LONG_MAX_CANTIDAD = 6;

        public const int LONG_MIN_UNIDADES = 1;
        public const int LONG_MAX_UNIDADES = 3;

        public const int LONG_MIN_VOLUMEN = 1;
        public const int LONG_MAX_VOLUMEN = 6;

        public const int LONG_MIN_CANTIDAD_ENVASADO = 2;
        public const int LONG_MAX_CANTIDAD_ENVASADO = 6;
    }
}
