using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaCashingSystemV2.Windows
{
    class MainWindowViewModel
    {
        public bool IsAdmin
        {
            get
            {
                return App.Current.User != null &&
                    App.Current.User.IsAdmin;
            }
        }
    }
}
