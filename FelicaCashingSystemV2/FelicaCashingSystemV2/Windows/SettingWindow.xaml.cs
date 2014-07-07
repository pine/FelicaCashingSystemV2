using MahApps.Metro.Controls;
using WpfCommonds;

namespace FelicaCashingSystemV2.Windows
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingWindow : MetroWindow
    {
        public SettingWindow()
        {
            InitializeComponent();
            this.SetEscClosableWindow();

            Messenger.Default.Unregister<DialogMessage>(typeof(Views.PageSettingViewModel));
            this.DataContext = new SettingWindowModel();
        }
    }
}
