using DMTool.Source;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DMTool.Converters
{
    public class SelectedPlayerToBorder : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter as string == "BorderThickness")
            {
                if (value as bool? == true)
                {
                    return 5;
                }
                else
                {
                    return 2;
                }
            }
            else if (parameter as string == "BorderColor")
            {
                if (value is Monster)
                {
                    return new SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 0, 0));
                }
                else
                {
                    return new SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 255, 0));
                }
            }
            else if (parameter as string == "Portrait")
            {
                Character c = value as Character;
                string path;
                if(string.IsNullOrEmpty(c.Portrait))
                {
                    path = Directory.GetCurrentDirectory() + "/portraits/unknown.jpg";
                }
                else
                {
                    path = Directory.GetCurrentDirectory() + "/portraits/" + c.Portrait;
                }

                return new Uri(path);
            }


            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
