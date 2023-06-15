using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DesignElements
{
    /// <summary>
    /// Interaktionslogik für UIButtons.xaml
    /// </summary>
    public partial class UIButtons : UserControl
    {
        public ICommand ClickCommand
        {
            get => (ICommand)GetValue(ClickCommandProperty);
            set => SetValue(ClickCommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the Label which is displayed next to the field
        /// </summary>
        public List<string> LabelList
        {
            get { return (List<string>)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("LabelList", typeof(List<string>),
              typeof(UIButtons), new PropertyMetadata(new List<string>() { "EMPTY" }));

        /// <summary>
        /// Identified the Label dependency property
        /// </summary>
        public static readonly DependencyProperty ClickCommandProperty =
            DependencyProperty.Register("ClickCommand", typeof(ICommand),
              typeof(UIButtons), new PropertyMetadata(null));

        public UIButtons()
        {
            InitializeComponent();
            LayoutRoot.DataContext = this;
        }
    }
}
