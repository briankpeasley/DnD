﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMTool.Source
{

    public class Spell : ViewModel, ICloneable
    {
        public Spell()
        {
            Cooldown = false;
        }

        public event EventHandler RemoveSpell;

        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "desc")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "higher_level")]
        public string HigherLevel { get; set; }

        [JsonProperty(PropertyName = "range")]
        public string Range { get; set; }

        [JsonProperty(PropertyName = "components")]
        public string Components { get; set; }

        [JsonProperty(PropertyName = "duration")]
        public string Duration { get; set; }

        [JsonProperty(PropertyName = "concentration")]
        public string Concentration { get; set; }

        [JsonProperty(PropertyName = "casting_time")]
        public string CastingTime { get; set; }

        [JsonProperty(PropertyName = "level")]
        public string Level { get; set; }

        public bool Cooldown
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public void Remove()
        {
            RemoveSpell?.Invoke(this, EventArgs.Empty);
        }

        public object Clone()
        {
            return new Spell()
            {
                Description = Description,
                Duration = Duration,
                CastingTime = CastingTime,
                Components = Components,
                Concentration = Concentration,
                HigherLevel = HigherLevel,
                Level = Level,
                Name = Name,
                Range = Range
            };
        }
    }

    public class SpellSlot : ViewModel, ICloneable
    {
        public SpellSlot(int level, int total, int used)
        {
            Total = total;
            Level = level;
            Used = used;
        }

        public SpellSlot()
        {
            Total = 0;
            Level = 0;
            Used = 0;
        }

        [JsonProperty(PropertyName = "Total")]
        public int Total
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "Used")]
        public int Used
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "Level")]
        public int Level { get; set; }

        public object Clone()
        {
            return new SpellSlot(Level, Total, Used);
        }
    }
}
