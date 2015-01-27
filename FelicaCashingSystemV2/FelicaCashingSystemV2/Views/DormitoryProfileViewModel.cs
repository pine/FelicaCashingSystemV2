using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using WpfCommonds;

namespace FelicaCashingSystemV2.Views
{
    class DormitoryProfileViewModel : MetroWindowViewModelBase
    {
        private FelicaData.User user = App.Current.User.Clone();

        public DormitoryProfileViewModel()
        {
            this.SaveCommand = new DelegateCommand(this.Save);
            App.Current.UserChanged += Current_UserChanged;
        }

        private void Current_UserChanged(object sender, FelicaData.User e)
        {
            if (App.Current.User != null)
            {
                this.user = App.Current.User.Clone();
            }
        }

        public string DormitoryRoomNo
        {
            get { return this.user.DormitoryRoomNumber; }
            set
            {
                this.user.DormitoryRoomNumber = value;
                this.OnPropertyChanged("DormitoryRoomNo");
            }
        }

        public string PhoneNumber
        {
            get { return this.user.PhoneNumber; }
            set
            {
                this.user.PhoneNumber = value;
                this.OnPropertyChanged("PhoneNumber");
            }
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
        
        public ICommand SaveCommand { get; private set; }
        private void Save()
        {
            this.ErrorMessage = string.Empty;

            try
            {
                App.Current.Collections.Users.UpdateUser(this.user);
            }
            catch (FelicaData.DatabaseException e)
            {
                this.ErrorMessage = e.Message;
                return;
            }

            this.ShowMessageBox("ドミトリーの情報を変更しました。", "変更完了");
            App.Current.UpdateUser();
        }

    }
}
