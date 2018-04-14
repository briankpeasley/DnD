using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace DMTool.Converters
{
    public class AttributeToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value.GetType() == typeof(int))
            {
                int baseValue = (int)value;
                if (baseValue < 1)
                {
                    baseValue = 1;
                }

                if (baseValue > 30)
                {
                    baseValue = 30;
                }

                int mod = (int)(baseValue / 2) - 5;

                string sign = mod >= 0 ? "+" : "";
                return $"({sign}{mod})";
            }

            return "!";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
