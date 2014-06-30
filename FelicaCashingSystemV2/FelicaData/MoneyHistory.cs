using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaData
{
    public class MoneyHistory : RavenModel
    {
        public MoneyHistory()
        {
            this.IsCancel = false;
            this.Money = 0;
            this.Comment = string.Empty;
        }

        public int UserId { get; set; }
        public int PerformerUserId { get; set; }
        public bool IsCancel { get; set; }
        public int Money { get; set; }
        public string Comment { get; set; }

    }
}
