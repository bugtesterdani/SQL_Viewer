using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

namespace DesignElements
{
    /// <summary>
    /// Interaktionslogik für Title.xaml
    /// </summary>
    public partial class Title : UserControl
    {
        #region Label

        /// <summary>
        /// Gets or sets the Label which is displayed next to the field
        /// </summary>
        public String TitleWindow
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("TitleWindow", typeof(string),
              typeof(Title), new PropertyMetadata(""));

        #endregion

        #region Command

        public ICommand MinimizeWindow
        {
            get => (ICommand)GetValue(MinimizeProperty);
            set => SetValue(MinimizeProperty, value);
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty MinimizeProperty =
            DependencyProperty.Register("MinimizeWindow", typeof(ICommand),
              typeof(Title), new PropertyMetadata(null));

        public ICommand MaximizeRestoreWindow
        {
            get => (ICommand)GetValue(MaximizeRestoreProperty);
            set => SetValue(MaximizeRestoreProperty, value);
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty MaximizeRestoreProperty =
            DependencyProperty.Register("MaximizeRestoreWindow", typeof(ICommand),
              typeof(Title), new PropertyMetadata(null));

        public ICommand CloseWindow
        {
            get => (ICommand)GetValue(CloseProperty);
            set => SetValue(CloseProperty, value);
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty CloseProperty =
            DependencyProperty.Register("CloseWindow", typeof(ICommand),
              typeof(Title), new PropertyMetadata(null));

        #endregion

        #region Drag and Drop Window

        // Importiere die Win32-Funktion zum Verschieben des Fensters
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                // Erfasse die Maus und verschiebe das Fenster
                ReleaseCapture();
                SendMessage(new WindowInteropHelper(Window.GetWindow(this)).Handle, 0xA1, 0x2, 0);
            }
        }

        #endregion

        public Title()
        {
            InitializeComponent();
            RootWindow.DataContext = this;
        }
    }
}
