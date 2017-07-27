using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace BiomasaEUPT.Domain.Converters
{
    public class PorcentajeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // return System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter);
            if (parameter == null)
                return 0.7 * System.Convert.ToDouble(value);

            string[] split = parameter.ToString().Split('.');
            double parameterDouble = System.Convert.ToDouble(split[0])
                + System.Convert.ToDouble(split[1]) / (Math.Pow(10, split[1].Length));

            return System.Convert.ToDouble(value) * parameterDouble;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
