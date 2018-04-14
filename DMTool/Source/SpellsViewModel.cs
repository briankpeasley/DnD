using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMTool.Source
{

    public class SloppySpell
    {
        public string Name { get; set; }
        public string Level { get; set; }
        public string School { get; set; }
        public string CastingTime { get; set; }
        public string Range { get; set; }
        public string Components { get; set; }
        public string Duration { get; set; }
        public string Description { get; set; }
    }

    public class SpellsViewModel : ViewModel
    {
        public ObservableCollection<Spell> DiscoveredSpells;

        public SpellsViewModel()
        {
            DiscoveredSpells = new ObservableCollection<Spell>();
            Refresh();
        }

        public void Refresh()
        {
            DiscoveredSpells.Clear();

            try
            {
                if (Directory.Exists("./Spells"))
                {
                    foreach (string file in Directory.EnumerateFiles("./Spells"))
                    {
                        try
                        {
                            string fileContents = File.ReadAllText(file);
                            Spell spell = JsonConvert.DeserializeObject<Spell>(fileContents);
                            DiscoveredSpells.Add(spell);
                        }
                        catch { }
                    }
                }
            }
            catch { }

            //foreach (Spell s in DiscoveredSpells)
            //{
            //    if (s.Name != null)
            //    {
            //        if (File.Exists($"./Spells/{s.Name.ToUpper()}.json") == false)
            //        {
            //            File.WriteAllText($"./Spells/{s.Name.ToUpper().Replace('/','_')}.json", JsonConvert.SerializeObject(s));
            //        }
            //    }
            //}

            //List<Spell> spellslist = DiscoveredSpells.ToList();
            //foreach(string file in Directory.EnumerateFiles("./SloppySpells"))
            //{
            //    SloppySpell sSpell = JsonConvert.DeserializeObject<SloppySpell>(File.ReadAllText(file));
            //    if(spellslist.Find(x => (x.Name == null ? null : x.Name.ToLower()) == sSpell.Name.ToLower()) == null)
            //    {
            //        Spell newSpell = new Spell()
            //        {
            //            Name = sSpell.Name,
            //            Level = sSpell.Level,
            //            CastingTime = sSpell.CastingTime,
            //            Range = sSpell.Range,
            //            Components = sSpell.Components,
            //            Duration = sSpell.Duration,
            //            Description = sSpell.Description
            //        };

            //        File.WriteAllText($"./Spells/{newSpell.Name}.json", JsonConvert.SerializeObject(newSpell));
            //    }
            //}
        }
    }
}
