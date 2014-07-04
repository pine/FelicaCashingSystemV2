﻿using System;
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

namespace FelicaCashingSystemV2.Views
{
    /// <summary>
    /// VersionInformationView.xaml の相互作用ロジック
    /// </summary>
    public partial class SideProfileView : UserControl
    {
        public SideProfileView()
        {
            InitializeComponent();
            this.DataContext = new SideProfileViewModel();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // double clicked
            if (e.IsDoubleClicked())
            {
                App.Current.ShowProfileWindowWithAvatar();
            }
        }
    }
}