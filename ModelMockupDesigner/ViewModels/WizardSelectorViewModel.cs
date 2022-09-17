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
using ComboBoxItem = ModelMockupDesigner.Models.ComboBoxItem;

namespace ModelMockupDesigner.ViewModels
{
    public class WizardSelectorViewModel : BaseViewModel
    {
        public WizardSelector Owner { get; set; }

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


        public ICommand LoadWizardCommand { get; set; }
        public ICommand CreateNewWizardCommand { get; set; }
        public ICommand CreateNewCategoryCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand CloseWizardSelectorCommand { get; set; }

        public EventHandler? OnListUpdated;

        public WizardSelectorViewModel(WizardSelector owner)
        {
            LoadWizardCommand = new RelayCommand(LoadWizard);
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

        private void LoadWizard()
        {
            if (Owner.CurrentSelection is WizardTreeViewItem wizardTreeViewItem)
            {
                Editor editor = new Editor();
                editor.LoadEditor(wizardTreeViewItem.Wizard);

                WindowControl.DisplayWindow(editor);
            }
        }
        private void CreateNewWizard()
        {
            DialogLauncher<WizardCreator> wizardCreator = new(Owner);
            wizardCreator.OnClose += WizardCreator_OnClose;
            if (wizardCreator.Control.ViewModel != null)
            {
                wizardCreator.Control.ViewModel.LoadCategoryList(CreateCategoryList());
                if (Owner.CurrentSelection != null && Owner.CurrentSelection is CategoryTreeViewItem treeViewItem)
                {
                    wizardCreator.Control.ViewModel.SetCategory(treeViewItem.Category.Id);
                }
            }
            wizardCreator.Show();
        }
        private void CreateNewCateogry()
        {
            DialogLauncher<CategoryCreator> categoryCreator = new(Owner);
            categoryCreator.OnClose += CategoryCreator_OnClose;
            categoryCreator.Show();
        }
        private void Delete()
        {
            if (Owner.CurrentSelection != null)
            {
                if (Owner.CurrentSelection is CategoryTreeViewItem categoryTreeViewItem)
                {
                    if (model != null)
                    {
                        if (!categoryTreeViewItem.DeleteFromParent())
                        {
                            model.Categories.Remove(categoryTreeViewItem.Category);
                        }

                        Owner.RefreshTreeView();
                    }
                }
                else if (Owner.CurrentSelection is WizardTreeViewItem wizardTreeViewItem)
                {
                    if (model != null)
                    {
                        
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

                            AddWizardToCategory(editor.WizardModel);
                            Owner.RefreshTreeView();

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
                else if (Model != null)
                {
                    Model.Categories.Add(category);
                    OnListUpdated?.Invoke(this, new EventArgs());
                }
            }
        }
        private void AddWizardToCategory(Wizard? wizard)
        {
            if (Model != null && wizard != null)
            {
                foreach (Category category in Model.Categories)
                {
                    if (category.Id == wizard.CateogryId)
                    {
                        category.AddWizard(wizard);
                        break;
                    }
                    if (category.Categories != null)
                    {
                        foreach (Category subCategory in category.Categories)
                        {
                            if (subCategory.AddWizardToCategory(wizard))
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }
        private List<ComboBoxItem> CreateCategoryList()
        {
            List<ComboBoxItem> categoryList = new List<ComboBoxItem>();

            if (Model != null)
            {
                foreach (Category category in Model.Categories)
                {
                    ComboBoxItem comboBox = new()
                    {
                        Text = category.Name,
                        Value = category.Id
                    };
                    categoryList.Add(comboBox);

                    if (category.Categories != null && category.Categories.Count > 0)
                    {
                        categoryList.AddRange(category.GetCategoryList());
                    }
                }
            }

            return categoryList;
        }
    }
}
