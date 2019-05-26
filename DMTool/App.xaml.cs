using CsvHelper;
using DMTool.Source;
using DMTool.UserControls;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace DMTool
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public RidersViewModel RiderViewModel { get; set; }
        public SpellsViewModel SpellsViewModel { get; set; }
        public MonstersViewModel MonstersViewModel { get; set; }
        public MonsterWindow MonsterWindow { get; set; }
        public CombatViewModel CombatViewModel { get; set; }
        public Random RNG { get; set; }
        public string Version { get; set; }
        public App()
        {
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            Version = fvi.FileVersion;

            RNG = new Random();
            RiderViewModel = new RidersViewModel();
            SpellsViewModel = new SpellsViewModel();
            MonstersViewModel = new MonstersViewModel();
        }
    }
}
