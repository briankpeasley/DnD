using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DMTool.Source
{
    public class SpecialAbility : ViewModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "desc")]
        public string Description
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }

    public class Action : ViewModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "desc")]
        public string Description
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }


    public class Monster : Character, ICloneable
    {
        public Monster()
        {
            try
            {
                Initiative = (App.Current as App).RNG.Next(1, 23);
            }
            catch
            {
                Initiative = 0;
            }
        }

        [JsonProperty(PropertyName = "challenge_rating")]
        public string ChallengeRating
        {
            get { return GetProperty<string>(); }
            set
            {
                SetProperty(value);
                Level = value;
            }
        }

        [JsonProperty(PropertyName = "special_abilities")]
        public SpecialAbility[] SpecialAbilities
        {
            get { return GetProperty<SpecialAbility[]>(); }
            set { SetProperty(value); }
        }

        [JsonProperty(PropertyName = "actions")]
        public SpecialAbility[] Actions
        {
            get { return GetProperty<SpecialAbility[]>(); }
            set { SetProperty(value); }
        }

        public void GenerateHitPoints(Random r = null, int hitDice = -1)
        {
            try
            {
                string[] splitOne = HitDice.Replace(" ", string.Empty).Split('d');
                string[] splitTwo = splitOne[1].Split('+');

                int diceCount = Int32.Parse(splitOne[0]);
                int diceValue = Int32.Parse(splitTwo[0]);
                int flat = splitTwo.Length == 2 ? Int32.Parse(splitTwo[1]) : 0;

                int hp = flat;
                r = r == null ? new Random() : r;

                for (int i = 0; i < diceCount; i++)
                {
                    hp += r.Next(1, diceValue);
                }
                HitPoints = hp;
            }
            catch
            {
                HitPoints = 0;
            }
        }

        [JsonProperty(PropertyName = "hit_dice")]
        public string HitDice
        {
            get { return GetProperty<string>(); }
            set
            {
                SetProperty(value);
                GenerateHitPoints();
            }
        }

        public object Clone()
        {
            return JsonConvert.DeserializeObject<Monster>(JsonConvert.SerializeObject(this));
        }
    }
}
