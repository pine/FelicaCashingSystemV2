using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FelicaCashingSystemV2
{
    /// <summary>
    /// 購入履歴をキャンセル可能な場合に表示する Visibility に変換するコンバーター
    /// </summary>
    class MoneyHistoryToCanCancelVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var history = value as FelicaData.MoneyHistory;

            if (history != null)
            {
                // 管理者による執行ではなく、既にキャンセルされていない場合、キャンセル可能
                if (history.UserId == history.PerformerUserId && !history.IsCancel)
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}