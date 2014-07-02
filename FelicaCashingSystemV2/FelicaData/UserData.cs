using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaData
{
    public partial class UserData : RavenData
    {
        private const string CONNECTION_STRING_NAME = "UserData";

        public UserData()
            : base(CONNECTION_STRING_NAME)
        {
        }

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

        #region Money Histories

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

        #endregion
    }
}
