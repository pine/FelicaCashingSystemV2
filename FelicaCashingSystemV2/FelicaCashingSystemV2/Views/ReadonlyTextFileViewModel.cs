using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaCashingSystemV2.Views
{
    class ReadonlyTextFileViewModel : WpfCommonds.ViewModelBase
    {
        private string content = string.Empty;
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }
    }
}
