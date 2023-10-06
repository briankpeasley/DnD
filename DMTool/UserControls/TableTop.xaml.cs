using DMTool.Properties;
using DMTool.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DMTool.UserControls
{

    /// <summary>
    /// Interaction logic for TableTop.xaml
    /// </summary>
    public partial class TableTop : Window
    {
        public TableTop()
        {
            InitializeComponent();
            PreventClosing = true;
        }

        public bool PreventClosing { get; set; }
        public CombatViewModel CombatViewModel { get; set; }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = PreventClosing;
        }

        private void Window_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            KeyboardInput.Process(sender, e);
        }

        public void SetScreen(int idx)
        {
            var currentScreen = Screen.AllScreens[idx];
            
            this.WindowState = WindowState.Normal;
            this.Left = currentScreen.WorkingArea.Left;
            this.Top = currentScreen.WorkingArea.Top;
            this.Width = currentScreen.WorkingArea.Width;
            this.Height = currentScreen.WorkingArea.Height;
            this.WindowState = WindowState.Maximized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SetScreen(1);
        }

        private void participants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
