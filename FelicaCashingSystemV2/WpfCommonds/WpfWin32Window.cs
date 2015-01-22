using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace WpfCommonds
{
    /// <summary>
    /// WPF のウィンドウを Windows Forms のウィンドウとして扱うためのラッパーライブラリ
    /// </summary>
    public class Wpf32Window : System.Windows.Forms.IWin32Window
    {
        public IntPtr Handle { get; private set; }

        public Wpf32Window(Window wpfWindow)
        {
            this.Handle = new WindowInteropHelper(wpfWindow).Handle;
        }
    }
}
