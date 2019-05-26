using DMTool.UserControls;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Input;

namespace DMTool.Source
{
    public static class KeyboardInput
    {
        public static void Process(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                (App.Current as App).CombatViewModel.NextTurn(1);
            }
            else if (e.Key == Key.Up)
            {
                (App.Current as App).CombatViewModel.NextTurn(-1);
            }

            if (e.Key == Key.A && (System.Windows.Input.Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
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
            else if (e.Key == Key.M && (System.Windows.Input.Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
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
            else if (e.Key == Key.S && (System.Windows.Input.Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                (App.Current as App).CombatViewModel.SaveAll(true);
            }
            else if (e.Key == Key.T && (System.Windows.Input.Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                AddCoinToParty window = new AddCoinToParty();
                window.DistributeCoin += (s, c) =>
                {
                    (App.Current as App).CombatViewModel.DistributeCoin(c);
                };
                window.ShowDialog();
            }
        }
    }
}
