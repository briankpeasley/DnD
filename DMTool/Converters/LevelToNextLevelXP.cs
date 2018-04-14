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
    public class LevelToNextLevelXP : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value.GetType() == typeof(string))
            {
                int xpToLevel = Charts.GetXPToLevel(value as string);
                if(xpToLevel > -1)
                {
                    return xpToLevel.ToString();
                }
            }

            return "???";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
