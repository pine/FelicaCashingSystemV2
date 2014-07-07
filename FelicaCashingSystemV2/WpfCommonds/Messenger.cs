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
    public delegate void MessengerDelegate<T>(T message);
    public delegate void MessengerDelegate<T, R>(T message, Action<R> callback);

    public class Messenger
    {
        private static Messenger defaultInstance = new Messenger();
        private List<ActionInfo> list = new List<ActionInfo>();

        public static Messenger Default
        {
            get { return Messenger.defaultInstance; }
        }
        
        public void Register<T>(
            FrameworkElement recipient,
            MessengerDelegate<T> action
            )
            where T: MessageBase
        {
            this.Register<T>(recipient, (Delegate)action);
        }

        public void Register<T, R>(
            FrameworkElement recipient,
            MessengerDelegate<T, R> action
            )
            where T: MessageBase
        {
            this.Register<T>(recipient, (Delegate)action);
        }

        public void Register<T>(
            ViewModelBase recipient,
            MessengerDelegate<T> action
            )
            where T : MessageBase
        {
            this.Register<T>(recipient, (Delegate)action);
        }

        public void Register<T, R>(
            ViewModelBase recipient,
            MessengerDelegate<T, R> action
            )
            where T : MessageBase
        {
            this.Register<T>(recipient, (Delegate)action);
        }

        private void Register<T>(FrameworkElement recipient, Delegate action)
            where T: MessageBase
        {
            list.Add(new ActionInfo
            {
                Type = typeof(T),
                View = recipient,
                ViewModel = (ViewModelBase)recipient.DataContext,
                Action = action
            });
        }

        private void Register<T>(ViewModelBase viewModel, Delegate action)
            where T : MessageBase
        {
            list.Add(new ActionInfo
            {
                Type = typeof(T),
                ViewModel = viewModel,
                Action = action
            });
        }

        public void Unregister<TMessage>(Type viewOrViewModel)
        {
            list.RemoveAll(x => x.Type == typeof(TMessage) && (x.View.GetType() == viewOrViewModel || x.ViewModel.GetType() == viewOrViewModel));
        }

        public void Send<T>(ViewModelBase sender, T message)
            where T: MessageBase
        {
            var query = from info in this.list
                        where info.ViewModel == sender &&
                            info.Type == typeof(T) &&
                            info.Action is MessengerDelegate<T> &&
                            info.View != null
                        select info;

            message.Sender = sender;

            foreach (var info in query)
            {
                ((MessengerDelegate<T>)info.Action)(message);
            }
        }

        public void Send<T, R>(ViewModelBase sender, T message, Action<R> cb)
            where T : MessageBase
        {
            var query = from info in list
                        where info.ViewModel == sender &&
                            info.Type == message.GetType() &&
                            info.Action is MessengerDelegate<T, R> &&
                            info.View != null
                        select info;

            message.Sender = sender;

            foreach (var info in query)
            {
                ((MessengerDelegate<T, R>)info.Action)(message, cb);
            }
        }

        public void Send<T>(FrameworkElement sender, T message)
            where T : MessageBase
        {
            var query = from info in this.list
                        where info.ViewModel == sender.DataContext &&
                            info.Type == typeof(T) &&
                            info.Action is MessengerDelegate<T> &&
                            info.View == null
                        select info;

            message.Sender = sender;

            foreach (var info in query)
            {
                ((MessengerDelegate<T>)info.Action)(message);
            }
        }

        public void Send<T, R>(FrameworkElement sender, T message, Action<R> cb)
            where T : MessageBase
        {
            var query = from info in list
                        where info.ViewModel == sender.DataContext &&
                            info.Type == message.GetType() &&
                            info.Action is MessengerDelegate<T, R> &&
                            info.View == null
                        select info;

            message.Sender = sender;

            foreach (var info in query)
            {
                ((MessengerDelegate<T, R>)info.Action)(message, cb);
            }
        }

        private class ActionInfo
        {
            public Type Type { get; set; }
            public FrameworkElement View { get; set; }
            public ViewModelBase ViewModel { get; set; }
            public Delegate Action { get; set; }
        }
    }
}
