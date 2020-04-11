using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MapReveal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PlayerView _pv;
        private EraseBlock _eraser;
        public MapViewModel MapViewModel { get; private set; }


        public MainWindow()
        {
            InitializeComponent();

            this.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("You did something only a dumbass would do!", "Don't be a dumbass!");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MapViewModel = new MapViewModel();

            _eraser = new EraseBlock();
            _eraser.HorizontalAlignment = HorizontalAlignment.Left;
            _eraser.VerticalAlignment = VerticalAlignment.Top;
            _eraser.Visibility = Visibility.Hidden;
            _eraser.IsHitTestVisible = false;
            this.grid.Children.Add(_eraser);
            _eraser.Loaded += _eraser_Loaded;

            _pv = new PlayerView();
            _pv.Loaded += Pv_Loaded;
            _pv.Show();

            _pv.laserPoint.Visibility = Visibility.Hidden;
        }

        private void _eraser_Loaded(object sender, RoutedEventArgs e)
        {
            _eraser.Width = MapViewModel.EraserSize;
            _eraser.Height = MapViewModel.EraserSize;
        }

        private void Pv_Loaded(object sender, RoutedEventArgs e)
        {
            
            MapViewModel.SetInkCanvas(this, _pv, DMView, _pv.InkCanvas());

            grid.DataContext = MapViewModel;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.RestoreDirectory = true;
                ofd.InitialDirectory = Environment.CurrentDirectory;
                ofd.Multiselect = false;
                bool? ret = ofd.ShowDialog();
                if (ret.HasValue && ret.Value)
                {
                    MapViewModel.MapFile = ofd.FileName;
                    MapViewModel.LoadImage();
                    MapViewModel.FillMap();
                }
            }
            else if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                Directory.CreateDirectory("Maps");
                MapViewModel.Save($"Maps/{MapViewModel.MapName}.map");
            }
            else if (e.Key == Key.L && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.RestoreDirectory = true;
                ofd.InitialDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, "Maps");
                ofd.Multiselect = false;
                bool? ret = ofd.ShowDialog();
                if (ret.HasValue && ret.Value)
                {
                    MapViewModel.Load(ofd.FileName);
                }
            }
            else if (e.Key == Key.Up)
            {
                MapViewModel.EraserSize += 5;
                _eraser.Width = MapViewModel.EraserSize;
                _eraser.Height = MapViewModel.EraserSize;
            }
            else if (e.Key == Key.Down)
            {
                MapViewModel.EraserSize -= 5;
                _eraser.Width = MapViewModel.EraserSize;
                _eraser.Height = MapViewModel.EraserSize;
            }
        }

        private void DMView_MouseMove(object sender, MouseEventArgs e)
        {
            Point pos = e.GetPosition(sender as IInputElement);
            _eraser.Margin = new Thickness(pos.X - MapViewModel.EraserSize / 2, pos.Y - MapViewModel.EraserSize / 2, 0, 0);
            _pv.laserPoint.Margin = new Thickness(pos.X - 5, pos.Y - 5, 0, 0);
        }

        private void DMView_MouseEnter(object sender, MouseEventArgs e)
        {
            _eraser.Visibility = Visibility.Visible;
            _pv.laserPoint.Visibility = Visibility.Visible;
        }

        private void DMView_MouseLeave(object sender, MouseEventArgs e)
        {
            _eraser.Visibility = Visibility.Hidden;
            _pv.laserPoint.Visibility = Visibility.Hidden;
        }
    }
}
