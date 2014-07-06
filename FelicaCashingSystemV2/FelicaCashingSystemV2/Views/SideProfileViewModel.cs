using System.Windows.Media;
using WpfCommonds;

namespace FelicaCashingSystemV2.Views
{
    class SideProfileViewModel : MetroWindowViewModelBase
    {
        public SideProfileViewModel()
        {
            this.User = App.Current.User;
            App.Current.UserChanged += App_UserChanged;
        }

        private FelicaData.NullableUser user = new FelicaData.NullableUser();
        private FelicaData.User User
        {
            get { return this.user.User; }
            set
            {
                this.user.User = value;
                this.OnPropertyChanged("Name", "Money", "IsAdmin");
                this.OnPropertyChanged("AvatarSource");
            }
        }

        public string Name
        {
            get
            {
                return this.user.Name;
            }
        }

        public int Money
        {
            get
            {
                return this.user.Money;
            }
        }

        public ImageSource AvatarSource
        {
            get
            {
                if (this.user.Avatar != null)
                {
                    return this.user.Avatar.ToBitmap().ToBitmapSource();
                }

                return null;
            }
        }

        private void App_UserChanged(object sender, FelicaData.User e)
        {
            this.User = e;
        }
    }
}
