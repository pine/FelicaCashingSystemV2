using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using WpfCommonds;

namespace FelicaCashingSystemV2.Windows
{
    class AssociationWindowViewModel : MetroWindowViewModelBase
    {
        public AssociationWindowViewModel()
        {
            this.AssosiateCommand = new DelegateCommand(this.Assosiate);
            App.Current.UserChanged += this.App_UserChanged;
        }

        private void App_UserChanged(object sender, FelicaData.User e)
        {
            this.OnPropertyChanged("Name", "Money");
        }

        public string Name
        {
            get
            {
                if (App.Current.User != null)
                {
                    return App.Current.User.Name;
                }

                return null;
            }
        }

        public int Money
        {
            get
            {
                if (App.Current.User != null)
                {
                    return App.Current.User.Money;
                }

                return 0;
            }
        }

        public ImageSource AvatarSource
        {
            get
            {
                if (App.Current.User != null)
                {
                    if (App.Current.User.Avatar != null)
                    {
                        return App.Current.User.Avatar.ToBitmapSource();
                    }
                }

                return null;
            }
        }

        private string cardName = string.Empty;
        public string CardName
        {
            get { return this.cardName; }
            set
            {
                this.cardName = value;
                this.OnPropertyChanged("CardName");
            }
        }

        private string errorMessage = string.Empty;
        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set 
            { 
                this.errorMessage = value;
                this.OnPropertyChanged("ErrorMessage");
            }
        }

        public ICommand AssosiateCommand { get; private set; }
        public void Assosiate()
        {
            if (App.Current.User == null) { return; }
            if (App.Current.UnregisteredCard== null) { return; }

            this.ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(this.CardName))
            {
                this.ErrorMessage = "カード名を入力してください。";
                return;
            }

            try
            {
                App.Current.Collections.Cards.Assoate(new FelicaData.Card
                {
                    Uid = App.Current.UnregisteredCard.Idm,
                    UserId = App.Current.User.Id,
                    Name = this.CardName
                });
            }

            catch (Exception e)
            {
                this.ErrorMessage = e.Message;
                return;
            }

            // 成功
            App.Current.UpdateCard();

            this.ShowMessageBox(
                "カードの関連付けに成功しました。",
                "関連付け成功",
                callback: () =>
                {
                    App.Current.ShowMainWindow(App.Current.User);
                });
        }
    }
}
