using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WpfCommonds;

namespace FelicaCashingSystemV2.Views
{
    class AdministeringUserViewModel : MoneyViewModel
    {

        public AdministeringUserViewModel()
                      : base(FelicaData.UiPageType.Administering)
        {
            App.Current.UserChanged += Current_UserChanged;
        }

        /// <summary>
        /// ユーザー情報が更新された時のイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Current_UserChanged(object sender, FelicaData.User e)
        {
            if (this.AdministeringUser.Id != null)
            {
                this.AdministeringUser = App.Current.Collections.Users.GetUser(this.AdministeringUser.Id);
            }
        }

        private FelicaData.NullableUser administeringUser = new FelicaData.NullableUser();
        public FelicaData.User AdministeringUser
        {
            set
            {
                this.administeringUser.User = value;
                this.User = value;

                this.OnPropertyChanged("AdministeringUser");
                this.OnPropertyChanged("Name");
                this.OnPropertyChanged("Money");
                this.OnPropertyChanged("AvatarSource");
            }
            get
            {
                return this.administeringUser.User;
            }
        }

        public string Name
        {
            get
            {
                return this.administeringUser.Name;
            }
        }

        public int Money
        {
            get
            {
                return this.administeringUser.Money;
            }
        }

        public ImageSource AvatarSource
        {
            get
            {
                if (this.AdministeringUser.Avatar != null)
                {
                    return this.AdministeringUser.Avatar.ToBitmap().ToBitmapSource();
                }

                return null;
            }
        }
    }
}
