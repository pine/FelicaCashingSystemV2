using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FelicaCashingSystemV2
{
    public static class PasswordBoxUtility
    {
        public static void Clear(this PasswordBox[] passwordBoxes)
        {
            foreach (var passwordBox in passwordBoxes)
            {
                passwordBox.Clear();
            }
        }

        public static void ClearAndFocus(this PasswordBox[] passwordBoxes)
        {
            passwordBoxes.Clear();

            if (passwordBoxes.Length > 0)
            {
                passwordBoxes[0].Focus();
            }
        }

        public static void ClearAndFocus(this PasswordBox passwordBox)
        {
            passwordBox.Clear();
            passwordBox.Focus();
        }
    }
}
