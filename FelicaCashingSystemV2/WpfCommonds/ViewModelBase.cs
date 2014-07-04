using System;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;

namespace WpfCommonds
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected ViewModelBase()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Raise PropertyChanged Event.
        /// </summary>
        /// <param name="names"></param>
        protected virtual void OnPropertyChanged(params string[] names)
        {
            var h = PropertyChanged;
            if (h == null) return;

            CheckPropertyName(names);

            foreach (var name in names)
            {
                h(this, new PropertyChangedEventArgs(name));
            }
        }
        [Conditional("DEBUG")]
        private void CheckPropertyName(params string[] names)
        {
            var props = GetType().GetProperties();
            foreach (var name in names)
            {
                var prop = props.Where(p => p.Name == name).SingleOrDefault();
                if (prop == null) throw new ArgumentException(name);
            }
        }
        protected void OnPropertyChanged<T>(params Expression<Func<T>>[] propertyExpression)
        {
            OnPropertyChanged(
                propertyExpression.Select(ex => ((MemberExpression)ex.Body).Member.Name).ToArray());
        }


        /// <summary>
        /// ファイルを開くダイアログを表示する
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>開いたファイルのパスを返す</returns>
        public IOpenFileDialogResult ShowOpenFileDialog(string filter = null)
        {
            var message = new OpenFileDialogMessage
            {
                Filter = filter
            };

            Messenger.Default.Send(
                this,
                message
                );

            // OK
            if (message.UserClickedOk)
            {
                return message;   
            }

            return null;
        }

        public void RegisterMessenger<T>(MessengerDelegate<T> d)
            where T: MessageBase
        {
            Messenger.Default.Register<T>(this, d);
        }
    }
}
