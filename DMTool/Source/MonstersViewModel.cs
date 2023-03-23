using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DMTool.Source
{
    public class MonstersViewModel : ViewModel
    {
        public ObservableCollection<Monster> DiscoveredMonsters;
        public ObservableCollection<Monster> EncounterableMonsters;

        public MonstersViewModel()
        {
            Refresh();
        }

        public void Refresh()
        {
            DiscoveredMonsters = new ObservableCollection<Monster>();
            EncounterableMonsters = new ObservableCollection<Monster>();
            string path = "./Monsters";

            if (Directory.Exists(path))
            {
                foreach (string file in Directory.EnumerateFiles(path))
                {
                    try
                    {
                        string s = File.ReadAllText(file);
                        Monster monster = JsonConvert.DeserializeObject<Monster>(s);
                        DiscoveredMonsters.Add(monster);
                    }
                    catch { }
                }
            }

        }

        public void LoadEncounters(string path)
        {
            if (File.Exists(path))
            {
                EncounterableMonsters.Clear();

                try
                {
                    string s = File.ReadAllText(path);
                    Monster[] monsters = JsonConvert.DeserializeObject<Monster[]>(s);

                    foreach (Monster m in monsters)
                    {
                        EncounterableMonsters.Add(m);
                    }
                }
                catch { }
            }
        }
    }
}
