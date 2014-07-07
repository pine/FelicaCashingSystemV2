using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaCashingSystemV2.Views
{
    class MainWithdrawViewModel : MoneyViewModel
    {
        public MainWithdrawViewModel()
            : base(FelicaData.UiPageType.Withdrawing)
        {
        }
    }
}
