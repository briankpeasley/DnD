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
    /// Interaction logic for MonsterUserControl.xaml
    /// </summary>
    public partial class MonsterUserControl : UserControl
    {
        public static readonly DependencyProperty MonsterProperty =
           DependencyProperty.Register("Monster", typeof(Monster), typeof(MonsterUserControl), new FrameworkPropertyMetadata(null, MonsterChanged));
        
        private static void MonsterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MonsterUserControl ctrl = d as MonsterUserControl;
            ctrl.grid.DataContext = e.NewValue;
            ctrl.test.DataContext = ctrl;
            ctrl.characterUserControl.DataContext = ctrl;
            ctrl.specialAbilities.ItemsSource = (e.NewValue as Monster).SpecialAbilities;
            ctrl.actions.ItemsSource = (e.NewValue as Monster).Actions;
        }

        public MonsterUserControl()
        {
            InitializeComponent();
        }

        public Monster Monster
        {
            get { return GetValue(MonsterProperty) as Monster; }
            set { SetValue(MonsterProperty, value); }
        }
    }
}
