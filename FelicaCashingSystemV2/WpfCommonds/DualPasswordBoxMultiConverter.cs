using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfCommonds
{
    public class DualPasswordBoxMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (values.Length)
            {
                case 0:
                    return new PasswordBox[] { null, null };

                case 1:
                    return new PasswordBox[] { (PasswordBox)values[0], null };

                default:
                    return new PasswordBox[] { (PasswordBox)values[0], (PasswordBox)values[1] };
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
