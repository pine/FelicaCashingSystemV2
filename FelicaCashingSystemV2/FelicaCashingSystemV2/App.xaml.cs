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
                this.windows.Add(window);

                if (isBlocking)
                {                    
                    window.ShowDialog();
                    this.windows.Remove(window);
                }

                else
                {
                    window.Show();
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

        protected void ShowRegisterWindow()
        {
            this.CloseAllWindows();
            this.ShowWindow<Windows.RegisterWindow>();
        }

        public void ShowInformationWindow()
        {
            this.ShowDialog<Windows.InformationWindow>();
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
            this.notifyIcon = new NotifyIcon("FelicaIcon.ico");
            this.notifyIcon.Click += this.notifyIcon_Click;
            this.notifyIcon.ExitClick += this.notifyIcon_ExitClick;

            this.felica = new FelicaSharp.EasyFelicaReader();
            this.felica.FelicaCardSet += felica_FelicaCardSet;

            this.UserData = new FelicaData.UserData();

            try
            {
                this.felica.Connect();
            }

            catch (FelicaSharp.FelicaException ee)
            {
                Debug.WriteLine(ee.Message);
            }
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            if (this.notifyIcon != null) { this.notifyIcon.Dispose(); }
            if (this.felica != null) { this.felica.Dispose(); }

            if (this.UserData != null) { this.UserData.Dispose(); }
        }

        private void notifyIcon_Click(object sender, EventArgs e)
        {
//            this.ShowLoginWindow();
            this.ShowRegisterWindow();
        }

        private void notifyIcon_ExitClick(object sender, EventArgs e)
        {
            this.Shutdown();
        }

        private void felica_FelicaCardSet(object sender, FelicaSharp.EasyFelicaCardSetEventHandlerArgs e)
        {
            Debug.WriteLine("felica_FelicaCardSet");
            Debug.WriteLine("Idm = " + e.Idm + ", Pmm = " + e.Pmm);
            
            this.Card = this.UserData.GetCard(e.Idm);

            if (this.Card != null)
            {
                this.User = this.UserData.GetUser(this.Card.UserId);
            }
            else
            {
                this.User = null;
            }

            if (this.User != null)
            {
                Debug.WriteLine("UserName = " + this.User.Name +
                    ", Password = " + this.User.Password);

                this.User.Password = "password";
                this.UserData.UpdateUser(this.User);
            }

            // ユーザーが存在しない場合
            if (this.User == null)
            {
                this.UnregisteredCard = e;
                this.ShowRegisterWindow();

                // Test code
                /*
                this.User = this.UserData.CreateUser(new FelicaData.User
                {
                    Name = "MIZUNE Pine"
                });

                this.Card = this.UserData.CreateCard(new FelicaData.Card
                {
                    Name = "学生証",
                    Uid = e.Idm,
                    UserId = this.User.Id
                });
                */
            }
            else
            {
                this.ShowMainWindow(this.User);
            }
        }

    }
}
