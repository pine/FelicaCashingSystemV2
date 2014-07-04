using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FelicaCashingSystemV2
{
    public static class MouseButtonEventArgsUtility
    {
        public static bool IsDoubleClicked(this MouseButtonEventArgs args)
        {
            return args.ChangedButton == MouseButton.Left &&
                args.ClickCount == 2;
        }
    }
}
