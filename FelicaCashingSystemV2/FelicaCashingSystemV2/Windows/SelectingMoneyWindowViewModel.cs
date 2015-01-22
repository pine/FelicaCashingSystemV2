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

        private int maxMoney = 0;
        public int MaxMoney
        {
            get
            {
                return this.maxMoney;
            }
            set
            {
                if (value < 0)
                {
                    this.ErrorMessage = "データベースの設定値が不正です。";
                }

                this.maxMoney = value;
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

            if (this.MaxMoney < 0)
            {
                this.ErrorMessage = "データベースの設定値が不正です。";
                return;
            }

            try
            {
                money = Convert.ToInt32(this.Money);
            }

            catch (FormatException)
            {
                this.ErrorMessage = "金額は、整数で指定してください。";
                return;
            }

            catch (OverflowException)
            {
                this.ErrorMessage = "金額が大きすぎます。";
                return;
            }

            if (money <= 0)
            {
                this.ErrorMessage = "金額は 「 1 」 円以上を指定してください。";
                return;
            }

            if (this.MaxMoney != 0 && money > this.MaxMoney)
            {
                this.ErrorMessage = "金額は 「 " + this.MaxMoney.ToCommaStringAbs() + " 」 円以下で指定してください。";
                return;
            }

            this.ShowConfirmDialog(
                "本当に 「 " + money.ToCommaStringAbs() + " 」 円でよろしいですか?",
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
