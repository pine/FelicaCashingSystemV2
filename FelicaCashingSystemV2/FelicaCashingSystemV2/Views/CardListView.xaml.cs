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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfCommonds;

namespace FelicaCashingSystemV2.Views
{
    /// <summary>
    /// VersionInformationView.xaml の相互作用ロジック
    /// </summary>
    public partial class CardListView : UserControl
    {
        public CardListView()
        {
            InitializeComponent();

            this.DataContext = new CardListViewModel();
            this.SetDialogMessageReceiver();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            App.Current.ShowLoginWindow();
        }
    }
}
