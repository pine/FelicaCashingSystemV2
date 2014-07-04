using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfCommonds
{
    [ValueConversion(typeof(int), typeof(Color))]
    [ValueConversion(typeof(string), typeof(Color))]
    public class MinusToColorConverter : IValueConverter
    {
        public static readonly Color Color = Colors.Red;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var color = MinusToColorConverter.Color;

            if (parameter is Color)
            {
                color = (Color)parameter;
            }

            var intValue = System.Convert.ToInt32(value);

            if (intValue < 0)
            {
                return color;
            }

            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
