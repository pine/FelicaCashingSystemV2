using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCommonds;

namespace FelicaCashingSystemV2
{
    abstract class FelicaViewModelBase : ViewModelBase
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
