using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MapReveal2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int EraserSize { get; set; } = 20;

        private Dispatcher _dispatcher;
        private WriteableBitmap _opacityMask;
        private PlayerWindow _playerWindow;
        private EraseBlock _eraser;
        private string _filenameDm;
        private string _filenamePlayer;

        public MainWindow()
        {
            InitializeComponent();

            _dispatcher = Dispatcher.CurrentDispatcher;

            _playerWindow = new PlayerWindow();
            _playerWindow.Show();

            _eraser = new EraseBlock();
            _eraser.HorizontalAlignment = HorizontalAlignment.Left;
            _eraser.VerticalAlignment = VerticalAlignment.Top;
            _eraser.Visibility = Visibility.Visible;
            _eraser.IsHitTestVisible = false;
            this.grid.Children.Add(_eraser);
            _eraser.Loaded += _eraser_Loaded;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DrawEraser(sender, e);
            }
            else if (e.RightButton == MouseButtonState.Pressed)
            {
                DrawEraser(sender, e, true);
            }

            Point pos = e.GetPosition(sender as IInputElement);
            _eraser.Margin = new Thickness(pos.X - EraserSize / 2, pos.Y - EraserSize / 2, 0, 0);

            Point mousePosition = e.GetPosition(OpacityImage);
            if (OpacityImage.ActualWidth == 0 || OpacityImage.ActualHeight == 0)
            {
                return;
            }

            var xPcnt = mousePosition.X / OpacityImage.ActualWidth;
            var yPcnt = mousePosition.Y / OpacityImage.ActualHeight;
            var borderLeft = Math.Max(_playerWindow.grid.ActualWidth - _playerWindow.OpacityImage.ActualWidth, 0) / 2;
            var borderTop = Math.Max(_playerWindow.grid.ActualHeight - _playerWindow.OpacityImage.ActualHeight, 0) / 2;
            var xPos = _playerWindow.OpacityImage.ActualWidth * xPcnt + borderLeft;
            var yPos = _playerWindow.OpacityImage.ActualHeight * yPcnt + borderTop;
            _playerWindow.laserPoint.Margin = new Thickness(xPos - 5, yPos - 5, 0, 0);
        }

        private void _eraser_Loaded(object sender, RoutedEventArgs e)
        {
            _eraser.Width = EraserSize;
            _eraser.Height = EraserSize;
        }

        private void DrawEraser(object sender, MouseEventArgs e, bool undo = false)
        {
            if (_opacityMask == null)
            {
                return;
            }

            Point mousePosition = e.GetPosition(OpacityImage);
            var xPcnt = mousePosition.X / OpacityImage.ActualWidth;
            var yPcnt = mousePosition.Y / OpacityImage.ActualHeight;
            var xPos = _opacityMask.Width * xPcnt;
            var yPos = _opacityMask.Height * yPcnt;

            var rect = new Int32Rect()
            {
                X = (int)xPos - EraserSize / 2,
                Y = (int)yPos - EraserSize / 2,
                Width = EraserSize,
                Height = EraserSize
            };

            DrawOnOpacityMask(_opacityMask, rect, 0, 0, 0, undo ? (byte)255 : (byte)0);
        }

        private void DrawOnOpacityMask(WriteableBitmap opacityMask, Int32Rect rect, byte r, byte g, byte b, byte a)
        {
            int x1 = Math.Max(Math.Min(rect.X + rect.Width, (int)opacityMask.Width), 0);
            int x0 = Math.Min(Math.Max(rect.X, 0), x1);
            int y1 = Math.Max(Math.Min(rect.Y + rect.Height, (int)opacityMask.Height), 0);
            int y0 = Math.Min(Math.Max(rect.Y, 0), y1);

            try
            {
                opacityMask.Lock();

                unsafe
                {
                    // Get a pointer to the back buffer.
                    var pixels = (byte*)opacityMask.BackBuffer;
                    int width = opacityMask.PixelWidth;
                    int height = opacityMask.PixelHeight;
                    int bytesPerPixel = (opacityMask.Format.BitsPerPixel + 7) / 8;
                    int stride = opacityMask.BackBufferStride;

                    // Initialize the image with blank.
                    for (int y = y0; y < y1; y++)
                    {
                        for (int x = x0; x < x1; x++)
                        {
                            var pixelPos = y * stride + (x * bytesPerPixel);
                            pixels[pixelPos + 0] = b;
                            pixels[pixelPos + 1] = g;
                            pixels[pixelPos + 2] = r;
                            pixels[pixelPos + 3] = a;
                        }
                    }
                }

                // Specify the area of the bitmap that changed.
                opacityMask.AddDirtyRect(new Int32Rect(x0, y0, x1 - x0, y1 - y0));
            }
            finally
            {
                // Release the back buffer and make it available for display.
                opacityMask.Unlock();
            }
        }

        private WriteableBitmap ClearOpacityMask(Image opacityImage, Image mapImage, byte r, byte g, byte b, byte a)
        {
            var pixelFormat = PixelFormats.Bgra32;
            int height = (int)mapImage.Source.Height;
            int width = (int)mapImage.Source.Width;
            int bpp = pixelFormat.BitsPerPixel / 8;
            var dpi = 96.0;

            var opacityMask = new WriteableBitmap(width, height, dpi, dpi, pixelFormat, null);

            try
            {
                opacityMask.Lock();

                unsafe
                {
                    // Get a pointer to the back buffer.
                    var pixels = (byte*)opacityMask.BackBuffer;

                    // Initialize the image with blank.
                    for (int y = 0; y < height; y++)
                    {
                        for (int x = 0; x < width; x++)
                        {
                            var pixelPos = y * opacityMask.BackBufferStride + (x * bpp);
                            pixels[pixelPos + 0] = b;
                            pixels[pixelPos + 1] = g;
                            pixels[pixelPos + 2] = r;
                            pixels[pixelPos + 3] = a;
                        }
                    }
                }

                // Specify the area of the bitmap that changed.
                opacityMask.AddDirtyRect(new Int32Rect(0, 0, width, height));
            }
            finally
            {
                // Release the back buffer and make it available for display.
                opacityMask.Unlock();
            }

            opacityImage.Source = opacityMask;
            return opacityMask;
        }

        private string LoadImage(Image image)
        {
            var ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Multiselect = false;
            bool? ret = ofd.ShowDialog();
            if (ret.HasValue && ret.Value)
            {
                SetImage(image, ofd.FileName);
            }

            return ofd.FileName;
        }

        private void SetImage(Image image, string filename)
        {
            var bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(filename);
            bitmap.EndInit();
            image.Source = bitmap;
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.N && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _dispatcher.Invoke(() =>
                {
                    _filenameDm = LoadImage(DMImage);
                    _filenamePlayer = LoadImage(_playerWindow.PlayerImage);
                    _opacityMask = ClearOpacityMask(OpacityImage, DMImage, 0, 0, 0, 255);
                    _playerWindow.OpacityImage.Source = _opacityMask;
                });
            }
            else if (e.Key == Key.Up)
            {
                EraserSize += 5;

                _eraser.Width = EraserSize;
                _eraser.Height = EraserSize;
            }
            else if (e.Key == Key.Down)
            {
                EraserSize -= 5;
                if (EraserSize < 2)
                {
                    EraserSize = 1;
                }

                _eraser.Width = EraserSize;
                _eraser.Height = EraserSize;
            }
            if (e.Key == Key.S && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _dispatcher.Invoke(() =>
                {
                    Save();
                });
            }
            if (e.Key == Key.L && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                _dispatcher.Invoke(() =>
                {
                    Load();
                });
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DrawEraser(sender, e);
        }

        private void Save()
        {
            Dictionary<string, string> config = new Dictionary<string, string>()
            {
                { "filenameDm", _filenameDm },
                { "filenamePlayer", _filenamePlayer },
                { "eraserSize", EraserSize.ToString() },
            };

            File.WriteAllText($"{_filenamePlayer}.save.json", JsonConvert.SerializeObject(config));

            using (FileStream stream = new FileStream($"{_filenamePlayer}.mask.png", FileMode.Create))
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(_opacityMask));
                encoder.Save(stream);

                MessageBox.Show("Saved");
            }
        }

        private void Load()
        {
            var ofd = new OpenFileDialog();
            ofd.RestoreDirectory = true;
            ofd.InitialDirectory = Environment.CurrentDirectory;
            ofd.Multiselect = false;
            bool? ret = ofd.ShowDialog();
            if (ret.HasValue && ret.Value)
            {
                var filename = ofd.FileName;
                Dictionary<string, string> config = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(filename));
                _filenameDm = config["filenameDm"];
                _filenamePlayer = config["filenamePlayer"];
                SetImage(DMImage, _filenameDm);
                SetImage(_playerWindow.PlayerImage, _filenamePlayer);
                EraserSize = int.Parse(config["eraserSize"]);
                _eraser.Width = EraserSize;
                _eraser.Height = EraserSize;

                var bitmap = new BitmapImage(new Uri($"{_filenamePlayer}.mask.png", UriKind.RelativeOrAbsolute));
                _opacityMask = new WriteableBitmap(bitmap);
                OpacityImage.Source = _opacityMask;
                _playerWindow.OpacityImage.Source = _opacityMask;
            }
        }

        private void Window_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            DrawEraser(sender, e, true);
        }
    }
}
