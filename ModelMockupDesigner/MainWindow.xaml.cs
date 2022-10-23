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

namespace ModelMockupDesigner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       private MainContainer mainContainer = new MainContainer();

        public void SetMinHeight(double minHeight)
        {
            MinHeight = minHeight;

            if (minHeight < Height)
            {
                Height = minHeight;
            }
        }
        public void SetMinWidth(double minWidth)
        {
            MinWidth = minWidth;
            
            if (minWidth < Width)
            {
                Width = minWidth;
            }
        }
        public void SetResizable(bool resizable)
        {
            if (resizable)
                ResizeMode = ResizeMode.CanResize;
            else
                ResizeMode = ResizeMode.NoResize;
        }

        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void UpdateWindowParameters(object sender, WindowParameters parameters)
        {
            SetMinHeight(parameters.MinHeight);
            SetMinWidth(parameters.MinWidth);
            SetResizable(parameters.CanResize);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mainContainer.Owner = this;
            Root.Children.Add(mainContainer);
            WindowControl.MainWindow = mainContainer.mainWindowStack;
            WindowControl.OnWindowDisplay += UpdateWindowParameters;

            LandingPage landingPage = new LandingPage();
            WindowControl.DisplayWindow(landingPage);
        }
    }
}
