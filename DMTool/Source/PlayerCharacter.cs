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

        public event EventHandler CoinsChanged;

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
            CoinsChanged?.Invoke(this, EventArgs.Empty);
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

        public double GetWeight()
        {
            return (Copper + Silver + Gold + Electrum + Platinum) / 50;
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

        public double Weight
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public double Count
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public string Value
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }

    public class Counter : ViewModel
    {
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public int Max
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        public int Current
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }
    }

    public class PlayerCharacter : Character
    {
        public PlayerCharacter()
        {
            Gear = new ObservableCollection<Gear>();
            Counters = new ObservableCollection<Counter>();
            Coin = new Coin();

            Coin.CoinsChanged += Coin_CoinsChanged;
            Gear.CollectionChanged += Gear_CollectionChanged;
        }

        private void Gear_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Gear g in e.NewItems)
                {
                    g.PropertyChanged += gearPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (Gear g in e.OldItems)
                {
                    g.PropertyChanged -= gearPropertyChanged;
                }
            }

            EvaluateWeight();
        }

        private void gearPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Weight" || e.PropertyName == "Count")
            {
                EvaluateWeight();
            }
        }

        public double XP
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        public ObservableCollection<Gear> Gear;
        public ObservableCollection<Counter> Counters;

        public Coin Coin
        {
            get { return GetProperty<Coin>(); }
            set { SetProperty(value); }
        }

        public double Weight
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        private void Coin_CoinsChanged(object sender, EventArgs e)
        {
            coinWeight = (sender as Coin).GetWeight();
            Weight = coinWeight + gearWeight;
        }

        private void EvaluateWeight()
        {
            gearWeight = 0;
            foreach (Gear g in Gear)
            {
                gearWeight = g.Count * g.Weight;
            }

            Weight = coinWeight + gearWeight;
        }

        private double coinWeight;
        private double gearWeight;
    }
}
