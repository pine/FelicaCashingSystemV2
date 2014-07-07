using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaData
{
    public class UiPageSettings : RavenModel
    {
        public UiPageType Type { get; set; }
        public int[] MoneyTiles { get; set; }
        public int MaxMoney { get; set; }
    }
}
