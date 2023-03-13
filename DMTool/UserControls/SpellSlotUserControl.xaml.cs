using DMTool.Source;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DMTool.UserControls
{
    /// <summary>
    /// Interaction logic for SpellSlotUserControl.xaml
    /// </summary>
    public partial class SpellSlotUserControl : UserControl
    {
        public static readonly DependencyProperty SpellSlotProperty =
           DependencyProperty.Register("SpellSlot", typeof(SpellSlot), typeof(SpellSlotUserControl), new FrameworkPropertyMetadata(null, SpellSlotChanged));

        public SpellSlotUserControl()
        {
            InitializeComponent();
        }

        public SpellSlot SpellSlot
        {
            get { return GetValue(SpellSlotProperty) as SpellSlot; }
            set { SetValue(SpellSlotProperty, value); }
        }

        private static void SpellSlotChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SpellSlotUserControl ctrl = d as SpellSlotUserControl;
            SpellSlot slot = e.NewValue as SpellSlot;
            if(slot == null)
            {
                return;
            }


            ctrl.SetSpellSlot(slot);
            ctrl.Draw();
        }

        private void SetSpellSlot(SpellSlot slot)
        {
            slot.PropertyChanged += Slot_PropertyChanged;
            txtLevel.Text = $"Level {slot.Level}";
            txtCount.Text = slot.Total.ToString();
        }

        private void Slot_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Used")
            {
                Draw();
            }
        }

        private void Draw()
        {
            (App.Current as Application).Dispatcher.Invoke(() =>
            {
                slots.Items.Clear();

                for (int i = 0; i < SpellSlot.Total; i++)
                {
                    slots.Items.Add(i < SpellSlot.Used);
                }
            });
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if(SpellSlot != null)
            {
                SpellSlot.Used++;
            }

            Draw();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SpellSlot != null)
            {
                SpellSlot.Used--;
            }

            Draw();
        }

        private void txtCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            int newTotal;
            if(int.TryParse(txtCount.Text, out newTotal))
            {
                SpellSlot.Total = newTotal;
                Draw();
            }
        }
    }
}
