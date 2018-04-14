using DMTool.Source;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DMTool.Converters
{
    public class CharacterToCurrentHitPoints : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null && value.GetType().BaseType == typeof(Character))
            {
                Character c = value as Character;
                int current = c.HitPoints + c.TemporaryHitPoints;
                foreach(int dmg in c.DamageLog)
                {
                    current += dmg;
                }

                return current.ToString();
            }

            return "!";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
