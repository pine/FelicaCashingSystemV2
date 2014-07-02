using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCommonds
{
    public abstract class MessageBase
    {
        public ViewModelBase Sender {
            get; 
            internal set; 
        }
    }
}
