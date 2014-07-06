using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaCashingSystemV2.Windows
{
    class AssociationWindowViewModel : MetroWindowViewModelBase
    {
        public AssociationWindowViewModel()
        {
            App.Current.UserChanged += this.App_UserChanged;
        }

        private void App_UserChanged(object sender, FelicaData.User e)
        {
            this.OnPropertyChanged("Name", "Money");
        }

        public string Name
        {
            get
            {
                if (App.Current.User != null)
                {
                    return App.Current.User.Name;
                }

                return null;
            }
        }

        public int Money
        {
            get
            {
                if (App.Current.User != null)
                {
                    return App.Current.User.Money;
                }

                return 0;
            }
        }

        private string cardName = string.Empty;
        public string CardName
        {
            get { return this.cardName; }
            set
            {
                this.cardName = value;
                this.OnPropertyChanged("CardName");
            }
        }
    }
}
