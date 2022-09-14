using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Models;
using ModelMockupDesigner.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ModelMockupDesigner.ViewModels
{
    public class WizardSelectorViewModel : BaseViewModel
    {
        public Project? Model
        {
            get => model;
            set
            {
                if (value != null)
                    ProjectName = value.ProjectName;
                else
                    projectName = "Not Set";

                model = value;
            }
        }
        private Project? model { get; set; }


        public string? ProjectName
        {
            get => projectName;
            set
            {
                if (projectName == value)
                    return;

                projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }
        private string? projectName { get; set; }


        public ICommand CreateNewWizardCommand { get; set; }
        public ICommand CloseWizardSelectorCommand { get; set; }

        public WizardSelectorViewModel()
        {
            CreateNewWizardCommand = new RelayCommand(CreateNewWizard);
            CloseWizardSelectorCommand = new RelayCommand(CloseWizardSelector);
        }

        private void CloseWizardSelector() 
        {
            WindowControl.CloseTopWindow();
        }

        private void CreateNewWizard()
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

    }
}
