using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfCommonds;


namespace FelicaCashingSystemV2.Views
{
    class MoneyHistoryViewModel : MetroWindowViewModelBase
    {
        public MoneyHistoryViewModel()
        {
            this.CancelCommand = new DelegateCommand<FelicaData.MoneyHistory>(this.Cancel);

            this.UpdateMoneyHistories();
            App.Current.UserData.Changed += UserData_Changed;
        }

        void UserData_Changed(object sender, Type e)
        {
             this.UpdateMoneyHistories();
        
        }

        private void Current_UserChanged(object sender, FelicaData.User e)
        {
            this.UpdateMoneyHistories();
        }

        private void UpdateMoneyHistories()
        {
            var histories = App.Current.UserData.GetMoneyHistories(App.Current.User.Id);
            histories.Reverse();
            this.MoneyHistories = histories;
        }

        private List<FelicaData.MoneyHistory> moneyHistories = null;
        public List<FelicaData.MoneyHistory> MoneyHistories
        {
            get { return this.moneyHistories; }
            set
            {
                this.moneyHistories = value;
                this.OnPropertyChanged("MoneyHistories");
            }
        }

        public ICommand CancelCommand { get; private set; }
        private void Cancel(FelicaData.MoneyHistory history)
        {
            if (history == null) { return; }
            if (history.IsCancel) { return; }

            this.ShowConfirmDialog(
                "「 " + history.CreatedAt.ToString("yyyy/MM/dd HH:mm") + " 」 に行われた" +
                " 「 " + history.Money.ToCommaStringAbs() + " 」 円の取引をキャンセルしてよろしいですか?",
                "本当にキャンセルしますか ?",
                (result) =>
                {
                    // OK でない場合は処理しない
                    if (result != MahApps.Metro.Controls.Dialogs.MessageDialogResult.Affirmative) { return; }

                    try
                    {
                        App.Current.UserData.Cancel(history);
                        this.ShowMessageBox("キャンセルに成功しました。キャンセルした分の、加算および減算は正常に行われました。", "キャンセル成功");
                        App.Current.UpdateUser();
                    }

                    catch (Exception e)
                    {
                        this.ShowMessageBox(e.Message, "キャンセル失敗");
                    }
                });
        }

    }
}
