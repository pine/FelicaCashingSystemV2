using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaCashingSystemV2.Views
{
    class MainBuyViewModel : MoneyViewModel
    {
        public MainBuyViewModel()
            : base(FelicaData.UiPageType.Buying)
        {
        }
    }
}
