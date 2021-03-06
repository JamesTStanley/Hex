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
using System.Windows.Shapes;

namespace WpfDisplaySample
{
    /// <summary>
    /// Interaction logic for HexControlSample.xaml
    /// </summary>
    public partial class HexControlSample : Window
    {
        public HexControlSample()
        {
            InitializeComponent();
        }

        private void RotateCCW_Click(object sender, RoutedEventArgs e)
        {
            SampleHex.RotateCounterClockwise();
        }

        private void RotateCW_Click(object sender, RoutedEventArgs e)
        {
            SampleHex.RotateClockwise();
        }
    }
}
