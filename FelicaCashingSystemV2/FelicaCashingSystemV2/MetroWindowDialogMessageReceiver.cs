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
    public static class MetroWindowDialogMessageReceiver
    {
        public static void SetDialogMessageReceiver(this MetroWindow window)
        {
            Messenger.Default.Register<DialogMessage>(
                window, MetroWindowDialogMessageReceiver.ShowMessage);
        }

        private static void ShowMessage(DialogMessage message, Window window)
        {
            ((MetroWindow)window).ShowMessageAsync(
                message.Title,
                message.Message
                );
        }
    }
}
