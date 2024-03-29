﻿using DMTool.Source;
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

namespace DMTool.UserControls
{
    /// <summary>
    /// Interaction logic for MonsterWindow.xaml
    /// </summary>
    public partial class MonsterWindow : Window
    {
        public MonsterWindow()
        {
            InitializeComponent();
            PreventClosing = true;
        }

        public bool PreventClosing { get; set; }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = PreventClosing;
        }
        
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            KeyboardInput.Process(sender, e);
        }
    }
}
