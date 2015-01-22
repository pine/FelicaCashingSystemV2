using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class AdministeringMailView : UserControl
    {
        private AdministeringMailViewModel vm = new AdministeringMailViewModel();

        public AdministeringMailView()
        {
            InitializeComponent();

            this.DataContext = vm;
            this.SetDialogMessageReceiver();
        }

        /// <summary>
        /// 管理対象のユーザー情報
        /// </summary>
        public FelicaData.User AdministeringUser
        {
            get {
                return this.vm.AdministeringUser;
            }
            set {
                this.vm.AdministeringUser = value;
            }
        }

        private void comboTemplates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.textboxMessage.Focus();
        }
    }
}
