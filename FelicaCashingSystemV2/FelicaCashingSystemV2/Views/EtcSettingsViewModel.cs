using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfCommonds;

namespace FelicaCashingSystemV2.Views
{
    class EtcSettingsViewModel : MetroWindowViewModelBase
    {
        public EtcSettingsViewModel()
        {
            this.SaveCommand = new DelegateCommand(this.Save);

            try
            {
                this.AutoLogoutTimeoutSec =
                    App.Current.Collections.UiIntegers.GetInteger(FelicaData.UiIntegerType.AutoLogoutTimeoutSec).ToString();
            }

            catch (FelicaData.DatabaseException)
            {
                this.ErrorMessage = "設定値の読み込みに失敗しました。";
            }
        }

        public ICommand SaveCommand
        {
            get;
            set;
        }

        /// <summary>
        /// 設定を保存する
        /// </summary>
        private void Save()
        {
            this.ErrorMessage = string.Empty;

            int autoLogoutTimeoutSec = 0;
            int autoLogoutTimeoutSecMin =
                FelicaCashingSystemV2.Properties.Settings.Default.AutoLogoutTimerSecMin;

            try
            {
                autoLogoutTimeoutSec = int.Parse(this.AutoLogoutTimeoutSec);
            }

            catch (Exception)
            {
                this.ErrorMessage = "タイムアウト時間の値が不正です。";
                return;
            }

            if (autoLogoutTimeoutSec < 0)
            {
                this.ErrorMessage = "タイムアウト時間に負の数は指定できません。";
                return;
            }

            if (autoLogoutTimeoutSec < autoLogoutTimeoutSecMin)
            {
                this.ErrorMessage =
                    "タイムアウト時間は 「 " + autoLogoutTimeoutSecMin.ToString() + " 」 秒以下には設定できません。";
                return;
            }

            try
            {
                App.Current.Collections.UiIntegers.UpdateInteger(
                    FelicaData.UiIntegerType.AutoLogoutTimeoutSec,
                    autoLogoutTimeoutSec
                    );
            }

            catch (FelicaData.DatabaseException e)
            {
                this.ErrorMessage = e.Message;
                Debug.WriteLine(e);
                return;
            }

            this.ShowMessageBox("設定を保存しました。", "保存成功");
        }

        private string autoLogoutTimeoutSec = "0";

        /// <summary>
        /// 自動ログアウト時のタイムアウト
        /// </summary>
        public string AutoLogoutTimeoutSec
        {
            get
            {
                return this.autoLogoutTimeoutSec;
            }
            set
            {
                this.autoLogoutTimeoutSec = value;
                this.OnPropertyChanged("AutoLogoutTimeoutSec");
            }
        }

        private string errorMessage = null;
        public string ErrorMessage
        {
            get
            {
                return this.errorMessage;
            }
            set
            {
                this.errorMessage = value;
                this.OnPropertyChanged("ErrorMessage");
            }
        }
    }
}
