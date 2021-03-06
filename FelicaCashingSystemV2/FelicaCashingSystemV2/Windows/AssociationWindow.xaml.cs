﻿using MahApps.Metro.Controls;
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
using System.Windows.Shapes;
using WpfCommonds;

namespace FelicaCashingSystemV2.Windows
{
    /// <summary>
    /// RegisterWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class AssociationWindow : MetroWindow
    {
        public AssociationWindow()
        {
            InitializeComponent();
            this.SetEscClosableWindow();

            this.DataContext = new AssociationWindowViewModel();
            this.SetDialogMessageReceiver();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.PART_CardName.Focus();
        }
    }
}
