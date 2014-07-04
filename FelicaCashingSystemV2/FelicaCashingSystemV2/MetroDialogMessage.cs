using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using WpfCommonds;

namespace FelicaCashingSystemV2
{
    public class MetroDialogMessage : DialogMessage
    {
        public MessageDialogStyle DialogStyle { get; set; }
        public bool IsAnimation { get; set; }
    }
}
