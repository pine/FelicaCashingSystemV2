using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfCommonds;

namespace FelicaCashingSystemV2
{
    class MoneyViewModel : MetroWindowViewModelBase
    {
        public MoneyViewModel()
        {
            this.BuyCommand = new DelegateCommand<int>(this.Buy);
            this.ChargeCommand = new DelegateCommand<int>(this.Charge);
        }

        /// <summary>
        /// 購入処理のロックオブジェクト
        /// </summary>
        private object buyingLockObj = new object();

        /// <summary>
        /// 購入処理中かどうか
        /// </summary>
        private bool isBuying = false;

        public ICommand BuyCommand { get; private set; }

        protected void Buy(int money)
        {
            this.Buy(money, true);
        }

        protected void Buy(int money, bool isAnimation)
        {
            lock (buyingLockObj)
            {
                if (this.isBuying) { return; }
                this.isBuying = true;

                Debug.WriteLine("Buy money = " + money.ToString());

                if (money <= 0 && money != -1)
                {
                    this.isBuying = false;
                    throw new ArgumentOutOfRangeException("money");
                }

                // 購入金額選択
                if (money == -1)
                {
                    this.SelectMoney((newMoney) =>
                    {
                        Debug.WriteLine("SelectMoney succeed: money = " + newMoney);

                        if (newMoney > 0)
                        {
                            this.Buy(newMoney); // 非同期処理なため、デッドロックはしない
                        }
                    });

                    this.isBuying = false;
                    return;
                }

                // 購入処理
                if (App.Current.User != null)
                {
                    this.ShowMessageBox(
                            "「 " + money.ToCommaStringAbs() + " 」 円の商品を購入しました。\n" +
                            "残高は 「 " + (App.Current.User.Money - money).ToCommaString() + " 」 円です。",
                            "購入成功",
                            isAnimation,
                            () =>
                            {
                                this.isBuying = false;
                            });

                    Task.Run(() =>
                    {
                        if (App.Current.UserData.Buy(App.Current.User.Id, money))
                        {
                            Debug.WriteLine("Buying succeed");
                            App.Current.UpdateUser();
                        }
                        else
                        {
                            Debug.WriteLine("Buying error");

                            this.ShowMessageBox(
                                "商品の購入でエラーが発生しました。正しく処理されているか、履歴を確認してください。",
                                "購入失敗",
                                isAnimation,
                                () =>
                                {
                                    this.isBuying = false;
                                });
                        }
                    });
                }
            }
        }

        public ICommand ChargeCommand { get; private set; }
        private void Charge(int money)
        {
            if (money < 0) { throw new ArgumentOutOfRangeException("money"); }
            if (money == 0) { return; }

            this.ChargeOrWithdraw(money);
        }

        public ICommand WithdrawCommand { get; private set; }
        private void Withdraw(int money) 
        {
            if (money <= 0) { throw new ArgumentOutOfRangeException("money"); }
            if (money == 0) { return; }

            this.ChargeOrWithdraw(-money);
        }

        /// <summary>
        /// 入金及び出勤の処理
        /// </summary>
        /// <param name="money"></param>
        protected void ChargeOrWithdraw(int money)
        {

        }

        /// <summary>
        /// 金額を選択する。
        /// </summary>
        private void SelectMoney(Action<int> cb)
        {
            Debug.WriteLine("SelectMoney");
            App.Current.ShowSelectingMoneyWindow(cb);  
        }
    }
}
