using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMTool.Source
{
    public class Coin : ViewModel
    {
        public Coin()
        {
            Copper = 0;
            Silver = 0;
            Gold = 0;
            Electrum = 0;
            Platinum = 0;
        }

        public int Copper
        {
            get { return GetProperty<int>(); }
            set
            {
                SetProperty(value);
                Total = EvaluateTotalWorth();
            }
        }

        public int Silver
        {
            get { return GetProperty<int>(); }
            set
            {
                SetProperty(value);
                Total = EvaluateTotalWorth();
            }
        }

        public int Gold
        {
            get { return GetProperty<int>(); }
            set
            {
                SetProperty(value);
                Total = EvaluateTotalWorth();
            }
        }

        public int Electrum
        {
            get { return GetProperty<int>(); }
            set
            {
                SetProperty(value);
                Total = EvaluateTotalWorth();
            }
        }

        public int Platinum
        {
            get { return GetProperty<int>(); }
            set
            {
                SetProperty(value);
                Total = EvaluateTotalWorth();
            }
        }

        public double Total
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public double EvaluateTotalWorth()
        {
            double total =
                Copper * 0.01
                + Silver * 0.1
                + Electrum * 0.5
                + Gold
                + Platinum * 10;

            total = Math.Round(total, 3);
            return total;
        }

        public void Add(Coin c)
        {
            Copper += c.Copper;
            Silver += c.Silver;
            Gold += c.Gold;
            Electrum += c.Electrum;
            Platinum += c.Platinum;
        }
    }

    public class Gear : ViewModel
    {
        [JsonIgnore]
        public Guid ID { get; set; } = Guid.NewGuid();

        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Description
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Weight
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Count
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Value
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }

    public class PlayerCharacter : Character
    {
        public PlayerCharacter()
        {
            Gear = new ObservableCollection<Source.Gear>();
            Coin = new Coin();
        }

        public double XP
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public ObservableCollection<Gear> Gear;

        public Coin Coin
        {
            get { return GetProperty<Coin>(); }
            set { SetProperty(value); }
        }
    }
}
