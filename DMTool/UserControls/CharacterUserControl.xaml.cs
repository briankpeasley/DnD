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
            if (e.Key == Key.Return)
            {
                TextBox tb = sender as TextBox;
                int dmg;
                if (Int32.TryParse(tb.Text, out dmg))
                {
                    Character.ApplyDamage(dmg);
                }

                tb.Text = "00";
                tb.SelectAll();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.Character != null)
            {
                this.Character.Save();
                MessageBox.Show("Character saved");
                (App.Current as App).MonstersViewModel.Refresh();
            }
        }

        private void IncrementRemainingHitDice(object sender, RoutedEventArgs e)
        {
            Character.RemainingHitDice++;
        }

        private void DecrementRemainingHitDice(object sender, RoutedEventArgs e)
        {
            Character.RemainingHitDice--;
        }

        private void AddSpellSlot(object sender, RoutedEventArgs e)
        {
            SpellSlot slot = new SpellSlot();
            slot.Level = this.Character.SpellSlots.Count + 1;
            slot.Total = 0;
            slot.Used = 0;
            this.Character.SpellSlots.Add(slot);
        }

        private void AddEffect(object sender, RoutedEventArgs e)
        {
            AddEffectWindow window = new AddEffectWindow();

            Point mousePosition = Mouse.GetPosition(null);
            Point screenPosition = PointToScreen(mousePosition);

            // Set the location of the new window to the mouse position
            window.Left = screenPosition.X;
            window.Top = screenPosition.Y;

            window.RiderViewModel = (App.Current as App).RiderViewModel;
            window.RiderSelected += (s, r) =>
            {
                Character.Riders.Add(r);
            };

            window.ShowDialog();
        }
    }
}
