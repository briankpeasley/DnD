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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DMTool.UserControls
{
    /// <summary>
    /// Interaction logic for CharacterUserControl.xaml
    /// </summary>
    public partial class CharacterUserControl : UserControl
    {
        public static readonly DependencyProperty CharacterProperty =
            DependencyProperty.Register("Character", typeof(Character), typeof(CharacterUserControl), new FrameworkPropertyMetadata(null, CharacterChanged));

        private static void CharacterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CharacterUserControl ctrl = d as CharacterUserControl;
            ctrl.grid.DataContext = e.NewValue;
        }

        public CharacterUserControl()
        {
            InitializeComponent();
        }

        public Character Character
        {
            get { return GetValue(CharacterProperty) as Character; }
            set { SetValue(CharacterProperty, value); }
        }

        private void SelectableTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            int dmg;
            if(e.Key == Key.Enter && Int32.TryParse((sender as TextBox).Text, out dmg))
            {
                List<int> newLog = new List<int>(Character.DamageLog);
                newLog.Add(dmg);
                Character.DamageLog = newLog;
                (sender as TextBox).Text = "0";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Character != null)
            {
                this.Character.Save();
                MessageBox.Show("Character saved");
            }
        }
    }
}
