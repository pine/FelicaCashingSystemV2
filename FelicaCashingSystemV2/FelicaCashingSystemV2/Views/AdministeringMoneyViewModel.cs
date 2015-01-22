using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FelicaMail;

namespace FelicaCashingSystemV2.Views
{
    class AdministeringMoneyViewModel : AdministeringUserViewModel
    {
        public const string MESSAGE_PREFIX = "管理者による代理操作を行いました。\n\n";
        public const string MESSAGE_SUFFIX = "\n\n利用者にはメールにて通知を行いました。";

        public AdministeringMoneyViewModel()
        {
            this.MoneyActionSucceeded += AdministeringMoneyViewModel_MoneyActionSucceeded;
        }

        /// <summary>
        /// アクション成功時に発生するイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdministeringMoneyViewModel_MoneyActionSucceeded(object sender, MoneyActionSucceededEventArgs e)
        {
            var user = App.Current.Collections.Users.GetUser(e.UserId);
            var adminUser = App.Current.Collections.Users.GetUser(e.PerformerUserId);

            if (user == null) { return; }
            if (adminUser == null) { return; }

            var args = new AdminMoneyArgs {
                Name = user.Name,
                AdminName = adminUser.Name,
                MoneyAfter = user.Money.ToCommaString(),
                MoneyBefore = (user.Money - e.MoneyDiff).ToCommaString(),
                MoneyDiff = e.MoneyDiff.ToCommaString()
            };

            Task.Run(() =>
            {
                App.Current.Mailer.SendAdminMoney(user.Email, args);
            });
        }

        protected override string GetBuyMessage(int money)
        {
            return MESSAGE_PREFIX + base.GetBuyMessage(money) + MESSAGE_SUFFIX;
        }

        protected override string GetChargeMessage(int money)
        {
            return MESSAGE_PREFIX + base.GetChargeMessage(money) + MESSAGE_SUFFIX;
        }

        protected override string GetWithdrawMessage(int money)
        {
            return MESSAGE_PREFIX + base.GetWithdrawMessage(money) + MESSAGE_SUFFIX;
        }
    }
}
