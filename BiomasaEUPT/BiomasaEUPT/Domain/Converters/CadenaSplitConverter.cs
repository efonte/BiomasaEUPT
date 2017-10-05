using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BiomasaEUPT.Domain.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class CadenaSplitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            if (parameter == null)
                return value.ToString();


            string[] parametros = parameter.ToString().Split(new char[] { '|' });
            int min = System.Convert.ToInt16(parametros[0]);
            int max = System.Convert.ToInt16(parametros[1]);
            return value.ToString().Substring(min, max);

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }
    }
}
