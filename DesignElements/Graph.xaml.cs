using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DesignElements
{
    /// <summary>
    /// Interaktionslogik für Graph.xaml
    /// </summary>
    public partial class Graph : UserControl
    {
        private const double ZoomFactor = 0.2; // Ändern Sie diesen Wert, um die Zoom-Geschwindigkeit anzupassen
        private const double MinZoomFactor = 0.1; // Mindestzoomfaktor
        private const double MaxZoomFactor = 10; // Maximaler Zoomfaktor
        private Point panStartPoint;
        private bool isPanning;
        private Point panStartPosition;

        public IList<Models.Segment> Segments
        {
            get => (IList<Models.Segment>)GetValue(GraphProperty);
            set => SetValue(GraphProperty, value);
        }

        public double Width
        {
            get => (double)GetValue(WidthProperty);
            set => SetValue(WidthProperty, value);
        }

        public double Height
        {
            get => (double)GetValue(HeightProperty);
            set => SetValue(HeightProperty, value);
        }

        public static readonly DependencyProperty GraphProperty =
            DependencyProperty.Register("Segments", typeof(IList<Models.Segment>),
              typeof(Graph), new PropertyMetadata(null));

        public static readonly DependencyProperty WidthProperty =
            DependencyProperty.Register("Width", typeof(double),
              typeof(Graph), new PropertyMetadata(null));

        public static readonly DependencyProperty HeightProperty =
            DependencyProperty.Register("Height", typeof(double),
              typeof(Graph), new PropertyMetadata(null));

        public Graph()
        {
            InitializeComponent();
            RootWindow.DataContext = this;

            RootWindow.LayoutTransform = new ScaleTransform(1, 1);

            Loaded += Graph_Loaded;
        }

        private void Graph_Loaded(object sender, RoutedEventArgs e)
        {
            CreateGridLines();
        }

        private void CreateGridLines()
        {
            double gridSpacing = 20; // Abstand zwischen den Gitterlinien

            // Vertikale Linien
            for (double x = 0; x <= Width; x += gridSpacing)
            {
                Line verticalLine = new Line
                {
                    X1 = x,
                    Y1 = 0,
                    X2 = x,
                    Y2 = Height,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 0.5
                };

                RootWindow.Children.Add(verticalLine);
            }

            // Horizontale Linien
            for (double y = 0; y <= Height; y += gridSpacing)
            {
                Line horizontalLine = new Line
                {
                    X1 = 0,
                    Y1 = y,
                    X2 = Width,
                    Y2 = y,
                    Stroke = Brushes.LightGray,
                    StrokeThickness = 0.5
                };

                RootWindow.Children.Add(horizontalLine);
            }
        }

        //private void UserControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        //{
        //    if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        //    {
        //        double zoomDelta = e.Delta > 0 ? ZoomFactor : -ZoomFactor;

        //        // Zoom-Transformation aktualisieren
        //        ZoomTransform.ScaleX += zoomDelta;
        //        ZoomTransform.ScaleY += zoomDelta;

        //        // Verhindern, dass der Zoom außerhalb bestimmter Grenzen geht
        //        if (ZoomTransform.ScaleX < 0.1)
        //            ZoomTransform.ScaleX = 0.1;
        //        if (ZoomTransform.ScaleY < 0.1)
        //            ZoomTransform.ScaleY = 0.1;

        //        // Invertieren der Y-Achse
        //        RootWindow.LayoutTransform = new ScaleTransform(1, -1);

        //        e.Handled = true; // Ereignis behandelt, um das Scrollen in der Elternsteuerelemente zu verhindern
        //    }
        //}

        private void UserControl_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                double zoomDelta = e.Delta > 0 ? ZoomFactor : -ZoomFactor;

                // Zoom-Transformation aktualisieren
                ZoomTransform.ScaleX += zoomDelta;
                ZoomTransform.ScaleY += zoomDelta;

                // Verhindern, dass der Zoom außerhalb bestimmter Grenzen geht
                if (ZoomTransform.ScaleX < MinZoomFactor)
                    ZoomTransform.ScaleX = MinZoomFactor;
                if (ZoomTransform.ScaleY < MinZoomFactor)
                    ZoomTransform.ScaleY = MinZoomFactor;
                if (ZoomTransform.ScaleX > MaxZoomFactor)
                    ZoomTransform.ScaleX = MaxZoomFactor;
                if (ZoomTransform.ScaleY > MaxZoomFactor)
                    ZoomTransform.ScaleY = MaxZoomFactor;

                // Berechnung der maximalen Verschiebung basierend auf dem verfügbaren Bereich
                double maxPanDeltaX = (RootWindow.ActualWidth - (Width * ZoomTransform.ScaleX)) / ZoomTransform.ScaleX;
                double maxPanDeltaY = (RootWindow.ActualHeight - (Height * ZoomTransform.ScaleY)) / ZoomTransform.ScaleY;

                // Begrenzung der aktuellen Verschiebung basierend auf dem verfügbaren Bereich
                PanTransform.X = Math.Max(-maxPanDeltaX, Math.Min(maxPanDeltaX, PanTransform.X));
                PanTransform.Y = Math.Max(-maxPanDeltaY, Math.Min(maxPanDeltaY, PanTransform.Y));

                RootWindow.LayoutTransform = new ScaleTransform(1, -1);

                e.Handled = true; // Ereignis behandelt, um das Scrollen in der Elternsteuerelemente zu verhindern
            }
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && !isPanning)
            {
                isPanning = true;
                panStartPoint = e.GetPosition(this);
                panStartPosition = new Point(PanTransform.X, PanTransform.Y);
                this.CaptureMouse();
            }
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPanning)
            {
                Point panEndPoint = e.GetPosition(this);
                Vector panDelta = panEndPoint - panStartPoint;

                double maxPanDeltaX = RootWindow.ActualWidth - (Width * ZoomTransform.ScaleX);
                double maxPanDeltaY = RootWindow.ActualHeight - (Height * ZoomTransform.ScaleY);

                // Begrenzung der Verschiebung auf einen bestimmten Bereich
                if (maxPanDeltaX < 0)
                    maxPanDeltaX = 0;
                if (maxPanDeltaY < 0)
                    maxPanDeltaY = 0;

                // Berechnung der Verschiebung
                double panDeltaX = panDelta.X / ZoomTransform.ScaleX;
                double panDeltaY = panDelta.Y / ZoomTransform.ScaleY;

                // Begrenzung der Verschiebung basierend auf dem verfügbaren Bereich
                if (panDeltaX > maxPanDeltaX)
                    panDeltaX = maxPanDeltaX;
                if (panDeltaX < -maxPanDeltaX)
                    panDeltaX = -maxPanDeltaX;
                if (panDeltaY > maxPanDeltaY)
                    panDeltaY = maxPanDeltaY;
                if (panDeltaY < -maxPanDeltaY)
                    panDeltaY = -maxPanDeltaY;

                // Anwenden der Verschiebung
                PanTransform.X = panStartPosition.X + panDeltaX;
                PanTransform.Y = panStartPosition.Y - (panDeltaY/* * ((ScaleTransform)RootWindow.LayoutTransform).ScaleY*/);
            }
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && isPanning)
            {
                isPanning = false;
                this.ReleaseMouseCapture();
            }
        }
    }
}
