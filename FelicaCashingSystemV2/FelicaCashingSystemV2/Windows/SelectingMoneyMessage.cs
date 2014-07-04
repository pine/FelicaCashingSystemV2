using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCommonds;

namespace FelicaCashingSystemV2.Windows
{
    class SelectingMoneyMessage : MessageBase
    {
        public int Money { get; set; }
    }
}
