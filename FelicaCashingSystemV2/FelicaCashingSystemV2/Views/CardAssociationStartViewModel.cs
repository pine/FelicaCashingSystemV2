using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfCommonds;

namespace FelicaCashingSystemV2.Views
{
    class CardAssociationStartViewModel : MetroWindowViewModelBase
    {
        public CardAssociationStartViewModel()
        {
            this.AssociationStartCommand = new DelegateCommand(this.AssociationStart);
        }

        public ICommand AssociationStartCommand { get; private set; }
        private void AssociationStart()
        {
            App.Current.StartAssociating();
        }
    }
}
