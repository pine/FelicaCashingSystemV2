using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfCommonds
{
    public abstract class MessageBase
    {
        public object Sender
        {
            get; 
            internal set; 
        }
    }
}
