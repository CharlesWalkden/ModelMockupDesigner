using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Models;
using ModelMockupDesigner.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using ComboBoxItem = ModelMockupDesigner.Models.ComboBoxItem;

namespace ModelMockupDesigner.ViewModels
{
    public class WizardSelectorViewModel : BaseViewModel
    {
        public WizardSelector Owner { get; set; }

        public Project? ProjectModel
        {
            get => projectModel;
            set
            {
                if (value != null)
                    ProjectName = value.ProjectName;
                else
                    projectName = "Not Set";

                if (value != null)
                    ProjectDescription = value.ProjectDescription;
                else
                    projecDescription = "Not Set";

                if (value != null)
                    CustomerName = value.CustomerName;
                else
                    customerName = "Not Set";

                projectModel = value;
            }
        }
        private Project? projectModel { get; set; }

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

        public string? ProjectDescription
        {
            get => projecDescription;
            set
            {
                if (projecDescription == value)
                    return;

                projecDescription = value;
                OnPropertyChanged(nameof(ProjectDescription));
            }
        }

        public string? CustomerName
        {
            get => customerName;
            set
            {
                if (customerName == value)
                    return;

                customerName = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }

        private string? projectName { get; set; }

        private string? projecDescription { get; set; }

        private string? customerName { get; set; }

        public ICommand EditWizardCommand { get; set; }
        public ICommand CreateNewWizardCommand { get; set; }
        public ICommand CreateNewCategoryCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CloseWizardSelectorCommand { get; set; }

        public EventHandler? OnListUpdated;

        public WizardSelectorViewModel(WizardSelector owner)
        {
            EditWizardCommand = new RelayCommand(LoadWizard);
            CreateNewWizardCommand = new RelayCommand(CreateNewWizard);
            CreateNewCategoryCommand = new RelayCommand(CreateNewCateogry);
            DeleteCommand = new RelayCommand(Delete);
            CloseWizardSelectorCommand = new RelayCommand(CloseWizardSelector);
            Owner = owner;
        }

        private void CloseWizardSelector() 
        {
            WindowControl.CloseTopWindow();
        }

        private async void LoadWizard()
        {
            if (Owner.CurrentSelection is WizardTreeViewItem wizardTreeViewItem)
            {
                switch (wizardTreeViewItem.Wizard.WizardType)
                {
                    case WizardType.Dynamic:
                        {
                            if (wizardTreeViewItem.Wizard is DynamicWizard dynamicWizard)
                            {
                                Editor editor = new Editor();
                                await editor.LoadEditor(dynamicWizard);
                                WindowControl.DisplayWindow(editor);
                            }

                            break;
                        }
                    default:
                        break;
                }
            }
        }
        private void CreateNewWizard()
        {
            DialogLauncher<WizardCreator> wizardCreator = new(Owner);
            wizardCreator.OnClose += WizardCreator_OnClose;
            if (wizardCreator.Control.ViewModel != null)
            {
                wizardCreator.Control.ViewModel.Project = projectModel;
                wizardCreator.Control.ViewModel.LoadCategoryList(ProjectModel?.CreateCategoryList());
                if (Owner.CurrentSelection != null && Owner.CurrentSelection is CategoryTreeViewItem treeViewItem)
                {
                    wizardCreator.Control.ViewModel.SetCategory(treeViewItem.Category.Id);
                }
            }
            wizardCreator.ShowDialog();
        }
        private void CreateNewCateogry()
        {
            DialogLauncher<CategoryCreator> categoryCreator = new(Owner);
            categoryCreator.OnClose += CategoryCreator_OnClose;
            categoryCreator.ShowDialog();
        }
        private void Delete()
        {
            if (Owner.CurrentSelection != null)
            {
                if (Owner.CurrentSelection is CategoryTreeViewItem categoryTreeViewItem)
                {
                    if (projectModel != null)
                    {
                        if (!categoryTreeViewItem.DeleteFromParent())
                        {
                            projectModel.Categories.Remove(categoryTreeViewItem.Category);
                        }

                        Owner.RefreshTreeView();
                    }
                }
                else if (Owner.CurrentSelection is WizardTreeViewItem wizardTreeViewItem)
                {
                    if (projectModel != null)
                    {
                        if (wizardTreeViewItem.Wizard.Category != null)
                        {
                            wizardTreeViewItem.Wizard.Category = null;
                        }
                        else
                        {
                            projectModel.LoneWizards.Remove(wizardTreeViewItem.Wizard);
                        }
                        projectModel.AllWizards.Remove(wizardTreeViewItem.Wizard);

                        Owner.RefreshTreeView();
                    }
                }
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

        private void CategoryCreator_OnClose(object? sender, DialogEventArgs e)
        {
            if (sender is DialogLauncher<CategoryCreator> categoryCreator && categoryCreator.Control != null && categoryCreator.Control.DialogResult == DialogResult.Accept &&
                categoryCreator.Control.ViewModel != null)
            {
                Category category = new(Guid.Empty)
                {
                    Name = categoryCreator.Control.ViewModel.CategoryName,
                    Description = categoryCreator.Control.ViewModel.CategoryDescription
                };

                if (Owner.CurrentSelection != null && Owner.CurrentSelection is CategoryTreeViewItem treeViewItem)
                {
                    if (treeViewItem.Category.Categories != null)
                    {
                        treeViewItem.Category.Categories.Add(category);
                        OnListUpdated?.Invoke(this, new EventArgs());
                    }
                }
                else if (ProjectModel != null)
                {
                    ProjectModel.Categories.Add(category);
                    OnListUpdated?.Invoke(this, new EventArgs());
                }
            }
        }
        
    }
}
