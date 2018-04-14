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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DMTool.UserControls
{
    /// <summary>
    /// Interaction logic for SpellUserControl.xaml
    /// </summary>
    public partial class SpellUserControl : UserControl
    {
        public static readonly DependencyProperty SpellProperty =
           DependencyProperty.Register("Spell", typeof(Spell), typeof(SpellUserControl), new FrameworkPropertyMetadata(null, SpellChanged));

        public SpellUserControl()
        {
            InitializeComponent();
        }

        public Spell Spell
        {
            get { return GetValue(SpellProperty) as Spell; }
            set { SetValue(SpellProperty, value); }
        }

        private static void SpellChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            SpellUserControl ctrl = d as SpellUserControl;
            Spell s = e.NewValue as Spell;
            if (s != null)
            {
                ctrl.txtLevel.Text = s.Level;
                ctrl.txtCastingTime.Text = s.CastingTime;
                ctrl.txtRange.Text = s.Range;
                ctrl.txtComponents.Text = s.Components;
                if (s.Concentration != null)
                {
                    ctrl.txtDuration.Text = (s.Concentration == "yes" ? "Concentration, " : string.Empty) + s.Duration;
                }
                else
                {
                    ctrl.txtDuration.Text = s.Duration;
                }

                if (s.Description != null)
                {
                    ctrl.txtDescription.Text = s.Description.Replace("<p>", string.Empty).Replace("</p>", string.Empty);
                }

                if (s.HigherLevel != null)
                {
                    ctrl.txtHigherLevel.Text = s.HigherLevel.Replace("<p>", string.Empty).Replace("</p>", string.Empty);
                }
            }
        }
    }
}
