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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModelMockupDesigner.Controls
{
    /// <summary>
    /// Interaction logic for SlidingButton.xaml
    /// </summary>
    public partial class SlidingButton : UserControl
    {
        public Brush BackgroundColor
        {
            get => backGroundColor;
            set
            {
                backGroundColor = value;
                ellipseBorder.Background = value;
            }
        }
        private Brush backGroundColor;

        public static readonly DependencyProperty Option1TextProperty =
            DependencyProperty.Register("Option1Text", typeof(string), typeof(SlidingButton));

        public static readonly DependencyProperty Option2TextProperty =
            DependencyProperty.Register("Option2Text", typeof(string), typeof(SlidingButton));

        public static readonly DependencyProperty CheckStatusProperty =
            DependencyProperty.Register(nameof(CheckState), typeof(bool), typeof(SlidingButton), new FrameworkPropertyMetadata(default(bool), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, null)); 

        public string Option1Text
        {
            get { return (string)GetValue(Option1TextProperty); }
            set { SetValue(Option1TextProperty, value); }
        }

        public string Option2Text
        {
            get { return (string)GetValue(Option2TextProperty); }
            set { SetValue(Option2TextProperty, value); }
        }

        public bool CheckState
        {
            get { return (bool)GetValue(CheckStatusProperty); }
            set { SetValue(CheckStatusProperty, value); }
        }

        public SlidingButton()
        {
            InitializeComponent();
            option1Text.Opacity = 1;
            option2Text.Opacity = 0.5;
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            ThicknessAnimation animation = new ThicknessAnimation
            {
                From = toggleEllipse.Margin,
                To = new Thickness(20, 0, 0, 0),
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };
            toggleEllipse.BeginAnimation(MarginProperty, animation);

            option1Text.Opacity = 0.5;
            option2Text.Opacity = 1;

            CheckState = true;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            ThicknessAnimation animation = new ThicknessAnimation
            {
                From = toggleEllipse.Margin,
                To = new Thickness(0, 0, 0, 0),
                Duration = new Duration(TimeSpan.FromSeconds(0.3))
            };
            toggleEllipse.BeginAnimation(MarginProperty, animation);

            option1Text.Opacity = 1;
            option2Text.Opacity = 0.5;

            CheckState = false;
        }

        private void ToggleButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Only want to run a click event if the user clicks on the ellipse 
            if (!IsMouseOverEllipse(e))
            {
                e.Handled = true;
            }
        }

        private bool IsMouseOverEllipse(MouseButtonEventArgs e)
        {
            var position = e.GetPosition(toggleEllipse);
            return position.X >= 0 && position.X <= toggleEllipse.ActualWidth &&
                   position.Y >= 0 && position.Y <= toggleEllipse.ActualHeight;
        }
    }
}
