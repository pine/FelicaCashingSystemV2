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

namespace FelicaCashingSystemV2.Windows
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class ProfileWindow : MetroWindow
    {
        public ProfileWindow()
        {
            InitializeComponent();
            this.SetEscClosableWindow();

            this.DataContext = new ProfileWindowViewModel();
            this.SetDialogMessageReceiver();
            this.SetOpenFileDialogReceiver();
        }

        /// <summary>
        /// アバタータブを開く
        /// </summary>
        public void SelectAvatarTab()
        {
            this.tabControl.SelectedItem = this.tabItemAvatar;
        }
    }
}
