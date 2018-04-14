using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMTool.Source
{
    public static class Charts
    {
        public static Dictionary<string, int> ChallengeToXp = new Dictionary<string, int>()
        {
            {"0", 10 },
            {"1/8", 25 },
            {"1/4", 50 },
            {"1/2", 100 },
            {"1", 200 },
            {"2", 450 },
            {"3", 700 },
            {"4", 1100 },
            {"5", 1800 },
            {"6", 2300 },
            {"7", 2900 },
            {"8", 3900 },
            {"9", 5000 },
            {"10", 5900 },
            {"11", 7200 },
            {"12", 8400 },
            {"13", 10000 },
            {"14", 11500 },
            {"15", 13000 },
            {"16", 15000 },
            {"17", 18000 },
            {"18", 20000 },
            {"19", 22000 },
            {"20", 25000 },
            {"21", 33000 },
            {"22", 41000 },
            {"23", 50000 },
            {"24", 62000 },
            {"25", 75000 },
            {"26", 90000 },
            {"27", 105000 },
            {"28", 120000 },
            {"29", 135000 },
            {"30", 155000 },
        };

        public static Dictionary<string, int> XPToLevel = new Dictionary<string, int>()
        {
            {"1", 0 },
            {"2", 300 },
            {"3", 900 },
            {"4", 2700 },
            {"5", 6500 },
            {"6", 14000 },
            {"7", 23000 },
            {"8", 34000 },
            {"9", 48000 },
            {"10", 64000 },
            {"11", 85000 },
            {"12", 100000 },
            {"13", 120000 },
            {"14", 140000 },
            {"15", 165000 },
            {"16", 195000 },
            {"17", 225000 },
            {"18", 265000 },
            {"19", 305000 },
            {"20", 355000 }
        };

        public static int[,] Threshold = new int[,]
        {
            {25,50,75,100}, // level 1
            {50,100,150,200 }, // level 2
            {75,150,225,400 }, // level 3
            {125,250,375,500 }, // 4
            {250,500,750,1100 }, // 5
            {300,600,900,1400 }, // 6
            {350,750,1100,1700 }, // 7
            {450,900,1400,2100 }, // 8
            {550,1100,1600,2400 }, // 9
            {600,1200,1900,2800 }, // 10
            {800,1600,2400,3600 }, // 11
            {1000,2000,3000,4500 }, // 12
            {1100,2200,3400,5100 }, // 13
            {1250,2500,3800,5700 }, // 14
            {1400,2800,4300,6400 }, // 15
            {1600,3200,4800,7200 }, // 16
            {2000,3900,5900,8800 }, // 17
            {2100,4200,6300,9500 }, // 18
            {2400,49600,7300,10900 }, // 19
            {28500,5700,8500,12700 }
        };

        public static int GetMonsterXP(string monsterLevel)
        {
            if (ChallengeToXp.ContainsKey(monsterLevel))
            {
                return ChallengeToXp[monsterLevel];
            }
            
            return 0;
        }

        public static int GetXPToLevel(string currentlevel)
        {
            int level;
            if(Int32.TryParse(currentlevel, out level))
            {
                return XPToLevel[Math.Min(20, level + 1).ToString()];
            }

            return -1;
        }

        public static int GetXPForLevel(string currentlevel)
        {
            int level;
            if (Int32.TryParse(currentlevel, out level))
            {
                return XPToLevel[Math.Min(20, level).ToString()];
            }

            return -1;
        }
    }
}
