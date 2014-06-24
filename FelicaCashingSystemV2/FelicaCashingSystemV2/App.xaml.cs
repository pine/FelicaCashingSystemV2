using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FelicaCashingSystemV2
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private NotifyIcon notifyIcon = null;
        private List<Window> windows = null;

        protected void ShowMainWindow(Window window)
        {
            this.MainWindow = window;
            window.Show();
        }

        public void ShowDialog(Window window)
        {

        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.notifyIcon = new NotifyIcon("FelicaIcon.ico");
            this.notifyIcon.Click += this.notifyIcon_Click;
            this.notifyIcon.ExitClick += this.notifyIcon_ExitClick;
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (this.notifyIcon != null) this.notifyIcon.Dispose();
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            if (this.MainWindow == null)
            {
                this.ShowMainWindow(new LoginWindow());
            }
        }

        private void notifyIcon_ExitClick(object sender, EventArgs e)
        {
            this.Shutdown();
        }
    }
}
