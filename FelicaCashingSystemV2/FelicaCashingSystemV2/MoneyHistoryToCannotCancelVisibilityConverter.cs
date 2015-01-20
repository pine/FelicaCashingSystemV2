using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FelicaCashingSystemV2
{
    /// <summary>
    /// 履歴をキャンセルできないときに表示される Visibility に変換するクラス
    /// </summary>
    public class MoneyHistoryToCannotCancelVisibilityConverter : IValueConverter
    {
        private static MoneyHistoryToCanCancelVisibilityConverter _converter =
            new MoneyHistoryToCanCancelVisibilityConverter();

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var converted = _converter.Convert(value, targetType, parameter, culture);
            var convertedVisibility = (Visibility)converted;

            if (convertedVisibility == Visibility.Visible)
            {
                return Visibility.Hidden;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
