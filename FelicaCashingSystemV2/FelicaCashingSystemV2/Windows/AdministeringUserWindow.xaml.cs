using MahApps.Metro.Controls;
using System.Diagnostics;
using WpfCommonds;

namespace FelicaCashingSystemV2.Windows
{
    /// <summary>
    /// ユーザー管理のウィンドウ
    /// </summary>
    public partial class AdministeringUserWindow : MetroWindow
    {

        public AdministeringUserWindow()
        {
            InitializeComponent();
            this.SetEscClosableWindow();
        }

        /// <summary>
        /// 管理対象のユーザー
        /// </summary>
        public FelicaData.User AdministeringUser
        {
            set
            {
                this.administeringSideProfileView.AdministeringUser = value;
                this.administeringMoneyView.AdministeringUser = value;
                this.administeringMailView.AdministeringUser = value;
            }
        }
    }
}
