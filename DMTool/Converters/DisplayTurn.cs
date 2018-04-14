using DMTool.Source;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace DMTool.Converters
{
    public class DisplayTurn : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values != null && values.Length == 3)
            {
                int turn = (int)values[0];
                ObservableCollection<Character> characters = values[1] as ObservableCollection<Character>;
                Character c = values[2] as Character;

                if (c != null)
                {
                    return turn == characters.IndexOf(c) ? Visibility.Visible : Visibility.Hidden;
                }
            }

            return Visibility.Hidden;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
