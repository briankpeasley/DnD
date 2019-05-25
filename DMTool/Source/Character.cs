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
    public class Character : ViewModel, IEquatable<Character>
    {
        public Character()
        {
            Level = "1";
            DamageLog = new List<int>();
            Riders = new ObservableCollection<Rider>();
            Riders.CollectionChanged += Riders_CollectionChanged;
            Cantrips = new ObservableCollection<Spell>();
            Level1 = new ObservableCollection<Spell>();
            Level2 = new ObservableCollection<Spell>();
            Level3 = new ObservableCollection<Spell>();
            Level4 = new ObservableCollection<Spell>();
            Level5 = new ObservableCollection<Spell>();
            Level6 = new ObservableCollection<Spell>();
            Level7 = new ObservableCollection<Spell>();
            Level8 = new ObservableCollection<Spell>();
            Level9 = new ObservableCollection<Spell>();

            HandleSpellList(Cantrips);
            HandleSpellList(Level1);
            HandleSpellList(Level2);
            HandleSpellList(Level3);
            HandleSpellList(Level4);
            HandleSpellList(Level5);
            HandleSpellList(Level6);
            HandleSpellList(Level7);
            HandleSpellList(Level8);
            HandleSpellList(Level9);
        }

        public event EventHandler RemoveCharacter;
        public void Remove()
        {
            RemoveCharacter?.Invoke(this, EventArgs.Empty);
        }

        private void HandleSpellList(ObservableCollection<Spell> spellList)
        {
            spellList.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (Spell spell in e.NewItems)
                    {
                        spell.RemoveSpell += (sender, evt) =>
                        {
                            spellList.Remove(sender as Spell);
                        };
                    }
                }
            };
        }

        private void Riders_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Rider r in e.NewItems)
                {
                    r.Remove += (s, evt) =>
                    {
                        Riders.Remove(r);
                    };
                }
            }
        }

        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty(PropertyName = "level")]
        public string Level
        {
            get { return GetProperty<string>(); }
            set
            {
                SetProperty(value);
                if (string.IsNullOrEmpty(value) == false)
                {
                    try
                    {
                        RemainingHitDice = Int32.Parse(value);
                    }
                    catch { }
                }
            }
        }

        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "class")]
        public string Class
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /* Flavor */
        [JsonProperty(PropertyName = "description")]
        public string Description;
        public string ImageLocation;

        /* Attributes */
        [JsonProperty(PropertyName = "strength")]
        public int Strength
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "dexterity")]
        public int Dexterity
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "constitution")]
        public int Constitution
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "intelligence")]
        public int Intelligence
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "wisdom")]
        public int Wisdom
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "charisma")]
        public int Charisma
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        /* Combat */
        [JsonProperty(PropertyName = "armor_class")]
        public int ArmorClass
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "speed")]
        public string Speed
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "hitPoints")]
        public int HitPoints
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "temp_hit_points")]
        public int TemporaryHitPoints
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "damage_log")]
        public List<int> DamageLog
        {
            get { return GetProperty<List<int>>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "RemainingHitDice")]
        public int RemainingHitDice
        {
            get { return GetProperty<int>(); }
            set {
                SetProperty(value);

                if (GetType() == typeof(Monster))
                {
                    Monster m = (Monster)this;
                    m.GenerateHitPoints(null, value);
                }
            }
        }

        [JsonProperty(PropertyName = "riders")]
        public ObservableCollection<Rider> Riders { get; set; }

        /* Spells */
        public ObservableCollection<Spell> Cantrips { get; set; }
        public ObservableCollection<Spell> Level1 { get; set; }
        public ObservableCollection<Spell> Level2 { get; set; }
        public ObservableCollection<Spell> Level3 { get; set; }
        public ObservableCollection<Spell> Level4 { get; set; }
        public ObservableCollection<Spell> Level5 { get; set; }
        public ObservableCollection<Spell> Level6 { get; set; }
        public ObservableCollection<Spell> Level7 { get; set; }
        public ObservableCollection<Spell> Level8 { get; set; }
        public ObservableCollection<Spell> Level9 { get; set; }

        /* Additional */
        [JsonProperty(PropertyName = "damage_resistances")]
        public string Resistances
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "damage_vulnerabilities")]
        public string Vulnerabilities
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "damage_immunities")]
        public string Immunities
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [JsonIgnore]
        public int Initiative
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "senses")]
        public string Senses
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Languages
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public void InvokePropertyChange(string Member)
        {
            this.Invoke(Member);
        }

        public bool Equals(Character other)
        {
            return Equals(other as object);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType().BaseType != typeof(Character))
            {
                return false;
            }

            return (obj as Character).Id == Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
