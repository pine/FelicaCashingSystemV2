using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfCommonds
{
    public static class MessengerWindowExtension
    {
        public static void RegisterMessenger<T>(
            this FrameworkElement element, 
            MessengerDelegate<T> callback
            )
            where T: MessageBase
        {
            Messenger.Default.Register<T>(element, callback);
        }

        public static void SendMessage<T>(
            this FrameworkElement element,
            T message
            )
            where T: MessageBase
        {
            Messenger.Default.Send<T>(element, message);
        }
    }
}
