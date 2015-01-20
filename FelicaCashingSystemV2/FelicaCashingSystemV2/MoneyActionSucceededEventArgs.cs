using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaCashingSystemV2
{
    class MoneyActionSucceededEventArgs
    {
        public int UserId;
        public int PerformerUserId;
        public int MoneyDiff;
    }
}
