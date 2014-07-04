using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfCommonds;

namespace FelicaCashingSystemV2.Windows
{
    class SelectingMoneyWindowViewModel : MetroWindowViewModelBase
    {
        public SelectingMoneyWindowViewModel()
        {
            this.ExecuteCommand = new DelegateCommand(this.Execute);
        }

        private string money = string.Empty;
        public string Money {
            get { return this.money.ToString(); }
            set
            {
                this.money = value;
                this.OnPropertyChanged("Money");
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

        public ICommand ExecuteCommand { get; private set; }
        private void Execute()
        {
            int money = 0;
            this.ErrorMessage = string.Empty;

            try
            {
                money = Convert.ToInt32(this.Money);
            }

            catch (FormatException)
            {
                this.ErrorMessage = "金額は、数値で指定してください。";
                return;
            }

            if (money <= 0)
            {
                this.ErrorMessage = "金額は 「 1 」 円以上を指定してください。";
                return;
            }

            this.ShowConfirmDialog(
                "本当に 「 " + this.Money + " 」 円でよろしいですか?",
                "金額確認",
                (result) =>
                {
                    // キャンセルの場合
                    if (result == MessageDialogResult.Negative)
                    {
                        return;
                    }

                    this.SendMessage(new SelectingMoneyMessage
                    {
                        Money = money
                    });
                });
        }
    }
}
