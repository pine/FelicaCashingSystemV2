using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaData
{
    [Serializable]
    public class MoneyHistory : RavenModel
    {
        public MoneyHistory()
        {
            this.IsCancel = false;
            this.Money = 0;
        }

        public MoneyHistory Clone()
        {
            return (MoneyHistory)this.MemberwiseClone();
        }

        public int UserId { get; set; }
        public int PerformerUserId { get; set; }
        public bool IsCancel { get; set; }
        public bool IsBuy { get; set; }
        public int Money { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
