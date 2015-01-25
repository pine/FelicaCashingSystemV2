using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using WpfCommonds;

namespace FelicaCashingSystemV2.Views
{
    class AdministeringPermissionViewModel : AdministeringMoneyViewModel
    {
        public AdministeringPermissionViewModel()
        {
            this.ChangePermissionCommand = new DelegateCommand(this.ChangePermission);
        }

        public ICommand ChangePermissionCommand { get; set; }
        private void ChangePermission()
        {
            this.ErrorMessage = string.Empty;

            // 既に管理者である場合
            if (this.AdministeringUser.IsAdmin)
            {
                this.ErrorMessage = "既に管理者です。";
                return;
            }

            try
            {
                this.AdministeringUser.IsAdmin = true;
                App.Current.Collections.Users.UpdateUser(this.AdministeringUser);
                App.Current.UpdateUser();
            }
            catch (FelicaData.DatabaseException e)
            {
                this.AdministeringUser.IsAdmin = false;
                this.ErrorMessage = e.Message;
                return;
            }

            this.ShowMessageBox("権限の変更に成功しました", "成功");
        }

        private string errorMessage = "";
        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set
            {
                this.errorMessage = value;
                this.OnPropertyChanged("ErrorMessage");
            }
        }
    }
}
