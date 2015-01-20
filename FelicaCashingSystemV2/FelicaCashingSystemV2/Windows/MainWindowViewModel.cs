using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using WpfCommonds;
using System.Windows.Media;

namespace FelicaCashingSystemV2.Windows
{
    class MainWindowViewModel : MoneyViewModel
    {

        public MainWindowViewModel() : base(FelicaData.UiPageType.Home)
        {
            this.MainBuyCommand = new DelegateCommand<int>(this.MainBuy);
        }

        public ICommand MainBuyCommand
        {
            get;
            private set;
        }

        private void MainBuy(int money)
        {
            this.Buy(money, false);
        }
    }
}
