using DMTool.Source;
using DMTool.UserControls;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DMTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PlayerCharacter TestCharacter { get; set; }


        public MainWindow()
        {
            InitializeComponent();
            ToolTipService.ShowDurationProperty.OverrideMetadata(
                typeof(DependencyObject), new FrameworkPropertyMetadata(Int32.MaxValue));

            (App.Current as App).MonsterWindow = new MonsterWindow();
            (App.Current as App).MonsterWindow.MonsterUserControl.Monster = new Monster()
            {
                Name = "Empty"
            };

            (App.Current as App).MonsterWindow.Show();
            (App.Current as App).CombatViewModel = new CombatViewModel();
            
            playCharUserControl.PlayerCharacter = new PlayerCharacter()
            {
                Name = "Empty"
            };

            // Used for testing a default character
            ////Character c = JsonConvert.DeserializeObject<PlayerCharacter>(File.ReadAllText("./PlayerCharacter/Test.json"));
            ////(c as PlayerCharacter).Counters.Add(new Counter()
            ////{
            ////    Name = "Test",
            ////    Max = 25,
            ////    Current = 10
            ////});
            ////(App.Current as App).CombatViewModel.Participants.Add(c);

            combatControl.DataContext = (App.Current as App).CombatViewModel;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                (App.Current as App).CombatViewModel.NextTurn(1);
            }
            else if (e.Key == Key.Up)
            {
                (App.Current as App).CombatViewModel.NextTurn(-1);
            }

            if (e.Key == Key.A && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.RestoreDirectory = true;
                ofd.InitialDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, "PlayerCharacter");
                ofd.Multiselect = true;
                bool? ret = ofd.ShowDialog();
                if (ret.HasValue && ret.Value)
                {
                    foreach (string filename in ofd.FileNames)
                    {
                        Character c = JsonConvert.DeserializeObject<PlayerCharacter>(File.ReadAllText(filename));
                        (App.Current as App).CombatViewModel.Participants.Add(c);
                    }
                }
            }
            else if (e.Key == Key.M && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                AddMonsterWindow window = new AddMonsterWindow();
                window.Monsters = (App.Current as App).MonstersViewModel.DiscoveredMonsters;
                window.EncounterableMonsters = (App.Current as App).MonstersViewModel.EncounterableMonsters;

                window.MonsterSelected += (s, m) =>
                {
                    (App.Current as App).CombatViewModel.Participants.Add(m as Character);
                };
                window.ShowDialog();
            }
            else if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                (App.Current as App).CombatViewModel.SaveAll();
            }
            else if (e.Key == Key.T && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                AddCoinToParty window = new AddCoinToParty();
                window.DistributeCoin += (s, c) =>
                {
                    (App.Current as App).CombatViewModel.DistributeCoin(c);
                };
                window.ShowDialog();
            }
        }

        private void combatControl_PlayerSelectionChanged(object sender, RoutedEventArgs e)
        {
            if ((e as CombatEventArgs).Character != null)
            {
                if ((e as CombatEventArgs).Character.GetType() == typeof(PlayerCharacter))
                {
                    playCharUserControl.PlayerCharacter = (e as CombatEventArgs).Character as PlayerCharacter;
                }
                else
                {
                    (App.Current as App).MonsterWindow.MonsterUserControl.Monster = (e as CombatEventArgs).Character as Monster;
                }
            }
        }
    }
}
