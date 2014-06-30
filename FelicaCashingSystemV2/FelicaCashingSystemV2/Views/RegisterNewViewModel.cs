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

            try
            {
                // データベースに登録
                var user = App.Current.UserData.CreateUser(new FelicaData.User
                {
                    Name = this.UserName,
                    Email = this.Email,
                    Password = password
                });

                if (user == null)
                {
                    this.ErrorMessage = "ユーザー登録に失敗しました。";
                    return;
                }
            }
            catch (FelicaData.DatabaseException e)
            {
                this.ErrorMessage = e.Message;
                return;
            }

            // メールを送信
            App.Current.Mailer.SendRegistered(this.Email, new RegisteredArgs
            {
                Name = this.UserName
            });
        }
    }
}
