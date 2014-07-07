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
        private readonly FelicaData.UiPageType pageType;

        public MoneyViewModel(FelicaData.UiPageType pageType)
        {
            this.pageType = pageType;

            this.UpdateMoneyTiles();
            App.Current.UiData.Changed += this.UiData_Changed;

            this.BuyCommand = new DelegateCommand<int>(this.Buy);
            this.ChargeCommand = new DelegateCommand<int>(this.Charge);
            this.WithdrawCommand = new DelegateCommand<int>(this.Withdraw);
        }

        private void UiData_Changed(object sender, Type e)
        {
            this.UpdateMoneyTiles();
        }

        protected virtual void UpdateMoneyTiles()
        {
            if (this.pageType != FelicaData.UiPageType.Unknown)
            {
                var page = App.Current.UiData.GetPage(this.pageType);
                var list = new List<int>();

                if (page != null && page.MoneyTiles != null)
                {
                    list.AddRange(page.MoneyTiles);
                }

                list.Add(-1); // etc
                this.MoneyTiles = list; // Update
            }
        }

        private List<int> moneyTiles = null;
        public List<int> MoneyTiles
        {
            get { return this.moneyTiles; }
            set
            {
                this.moneyTiles = value;
                this.OnPropertyChanged("MoneyTiles");
            }
        }


        public ICommand BuyCommand { get; private set; }

        protected void Buy(int money)
        {
            this.Buy(money, true);
        }

        protected void Buy(int money, bool isAnimation)
        {
            this.MoneyExecute(
                money,
                () => this.GetExecuteMax(FelicaData.UiPageType.Buying),
                isAnimation,
                "購入",
                (newMoney) =>
                {
                    return "「 " + newMoney.ToCommaStringAbs() + " 」 円の商品を購入しました。\n" +
                            "残高は 「 " + (App.Current.User.Money - newMoney).ToCommaString() + " 」 円です。";
                },
                "購入に失敗しました",
                (newMoney) =>
                {
                    return App.Current.UserData.Buy(App.Current.User.Id, newMoney);
                });
        }

        public ICommand ChargeCommand { get; private set; }
        private void Charge(int money)
        {
            this.MoneyExecute(
                money,
                () => this.GetExecuteMax(FelicaData.UiPageType.Charging),
                true,
                "チャージ",
                (newMoney) =>
                {
                    return "「 " + newMoney.ToCommaStringAbs() + " 」 円をチャージしました。\n" +
                            "残高は 「 " + (App.Current.User.Money + newMoney).ToCommaString() + " 」 円です。";
                },
                "チャージに失敗しました",
                (newMoney) =>
                {
                    return App.Current.UserData.Charge(App.Current.User.Id, newMoney);
                });
        }

        public ICommand WithdrawCommand { get; private set; }
        private void Withdraw(int money) 
        {
            this.MoneyExecute(
                money,
                () => this.GetExecuteMax(FelicaData.UiPageType.Withdrawing),
                true,
                "出金",
                (newMoney) =>
                {
                    return "「 " + newMoney.ToCommaStringAbs() + " 」 円を引き出しました。\n" +
                            "残高は 「 " + (App.Current.User.Money - newMoney).ToCommaString() + " 」 円です。";
                },
                "出金に失敗しました",
                (newMoney) =>
                {
                    return App.Current.UserData.Withdraw(App.Current.User.Id, newMoney);
                });
        }

        /// <summary>
        /// 金額を選択する。
        /// </summary>
        private void SelectMoney(int max, Action<int> cb)
        {
            Debug.WriteLine("SelectMoney");
            App.Current.ShowSelectingMoneyWindow(max, cb);
        }

        /// <summary>
        /// 取引の最大値を取得する
        /// </summary>
        /// <param name="pageType"></param>
        private int GetExecuteMax(FelicaData.UiPageType pageType)
        {
            var page = App.Current.UiData.GetPage(pageType);
            return page.MaxMoney;
        }

        /// <summary>
        /// 金銭処理のロックオブジェクト
        /// </summary>
        private object executingLockObject = new object();

        /// <summary>
        /// 金銭処理中かどうか
        /// </summary>
        private bool isExecuting = false;

        /// <summary>
        /// 金銭処理を行うメソッド
        /// </summary>
        /// <param name="money"></param>
        /// <param name="isAnimation"></param>
        /// <param name="actionName"></param>
        /// <param name="succeedMessage">成功メッセージを生成するデリゲート、引数は処理金額</param>
        /// <param name="errorMessage"></param>
        /// <param name="moneyAction">実際の処理を行うデリゲート</param>
        private void MoneyExecute(
            int money,
            Func<int> max,
            bool isAnimation,
            string actionName,
            Func<int, string> succeedMessage,
            string errorMessage,
            Func<int, bool> moneyAction)
        {
            // 複数同時並行は禁止
            lock (executingLockObject)
            {
                if (this.isExecuting) { return; }
                this.isExecuting = true;

                // エラー
                if (money <= 0 && money != -1)
                {
                    this.isExecuting = false;
                    throw new ArgumentOutOfRangeException("money");
                }

                // 購入金額選択
                if (money == -1)
                {
                    this.SelectMoney(max(), (newMoney) =>
                    {
                        if (newMoney > 0)
                        {
                            // SelectMoney が非同期処理なため、デッドロックはしない
                            this.MoneyExecute(
                                newMoney,
                                max,
                                isAnimation,
                                actionName,
                                succeedMessage,
                                errorMessage,
                                moneyAction);
                        }
                    });

                    this.isExecuting = false;
                    return;
                }

                // 購入処理
                if (App.Current.User != null)
                {
                    this.ShowMessageBox(
                            succeedMessage(money),
                            actionName + "成功",
                            isAnimation,
                            () =>
                            {
                                this.isExecuting = false;
                            });

                    Task.Run(() =>
                    {
                        if (moneyAction(money))
                        {
                            App.Current.UpdateUser();
                        }
                        else
                        {
                            this.ShowMessageBox(
                                errorMessage,
                                actionName + "失敗",
                                isAnimation,
                                () =>
                                {
                                    this.isExecuting = false;
                                });
                        }
                    });
                }
            }
        }

    }
}
