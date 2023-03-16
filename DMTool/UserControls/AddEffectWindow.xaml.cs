using DMTool.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DMTool.UserControls
{
    /// <summary>
    /// Interaction logic for AddEffectWindow.xaml
    /// </summary>
    public partial class AddEffectWindow : Window
    {
        public event EventHandler<Rider> RiderSelected;

        public RidersViewModel RiderViewModel { get; set; }

        public AddEffectWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            riders.ItemsSource = RiderViewModel.DiscoveredRiders;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (riders.SelectedItem != null)
                {
                    Rider r = (riders.SelectedItem as Rider).Clone() as Rider;
                    int duration;
                    if (Int32.TryParse(txtDuration.Text, out duration))
                    {
                        r.Duration = duration;
                        RiderSelected?.Invoke(this, r);
                    }
                }
            }
        }

        private void customRider_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && customRider.Text != string.Empty)
            {
                Rider newRider = new Rider();
                newRider.Description = "";
                newRider.Name = customRider.Text;

                if (Int32.TryParse(txtDuration.Text, out int duration))
                {
                    newRider.Duration = duration;
                    RiderSelected?.Invoke(this, newRider);
                }

                e.Handled = true;
            }
        }
    }
}
