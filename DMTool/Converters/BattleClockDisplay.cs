using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DMTool.Converters
{
    public class BattleClockDisplay : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           if(value != null)
            {
                float totalSeconds = (float)value;
                TimeSpan span = TimeSpan.FromSeconds(totalSeconds);
                return $"{span.Minutes}:{span.Seconds}.{span.Milliseconds}";
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
