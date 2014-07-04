using System;
using System.Windows.Input;
using System.Reflection;

namespace WpfCommonds
{
    #region No parameter DelegateCommand
    public sealed class DelegateCommand : ICommand
    {
        private Action _execute;
        private Func<bool> _canExecute;

        public DelegateCommand(Action execute)
            : this(execute, () => true)
        {
        }
        public DelegateCommand(Action execute, Func<bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute()
        {
            return _canExecute();
        }

        public void Execute()
        {
            _execute();
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        #region ICommand
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute();
        }
        void ICommand.Execute(object parameter)
        {
            Execute();
        }
        #endregion
    }
    #endregion

    #region Parameter DelegateCommand
    public sealed class DelegateCommand<T> : ICommand
    {
        private Action<T> _execute;
        private Func<T, bool> _canExecute;

        private static readonly bool IS_VALUE_TYPE;

        static DelegateCommand()
        {
            IS_VALUE_TYPE = typeof(T).IsValueType;
        }


        public DelegateCommand(Action<T> execute)
            : this(execute, o => true)
        {
        }

        public DelegateCommand(Action<T> execute, Func<T, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(T parameter)
        {
            return _canExecute(parameter);
        }

        public void Execute(T parameter)
        {
            _execute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        #region ICommand
        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute(Cast(parameter));
        }

        void ICommand.Execute(object parameter)
        {
            Execute(Cast(parameter));
        }
        #endregion

        /// <summary>
        /// convert parameter value
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private T Cast(object parameter)
        {
            if (parameter == null && IS_VALUE_TYPE)
            {
                return default(T);
            }

            if (parameter is string && IS_VALUE_TYPE)
            {
                object result = ConvertTryParseType.TryParse<T>((string)parameter);

                if (result != null)
                {
                    return (T)result;
                }

                else
                {
                    return default(T);
                }
            }

            return (T)parameter;
        }


        #region Convert TryParse Type

        private static class ConvertTryParseType
        {
            // http://pullup.net/technical/csharp/index.html
            public static object TryParse<X>(string str)
            {
                // 変換タイプ
                Type classType = typeof(X); //←変換後の型。とりあえずintとする

                // 変換後の値を取得するために、値を受け取るオブジェクトを作る
                object ansObject = Activator.CreateInstance(classType);

                // TryParse()に渡す引数を作る
                object[] args = new object[] { str, ansObject }; // 文字列"12345"を数値化させる

                // 型に対応するTryParseを実行
                bool result = (bool)classType.InvokeMember(
                    "TryParse",
                    BindingFlags.InvokeMethod,
                    null,
                    null,
                    args
                    );

                // 失敗したら入力に戻る
                if (!result)
                {
                    return null;
                }

                // 変換に成功したら値を受け入れる
                return args[1]; // objectで返す
            }
        }

        #endregion
    }

    #endregion
}