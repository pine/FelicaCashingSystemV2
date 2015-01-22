﻿using System;
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
        public const string MUTEX_NAME = "FelicaCashingSystem_V2";
        /// <summary>
        /// 二重起動防止用の Mutex
        /// </summary>
        System.Threading.Mutex mutex =
            new System.Threading.Mutex(false, MUTEX_NAME);

        private NotifyIcon notifyIcon = null;
        private SynchronizedCollection<Window> windows
            = new SynchronizedCollection<Window>();
        private FelicaSharp.EasyFelicaReader felica = null;
        private bool associationStarted = false;

        public FelicaSharp.EasyFelicaCardSetEventHandlerArgs UnregisteredCard { get; private set; }

        private FelicaData.DatabaseManager DatabaseManager { get; set; }
        public FelicaData.CollectionManager Collections { get; set; }
       // public FelicaData UiData { get; private set; }

        public FelicaData.Card Card { get; private set; }
        public FelicaMail.Mailer Mailer { get; private set; }

        private FelicaData.User user = null;
        public FelicaData.User User
        {
            get { return this.user; }
            private set 
            {
                this.user = value;
                this.OnUserChanged(value);
            }
        }

        public event EventHandler<FelicaData.User> UserChanged;
        
        public static new App Current {
            get
            {
                return (App)Application.Current;
            }
        }

        public void UpdateUser()
        {
            if (this.User != null)
            {
                this.User = this.Collections.Users.GetUser(this.User.Id);
            }
        }

        public void UpdateCard()
        {
            if (this.Card != null)
            {
                this.Card = this.Collections.Cards.GetCard(this.Card.Id);
            }

            else if (this.UnregisteredCard != null)
            {
                this.Card = this.Collections.Cards.GetCardByUid(this.UnregisteredCard.Idm);
            }
        }

        protected void ShowWindow<T>()
            where T: Window, new()
        {
            this.ShowDialog<T>(true, false);
        }

        public void ShowDialog<T>(
            bool isSingle = false,
            bool isBlocking = true,
            Action<T> beforeAction = null,
            Action<T> afterAction = null
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

                if (beforeAction != null)
                {
                    beforeAction(window);
                }

                if (isBlocking)
                {
                    window.ShowDialog();
                    this.windows.Remove(window);

                    if (afterAction != null)
                    {
                        afterAction(window);
                    }
                }

                else
                {
                    window.Topmost = true;
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
            if (this.User != null)
            {
                this.ShowDialog<Windows.ProfileWindow>();
            }
        }

        public void ShowProfileWindowWithAvatar()
        {
            if (this.User != null)
            {
                this.ShowDialog<Windows.ProfileWindow>(
                    beforeAction: (window) =>
                    {
                        window.SelectAvatarTab();
                    });
            }
        }

        public void ShowSelectingMoneyWindow(int max, Action<int> cb)
        {
            this.ShowDialog<Windows.SelectingMoneyWindow>(
                beforeAction: (window) =>{
                    window.MaxMoney = max;
                },
                afterAction: (window) =>
                {
                    cb(window.Money);
                });
        }

        /// <summary>
        /// ユーザーの管理ウィンドウを表示する
        /// </summary>
        /// <param name="administeringUser">管理対象のユーザー</param>
        public void ShowAdministeringUserWindow(FelicaData.User administeringUser)
        {
            if (this.User == null) { return; }
            if (!this.User.IsAdmin) { return; }
            if (administeringUser == null) { return; }
            if (this.User.Id == administeringUser.Id) { return; }

            this.ShowDialog<Windows.AdministeringUserWindow>(
                beforeAction: window =>
                {
                    window.AdministeringUser = administeringUser;
                });
            
        }

        public void ShowSettingWindow()
        {
            if (this.User == null) { return; }

            this.ShowDialog<Windows.SettingWindow>();
        }

        public void ShowAssociationWaitingWindow(FelicaData.User user)
        {
            this.CloseAllWindows();

            this.associationStarted = true;
            this.User = user;
            this.ShowWindow<Windows.AssociationWaitingWindow>();
        }

        public void ShowAssociationWindow(
            FelicaData.User user,
            FelicaSharp.EasyFelicaCardSetEventHandlerArgs unregisteredCard)
        {
            this.CloseAllWindows();

            this.User = user;
            this.UnregisteredCard = unregisteredCard;
            this.ShowWindow<Windows.AssociationWindow>();
        }

        public void StartAssociating()
        {
            if (this.User == null) { return; }
            if (this.associationStarted) { return; }

            this.ShowAssociationWaitingWindow(this.User);
        }

        public void EndAssociating()
        {
            this.associationStarted = false;
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
            this.associationStarted = false;
        }

        protected virtual void OnUserChanged(FelicaData.User user)
        {
            if (this.UserChanged != null)
            {
                this.UserChanged(this, user);
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 二重起動を検出
            if (!this.mutex.WaitOne(0, false))
            {
                // 二重起動検出
                MessageBox.Show(
                    "既に Felica Cashing System V2 は起動しています。同時に複数は起動できません",
                    "起動失敗",
                    MessageBoxButton.OK,
                    MessageBoxImage.Stop
                    );

                this.Shutdown(1);
                return;
            }

            this.notifyIcon = new NotifyIcon(WindowIcon.GetIcon());
            this.notifyIcon.Click += this.notifyIcon_Click;
            this.notifyIcon.ExitClick += this.notifyIcon_ExitClick;

#if DEBUG
            this.ShowBalloonTip(
                "起動中",
                "Felica Cashing Sytem V2 をデバッグモードで起動しています。しばらくお待ちください。",
                NotifyIcon.ToolTipIcon.Warning
                );
#else
            this.ShowBalloonTip(
                "起動中",
                "Felica Cashing Sytem V2 を起動しています。しばらくお待ちください。",
                NotifyIcon.ToolTipIcon.Info
                );
#endif

            try
            {
                this.DatabaseManager = new FelicaData.DatabaseManager();
                this.Collections = new FelicaData.CollectionManager(this.DatabaseManager);
            }
            catch (FelicaData.DatabaseException dbe)
            {
                MessageBox.Show(
                    dbe.Message,
                    "起動に失敗しました",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );

                this.Shutdown(1);
                return;
            }

#if DEBUG
            try
            {
                this.Collections.Users.CreateUser(new FelicaData.User
                {
                    Name = "テスト用ユーザー",
                    Email = "tester@tester.jp",
                    IsAdmin = true,
                    Password = "tester"
                });
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.Message);
            }

            try
            {
                this.Collections.Users.CreateUser(new FelicaData.User
                {
                    Name = "テスト用ユーザー (非管理者)",
                    Email = "tester@tester.jp",
                    IsAdmin = false,
                    Password = "tester"
                });
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.Message);
            }

            try
            {
                this.Collections.Cards.CreateCard(new FelicaData.Card
                {
                    Name = "最初に登録したカード",
                    Uid = "TEST-UID-1"
                });
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.Message);
            }
            
            try
            {
                this.Collections.Cards.CreateCard(new FelicaData.Card
                {
                    Name = "ミールカード",
                    Uid = "TEST-UID-2"
                });
            }
            catch (Exception ee)
            {
                Debug.WriteLine(ee.Message);
            }

