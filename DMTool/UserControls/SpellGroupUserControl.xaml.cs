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
    /// <summary>
    /// Interaction logic for SpellGroupUserControl.xaml
    /// </summary>
    public partial class SpellGroupUserControl : UserControl
    {
        public static readonly DependencyProperty SpellGroupProperty =
           DependencyProperty.Register("SpellGroup", typeof(ObservableCollection<Spell>), typeof(SpellGroupUserControl), new FrameworkPropertyMetadata(null, SpellGroupChanged));

        public static readonly DependencyProperty SpellGroupNameProperty =
           DependencyProperty.Register("SpellGroupName", typeof(string), typeof(SpellGroupUserControl), new FrameworkPropertyMetadata(null, SpellGroupNameChanged));


        public SpellGroupUserControl()
        {
            InitializeComponent();
        }

        public ObservableCollection<Spell> SpellGroup
        {
            get { return GetValue(SpellGroupProperty) as ObservableCollection<Spell>; }
            set { SetValue(SpellGroupProperty, value); }
        }

        public string SpellGroupName
        {
            get { return GetValue(SpellGroupNameProperty) as string; }
            set { SetValue(SpellGroupNameProperty, value); }
        }

        private static void SpellGroupChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SpellGroupUserControl ctrl = d as SpellGroupUserControl;
            ctrl.grid.DataContext = e.NewValue;
            ctrl.spellsItemsControl.ItemsSource = e.NewValue as ObservableCollection<Spell>;
        }

        private static void SpellGroupNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SpellGroupUserControl ctrl = d as SpellGroupUserControl;
            ctrl.nameTextBox.Text = e.NewValue as string;
        }

        private void RemoveSpell(object sender, RoutedEventArgs e)
        {
            ((sender as Button).Tag as Spell).Remove();
        }

        private void AddSpell(object sender, RoutedEventArgs e)
        {
            AddSpellWindow wind = new AddSpellWindow();
            wind.Spells = (App.Current as App).SpellsViewModel.DiscoveredSpells;
            wind.SpellSelected += (s, spell) =>
            {
                SpellGroup.Add(spell);
            };

            wind.ShowDialog();
        }
    }
}
