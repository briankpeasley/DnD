using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DMTool.Source
{
    public class CombatViewModel : ViewModel
    {
        private static int secondsPerRound = 6;

        private object participantsLock = new object();
        private Random rnJesus = new Random();

        public CombatViewModel()
        {
            Participants = new ObservableCollection<Character>();
            Participants.CollectionChanged += Participants_CollectionChanged;
            ActiveParticipant = 0;
            Clock = 0;
        }

        public void SaveAll(bool showAcknowledgementUI)
        {
            foreach (Character c in Participants)
            {
                if (c.GetType() == typeof(PlayerCharacter))
                {
                    c.Save();
                }
            }

            if (showAcknowledgementUI)
            {
                MessageBox.Show("Player Characters have been saved");
            }
        }

        public void DistributeCoin(Coin coin)
        {
            int playerCharacterCount = 0;
            foreach (Character c in Participants)
            {
                if (c.GetType() == typeof(PlayerCharacter))
                {
                    playerCharacterCount++;
                }
            }

            if (playerCharacterCount > 0)
            {
                Coin d = new Coin()
                {
                    Copper = (int)Math.Ceiling((double)coin.Copper / (double)playerCharacterCount),
                    Silver = (int)Math.Ceiling((double)coin.Silver / (double)playerCharacterCount),
                    Electrum = (int)Math.Ceiling((double)coin.Electrum / (double)playerCharacterCount),
                    Gold = (int)Math.Ceiling((double)coin.Gold / (double)playerCharacterCount),
                    Platinum = (int)Math.Ceiling((double)coin.Platinum / (double)playerCharacterCount)
                };

                foreach (Character c in Participants)
                {
                    if (c.GetType() == typeof(PlayerCharacter))
                    {
                        (c as PlayerCharacter).Coin.Add(d);
                    }
                }
            }
        }

        public void LongRest()
        {
            foreach (Character c in Participants)
            {
                c.DamageLog.Clear();
                c.ComputeCurrentHitPoints();
                c.InvokePropertyChange("HitPoints");

                if (c.GetType() == typeof(PlayerCharacter))
                {
                    foreach (Counter counter in (c as PlayerCharacter).Counters)
                    {
                        if (counter.Current < counter.Max)
                        {
                            counter.Current = counter.Max;
                        }
                    }
                }

                try
                {
                    c.RemainingHitDice = Int32.Parse(c.Level);
                }
                catch { }

                foreach(SpellSlot s in c.SpellSlots)
                {
                    s.Used = 0;
                }
            }
        }

        public void Sort()
        {
            for (int i = 0; i < Participants.Count - 1; i++)
            {
                for (int j = i + 1; j < Participants.Count; j++)
                {
                    if (
                        Participants[j].Initiative > Participants[i].Initiative ||
                        Participants[j].Initiative == Participants[i].Initiative && Participants[j].GetType() == typeof(PlayerCharacter)
                        )
                    {
                        // swap
                        var val = Participants[j];
                        Participants[j] = Participants[i];
                        Participants[i] = val;
                    }
                }
            }
        }

        public void RollInitiative()
        {
            for (int i = 0; i < Participants.Count; i++)
            {
                Participants[i].Initiative = Math.Max(1, rnJesus.Next(1, 20) + Modifiers.Compute(Participants[i].Dexterity));
            }

            Sort();
        }

        private void GiveExperiencePointsToPlayersForMonster(Monster m)
        {
            double xp = 0;
            try
            {
                xp = Charts.ChallengeToXp[m.ChallengeRating];
            }
            catch { }

            var currentHitPoints = m.ComputeCurrentHitPoints();
            if (currentHitPoints < 0)
            {
                currentHitPoints = 0;
            }

            var percentOfMax = 0.0;
            if (currentHitPoints >= m.HitPoints)
            {
                xp = 0;
            }
            else
            {
                percentOfMax = (m.HitPoints - currentHitPoints) / (double)m.HitPoints;
                xp *= Math.Min(percentOfMax, 1.0);
            }

            int pcCount = 0;
            foreach (Character pc in Participants)
            {
                if (pc.GetType() == typeof(PlayerCharacter))
                {
                    pcCount++;
                }
            }

            System.Diagnostics.Debug.WriteLine($"Handing out {xp} xp for {m.Name} with c/r {m.ChallengeRating} and destriction {percentOfMax} to {pcCount} players");

            if (pcCount > 1)
            {
                xp /= pcCount;
            }

            xp = Math.Round(xp, 2);
            if (xp <= 0)
            {
                return;
            }

            foreach (Character pc in Participants)
            {
                if (pc.GetType() == typeof(PlayerCharacter))
                {
                    System.Diagnostics.Debug.WriteLine($"    - Giving {xp} xp to {pc.Name}");

                    (pc as PlayerCharacter).XP += xp;
                }
            }
        }

        private void Participants_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (Character c in e.NewItems)
                {
                    c.SubscribeToRemoveCharacter((s, evt) =>
                    {
                        lock (participantsLock)
                        {
                            // update the active player to the first player in initiation order
                            // if this participant was last in the order
                            int index = Participants.IndexOf(s as Character);
                            if (index == Participants.Count - 1 && ActiveParticipant == index)
                            {
                                ActiveParticipant = 0;
                            }

                            // assign XP to the players
                            if (s.GetType() == typeof(Monster) && Participants.Contains(s))
                            {
                                var m = s as Monster;
                                GiveExperiencePointsToPlayersForMonster(m);
                            }

                            // Remove from participant combat
                            Participants.Remove(s as Character);
                        }
                    });
                }
            }
        }

        public ObservableCollection<Character> Participants { get; private set; }
        public int ActiveParticipant
        {
            get { return GetProperty<int>(); }
            set {
                SetProperty(value); 
            }
        }

        public float Clock
        {
            get { return GetProperty<float>(); }
            set { SetProperty(value); }
        }

        public void NextTurn(int dir)
        {
            SaveAll(false);
            int active = ActiveParticipant;

            active += dir;
            float clockIncrement = dir * (float)secondsPerRound / (float)Participants.Count;
            Clock += clockIncrement;

            foreach (Character c in Participants)
            {
                for (int i = c.Riders.Count - 1; i >= 0; i--)
                {
                    if (c.Riders[i].ClockTick(clockIncrement))
                    {
                        c.Riders.RemoveAt(i);
                    }
                }
            }

            if (active >= Participants.Count)
            {
                active = 0;
            }
            else if (active < 0)
            {
                active = Participants.Count - 1;
            }

            ActiveParticipant = active;

            foreach (Character c in Participants)
            {
                c.TheirTurn = false;
            }

            if (ActiveParticipant >= 0 && ActiveParticipant < Participants.Count)
            {
                Participants[active].TheirTurn = true;
            }
        }
    }
}
