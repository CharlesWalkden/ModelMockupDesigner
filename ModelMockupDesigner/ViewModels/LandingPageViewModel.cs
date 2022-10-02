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
    public class LandingPageViewModel
    {
        public RecentProjectListItemViewModel? CurrentSelected { get; set; } = null;
        public CollectionList<Project> AllProjects { get; set; }
        public CollectionList<RecentProjectListItemViewModel> RecentProjects { get; set; }
        public LandingPageViewModel(UserControl owner)
        {
            Parent = owner;
            AllProjects = new();
            AllProjects.OnAddEntry += AllProjects_OnAddEntry; 
            RecentProjects = new();

            NewProjectCommand = new RelayCommand(NewProject);
            LoadProjectCommand = new RelayCommand(LoadProject);
            ExportProjectCommand = new RelayCommand(ExportProject);
            OpenSettingsCommand = new RelayCommand(OpenSettings);
            ExitApplicationCommand = new RelayCommand(ExitApplication);
        }

        public ICommand NewProjectCommand { get; set; } 
        public ICommand LoadProjectCommand { get; set; } 
        public ICommand ExportProjectCommand { get; set; } 
        public ICommand OpenSettingsCommand { get; set; } 
        public ICommand ExitApplicationCommand { get; set; }

        public List<RecentProjectListItemViewModel> ConvertToViewModel(IEnumerable<Project> projects)
        {
            List<RecentProjectListItemViewModel> recents = new();
            foreach (Project project in projects)
            {
                RecentProjectListItemViewModel vm = new()
                {
                    ProjectModel = project
                };
                vm.OnSelected += RecentProject_Selected;

                recents.Add(vm);
            }

            return recents;
        }
        private void NewProject() 
        {
            DialogLauncher<ProjectCreator> projectCreator = new(Parent);
            projectCreator.OnClose += ProjectCreator_OnClose;
            projectCreator.ShowDialog();
        }
        private void LoadProject() 
        {
            if (CurrentSelected != null && CurrentSelected.ProjectModel != null)
            {
                WizardSelector wizardSelector = new WizardSelector(CurrentSelected.ProjectModel);

                WindowControl.DisplayWindow(wizardSelector);
            }
        }
        private void ExportProject() 
        {

        }
        private void OpenSettings()
        {

        }
        private void ExitApplication()
        {
            Application.Current.Shutdown();
        }

        #region Events
        private void AllProjects_OnAddEntry(object? sender, EventArgs e)
        {
            List<Project> newList = AllProjects.OrderByDescending(x => x.LastAccess).ToList();

            if (AllProjects.Count < 6)
            {
                RecentProjects.AddNewRange(ConvertToViewModel(newList));
            }
            else
            {
                RecentProjects.AddNewRange(ConvertToViewModel(newList.Take(5)));
            }
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
        private void ProjectCreator_OnClose(object? sender, DialogEventArgs e)
        {
            if (sender is DialogLauncher<ProjectCreator> projectCreator && projectCreator.Control != null && projectCreator.Control.DialogResult == DialogResult.Accept &&
                projectCreator.Control.ViewModel != null)
            {
                Project project = new(projectCreator.Control.ViewModel);
                AllProjects.AddEntry(project);

                WizardSelector wizardSelector = new WizardSelector(project);

                WindowControl.DisplayWindow(wizardSelector);
            }
        }
        private void RecentProject_Selected(object? sender, EventArgs e)
        {
            if (sender is RecentProjectListItemViewModel viewModel)
            {
                if (viewModel.Selected)
                {
                    viewModel.Selected = false;
                    if (CurrentSelected == viewModel)
                        CurrentSelected = null;
                }
                else
                {
                    viewModel.Selected = true;
                    if (CurrentSelected != null)
                    {
                        CurrentSelected.Selected = false;
                    }
                    CurrentSelected = viewModel;

                }
            }
        }

        #endregion

        #region Owner

        private UserControl Parent { get; set; }

        #endregion
    }
}
