using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCommonds;

namespace FelicaCashingSystemV2
{
    abstract class MetroWindowViewModelBase : ViewModelBase
    {
        public bool IsAdmin
        {
            get
            {
                return App.Current.User != null &&
                    App.Current.User.IsAdmin;
            }
        }
        
        /// <summary>
        /// メッセージボックスを表示する
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        public void ShowMessageBox(
            string message, 
            string title = null,
            bool isAnimation = true,
            Action callback = null
            )
        {
            this.ShowDialogMessage(
                message,
                title,
                MessageDialogStyle.Affirmative,
                isAnimation,
                callback: result => {
                    if (callback != null) { callback(); }
                });
        }

        /// <summary>
        /// 確認ダイアログを表示する
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        public void ShowConfirmDialog(
            string message,
            string title = null,
            Action<MessageDialogResult> callback = null
            )
        {
            this.ShowDialogMessage(message, title, MessageDialogStyle.AffirmativeAndNegative, callback: callback);
        }

        /// <summary>
        /// メッセージボックスを表示する
        /// </summary>
        /// <param name="message"></param>
        /// <param name="title"></param>
        private void ShowDialogMessage(
            string message,
            string title,
            MessageDialogStyle style,
            bool isAnimation = true,
            Action<MessageDialogResult> callback = null
            )
        {
            Messenger.Default.Send<MetroDialogMessage, MessageDialogResult>(
                this,
                new MetroDialogMessage
                {
                    Title = title,
                    Message = message,
                    DialogStyle = style,
                    IsAnimation = isAnimation
                },
                result =>
                {
                    if (callback != null)
                    {
                        callback(result);
                    }
                });
        }


    }
}
