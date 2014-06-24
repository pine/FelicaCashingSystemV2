using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FelicaCashingSystemV2.Views
{
    class VersionInformationViewModel : WpfCommonds.ViewModelBase
    {
        public string Version
        {
            get { return SystemInformation.Version; }
        }
    
        public string AppName
        {
            get { return SystemInformation.AppName; }
        }

        public string Copyright
        {
            get { return SystemInformation.AppName; }
        }
    }
}
