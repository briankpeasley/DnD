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
    public class SpellToTooltip : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value != null && value.GetType() == typeof(Spell))
            {
                Spell s = value as Spell;
                return
                    $"Level {s.Level} spell\n" +
                    $"Casting Time: {s.CastingTime}\n" +
                    $"Range: {s.Range}\n" +
                    $"Components: {s.Components}\n" +
                    $"Duration: {s.Duration}\n\n" +
                    $"{s.Description}";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
