using ModelMockupDesigner.Controls;
using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using ModelMockupDesigner.Models;
using ModelMockupDesigner.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using ComboBoxItem = ModelMockupDesigner.ViewModels.ComboBoxItem;

namespace ModelMockupDesigner
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : UserControl, IWindowStack
    {
#pragma warning disable CS8603 // Will never be null as we create a new viewmodel in constructor.
        public WizardEditorViewModel ViewModel { get => DataContext as WizardEditorViewModel; }
#pragma warning restore CS8603 
        public Wizard? WizardModel { get; set; }

        public IIsSelectable? CurrentSelection { get; set; }

        public List<EditorSection> Pages { get; set; }
        public EditorSection? CurrentPage { get; set; }

        public Editor()
        {
            InitializeComponent();
            DataContext = new WizardEditorViewModel();
            Pages = new();
        }

        public async Task LoadEditor(WizardCreatorViewModel creatorModel)
        {
            WizardModel = new()
            {
                Name = creatorModel.WizardName,
                Description = creatorModel.WizardDescription,
                WizardType = creatorModel.WizardType,
                WizardTheme = creatorModel.WizardTheme
            };
            if (ViewModel != null)
                ViewModel.WizardName = creatorModel.WizardName;

            // Pass in Empty guid as this is a new wizard and we don't currently have one.
            await WizardModel.BuildWizard(Guid.Empty);

            await LoadUI();
        }
        public async Task LoadEditor(Guid identifier)
        {
            await CreateWizardModel(identifier);

            await LoadUI();
            
        }
        private async Task CreateWizardModel(Guid wizardId)
        {
            WizardModel = new Wizard();

            await WizardModel.BuildWizard(wizardId);
        }
        private async Task LoadUI()
        {
            ContentContainer.Children.Clear();

            if (WizardModel == null)
                return;

            foreach (WizardSection wizardSection in WizardModel.Sections)
            {
                EditorSection editorSection = new();
                editorSection.OnSelected += UpdateCurrentSelection;

                await editorSection.LoadModel(wizardSection);

                AddPage(editorSection);
            }

            LoadPage(0);
        }

        public void LoadPage(int pageIndex)
        {
            ContentContainer.Children.Clear();

            EditorSection page = Pages[pageIndex];

            ContentContainer.Children.Add(page);

            CurrentPage = page;

            PageSelector.SelectionChanged -= PageSelector_SelectionChanged;

            if (CurrentPage.Model is WizardSection wizardSection)
            {
                ViewModel.SetCurrentPage(wizardSection.OrderId);
            }

            PageSelector.SelectionChanged += PageSelector_SelectionChanged;

            DisableEnableNavButtons(pageIndex);

            UpdateCurrentSelection(this, null);
        }
        private void DisableEnableNavButtons(int pageIndex)
        {
            // Check to see if we are on the first page.
            if (pageIndex == 0)
            {
                // If the index is 0, we know that we are on the first page so disable the navPrevious button.
                NavPreviousButton.IsEnabled = false;
                NavPreviousButton.Opacity = 0.25;
            }
            else
            {
                // If we are not on the first page, enable the navPrevious button.
                NavPreviousButton.IsEnabled = true;
                NavPreviousButton.Opacity = 1;
            }
            // Check to see if we are on the last page.
            if (pageIndex == Pages.Count - 1)
            {
                // If we are on the last page, disable the navNext button.
                NavNextButton.IsEnabled = false;
                NavNextButton.Opacity = 0.25;
            }
            else
            {
                // If we are not on the last page, enable the navNext button.
                NavNextButton.IsEnabled = true;
                NavNextButton.Opacity = 1;
            }
            // Check to see we only have one page left.
            if (Pages.Count == 1)
            {
                // If we only have one page, disable pageDelete button as we do not want the user to be able to delete the last page.
                DeletePageButton.IsEnabled = false;
                DeletePageButton.Opacity = 0.25;
            }
            else
            {
                // If we have more than one page, enable the pageDelete button.
                DeletePageButton.IsEnabled = true;
                DeletePageButton.Opacity = 1;
            }

        }
        private void UpdateCurrentSelection(object? sender, IIsSelectable? newSelection)
        {
            if (CurrentSelection != null)
            {
                if (CurrentSelection.Equals(newSelection))
                {
                    return;
                }
                else
                {
                    CurrentSelection.Unselect();
                }
            }

            CurrentSelection = newSelection;
            
        }
        private async void CreateNewPage(WizardSection? sectionModel = null)
        {
            if (WizardModel == null)
                return;

            EditorSection page = new();
            page.OnSelected += UpdateCurrentSelection;
            if (sectionModel != null)
            {
                await page.LoadModel(sectionModel);
            }
            else
            {
                await page.LoadModel(new WizardSection(WizardModel));
            }
            page.Model.OrderId = GetNextPageOrderNumber();

            AddPage(page);

            LoadPage(page.Model.OrderId);
        }
        private void AddPage(EditorSection page)
        {
            Pages.Add(page);

            ComboBoxItem comboBoxItem = new()
            {
                Text = page.Model.Name,
                Value = page.Model.OrderId
            };

            ViewModel.PageList.Add(comboBoxItem);
        }
        private void LoadNextPage()
        {
            if (CurrentPage != null)
            {
                int pageIndex = Pages.IndexOf(CurrentPage);

                if (pageIndex == Pages.Count - 1)
                {
                    return;
                }
                else
                {
                    pageIndex++;
                }

                LoadPage(pageIndex);
            }

        }
        private void LoadPreviousPage()
        {
            if (CurrentPage != null)
            {
                int pageIndex = Pages.IndexOf(CurrentPage);

                if (pageIndex == 0)
                {
                    return;
                }
                else
                {
                    pageIndex--;
                }

                LoadPage(pageIndex);
            }
        }
        private void DeleteCurrentPage()
        {
            if (CurrentPage != null)
            {
                Pages.Remove(CurrentPage);
                ViewModel.DeleteCurrentPage();
            }
        }
        private int GetNextPageOrderNumber()
        {
            if (Pages.Count == 0)
                return 0;

            return Pages.Max(x => x.Model.OrderId) + 1;
        }
       
        private void Save_Click(object sender, RoutedEventArgs e) 
        {
            // Save wizard.
            WindowControl.CloseTopWindow();
        }

        private void DeletePageButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage != null)
            {
                int index = Pages.IndexOf(CurrentPage);

                DeleteCurrentPage();

                if (index == 0)
                {
                    LoadPage(0);
                }
                else
                {
                    LoadPage(index - 1);
                }
            }
        }

        private void NewPageButton_Click(object sender, RoutedEventArgs e)
        {
            CreateNewPage();
        }

        private void NavPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPreviousPage();
        }

        private void NavNextButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNextPage();
        }

        private void PageSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                int order = (int)((ComboBoxItem)e.AddedItems[0]).Value;

                int index = Pages.IndexOf(Pages.FirstOrDefault(x => x.Model.OrderId == order));

                LoadPage(index);
            }
        }

        #region Interface

        public event EventHandler? OnClosed;
        public void CloseAsync()
        {
            OnClosed?.Invoke(this, new EventArgs());
        }

        #endregion

        #region Temp new field item drag

        private Border? Source { get; set; }
        private NewControl? SelectedControl { get; set; }
        private void Border_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (Source != null && SelectedControl != null)
                    DragDrop.DoDragDrop(Source, SelectedControl, DragDropEffects.All);
            }
        }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Source = (Border)sender;
            SelectedControl = new NewControl() { ElementType = ElementType.Table };
        }

        #endregion
    }
}