#endif

            this.Mailer = new FelicaMail.Mailer();

            var settings = FelicaCashingSystemV2.Properties.Settings.Default;
            this.Mailer.Setup(
                settings.MailFrom,
                settings.MailFromName,
                settings.SmtpHost,
                settings.SmtpPort,
                settings.SmtpUser,
                settings.SmtpPassword
                );


            this.felica = new FelicaSharp.EasyFelicaReader();
            this.felica.FelicaCardSet += felica_FelicaCardSet;

            try
            {
                this.felica.Connect();

                this.ShowBalloonTip(
                    "起動完了",
                    "Felica Cashing Sytem V2 の起動が完了しました。",
                    NotifyIcon.ToolTipIcon.Info
                    );
            }

            catch (FelicaSharp.FelicaException ee)
            {
                Debug.WriteLine(ee.Message);

                this.ShowBalloonTip(
                    "警告",
                    "カードリーダーが検出されませんでした。パスワードログインのみが可能です。",
                    NotifyIcon.ToolTipIcon.Warning
                    );
            }

            Debug.WriteLine("Startup succeed");
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            this.CloseAllWindows();

            if (this.notifyIcon != null) { this.notifyIcon.Dispose(); }
            if (this.felica != null) { this.felica.Dispose(); }

            // Mutex を開放する
            if (this.mutex != null)
            {
                this.mutex.ReleaseMutex();
                this.mutex.Dispose();
                this.mutex = null;
            }
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

            if (this.Collections == null) { return; }

            this.Card = this.Collections.Cards.GetCard(e.Idm);

            // 関連付け
            // 既にカードが登録されていないこと
            if (this.associationStarted && this.User != null && this.Card == null)
            {
                this.ShowAssociationWindow(this.User, e);
                return;
            }

            this.User = null;

            // カードが存在する場合
            if (this.Card != null)
            {
                Debug.WriteLine(
                    "CardId = " + this.Card.Id +
                    ", Name = " + this.Card.Name +
                    ", UserId = " + this.Card.UserId);

                this.User = this.Collections.Users.GetUser(this.Card.UserId);
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
