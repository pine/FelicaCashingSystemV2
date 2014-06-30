using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaData
{
    public class UserData : RavenData
    {
        private const string CONNECTION_STRING_NAME = "UserData";

        public UserData()
            : base(CONNECTION_STRING_NAME)
        {
        }

        #region User

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
        public User CreateUser(User user, Card card)
        {
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

            return null;
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

        #endregion

        #region Card

        public Card GetCard(int cardId)
        {
            return this.Query<Card>(c => c.Id == cardId).FirstOrDefault();
        }

        public Card GetCard(string uid)
        {
            return this.Query<Card>(c => c.Uid == uid).FirstOrDefault();
        }

        public Card GetCardByName(int userId, string name)
        {
            return this.Query<Card>(c => c.UserId == userId && c.Name == name).FirstOrDefault();
        }

        public List<Card> GetCards(int userId)
        {
            return this.Query<Card>(c => c.UserId == userId).ToList();
        }

        public Card CreateCard(Card card)
        {
            var user = this.GetUser(card.UserId);
            var sameUid = this.GetCard(card.Uid);
            var sameName = this.GetCardByName(card.UserId, card.Name);

            if (user == null)
            {
                throw new DatabaseException("ユーザーが存在しません。");
            }

            if (sameUid != null)
            {
                throw new DatabaseException("既にカードが登録されています。");
            }

            if (sameName != null)
            {
                throw new DatabaseException("既に同じ名前のカードが存在します。");
            }

            return this.Create(card);
        }

        public void UpdateCard(Card card)
        {
            var oldCard = this.GetCard(card.Id);

            if (oldCard != null)
            {
                // 変更不可
                card.Uid = oldCard.Uid;
                card.UserId = oldCard.UserId;

                this.Update(card);
            }
        }

        #endregion
    }
}
