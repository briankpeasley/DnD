using DMTool.Source;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    public static class ListExtension
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    /// <summary>
    /// Interaction logic for AddMonsterWindow.xaml
    /// </summary>
    public partial class AddMonsterWindow : Window
    {
        public ObservableCollection<Monster> Monsters { get; set; }
        public ObservableCollection<Monster> EncounterableMonsters { get; set; }
        public Random rng = new Random();
        public event EventHandler<Monster> MonsterSelected;

        public AddMonsterWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            monsters.ItemsSource = Monsters;
            encounterableMonsters.ItemsSource = EncounterableMonsters;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(monsters.ItemsSource);
            view.Filter = Filter;
        }

        private bool Filter(object obj)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                return true;
            }

            if (string.IsNullOrEmpty((obj as Monster).Name))
            {
                return false;
            }

            return (obj as Monster).Name.ToLower().Contains(txtSearch.Text.ToLower());
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(monsters.ItemsSource).Refresh();
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (monsters.SelectedItem != null)
            {
                if (e.Key == Key.Enter)
                {
                    Monster m = (monsters.SelectedItem as Monster).Clone() as Monster;
                    MonsterSelected?.Invoke(this, m);
                }
                else if (e.Key == Key.Right)
                {
                    foreach (var item in (sender as AddMonsterWindow).monsters.SelectedItems)
                    {
                        if (string.IsNullOrEmpty((item as Monster).Name) == false)
                        {
                            EncounterableMonsters.Add(item as Monster);
                        }
                    }
                }
                else if (e.Key == Key.Left)
                {
                    var selected = (sender as AddMonsterWindow).encounterableMonsters.SelectedItems;
                    if (selected != null)
                    {
                        for (int i = selected.Count - 1; i >= 0; i--)
                        {
                            EncounterableMonsters.Remove(selected[i] as Monster);
                        }
                    }
                }
            }
        }

        private void RefreshMonsters(object sender, RoutedEventArgs e)
        {
            (App.Current as App).MonstersViewModel.Refresh();
        }

        private void discoverableMonsters_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void monsters_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void SaveEncounterTable(object sender, RoutedEventArgs e)
        {
            string path = "./Encounters.json";

            File.WriteAllText(path, JsonConvert.SerializeObject(EncounterableMonsters.ToArray()));
            MessageBox.Show("Encounter table saved");
        }

        private double ComputeDifficulty(List<Monster> monsters, int partySize)
        {
            double difficulty = 0;
            foreach (Monster m in monsters)
            {
                difficulty += Charts.GetMonsterXP(m.Level);
            }

            if (monsters.Count() >= 15)
            {
                difficulty *= 4;
            }
            else if (monsters.Count() >= 11)
            {
                difficulty *= 3;
            }
            else if (monsters.Count() >= 7)
            {
                difficulty *= 2.5;
            }
            else if (monsters.Count() >= 3)
            {
                difficulty *= 2;
            }
            else if (monsters.Count() == 2)
            {
                difficulty *= 1.5;
            }

            if (partySize >= 6)
            {
                difficulty *= 0.75;
            }

            return difficulty;
        }

        private void GenerateEncounter(object sender, RoutedEventArgs e)
        {
            int difficulty = 0;
            if (medium.IsChecked.HasValue && medium.IsChecked.Value == true)
            {
                difficulty = 1;
            }
            else if (hard.IsChecked.HasValue && hard.IsChecked.Value == true)
            {
                difficulty = 2;
            }
            else if (deadly.IsChecked.HasValue && deadly.IsChecked.Value == true)
            {
                difficulty = 3;
            }

            double threshold = 0;
            int partySize = 0;

            // Get party difficulty threshold
            foreach (Character c in (App.Current as App).CombatViewModel.Participants)
            {
                if (c.GetType() == typeof(PlayerCharacter))
                {
                    int level;

                    if (Int32.TryParse(c.Level, out level) == false)
                    {
                        MessageBox.Show("One characters level is not formatted correctly");
                        return;
                    }

                    level = level < 1 ? 1 : level;
                    level = level > 20 ? 20 : level;
                    level--;
                    partySize++;
                    threshold += (double)(Charts.Threshold[level, (int)difficulty]);
                }
            }

            List<Monster> m = (App.Current as App).MonstersViewModel.EncounterableMonsters.ToList();
            List<Monster> generatedMonsteres = new List<Monster>();
            do
            {
                // shuffle the list of monsteres to pull from
                m.Shuffle();

                // Loop through all monsters to pull from and determine if they can still be added.
                // If so, add to encountered monsters
                for (int i = m.Count - 1; i >= 0; i--)
                {
                    generatedMonsteres.Add(m[i].Clone() as Monster);
                    
                    if (ComputeDifficulty(generatedMonsteres, partySize) > threshold)
                    {
                        // monster is too hard (remove)
                        generatedMonsteres.RemoveAt(generatedMonsteres.Count() - 1);
                        m.RemoveAt(i);
                    }
                }
            } while (m.Count() > 0);

            foreach (Monster monster in generatedMonsteres)
            {
                monster.GenerateHitPoints(rng);
                (App.Current as App).CombatViewModel.Participants.Add(monster);
            }
        }
    }
}
