using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using WpfCommonds;
using System.Windows;

namespace FelicaCashingSystemV2
{
    public static class MetroDialogMessageReceiver
    {
        public static void SetDialogMessageReceiver(this MetroWindow window)
        {
            Messenger.Default.Register<MetroDialogMessage, MessageDialogResult>(
                window,
                (message, cb) => {
                    MetroDialogMessageReceiver.ShowMessage(message, window, cb);
                });
        }

        private static void ShowMessage(
            MetroDialogMessage message, 
            FrameworkElement element, 
            Action<MessageDialogResult> cb
            )
        {
            var window = (MetroWindow)element;
            var settings = new MetroDialogSettings
            {
                AnimateShow = message.IsAnimation,
                AnimateHide = message.IsAnimation
            };

            window.Dispatcher.BeginInvoke((Action)(() =>
            {
                var task = window.ShowMessageAsync(
                    message.Title,
                    message.Message,
                    message.DialogStyle,
                    settings
                    );

                task.ContinueWith(x =>
                {
                    cb(task.Result);
                });
            }));
        }
    }
}
