using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using WpfCommonds;

namespace FelicaCashingSystemV2.Views
{
    class CardListViewModel : MetroWindowViewModelBase
    {
        public CardListViewModel()
        {
            this.DeleteCardCommand = new DelegateCommand<FelicaData.Card>(this.DeleteCard);

            this.UpdateCards();

            App.Current.UserChanged += (s, e) =>
            {
                this.UpdateCards();
            };
        }

        private List<FelicaData.Card> cards = null;
        public List<FelicaData.Card> Cards
        {
            get { return this.cards; }
            set {
                this.cards = value;
                this.OnPropertyChanged("Cards");
            }
        }

        public string CurrentCardId
        {
            get
            {
                if (App.Current.Card != null)
                {
                    return App.Current.Card.Id;
                }

                return null;
            }
        }

        public ICommand DeleteCardCommand { get; private set; }
        private void DeleteCard(FelicaData.Card card)
        {
            if (card == null) { return; }

            this.ShowConfirmDialog(
                "本当にカード 「 " + card.Name + " 」 を削除しますか? 削除したカードでは、今後ログインできません。",
                "削除確認",
                (result) =>
                {
                    if (result == MessageDialogResult.Affirmative)
                    {
                        try
                        {
                            App.Current.Collections.Cards.DeleteCard(card.Id);
                            this.ShowMessageBox("カードの削除に成功しました。", "削除成功");
                        }
                        catch (FelicaData.DatabaseException e)
                        {
                            this.ShowMessageBox(e.Message, "エラー");
                        }

                        App.Current.UpdateUser();
                    }
                });
        }

        /// <summary>
        /// カード一覧を更新する
        /// </summary>
        private void UpdateCards()
        {
            if (App.Current.User != null)
            {
                this.Cards = App.Current.Collections.Cards.GetCardsByUserId(App.Current.User.Id);
            }
        }
    }
}
