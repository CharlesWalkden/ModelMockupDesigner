using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Models;
using ModelMockupDesigner.ViewModels;
using ModelMockupDesigner.Windows;
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

        private void NewProject_Click(object sender, RoutedEventArgs e) 
        {
            DialogLauncher<ProjectCreator> projectCreator = new(this);
            projectCreator.OnClose += ProjectCreator_OnClose;
            projectCreator.Show();
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
                            Editor editor = new Editor();
                            await editor.LoadEditor(wizardCreator.Control.ViewModel);
                            WindowControl.DisplayWindow(editor);

                            break;
                        }
                    default:
                        break;
                }
            }
        }
        private async void ProjectCreator_OnClose(object? sender, DialogEventArgs e)
        {
            if (sender is DialogLauncher<ProjectCreator> projectCreator && projectCreator.Control != null && projectCreator.Control.DialogResult == DialogResult.Accept &&
                projectCreator.Control.ViewModel != null)
            {

                Project project = new(projectCreator.Control.ViewModel);

                WizardSelector wizardSelector = new WizardSelector(project);

                WindowControl.DisplayWindow(wizardSelector);
                
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e) 
        {
            //WizardSelector wizardSelector = new();
            //WindowControl.DisplayWindow(wizardSelector);
        }
    }
}
