using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FelicaCashingSystemV2
{
    class TileMoneyToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var intVal = System.Convert.ToInt32(value);

            if (intVal < 0)
            {
                return "・・・";
            }

            return intVal.ToCommaStringAbs();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
