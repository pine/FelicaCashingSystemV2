using System.Windows;
using System.Windows.Input;

namespace WpfCommonds
{
    public static class EscClosableWindow
    {
        public static void SetEscClosableWindow(this Window window)
        {
            window.PreviewKeyDown += EscClosableWindow.WindowBase_PreviewKeyDown;
        }

        private static void WindowBase_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ((Window)sender).Close();
            }
        }
    }
}
