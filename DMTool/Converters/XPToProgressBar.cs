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
    public class XPToProgressBar : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values != null && values.Length == 2 && values[0].GetType() == typeof(double) && values[1].GetType() == typeof(string))
            {
                double XP = (double)values[0];
                double xpToLevel = (double)Charts.GetXPToLevel(values[1] as string);
                double xpForLevel = (double)Charts.GetXPForLevel(values[1] as string);

                if(xpToLevel > 0 && xpForLevel > 0)
                {
                    return Math.Max(0, Math.Min(100, 100 * (XP - xpForLevel) / (xpToLevel - xpForLevel)));
                }
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
