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
using System.Windows.Shapes;

namespace DMTool.UserControls
{
    /// <summary>
    /// Interaction logic for AddSpellWindow.xaml
    /// </summary>
    public partial class AddSpellWindow : Window
    {
        public ObservableCollection<Spell> Spells { get; set; }
        public event EventHandler<Spell> SpellSelected;

        public AddSpellWindow()
        {
            InitializeComponent();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            spells.ItemsSource = Spells;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(spells.ItemsSource);
            view.Filter = Filter;    
        }

        private bool Filter(object obj)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                return true;
            }

            if (string.IsNullOrEmpty((obj as Spell).Name))
            {
                return false;
            }

            return (obj as Spell).Name.ToLower().Contains(txtSearch.Text.ToLower());
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(spells.ItemsSource).Refresh();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (spells.SelectedItem != null)
            {
                if (e.Key == Key.Enter)
                {
                    Spell s = (spells.SelectedItem as Spell).Clone() as Spell;
                    SpellSelected?.Invoke(this, s);
                }
            }
        }

        private void RefreshSpells(object sender, RoutedEventArgs e)
        {
            (App.Current as App).SpellsViewModel.Refresh();
        }
    }
}
