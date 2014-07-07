using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfCommonds
{
    public class BooleanToInverseVisibilityConverter : IValueConverter
    {
        private static BooleanToVisibilityConverter _converter =
            new BooleanToVisibilityConverter();

        private static readonly object TrueValue;
        private static readonly object FalseValue;

        static BooleanToInverseVisibilityConverter()
        {
            TrueValue = _converter.Convert(true, null, null, null);
            FalseValue = _converter.Convert(false, null, null, null);
        }
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (_converter.Convert(value, null, null, null).Equals(TrueValue))
            {
                return FalseValue;
            }

            return TrueValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
