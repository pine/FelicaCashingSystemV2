using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfCommonds
{
    [ValueConversion(typeof(int), typeof(Color))]
    [ValueConversion(typeof(string), typeof(Color))]
    public class MinusToColorConverter : IValueConverter
    {
        public static readonly Color RedColor = Colors.Red;
        public static readonly Color NormalColor = SystemColors.ControlTextColor;

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var intValue = System.Convert.ToInt32(value);

            if (intValue < 0)
            {
                return RedColor;
            }

            else
            {
                return NormalColor;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
