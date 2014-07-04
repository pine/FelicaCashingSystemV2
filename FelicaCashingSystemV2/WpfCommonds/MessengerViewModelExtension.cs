using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfCommonds
{
    public static class MessengerViewModelExtension
    {
        public static void SendMessage<T>(this ViewModelBase vm, T message)
            where T: MessageBase
        {
            Messenger.Default.Send<T>(vm, message);
        }
    }
}
