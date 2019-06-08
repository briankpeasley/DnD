using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MapReveal
{
    public class MapViewModel : ViewModel
    {
        private InkCanvas _dmView;
        private InkCanvas _playerView;
        private Window _dmWindow;
        private Window _playerWindow;

        private Dispatcher _dispatcher;

        public MapViewModel()
        {
            EraserSize = 20;
        }
        
        public List<Rect> ErasePoints { get; set; }

        [JsonIgnore]
        public int EraserSize { get; set; }

        public string MapFile
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string MapName
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public void SetInkCanvas(Window dmWindow, Window playerWindow, InkCanvas dmView, InkCanvas playerView)
        {
            _dmWindow = dmWindow;
            _playerWindow = playerWindow;

            _dispatcher = Dispatcher.CurrentDispatcher;
            ErasePoints = new List<Rect>();
            _dmView = dmView;
            _playerView = playerView;

            _dmView.EditingMode = InkCanvasEditingMode.None;
            _playerView.EditingMode = InkCanvasEditingMode.None;

            _dmView.MouseMove += _dmView_MouseMove;
            FillMap();
        }

        public void FillMap()
        {
            this.ErasePoints.Clear();
            Fill.FillInkCanvas(_dmView, Color.FromArgb(128, 0, 0, 0));
            Fill.FillInkCanvas(_playerView, Colors.Black);
        }

        public void LoadImage()
        {
            if (File.Exists(MapFile))
            {
                BitmapImage bmp = new BitmapImage(new Uri(MapFile, UriKind.Relative));
                ScaleTransform transform = new ScaleTransform();


                _dmWindow.Width = bmp.Width;
                _dmWindow.Height = bmp.Height;
                _playerWindow.Width = bmp.Width;
                _playerWindow.Height = bmp.Height;

                ImageBrush brush = new ImageBrush(new BitmapImage(new Uri(MapFile, UriKind.Relative)));
                brush.AlignmentX = AlignmentX.Center;
                brush.AlignmentY = AlignmentY.Center;
            
                _dmView.Background = brush;
                _playerView.Background = brush;
            }
            else
            {
                MessageBox.Show($"Cannot find map {MapFile}");
            }
        }

        public void Save(string fileName)
        {
            try
            {
                File.WriteAllText(fileName, JsonConvert.SerializeObject(this));

                using (FileStream fs = new FileStream($"{fileName}_dmView.isf", FileMode.Create))
                {
                    _dmView.Strokes.Save(fs);
                }

                using (FileStream fs = new FileStream($"{fileName}_playerView.isf", FileMode.Create))
                {
                    _playerView.Strokes.Save(fs);
                }

                MessageBox.Show("Map saved");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error saving map: {e.Message}");
            }
        }

        public void Load(string fileName)
        {
            try
            {
                MapViewModel loaded = JsonConvert.DeserializeObject<MapViewModel>(File.ReadAllText(fileName));
                this.MapFile = loaded.MapFile;
                this.MapName = loaded.MapName;
                this.LoadImage();
                this.FillMap();
                
                Task.Run(() =>
                {
                    Console.WriteLine("Loading...");

                    for (int i = 0; i < loaded.ErasePoints.Count; i++)
                    {
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write($" { (100 * (double)i / (double)loaded.ErasePoints.Count).ToString("0")}%");
                        this.ErasePoints.Add(loaded.ErasePoints[i]);
                    }

                    _dispatcher.Invoke(() =>
                    {
                        using (FileStream fs = new FileStream($"{fileName}_dmView.isf", FileMode.Open, FileAccess.Read))
                        {
                            StrokeCollection strokes = new StrokeCollection(fs);
                            _dmView.Strokes = strokes;
                        }

                        using (FileStream fs = new FileStream($"{fileName}_playerView.isf", FileMode.Open, FileAccess.Read))
                        {
                            StrokeCollection strokes = new StrokeCollection(fs);
                            _playerView.Strokes = strokes;
                        }
                    });
                });
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error loading map: {e.Message}");
            }
        }

        private void _dmView_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point mousePosition = e.GetPosition(sender as IInputElement);

                Rect erase = new Rect()
                {
                    X = mousePosition.X - EraserSize / 2,
                    Y = mousePosition.Y - EraserSize / 2,
                    Width = EraserSize,
                    Height = EraserSize
                };

                ErasePoints.Add(erase);
                Erase(erase);
            }
        }

        private void Erase(Rect erase)
        {
            _dmView.Strokes.Erase(erase);
            _playerView.Strokes.Erase(erase);
        }
    }
}
