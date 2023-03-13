using DMTool.Source;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class CombatEventArgs : RoutedEventArgs
    {
        public CombatEventArgs(RoutedEvent evt) : base(evt) { }

        public Character Character { get; set; }
    }

    /// <summary>
    /// Interaction logic for Combat.xaml
    /// </summary>
    public partial class Combat : UserControl
    {
        public static readonly RoutedEvent PlayerSelectionChangedRoutedEvent = 
            EventManager.RegisterRoutedEvent("PlayerSelectionChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Combat));

        public event RoutedEventHandler PlayerSelectionChanged
        {
            add { AddHandler(PlayerSelectionChangedRoutedEvent, value); }
            remove { RemoveHandler(PlayerSelectionChangedRoutedEvent, value); }
        }

        public Combat()
        {
            InitializeComponent();
        }

        private void SelectableTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                TextBox tb = sender as TextBox;
                int dmg;
                if (Int32.TryParse(tb.Text, out dmg))
                {
                    (tb.Tag as Character).ApplyDamage(dmg);
                }

                tb.Text = "00";
                tb.SelectAll();
            }
        }

        private void DeleteRider(object sender, RoutedEventArgs e)
        {
            Rider r = (sender as Button).Tag as Rider;
            r.Removed();
        }

        private void ShowEffectForm(object sender, RoutedEventArgs e)
        {
            AddEffectWindow window = new AddEffectWindow();
            window.RiderViewModel = (App.Current as App).RiderViewModel;
            window.RiderSelected += (s, r) =>
            {
                ((sender as Button).Tag as Character).Riders.Add(r);
            };

            window.ShowDialog();
        }

        private void participantsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CombatEventArgs args = new CombatEventArgs(PlayerSelectionChangedRoutedEvent);
            args.Character = (sender as ListBox).SelectedItem as Character;
            RaiseEvent(args);
        }

        private void RemoveCharacter(object sender, RoutedEventArgs e)
        {
            Character c = (sender as Button).Tag as Character;
            c.Remove();
        }

        private void SortClick(object sender, RoutedEventArgs e)
        {
            (App.Current as App).CombatViewModel.Sort();
        }

        private void LongRestClick(object sender, RoutedEventArgs e)
        {
            (App.Current as App).CombatViewModel.LongRest();
        }

        private void RollInitiative(object sender, RoutedEventArgs e)
        {
            (App.Current as App).CombatViewModel.RollInitiative();
        }

        private void ResetClock_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).CombatViewModel.Clock = 0;
        }

        private void participantsListView_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //e.Handled = true;
        }

        private void participantsListView_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            //e.Handled = true;
        }
    }
}
