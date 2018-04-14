using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMTool.Source
{
    public static class CharacterExtensions
    {
        public static void Save(this Character c)
        {
            if (c != null)
            {
                string result = JsonConvert.SerializeObject(c);
                string dir = c.GetType() == typeof(PlayerCharacter) ? "./PlayerCharacter" : "./Monsters";

                if (Directory.Exists(dir) == false)
                {
                    Directory.CreateDirectory(dir);
                }

                if (File.Exists($"${dir}/{c.Name}.json") == false)
                {
                    using (FileStream fs = File.Create($"{dir}/{c.Name}.json")) { }
                }

                File.WriteAllText($"{dir}/{c.Name}.json", result);
            }
        }

        public static void ApplyDamage(this Character c, int damage)
        {
            if (damage > 0)
            {
                int min = Math.Min(damage, c.TemporaryHitPoints);
                damage -= min;
                c.TemporaryHitPoints -= min;

                if (damage > 0)
                {
                    c.DamageLog.Add(damage);
                }
            }
            else
            {
                int current = c.ComputeCurrentHitPoints();
                damage = Math.Min(-damage, Math.Max(0, c.HitPoints - current));
                if (damage > 0)
                {
                    c.DamageLog.Add(-damage);
                }
            }

            c.InvokePropertyChange("DamageLog");
        }

        public static int ComputeCurrentHitPoints(this Character c)
        {
            int totalDmg = 0;
            foreach (int dmg in c.DamageLog)
            {
                totalDmg += dmg;
            }

            int current = Math.Min(c.HitPoints + c.TemporaryHitPoints, (c.HitPoints + c.TemporaryHitPoints - totalDmg));
            return current;
        }
    }
}
