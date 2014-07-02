using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfCommonds
{
    public class Messenger
    {
        private static Messenger defaultInstance = new Messenger();
        private List<ActionInfo> list = new List<ActionInfo>();

        public static Messenger Default
        {
            get { return Messenger.defaultInstance; }
        }

        public void Register<TMessage>(Window recipient, Action<TMessage, Window> action)
            where TMessage: MessageBase
        {
            list.Add(new ActionInfo
            {
                Type = typeof(TMessage),
                Window = recipient,
                Sender = recipient.DataContext as ViewModelBase,
                Action = action
            });
        }

        public void Send<TMessage>(ViewModelBase sender, TMessage message)
            where TMessage: MessageBase
        {
            var query = list
                .Where(o => o.Sender == sender && o.Type == message.GetType())
                .Select(o => o);
            message.Sender = sender;

            foreach (var info in query)
            {

                ((Action<TMessage, Window>)info.Action)(message, info.Window);
            }
        }

        private class ActionInfo
        {
            public Type Type { get; set; }
            public Window Window { get; set; }
            public ViewModelBase Sender { get; set; }
            public Delegate Action { get; set; }
        }
    }
}
