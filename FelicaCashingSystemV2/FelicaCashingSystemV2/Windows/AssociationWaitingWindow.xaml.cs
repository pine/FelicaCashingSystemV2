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
    public partial class AssociationWaitingWindow : MetroWindow
    {
        public AssociationWaitingWindow()
        {
            InitializeComponent();
            this.SetEscClosableWindow();
        }

        private void MetroWindow_Closed(object sender, EventArgs e)
        {
            App.Current.EndAssociating();
        }
    }
}
