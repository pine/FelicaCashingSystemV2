using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;

namespace FelicaCashingSystemV2.Views
{
    /// <summary>
    /// ReadonlyTextFileView.xaml の相互作用ロジック
    /// </summary>
    public partial class ReadonlyTextFileView : UserControl
    {
        private ReadonlyTextFileViewModel vm = new ReadonlyTextFileViewModel();

        public ReadonlyTextFileView()
        {
            InitializeComponent();
            this.DataContext =  vm;
        }

        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                "Source",
                typeof(string),
                typeof(ReadonlyTextFileView),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnSourceChanged)));

        private static void OnSourceChanged(
            DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var self = obj as ReadonlyTextFileView;
            var newValue = e.NewValue as string;

            if (self != null && newValue != null)
            {
                self.LoadTextFile(newValue);
            }
        }

        /// <summary>
        /// 読み込むファイルのパス
        /// </summary>
        public string Source
        {
            get { return (string)this.GetValue(SourceProperty); }
            set { this.SetValue(SourceProperty, value); }
        }

        /// <summary>
        /// テキストファイルを読み込んで GUI に反映させる
        /// </summary>
        /// <param name="path"></param>
        private void LoadTextFile(string path)
        {
            if (path != null)
            {
                try
                {
                    var content = File.ReadAllText(path, Encoding.UTF8);
                    this.vm.Content = content;
                    return;
                }
                catch (IOException e)
                {
                    Debug.WriteLine(e.Message);
                }
            }

            this.vm.Content = string.Empty;
        }
    }
}
