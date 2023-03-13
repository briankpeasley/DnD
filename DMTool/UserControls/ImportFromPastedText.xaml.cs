using DMTool.Source;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;

namespace DMTool.UserControls
{
    public partial class ImportFromPastedText : Window
    {
        private Monster _monster;

        public ImportFromPastedText()
        {
            InitializeComponent();

            _monster = new Monster();
            MonsterUserControl.Monster = _monster;
        }

        private Monster ImportDndBeyondCharacter(string text, bool monster = true)
        {
            char[] spaceSplit = { ' ' };
            char[] commaSplit = { ',' };

            var m = new Monster();

            string hasError = "";
            string name = "";
            string descr = "";
            string hitDice = "";
            int hitPoints = 0;
            int armorClass = 0;
            string speed = "";
            int str = 0;
            int dex = 0;
            int intelligence = 0;
            int con = 0;
            int cha = 0;
            int wis = 0;
            string senses = "";
            string languages = "";
            string immunities = "";
            string resistance = "";
            string vulnerabilities = "";
            string challenge = "";

            List<SpecialAbility> specialAbilities = new List<SpecialAbility>();
            List<SpecialAbility> actions = new List<SpecialAbility>();

            bool inActions = false;
            string lastLine = "";
            string lastLineLower = "";
            string line = "";
            string lineLower = "";
            string level = "";
            var lines = text.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                lastLine = line;
                lastLineLower = lineLower;
                line = lines[i].Trim().Replace("\r", "");
                System.Diagnostics.Debug.WriteLine(line);
                lineLower = line.ToLower();

                try
                {
                    if (line.Length == 0)
                    {
                        continue;
                    }

                    if (name == "")
                    {
                        name = line;
                        continue;
                    }

                    if (descr == "")
                    {
                        descr = line;
                        continue;
                    }

                    if (lineLower.Contains("armor class") && armorClass == 0)
                    {
                        armorClass = int.Parse(line.Split(spaceSplit)[2]);
                        continue;
                    }

                    if (lineLower.Contains("hit points") && hitPoints == 0)
                    {
                        var match = Regex.Match(lineLower, @"hit points ([0-9]*) \((.*)\)");
                        hitPoints = int.Parse(match.Groups[1].Value);
                        hitDice = match.Groups[2].Value;
                        continue;
                    }

                    if (lineLower.StartsWith("speed") && speed == "")
                    {
                        var match = Regex.Match(lineLower, @"speed (.*)");
                        speed = match.Groups[1].Value;
                        continue;
                    }

                    if (lastLineLower.StartsWith("str") && str == 0)
                    {
                        var match = Regex.Match(lineLower, @"([0-9]*)[ ]*\(.*\)");
                        str = int.Parse(match.Groups[1].Value);
                        continue;
                    }

                    if (lastLineLower.StartsWith("dex") && dex == 0)
                    {
                        var match = Regex.Match(lineLower, @"([0-9]*)[ ]*\(.*\)");
                        dex = int.Parse(match.Groups[1].Value);
                        continue;
                    }

                    if (lastLineLower.StartsWith("con") && con == 0)
                    {
                        var match = Regex.Match(lineLower, @"([0-9]*)[ ]*\(.*\)");
                        con = int.Parse(match.Groups[1].Value);
                        continue;
                    }

                    if (lastLineLower.StartsWith("int") && intelligence == 0)
                    {
                        var match = Regex.Match(lineLower, @"([0-9]*)[ ]*\(.*\)");
                        intelligence = int.Parse(match.Groups[1].Value);
                        continue;
                    }

                    if (lastLineLower.StartsWith("wis") && wis == 0)
                    {
                        var match = Regex.Match(lineLower, @"([0-9]*)[ ]*\(.*\)");
                        wis = int.Parse(match.Groups[1].Value);
                        continue;
                    }

                    if (lastLineLower.StartsWith("cha") && cha == 0)
                    {
                        var match = Regex.Match(lineLower, @"([0-9]*)[ ]*\(.*\)");
                        cha = int.Parse(match.Groups[1].Value);
                        continue;
                    }

                    if (lineLower.StartsWith("saving throws"))
                    {
                        SpecialAbility a = new SpecialAbility
                        {
                            Name = "Saving Throws",
                            Description = line.Substring(14),
                        };

                        specialAbilities.Add(a);
                    }

                    if (lineLower.StartsWith("skills"))
                    {
                        SpecialAbility a = new SpecialAbility
                        {
                            Name = "Skills",
                            Description = line.Substring(7),
                        };

                        specialAbilities.Add(a);
                    }

                    if (lineLower.StartsWith("senses"))
                    {
                        senses = line.Substring(7);
                    }

                    if (lineLower.StartsWith("languages"))
                    {
                        languages = line.Substring("languages".Length);
                    }

                    if (lineLower.StartsWith("vulerabilities"))
                    {
                        vulnerabilities = line.Substring("vulerabilities".Length);
                    }

                    if (lineLower.StartsWith("damage immunities"))
                    {
                        immunities += line.Substring("damage immunities".Length);
                    }

                    if (lineLower.StartsWith("condition immunities"))
                    {
                        immunities += line.Substring("condition immunities".Length);
                    }

                    if (lineLower.StartsWith("damage resistances"))
                    {
                        resistance = line.Substring("damage resistances".Length);
                    }

                    if (lineLower.StartsWith("level"))
                    {
                        level = line.Substring("level".Length);
                    }

                    if (lineLower.StartsWith("challenge"))
                    {
                        var match = Regex.Match(lineLower, @"challenge ([0-9]*) \(.*\)");
                        challenge = match.Groups[1].Value;
                        continue;
                    }

                    //if (lineLower.StartsWith("cantrips"))
                    //{
                    //    var match = Regex.Match(lineLower, @"(cantrips.*): (.*)");
                    //    var cantrips = match.Groups[2].Value.Split(commaSplit);
                    //    foreach (var cantrip in cantrips)
                    //    {
                    //        m.Cantrips.Add(new Spell()
                    //        {
                    //            Name = cantrip.Trim(),
                    //        });
                    //    }

                    //    actions.Add(new SpecialAbility()
                    //    {
                    //        Name = match.Groups[1].Value,
                    //        Description = match.Groups[2].Value,
                    //    });

                    //    continue;
                    //}

                    for (int spellLvlIndex = 1; spellLvlIndex <= 9; spellLvlIndex++)
                    {
                        if (lineLower.StartsWith(spellLvlIndex.ToString()) && lineLower.Contains("slot"))
                        {
                            var match = Regex.Match(lineLower, @"(.*\(.*\)): (.*)");
                            var spells = match.Groups[2].Value.Split(commaSplit);

                            ICollection<Spell> spellsCollection = new List<Spell>();
                            foreach (var spell in spells)
                            {
                                spellsCollection.Add(new Spell()
                                {
                                    Name = spell.Trim(),
                                });
                            }

                            ObservableCollection<Spell> col = new ObservableCollection<Spell>();
                            //switch (spellLvlIndex)
                            //{
                            //    case 1: col = m.Level1; break;
                            //    case 2: col = m.Level2; break;
                            //    case 3: col = m.Level3; break;
                            //    case 4: col = m.Level4; break;
                            //    case 5: col = m.Level5; break;
                            //    case 6: col = m.Level6; break;
                            //    case 7: col = m.Level7; break;
                            //    case 8: col = m.Level8; break;
                            //    case 9: col = m.Level9; break;
                            //}

                            foreach (var spell in spellsCollection)
                            {
                                col.Add(spell);
                            }

                            actions.Add(new SpecialAbility()
                            {
                                Name = match.Groups[1].Value,
                                Description = match.Groups[2].Value,
                            });

                            continue;
                        }
                    }

                    if (lineLower.StartsWith("actions"))
                    {
                        inActions = true;
                        continue;
                    }

                    var catchAll = Regex.Match(line, @"(.*)\. (.*)");
                    if (catchAll.Success)
                    {
                        if (inActions)
                        {
                            actions.Add(new SpecialAbility()
                            {
                                Name = catchAll.Groups[1].Value,
                                Description = catchAll.Groups[2].Value,
                            });
                        }
                        else
                        {
                            specialAbilities.Add(new SpecialAbility()
                            {
                                Name = catchAll.Groups[1].Value,
                                Description = catchAll.Groups[2].Value,
                            });
                        }

                        continue;
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                    hasError = $"Error Line [{i}]\n" + line + "\n" + e.ToString();
                }
            }

            m.Name = name;
            m.Description = descr;
            m.HitDice = hitDice;
            m.HitPoints = hitPoints;
            m.ArmorClass = armorClass;
            m.Speed = speed;
            m.Strength = str;
            m.Dexterity = dex;
            m.Constitution = con;
            m.Intelligence = intelligence;
            m.Wisdom = wis;
            m.Charisma = cha;
            m.Senses = senses;
            m.Languages = languages;
            m.Resistances = resistance;
            m.Vulnerabilities = vulnerabilities;
            m.Immunities = immunities;
            m.ChallengeRating = challenge;
            m.Level = level;
            m.SpecialAbilities = specialAbilities.ToArray();
            m.Actions = actions.ToArray();

            if (hasError != "")
            {
                MessageBox.Show(hasError);
            }

            return m;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var saveType = (SaveAsNPC.IsChecked.HasValue && SaveAsNPC.IsChecked.Value) ? SaveType.Character : SaveType.Monster;
            _monster.Save(saveType);
            MessageBox.Show("Saved");
            Close();
        }

        private void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            _monster = ImportDndBeyondCharacter(ImportText.Text);
            MonsterUserControl.Monster = _monster;
            MainScrollViewer.ScrollToEnd();
        }
    }
}
