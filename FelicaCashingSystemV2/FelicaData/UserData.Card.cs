using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaData
{
    public partial class UserData
    {
        #region Card

        public Card GetCard(int cardId)
        {
            return this.Query<Card>(c => c.Id == cardId).FirstOrDefault();
        }

		/// <summary>
		/// カードを UID で探す
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
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

		/// <summary>
		/// カードを削除します
		/// </summary>
        public void DeleteCard(Card card)
        {
            if (card == null || card.Id == 0)
            {
                throw new DatabaseException("カードが無効です。");
            }

            var sameCard = this.GetCard(card.Id);

            if (sameCard == null)
            {
                throw new DatabaseException("このカードは削除できません。");
            }

            this.Delete(card);
        }

        /// <summary>
        /// カードを既存ユーザーに関連付ける
        /// </summary>
        /// <param name="user"></param>
        /// <param name="card"></param>
        public Card Assoate(Card card)
        {
            var sameIdUser = this.GetUser(card.UserId);

            if (sameIdUser == null)
            {
                throw new DatabaseException("ユーザーが存在しません");
            }

            var sameUidCard = this.GetCard(card.Uid);

            if (sameUidCard != null)
            {
                throw new DatabaseException("カードが既に登録されています。");
            }

            return this.CreateCard(card);
        }

        #endregion
    }
}
