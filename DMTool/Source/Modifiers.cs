using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMTool.Source
{
    internal static class Modifiers
    {
        public static int Compute(int baseValue)
        {
            if (baseValue < 1)
            {
                baseValue = 1;
            }

            if (baseValue > 30)
            {
                baseValue = 30;
            }

            return (int)(baseValue / 2) - 5;
        }
    }
}
