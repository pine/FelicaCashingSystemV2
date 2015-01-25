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

        public MoneyViewModel(
            FelicaData.UiPageType pageType
            )
        {
            this.pageType = pageType;

            this.UpdateMoneyTiles();
            App.Current.Collections.UiPages.Changed += this.UiData_Changed;

            this.BuyCommand = new DelegateCommand<int>(this.Buy);
            this.ChargeCommand = new DelegateCommand<int>(this.Charge);
            this.WithdrawCommand = new DelegateCommand<int>(this.Withdraw);
        }

        ~MoneyViewModel()
        {
            if (App.Current != null && App.Current.Collections != null)
            {
                App.Current.Collections.UiPages.Changed -= this.UiData_Changed;
            }
        }

        private void UiData_Changed(object sender, object e)
        {
            this.UpdateMoneyTiles();
        }

        public event EventHandler<MoneyActionSucceededEventArgs> MoneyActionSucceeded;

        private void OnMoneyActionSucceededEvent(
            string userId,
            string performerUserId,
            int moneyDiff
            )
        {
            var args = new MoneyActionSucceededEventArgs();

            args.UserId = userId;
            args.PerformerUserId = performerUserId;
            args.MoneyDiff = moneyDiff;

            this.OnMoneyActionSucceededEvent(args);
        }

        protected virtual void OnMoneyActionSucceededEvent(MoneyActionSucceededEventArgs e)
        {
            var handler = this.MoneyActionSucceeded;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        private FelicaData.User user = null;

        /// <summary>
        /// 処理対象のユーザーを表す
        /// </summary>
        protected virtual FelicaData.User User
        {
            get
            {
                if (this.user == null)
                {
                    return App.Current.User;
                }

                return user;
            }
            set
            {
                this.user = value;
            }
        }

        protected virtual void UpdateMoneyTiles()
        {
            if (this.pageType != FelicaData.UiPageType.Unknown)
            {
                var list = new List<int>();
                try
                {
                    var page = App.Current.Collections.UiPages.GetUiPage(this.pageType);

                    if (page != null && page.MoneyTiles != null)
                    {
                        list.AddRange(page.MoneyTiles);
                    }
                }
                catch (FelicaData.DatabaseException e) {
                    Debug.WriteLine(e);
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
                this.GetBuyMessage,
                "購入に失敗しました",
                (newMoney) =>
                {
                    if (App.Current.Collections.Users.Buy(this.User.Id, newMoney, App.Current.User.Id))
                    {
                        this.OnMoneyActionSucceededEvent(this.User.Id, App.Current.User.Id, -newMoney);
                        return true;
                    }

                    return false;
                });
        }

        /// <summary>
        /// 購入時のメッセージを返す
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        protected virtual string GetBuyMessage(int money)
        {
            return "「 " + money.ToCommaStringAbs() + " 」 円の商品を購入しました。\n" +
                    "残高は 「 " + (this.User.Money - money).ToCommaString() + " 」 円です。";
        }

        public ICommand ChargeCommand { get; private set; }
        private void Charge(int money)
        {
            this.MoneyExecute(
                money,
                () => this.GetExecuteMax(FelicaData.UiPageType.Charging),
                true,
                "チャージ",
                this.GetChargeMessage,
                "チャージに失敗しました",
                (newMoney) =>
                {
                    if (App.Current.Collections.Users.Charge(this.User.Id, newMoney, App.Current.User.Id))
                    {
                        this.OnMoneyActionSucceededEvent(this.User.Id, App.Current.User.Id, newMoney);
                        return true;
                    }

                    return false;
                });
        }

        /// <summary>
        /// チャージ時のメッセージを返す
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        protected virtual string GetChargeMessage(int money)
        {
            return "「 " + money.ToCommaStringAbs() + " 」 円をチャージしました。\n" +
                    "残高は 「 " + (this.User.Money + money).ToCommaString() + " 」 円です。";
        }

        public ICommand WithdrawCommand { get; private set; }
        private void Withdraw(int money) 
        {
            this.MoneyExecute(
                money,
                () => this.GetExecuteMax(FelicaData.UiPageType.Withdrawing),
                true,
                "出金",
                this.GetWithdrawMessage,
                "出金に失敗しました",
                (newMoney) =>
                {
                    if (App.Current.Collections.Users.Withdraw(this.User.Id, newMoney, App.Current.User.Id))
                    {
                        this.OnMoneyActionSucceededEvent(this.User.Id, App.Current.User.Id, -newMoney);
                        return true;
                    }

                    return false;
                });
        }

        /// <summary>
        /// 引き出し時のメッセージを返す
        /// </summary>
        /// <param name="money"></param>
        /// <returns></returns>
        protected virtual string GetWithdrawMessage(int money){
            return "「 " + money.ToCommaStringAbs() + " 」 円を引き出しました。\n" +
                    "残高は 「 " + (this.User.Money - money).ToCommaString() + " 」 円です。";
        }

        /// <summary>
        /// 金額を選択する。
        /// </summary>
        private void SelectMoney(int max, Action<int> cb)
        {
            App.Current.ShowSelectingMoneyWindow(max, cb);
        }

        /// <summary>
        /// 取引の最大値を取得する
        /// </summary>
        /// <param name="pageType"></param>
        /// <returns>失敗時は -1 を返す</returns>
        private int GetExecuteMax(FelicaData.UiPageType pageType)
        {
            try
            {
                var page = App.Current.Collections.UiPages.GetUiPage(pageType);
                return page.MaxMoney;
            }
            catch (FelicaData.DatabaseException e)
            {
                Debug.WriteLine(e);
                return -1;
            }
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
                    if (moneyAction(money))
                    {
                        this.ShowMessageBox(
                            succeedMessage(money),
                            actionName + "成功",
                            isAnimation,
                            () =>
                            {
                                this.isExecuting = false;
                            });
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
                }
            }
        }

    }
}
