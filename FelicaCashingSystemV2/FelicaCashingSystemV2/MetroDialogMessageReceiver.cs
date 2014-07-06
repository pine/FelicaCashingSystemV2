using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

using WpfCommonds;
using System.Windows;
using System.Windows.Controls;
using System.Threading;

namespace FelicaCashingSystemV2
{
    public static class MetroDialogMessageReceiver
    {
        public static void SetDialogMessageReceiver(this FrameworkElement element)
        {
            Messenger.Default.Unregister<MetroDialogMessage>(element.GetType());
            Messenger.Default.Register<MetroDialogMessage, MessageDialogResult>(
                element,
                (message, cb) => {
                    MetroDialogMessageReceiver.ShowMessage(message, element, cb);
                });
        }

        private static void ShowMessage(
            MetroDialogMessage message,
            FrameworkElement element,
            Action<MessageDialogResult> cb
            )
        {
            if (!element.Dispatcher.Thread.Equals(Thread.CurrentThread))
            {
                element.Dispatcher.BeginInvoke((Action)(() =>
                {
                    ShowMessage(message, element, cb);
                }));
                return;
            }

            MetroWindow window;

            if (element is MetroWindow)
            {
                window = (MetroWindow)element;
            }
            else
            {
                window = (MetroWindow)Window.GetWindow(element);
            }

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
