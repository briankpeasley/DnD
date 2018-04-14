using DMTool.Source;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace DMTool.Converters
{
    public class ComputeCurrentHitPoints : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {   
            if(values.Length == 4 && values[0].GetType() == typeof(int) && values[1].GetType() == typeof(int) && values[2].GetType() == typeof(List<int>) && values[3].GetType().BaseType == typeof(Character))
            {
                int max = (int)values[0];
                int temp = (int)values[1];
                List<int> log = values[2] as List<int>;

                int totalDmg = 0;
                foreach(int dmg in log)
                {
                    totalDmg += dmg;
                }

                int current = Math.Min(max + temp, (max + temp - totalDmg));
                double ratio = (double)current / (double)max;
                string flavor;
                SolidColorBrush backgroundBrush;
                if(ratio >= 0.9)
                {
                    flavor = "Healthy";
                    backgroundBrush = new SolidColorBrush(Color.FromArgb(63, 0, 255, 0));
                }
                else if(ratio >= 0.6)
                {
                    flavor = "Bruised";
                    backgroundBrush = new SolidColorBrush(Color.FromArgb(63, 255, 255, 0));
                }
                else if(ratio >= 0.2)
                {
                    flavor = "Injured";
                    backgroundBrush = new SolidColorBrush(Color.FromArgb(63, 255, 0, 0));
                }
                else if(ratio > 0)
                {
                    flavor = "Critical";
                    backgroundBrush = new SolidColorBrush(Color.FromArgb(63, 255, 0, 255));
                }
                else
                {
                    flavor = "Down";
                    backgroundBrush = new SolidColorBrush(Color.FromArgb(63, 0, 0, 0));
                }

                switch(parameter as string)
                {
                    case "0":
                        return current.ToString();
                    case "1":
                        return backgroundBrush;
                    case null:
                        if (values[3].GetType() == typeof(PlayerCharacter))
                        {
                            return $"{current} [{max}]";
                        }
                        else
                        {
                            return flavor;
                        }
                }
            }

            return "!";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}