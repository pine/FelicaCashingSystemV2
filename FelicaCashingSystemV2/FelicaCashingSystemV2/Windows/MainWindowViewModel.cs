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

namespace FelicaCashingSystemV2.Windows
{
    class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            this.BuyCommand = new DelegateCommand<int>(this.Buy);
        }

        public bool IsAdmin
        {
            get
            {
                return App.Current.User != null &&
                    App.Current.User.IsAdmin;
            }
        }

        public ICommand BuyCommand { get; set; }
        private void Buy(int money)
        {
            Debug.WriteLine("Buy money = " + money.ToString());

            if (money <= 0)
            {
                throw new ArgumentOutOfRangeException("money");
            }

            // 購入処理
            if (App.Current.User != null)
            {
                if (App.Current.UserData.Buy(App.Current.User.Id, money))
                {
                    Debug.WriteLine("Buying succeed");

                    MessageBox.Show("#");
                }
                else
                {
                    Debug.WriteLine("Buying error");

                    MessageBox.Show("#");
                    
                    
                }
            }
        }
    }
}
