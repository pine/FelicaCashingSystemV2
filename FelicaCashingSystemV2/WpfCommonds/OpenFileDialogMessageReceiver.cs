using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace WpfCommonds
{
    public static class OpenFileDialogMessageReceiver
    {
        public static void SetOpenFileDialogReceiver(this Window window)
        {
            Messenger.Default.Register<OpenFileDialogMessage>(
                window,
                OpenFileDialogMessageReceiver.ShowOpenFileDialog
                );
        }

        private static void ShowOpenFileDialog(OpenFileDialogMessage message, Window window)
        {
            var ofd = new OpenFileDialog();

            ofd.Filter = message.Filter;

            var userClickedOk = ofd.ShowDialog();
            message.UserClickedOk = userClickedOk.GetValueOrDefault(false);

            if (userClickedOk == true)
            {
                message.FileName = ofd.FileName;
            }
        }
    }
}
