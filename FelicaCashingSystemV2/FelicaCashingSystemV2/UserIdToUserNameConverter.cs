using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FelicaCashingSystemV2
{
    class UserIdToUserNameConverter : IValueConverter
    {
        private Dictionary<int, string> userNameCached = new Dictionary<int, string>();

        public UserIdToUserNameConverter()
        {
            App.Current.UserChanged += this.App_UserChanged;
        }

        private void App_UserChanged(object sender, FelicaData.User e)
        {
            this.userNameCached.Clear();
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (App.Current.UserData == null) { return null; }

            int userId = System.Convert.ToInt32(value);
            if (userId == 0) { return null; }

            if (this.userNameCached.ContainsKey(userId))
            {
                return this.userNameCached[userId];
            }

            else
            {
                var user = App.Current.UserData.GetUser(userId);

                if (user != null)
                {
                    this.userNameCached[userId] = user.Name;
                    return user.Name;
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
