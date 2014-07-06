using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfCommonds;

namespace FelicaCashingSystemV2.Views
{
    class UserListViewModel : MetroWindowViewModelBase
    {
        public UserListViewModel()
        {
            this.AdministerCommand = new DelegateCommand<FelicaData.User>(this.Administer);
            App.Current.UserChanged += this.App_UserChanged;
        }

        void App_UserChanged(object sender, FelicaData.User e)
        {
            this.Users = App.Current.UserData.GetUsers();
        }

        private List<FelicaData.User> users = App.Current.UserData.GetUsers();
        public List<FelicaData.User> Users
        {
            get { return this.users; }
            set
            {
                this.users = value;
                this.OnPropertyChanged("Users");
            }
        }

        public ICommand AdministerCommand { get; private set; }
        private void Administer(FelicaData.User user)
        {
            App.Current.ShowAdministeringUserWindow(user);   
        }
    }
}
