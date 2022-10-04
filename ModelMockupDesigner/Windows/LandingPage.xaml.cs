using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using ModelMockupDesigner.ViewModels;
using ModelMockupDesigner.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Management;
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
    /// Interaction logic for LandingPage.xaml
    /// </summary>
    public partial class LandingPage : UserControl, IWindowStack
    {
        private LandingPageViewModel? ViewModel { get => DataContext as LandingPageViewModel; }
        public LandingPage()
        {
            InitializeComponent();
            DataContext = new LandingPageViewModel(this);
            Loaded += LandingPage_Loaded;
        }

        private void LandingPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel?.RefreshRecentProjectsList();
        }

        public WindowParameters GetWindowParameters()
        {
            WindowParameters windowParameters = new WindowParameters()
            {
                CanResize = false,
                MinWidth = 1280,
                MinHeight = 595
            };

            return windowParameters;
        }

        #region Not used, needed for interface

        public event EventHandler? OnClosed;
        public void CloseAsync()
        {
            throw new NotImplementedException();
        }

        #endregion

        
    }
}
