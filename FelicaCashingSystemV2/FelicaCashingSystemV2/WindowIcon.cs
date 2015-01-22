using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FelicaCashingSystemV2
{
    static class WindowIcon
    {
        public const string ICON_URI = "pack://application:,,,/Resources/FelicaIcon.ico";

        public static System.Drawing.Icon GetIcon()
        {
            var iconUri = new Uri(ICON_URI);
            
            using (Stream iconStream = Application.GetResourceStream(iconUri).Stream)
            {
                return new System.Drawing.Icon(iconStream);
            }
        }
    }   
}
