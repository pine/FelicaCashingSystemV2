using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace FelicaCashingSystemV2.Views
{
    /// <summary>
    /// VersionInformationView.xaml の相互作用ロジック
    /// </summary>
    public partial class SideProfileView : UserControl
    {
        public SideProfileView()
        {
            InitializeComponent();
            this.DataContext = new SideProfileViewModel();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // double clicked
            if (e.IsDoubleClicked())
            {
                App.Current.ShowProfileWindowWithAvatar();
            }
        }

        
        private void Image_DoubleClicked(MouseButtonEventArgs e, string uri)
        {
            if (e.IsDoubleClicked())
            {
                Task.Run(() =>
                {
                    Process.Start(uri);
                });
            }
        }

        private void imageRobotClub_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Image_DoubleClicked(e, Properties.Settings.Default.RobotClubUri);
        }

        private void imageFelica_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Image_DoubleClicked(e, Properties.Settings.Default.FelicaUri);
        }
    }
}
