using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaData
{
    class UserData : RavenData
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

        public void CreateUser(User user)
        {
            this.Create(user);
        }

        public void UpdateUser(User user)
        {
            this.Update(user);
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

        public List<Card> GetCards(int userId)
        {
            return this.Query<Card>(c => c.UserId == userId).ToList();
        }

        public void CreateCard(Card card)
        {
            this.Create(card);
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
