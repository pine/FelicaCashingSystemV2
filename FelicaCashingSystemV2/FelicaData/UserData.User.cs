using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaData
{
    public partial class UserData
    {
        public List<User> GetUsers()
        {
            return this.Query<User>();
        }

        public User GetUser(int id)
        {
            return this.Query<User>(u => u.Id == id).FirstOrDefault();
        }

        public User GetUserByName(string name)
        {
            return this.Query<User>(u => u.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// ユーザーを新規作成します。
        /// </summary>
        /// <param name="user">新規作成するユーザーの情報</param>
        /// <returns>新規作成に成功した場合、作成したユーザー</returns>
        /// <exception cref="DatabaseException">ユーザーの作成でエラーが発生した場合</exception>
        public User CreateUser(User user, Card card = null)
        {
            if (user == null) { throw new ArgumentNullException("user"); }

            if (user.Name == null)
            {
                throw new DatabaseException("ユーザー名が無効です。");
            }
            else
            {
                // 同じ名前のユーザーが居るか確認
                var sameName = this.GetUserByName(user.Name);
                var sameCard = this.GetCard(card.Uid);

                if (sameName != null)
                {
                    throw new DatabaseException("既に同じ名前のユーザーが存在します。");
                }
                else if (sameCard != null)
                {
                    throw new DatabaseException("既に同じカードが登録されています。");
                }
                else
                {
                    this.Create(user);

                    if (user.Id > 0)
                    {
                        if (card != null)
                        {
                            card.UserId = user.Id;
                            this.CreateCard(card);

                            // カード登録失敗
                            if (card.Id == 0)
                            {
                                this.DeleteUser(user.Id);
                            }
                            else
                            {
                                return user;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public bool Buy(int userId, int money)
        {
            var user = this.GetUser(userId);

            if (user != null)
            {
                // 履歴の追加
                var history = new FelicaData.MoneyHistory
                {
                    UserId = userId,
                    PerformerUserId = userId,
                    Money = money
                };

                user.Money -= money;
                this.UpdateUser(user);

                if (this.CreateMoneyHistory(history) != null)
                {
                    return true;
                }
            }

            return false;
        }

        public void UpdateUser(User user)
        {
            this.Update(user);
        }

        public void DeleteUser(int id)
        {
            var user = this.GetUser(id);
            this.Delete(user);
        }
    }
}
