using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
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
        private SynchronizedCollection<Window> windows
            = new SynchronizedCollection<Window>();
        private FelicaSharp.EasyFelicaReader felica = null;

        public FelicaSharp.EasyFelicaCardSetEventHandlerArgs UnregisteredCard { get; private set; }
        public FelicaData.User User { get; private set; }
        public FelicaData.Card Card { get; private set; }
        public FelicaData.UserData UserData { get; private set; }
        public FelicaMail.Mailer Mailer { get; private set; }

        public static new App Current {
            get
            {
                return (App)Application.Current;
            }
        }

        protected void ShowWindow<T>()
            where T: Window, new()
        {
            this.ShowDialog<T>(true, false);
        }

        public void ShowDialog<T>(
            bool isSingle = false,
            bool isBlocking = true
            )
            where T: Window, new()
        {
            this.Dispatcher.BeginInvoke((Action)(() =>
            {
                if (isSingle && this.IsShownWindow<T>())
                {
                    return;
                }

                var window = new T();
                window.Topmost = true;

                this.windows.Add(window);

                if (isBlocking)
                {                    
                    window.ShowDialog();
                    this.windows.Remove(window);
                }

                else
                {
                    window.Show();
                    window.Topmost = false;
                    window.Activate();
                }
            }));
        }

        protected bool IsShownWindow<T>()
            where T: Window
        {
            lock (this.windows.SyncRoot)
            {
                foreach (var window in this.windows)
                {
                    if (window is T && window.IsVisible)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// メインウィンドウを指定したユーザーでログインして表示する。
        /// </summary>
        /// <param name="user">ログインするユーザー</param>
        public void ShowMainWindow(FelicaData.User user)
        {
            this.CloseAllWindows();

            this.User = user;
            this.ShowWindow<Windows.MainWindow>();
        }

        public void ShowLoginWindow()
        {
            if (!this.IsShownWindow<Windows.LoginWindow>())
            {
                this.CloseAllWindows();
                this.ShowWindow<Windows.LoginWindow>();
            }
        }

        protected void ShowRegisterWindow(
            FelicaSharp.EasyFelicaCardSetEventHandlerArgs unregisteredCard)
        {
            this.CloseAllWindows();
            this.UnregisteredCard = unregisteredCard;
            this.ShowWindow<Windows.RegisterWindow>();
        }

        public void ShowInformationWindow()
        {
            this.ShowDialog<Windows.InformationWindow>();
        }

        public void ShowProfileWindow()
        {
            this.ShowDialog<Windows.ProfileWindow>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        /// <param name="timeout">タイムアウトをミリ秒で指定します。</param>
        public void ShowBalloonTip(
            string title, 
            string text, 
            NotifyIcon.ToolTipIcon icon = NotifyIcon.ToolTipIcon.None,
            int timeout = 20000
            )
        {
            if (this.notifyIcon != null)
            {
                this.notifyIcon.ShowBalloonTip(title, text, icon, timeout);
            }
        }

        /// <summary>
        /// 表示されているウィンドウをすべて閉じる
        /// </summary>
        public void CloseAllWindows()
        {
            lock (this.windows.SyncRoot)
            {
                foreach (var window in this.windows)
                {
                    window.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        window.Close();
                    }));
                }

                this.windows.Clear();
            }

            this.User = null;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            this.notifyIcon = new NotifyIcon(new Uri("pack://application:,,,/Resources/FelicaIcon.ico"));
            this.notifyIcon.Click += this.notifyIcon_Click;
            this.notifyIcon.ExitClick += this.notifyIcon_ExitClick;

            this.ShowBalloonTip(
                "起動中",
                "Felica Cashing Sytem V2 を起動しています。しばらくお待ちください。",
                NotifyIcon.ToolTipIcon.Info,
                30000
                );

            this.felica = new FelicaSharp.EasyFelicaReader();
            this.felica.FelicaCardSet += felica_FelicaCardSet;

            this.UserData = new FelicaData.UserData();

            /*
            // Test code
            this.UserData.CreateUser(new FelicaData.User
            {
                Name = "tester",
                Email = "tester@tester.jp",
                IsAdmin = true,
                Password = "tester"
            });
            */

            this.Mailer = new FelicaMail.Mailer();

            var settings = FelicaCashingSystemV2.Properties.Settings.Default;
            this.Mailer.Setup(
                settings.MailFrom,
                settings.SmtpHost,
                settings.SmtpPort,
                settings.SmtpUser,
                settings.SmtpPassword
                );

            try
            {
                this.felica.Connect();
            }

            catch (FelicaSharp.FelicaException ee)
            {
                Debug.WriteLine(ee.Message);
            }
            
            Debug.WriteLine("Startup succeed");

            this.ShowBalloonTip(
                "起動完了",
                "Felica Cashing Sytem V2 の起動が完了しました。",
                NotifyIcon.ToolTipIcon.Info
                );
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            this.CloseAllWindows();

            if (this.notifyIcon != null) { this.notifyIcon.Dispose(); }
            if (this.felica != null) { this.felica.Dispose(); }

            if (this.UserData != null) { this.UserData.Dispose(); }

        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
            if (!this.IsShownWindow<Windows.MainWindow>())
            {
                this.ShowLoginWindow();
            }
        }

        /// <summary>
        /// プログラムを終了する唯一のメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_ExitClick(object sender, EventArgs e)
        {
            this.Shutdown();
        }

        private void felica_FelicaCardSet(object sender, FelicaSharp.EasyFelicaCardSetEventHandlerArgs e)
        {
            Debug.WriteLine("felica_FelicaCardSet");
            Debug.WriteLine("Idm = " + e.Idm + ", Pmm = " + e.Pmm);

            this.Card = this.UserData.GetCard(e.Idm);
            this.User = null;

            // カードが存在する場合
            if (this.Card != null)
            {
                Debug.WriteLine(
                    "CardId = " + this.Card.Id +
                    ", Name = " + this.Card.Name +
                    ", UserId = " + this.Card.UserId);

                this.User = this.UserData.GetUser(this.Card.UserId);
            }

            // ユーザーが存在する場合
            if (this.User != null)
            {
                Debug.WriteLine(
                    "UserId = " + this.User.Id +
                    ", Name = " + this.User.Name +
                    ", Email = " + this.User.Email +
                    ", IsAdmin = " + this.User.IsAdmin.ToString() +
                    ", Password = " + this.User.Password);

                // メインウィンドウを表示
                this.ShowMainWindow(this.User);
            }
            else
            {
                // 登録ウィンドウを表示
                this.ShowRegisterWindow(e);
            }

        }

    }
}
