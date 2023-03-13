using DMTool.Source;
using System;
using System.Collections.Generic;

using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace DMTool.Converters
{
    public class CharacterTextStyling : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string mode = parameter as string;


            if (mode == "weight")
            {
                if (value != null && value.GetType() == typeof(PlayerCharacter))
                {
                    return FontWeights.ExtraBold;
                }

                return FontWeights.Thin;
            }
            else if (mode == "colorBadge")
            {
                if (value != null && value.GetType() == typeof(PlayerCharacter))
                {
                    return new SolidColorBrush(Color.FromArgb(200, 0, 0, 200));
                }

                return new SolidColorBrush(Color.FromArgb(200, 200, 0, 0));
            }

            throw new NotImplementedException($"Parameter {mode} is not implemented in CharacterTextStyling");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
