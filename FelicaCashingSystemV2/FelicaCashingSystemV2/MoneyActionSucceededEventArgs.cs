using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaCashingSystemV2
{
    class MoneyActionSucceededEventArgs
    {
        public string UserId;
        public string PerformerUserId;
        public int MoneyDiff;
    }
}
