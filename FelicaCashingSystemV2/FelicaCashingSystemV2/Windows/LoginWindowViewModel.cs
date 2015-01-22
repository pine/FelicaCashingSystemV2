using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace FelicaCashingSystemV2.Windows
{
    class LoginWindowViewModel : WpfCommonds.ViewModelBase
    {
        public LoginWindowViewModel()
        {
            this.Users =
                new ObservableCollection<FelicaData.User>(App.Current.Collections.Users.GetUsers());

            this.LoginCommand = new WpfCommonds.DelegateCommand<PasswordBox>(this.Login);
        }

        private ObservableCollection<FelicaData.User> users = null;

        public ObservableCollection<FelicaData.User> Users
        {
            get { return this.users; }
            set {
                lock (value)
                {
                    if (value.Count > 0)
                    {
                        this.userId = value.First().Id;
                    }
                }

                this.users = value;
                this.OnPropertyChanged("Users");
            }
        }

        private string userId = null;

        public string UserId
        {
            get { return this.userId; }
            set {
                this.userId = value;
                this.OnPropertyChanged("UserId");
            }
        }

        public FelicaData.User User
        {
            get
            {
                if (this.Users != null)
                {
                    foreach (var user in this.Users)
                    {
                        if (user.Id == this.UserId)
                        {
                            return user;
                        }
                    }
                }

                return null;
            }
        }

        private string errorMessage = "";
        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set {
                this.errorMessage = value;
                this.OnPropertyChanged("ErrorMessage");
            }
        }

        public ICommand LoginCommand { get; set; }

        private void Login(PasswordBox passwordBox)
        {
            Debug.WriteLine("Login started");
            this.ErrorMessage = "";

            var user = this.User;
            if (user == null)
            {
                this.ErrorMessage = "ユーザーが選択されていません。";
                return;
            }

            var password = passwordBox.Password;
            if (string.IsNullOrWhiteSpace(password))
            {
                this.ErrorMessage = "パスワードが入力されていません。";
                return;
            }

            Debug.WriteLine("UserName = " + user.Name +
                ", UserId = " + this.UserId +
                ", Password = " + password);

            if (!user.Auth(password))
            {
                this.ErrorMessage = "パスワードが違います。";

                passwordBox.Clear();
                passwordBox.Focus();

                return;
            }

            Debug.WriteLine("Login succeed");
            App.Current.ShowMainWindow(user);
        }
    }
}
