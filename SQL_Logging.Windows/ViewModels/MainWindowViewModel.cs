using DesignElements.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Point = System.Drawing.Point;
using System.Windows.Input;
using System.Collections.Generic;
using DesignElements.Models;

namespace SQL_Logging.Windows.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private int _shoesize;
        private double _height;
        private double _height2;
        private double _graphWidth;
        private double _graphHeight;
        private bool _loadingpic;
        private string _title;
        private ICommand _btnclick;
        private ICommand _minimizeWindow;
        private ICommand _maximizeWindow;
        private ICommand _closeWindow;
        private IList<Segment> _segmentview;

        public int Shoesize
        {
            get => _shoesize;
            set { if (_shoesize != value) { _shoesize = value; OnPropertyChanged(); } }
        }

        public double Height
        {
            get => _height;
            set { if (_height != value) { _height = value; OnPropertyChanged(); } }
        }

        public double Height2
        {
            get => _height2;
            set { if (_height2 != value) { _height2 = value; OnPropertyChanged(); } }
        }

        public double GraphWidth
        {
            get => _graphWidth;
            set { if (_graphWidth != value) { _graphWidth = value; OnPropertyChanged(); } }
        }

        public double GraphHeight
        {
            get => _graphHeight;
            set { if (_graphHeight != value) { _graphHeight = value; OnPropertyChanged(); } }
        }

        public bool LoadingProgBarVisible
        {
            get => _loadingpic;
            set { if (_loadingpic != value) { _loadingpic = value; OnPropertyChanged(); } }
        }

        public string Title
        {
            get => _title;
            set { if (_title != value) { _title = value; OnPropertyChanged(); } }
        }

        public ICommand BTN_Click => _btnclick ?? (_btnclick = new RelayCommand(p => Command(p), p => true));
        public ICommand MinimizeWindow => _minimizeWindow ?? (_minimizeWindow = new RelayCommand(p => Minimize(p), p => true));
        public ICommand MaximizeWindow => _maximizeWindow ?? (_maximizeWindow = new RelayCommand(p => MaximizeRestore(p), p => true));
        public ICommand CloseWindow => _closeWindow ?? (_closeWindow = new RelayCommand(p => ExitApplication(p), p => true));

        public IList<Segment> SegmentsView
        {
            get => _segmentview;
            set { if (value != _segmentview) { _segmentview = value; OnPropertyChanged(); } }
        }

        private void Command(object parameter)
        {
            MessageBox.Show("Hello");
            LoadingProgBarVisible ^= true;
        }

        private void ExitApplication(object parameter)
        {
            Application.Current.Shutdown();
        }

        private void Minimize(object parameter)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void MaximizeRestore(object parameter)
        {
            Application.Current.MainWindow.WindowState ^= WindowState.Maximized;
        }

        public List<string> Text2 { get => new List<string>() { "Shoesize:", "Value2", "TEST" }; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        internal MainWindowViewModel()
        {
            Height = 34;
            Height2 = 35;
            LoadingProgBarVisible = false;
            Title = "Main Window V2";
            IList<Segment> segments = new List<Segment>();
            segments.Add(new Segment() { From = new Point(10, 2), To = new Point(20, 2) });
            segments.Add(new Segment() { From = new Point(20, 2), To = new Point(30, 4) });
            segments.Add(new Segment() { From = new Point(30, 4), To = new Point(400, 300) });
            GraphHeight = 800;
            GraphWidth = 800;
            SegmentsView = segments;
        }
    }
}
