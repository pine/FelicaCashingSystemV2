using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfCommonds;

namespace FelicaCashingSystemV2.Windows
{
    /// <summary>
    /// RegisterWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SelectingMoneyWindow : MetroWindow
    {
        public SelectingMoneyWindow()
        {
            InitializeComponent();
            this.SetEscClosableWindow();

            this.DataContext = new SelectingMoneyWindowViewModel();
            this.SetDialogMessageReceiver();

            this.RegisterMessenger<SelectingMoneyMessage>(message =>
            {
                this.Money = message.Money;

                this.Dispatcher.BeginInvoke((Action)(() =>
                {
                    this.Close();
                }));
            });
        }

        /// <summary>
        /// 選択された金額。入力が正しく終了しなかった場合は 0 が格納される。
        /// </summary>
        public int Money { get; private set; }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.textBoxMoney.Focus();
        }
    }
}
