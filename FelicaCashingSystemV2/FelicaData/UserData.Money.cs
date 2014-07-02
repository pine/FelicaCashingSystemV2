﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaData
{
    public partial class UserData
    {
		/// <summary>
		/// 購入処理
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="money"></param>
		/// <returns></returns>
        public bool Buy(
            int userId, 
            int money,
            int performerUserId = 0,
            string comment = null
            )
        {
            User performerUser = null;
            var user = this.GetUser(userId);

			// 実行者が未指定な場合、本人の購入とみなす
			// 他者の購入
            if (performerUserId != 0)
            {
                performerUser = this.GetUser(performerUserId);

				// 他者の購入でコメントが存在しない場合
                if (string.IsNullOrWhiteSpace(comment))
                {
                    return false;
                }
            }
            else
            {
				// 本人の購入
                performerUserId = userId;
                performerUser = user;
                comment = string.Empty;
            }

            if (user != null && performerUser != null)
            {
                // 履歴の追加
                var history = new FelicaData.MoneyHistory
                {
                    UserId = userId,
                    PerformerUserId = performerUserId,
                    Money = money,
					Comment = comment
                };

                user.Money -= money;

                this.UpdateUser(user);
                this.CreateMoneyHistory(history);
                
                return true;
            }

            return false; // 失敗
        }

        public MoneyHistory CreateMoneyHistory(MoneyHistory history)
        {
            var user = this.GetUser(history.UserId);

            if (user != null)
            {
                return this.Create(history);
            }

            return null;
        }

        public List<MoneyHistory> GetMoneyHistories(int userId)
        {
            return this.Query<MoneyHistory>(h => h.UserId == userId);
        }

    }
}