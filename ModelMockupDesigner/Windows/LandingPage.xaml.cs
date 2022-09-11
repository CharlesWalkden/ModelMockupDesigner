using ModelMockupDesigner.Enums;
using ModelMockupDesigner.ViewModels;
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
    /// Interaction logic for LandingPage.xaml
    /// </summary>
    public partial class LandingPage : UserControl
    {
        public LandingPage()
        {
            InitializeComponent();
        }

        private void ExitApplication_Click(object sender, RoutedEventArgs e) 
        {
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogLauncher<WizardCreator> wizardCreator = new(this);
            wizardCreator.OnClose += WizardCreator_OnClose;
            wizardCreator.Show();
        }

        private async void WizardCreator_OnClose(object? sender, DialogEventArgs e)
        {
            if (sender is DialogLauncher<WizardCreator> wizardCreator && wizardCreator.Control != null && wizardCreator.Control.DialogResult == DialogResult.Accept && 
                wizardCreator.Control.ViewModel != null)
            {
                switch (wizardCreator.Control.ViewModel.WizardType)
                {
                    case WizardType.Dynamic:
                        {
                            DialogLauncher<Editor> editor = new(this);
                            if (editor.Control != null)
                            {
                                await editor.Control.LoadEditor(wizardCreator.Control.ViewModel);
                            }
                            editor.Show();
                            break;
                        }
                    default:
                        break;
                }
            }
        }
    }
}
