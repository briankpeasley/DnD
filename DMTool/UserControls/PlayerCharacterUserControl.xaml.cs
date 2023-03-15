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
    /// Interaction logic for PlayerCharacterUserControl.xaml
    /// </summary>
    public partial class PlayerCharacterUserControl : UserControl
    {
        public static readonly DependencyProperty PlayerCharacterProperty =
            DependencyProperty.Register("PlayerCharacter", typeof(PlayerCharacter), typeof(PlayerCharacterUserControl), new FrameworkPropertyMetadata(null, PlayerCharacterChanged));

        private static void PlayerCharacterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PlayerCharacterUserControl ctrl = d as PlayerCharacterUserControl;
            ctrl.grid.DataContext = e.NewValue;
            ctrl.gear.ItemsSource = (e.NewValue as PlayerCharacter).Gear;
            // ctrl.counters.ItemsSource = (e.NewValue as PlayerCharacter).Counters;
        }

        public PlayerCharacterUserControl()
        {
            InitializeComponent();

            grid.DataContext = this;
            characterUserControl.DataContext = this;
            xpStack.DataContext = this;
            gear.DataContext = this;
        }

        public PlayerCharacter PlayerCharacter
        {
            get { return GetValue(PlayerCharacterProperty) as PlayerCharacter; }
            set { SetValue(PlayerCharacterProperty, value); }
        }

        private void RemoveGear(object sender, RoutedEventArgs e)
        {
            Gear g = (sender as Button).Tag as Gear;
            if (g != null)
            {
                PlayerCharacter.Gear.Remove(g);
            }
        }

        private void IncrementCounter(object sender, RoutedEventArgs e)
        {
            Counter c = (sender as Button).Tag as Counter;
            c.Current++;
        }

        private void DecrementCounter(object sender, RoutedEventArgs e)
        {
            Counter c = (sender as Button).Tag as Counter;
            c.Current--;
        }

        private void newCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidateNumericInput(sender, e);
        }

        private void newWeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidateNumericInput(sender, e);
        }

        private void ExistingCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidateNumericInput(sender, e);
        }

        private void ExistingWeight_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidateNumericInput(sender, e);
        }

        private void ValidateNumericInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == ".")
            {
                int count = (sender as TextBox).Text.Count((char c) =>
                {
                    return c == '.';
                });

                e.Handled = count > 0;
                return;
            }

            e.Handled = !double.TryParse(e.Text, out double foo);
        }

        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (double.TryParse(newCount.Text, out double count) == false)
                {
                    MessageBox.Show("Count needs to be a number");
                    return;
                }


                if (double.TryParse(newWeight.Text, out double weight) == false)
                {
                    MessageBox.Show("Weight needs to be a number");
                    return;
                }

                Gear g = new Gear()
                {
                    Name = newName.Text,
                    Count = count,
                    Description = newDescription.Text,
                    Weight = weight,
                    Value = newValue.Text
                };

                newName.Text = string.Empty;
                newCount.Text = string.Empty;
                newDescription.Text = string.Empty;
                newWeight.Text = string.Empty;
                newValue.Text = string.Empty;
                PlayerCharacter.Gear.Add(g);
            }
        }

        //private void StackPanel_KeyUp_1(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //    {
        //        try
        //        {
        //            Counter c = new Counter()
        //            {
        //                Name = newCounter.Text,
        //                Max = Int32.Parse(newCounterMax.Text),
        //                Current = Int32.Parse(newCounterMax.Text)
        //            };

        //            newCounter.Text = string.Empty;
        //            newCounterMax.Text = string.Empty;
        //            PlayerCharacter.Counters.Add(c);
        //        }
        //        catch
        //        {
        //            MessageBox.Show("Type it better yah dumbass");
        //        }
        //    }
        //}
    }
}
