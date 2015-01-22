using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FelicaCashingSystemV2
{
    class UserIdToUserNameConverter : IValueConverter
    {
        private Dictionary<string, string> userNameCached = new Dictionary<string, string>();

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
            if (App.Current.Collections == null) { return null; }

            string userId = value as string;
            if (userId == null) { return null; }

            if (this.userNameCached.ContainsKey(userId))
            {
                return this.userNameCached[userId];
            }

            else
            {
                try
                {
                    var user = App.Current.Collections.Users.GetUser(userId);

                    if (user != null)
                    {
                        this.userNameCached[userId] = user.Name;
                        return user.Name;
                    }
                }
                catch (FelicaData.DatabaseException e)
                {
                    Debug.WriteLine(e);
                    return "データベースエラー";
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
