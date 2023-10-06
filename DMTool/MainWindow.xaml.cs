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

            (App.Current as App).CombatViewModel = new CombatViewModel();
            (App.Current as App).TableTopWindow = new TableTop();
            (App.Current as App).TableTopWindow.participants.DataContext = (App.Current as App).CombatViewModel;
            (App.Current as App).TableTopWindow.Show();
            
            playCharUserControl.PlayerCharacter = new PlayerCharacter()
            {
                Name = "Empty"
            };

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
                }
                else
                {
                    monsterUserControl.Monster = (e as CombatEventArgs).Character as Monster;
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (App.Current as App).TableTopWindow.PreventClosing = false;
            (App.Current as App).TableTopWindow.Close();
        }
    }
}
