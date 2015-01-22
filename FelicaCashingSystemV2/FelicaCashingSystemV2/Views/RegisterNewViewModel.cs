using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using FelicaMail;

namespace FelicaCashingSystemV2.Views
{
    class RegisterNewViewModel : WpfCommonds.ViewModelBase
    {
        public RegisterNewViewModel()
        {
            this.RegisterCommand = new WpfCommonds.DelegateCommand<PasswordBox>(this.Register);
        }

        private string userName = string.Empty;
        public string UserName

        {
            get { return this.userName; }
            set {
                this.userName = value;
                this.OnPropertyChanged("UserName");
            }
        }

        private string email = string.Empty;
        public string Email
        {
            get { return this.email; }
            set
            {
                this.email = value;
                this.OnPropertyChanged("Email");
            }
        }

        private string errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set { 
                this.errorMessage = value;
                this.OnPropertyChanged("ErrorMessage");
            }
        }

        public ICommand RegisterCommand { get; set; }
        private void Register(PasswordBox passwordBox)
        {
            Debug.WriteLine("Register start");

            if (passwordBox == null)
            {
                this.ErrorMessage = "原因不明のエラーが発生しました。";
                return;
            }

            this.ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(this.UserName))
            {
                this.ErrorMessage = "名前を正しく入力してください。";
                return;
            }

            if (string.IsNullOrWhiteSpace(this.Email))
            {
                this.ErrorMessage = "メールアドレスを入力してください。";
                return;
            }

            if (!FelicaMail.MailAddress.IsValidEmail(this.Email))
            {
                this.ErrorMessage = "メールアドレスを正しく入力してください。";
                return;
            }

            var password = passwordBox.Password;

            if (string.IsNullOrWhiteSpace(password))
            {
                this.ErrorMessage = "パスワードを入力してください。";
                passwordBox.Clear();
                passwordBox.Focus();
                return;
            }

            string cardUid = App.Current.UnregisteredCard.Idm;
            bool isAdmin =
                FelicaCashingSystemV2.Properties.Settings.Default.AdminCardUids.Contains(cardUid);
            FelicaData.User registeredUser = null;
            
            try
            {
                // データベースに登録
                registeredUser = App.Current.Collections.Users.CreateUser(
                    new FelicaData.User
                    {
                        Name = this.UserName,
                        Email = this.Email,
                        Password = password,
                        IsAdmin = isAdmin
                    },
                    new FelicaData.Card
                    {
                        Name = "最初に登録したカード",
                        Uid = cardUid
                    });
            }
            catch (FelicaData.DatabaseException e)
            {
                this.ErrorMessage = e.Message;
                return;
            }

            if (registeredUser == null)
            {
                this.ErrorMessage = "ユーザー登録に失敗しました。";
                return;
            }

            // メールを送信
            Task.Run(() =>
            {
                App.Current.Mailer.SendRegistered(this.Email, new RegisteredArgs
                {
                    Name = this.UserName
                });
            });

            // 登録したユーザーでログイン
            App.Current.UpdateCard();
            App.Current.ShowMainWindow(registeredUser);
        }
    }
}
