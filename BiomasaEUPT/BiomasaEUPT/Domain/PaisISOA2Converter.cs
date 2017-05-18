using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BiomasaEUPT.Domain
{
    public class PaisISOA2ImagenConverter : IValueConverter
    {
        // https://es.wikipedia.org/wiki/ISO_3166-1
        // Listado de paises: http://www.worldatlas.com/aatlas/ctycodes.htm
        // Iconos: https://github.com/hjnilsson/country-flags
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return @"pack://application:,,,/" + Assembly.GetExecutingAssembly().GetName().Name + ";component/Resources/Paises/" + value.ToString() + ".png";
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
