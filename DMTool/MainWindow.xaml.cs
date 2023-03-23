using DMTool.Source;
using DMTool.UserControls;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            Title = $"DMTool.  Version: {(App.Current as App).Version}";
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

            Character c = JsonConvert.DeserializeObject<PlayerCharacter>(File.ReadAllText("./PlayerCharacter/Pippin.json"));
            (App.Current as App).CombatViewModel.Participants.Add(c);

            //c = JsonConvert.DeserializeObject<PlayerCharacter>(File.ReadAllText("./PlayerCharacter/Niko Fogshine.json"));
            //(App.Current as App).CombatViewModel.Participants.Add(c);

            // Used for testing a default character
            /*for (int i = 0; i < 5; i++)
            {
                Character c = JsonConvert.DeserializeObject<PlayerCharacter>(File.ReadAllText("./PlayerCharacter/Test.json"));
                (c as PlayerCharacter).Counters.Add(new Counter()
                {
                    Name = "Test",
                    Max = 25,
                    Current = 10
                });
                (App.Current as App).CombatViewModel.Participants.Add(c);
            }*/

            combatControl.DataContext = (App.Current as App).CombatViewModel;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            KeyboardInput.Process(sender, e);
        }

        private void combatControl_PlayerSelectionChanged(object sender, RoutedEventArgs e)
        {
            if ((e as CombatEventArgs).Character != null)
            {
                if ((e as CombatEventArgs).Character.GetType() == typeof(PlayerCharacter))
                {
                    playCharUserControl.PlayerCharacter = (e as CombatEventArgs).Character as PlayerCharacter;
                    (App.Current as App).MonsterWindow.PlayerCharacterUserControl.PlayerCharacter = (e as CombatEventArgs).Character as PlayerCharacter;
                }
                else
                {
                    (App.Current as App).MonsterWindow.MonsterUserControl.Monster = (e as CombatEventArgs).Character as Monster;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (App.Current as App).MonsterWindow.PreventClosing = false;
            (App.Current as App).MonsterWindow.Close();
        }
    }
}
