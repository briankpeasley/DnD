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
    /// Interaction logic for AddCoinToParty.xaml
    /// </summary>
    public partial class AddCoinToParty : Window
    {
        public event EventHandler<Coin> DistributeCoin;

        public AddCoinToParty()
        {
            InitializeComponent();
        }

        private void DistributeClicked(object sender, RoutedEventArgs e)
        {
            int copper;
            int silver;
            int electrum;
            int gold;
            int platinum;

            if(Int32.TryParse(txtCopper.Text, out copper) == false)
            {
                MessageBox.Show("Copper is not correct format");
                return;
            }

            if (Int32.TryParse(txtSilver.Text, out silver) == false)
            {
                MessageBox.Show("Silver is not correct format");
                return;
            }

            if (Int32.TryParse(txtElectrum.Text, out electrum) == false)
            {
                MessageBox.Show("Electrum is not correct format");
                return;
            }

            if (Int32.TryParse(txtGold.Text, out gold) == false)
            {
                MessageBox.Show("Gold is not correct format");
                return;
            }

            if (Int32.TryParse(txtPlatinum.Text, out platinum) == false)
            {
                MessageBox.Show("Platinum is not correct format");
                return;
            }

            Coin c = new Coin()
            {
                Copper = copper,
                Silver = silver,
                Electrum = electrum,
                Gold = gold,
                Platinum = platinum
            };

            DistributeCoin?.Invoke(this, c);
            Close();
        }
    }
}
