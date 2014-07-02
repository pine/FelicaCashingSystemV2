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
using MahApps.Metro.Controls;
using WpfCommonds;
using System.Diagnostics;

namespace FelicaCashingSystemV2.Windows
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            
            this.SetEscClosableWindow();

            this.DataContext = new MainWindowViewModel();
            this.SetDialogMessageReceiver();
        }

        private void SystemInformationButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.ShowInformationWindow();
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            App.Current.ShowProfileWindow();
        }
    }
}
