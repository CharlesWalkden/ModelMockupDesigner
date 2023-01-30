using ModelMockupDesigner.Controls;
using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using ModelMockupDesigner.ViewModels;
using ModelMockupDesigner.WizardPreview.Wizards;
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
using ComboBoxItem = ModelMockupDesigner.Models.ComboBoxItem;

namespace ModelMockupDesigner
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : UserControl, IWindowStack
    {
        public WizardEditorViewModel ViewModel { get => DataContext as WizardEditorViewModel; }

        public DynamicWizard WizardModel { get; set; }

        public IIsSelectable CurrentSelection { get; set; }

        public List<EditorSection> Pages { get; set; }
        public EditorSection CurrentPage { get; set; }

        public WizardDesignPreview DesignPreview;

        public event EventHandler<DynamicWizard> OnWizardUpdated;

        public Editor()
        {
            InitializeComponent();
            DataContext = new WizardEditorViewModel();
            Pages = new List<EditorSection>();
        }
        public async Task LoadEditor(DynamicWizard wizard)
        {
            if (ViewModel != null)
                ViewModel.WizardName = wizard.Name;

            WizardModel = wizard;

            await LoadUI();

        }
        public async Task LoadEditor(WizardCreatorViewModel creatorModel)
        {
            WizardModel = new DynamicWizard()
            {
                Name = creatorModel.WizardName,
                Description = creatorModel.WizardDescription,
                WizardType = creatorModel.WizardType,
                WizardTheme = creatorModel.WizardTheme,
                Project = creatorModel.Project
            };

            if (creatorModel.CurrentCategorySelection != null)
            {
                WizardModel.Category = creatorModel.CurrentCategorySelection.Value as Category;
            }
            else
            {
                // Set this to null so it will run through and add this to the lonewizard list.
                WizardModel.Category = null;
            }

            if (ViewModel != null)
                ViewModel.WizardName = creatorModel.WizardName;

            await WizardModel.CreateNew();

            await LoadUI();
        }
        private async Task LoadDesignPreview()
        {
            DialogLauncher<WizardDesignPreview> designPreview = new DialogLauncher<WizardDesignPreview>(this, ResizeMode.NoResize, true) ;
            designPreview.OnClose += OnWizardDesignPreviewClose;
            await designPreview.Control.LoadWizard(WizardModel);
            OnWizardUpdated += designPreview.Control.WizardDesignPreview_OnWizardUpdated;
            designPreview.Show();

            

            DesignPreview = designPreview.Control;
        }
        private async Task LoadUI()
        {
            if (WizardModel == null)
                return;

            // When loading UI, open up Preview window also.
            await LoadDesignPreview();

            ContentContainer.Children.Clear();

            foreach (DynamicWizardSection wizardSection in WizardModel.Sections)
            {
                EditorSection editorSection = new EditorSection(ContentContainer);
                editorSection.OnSelected += UpdateCurrentSelection;
                editorSection.OnWizardUpdated += OnWizardUpdated;

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

            if (CurrentPage.Model is DynamicWizardSection wizardSection)
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
                // If the index is 0, we know that we are on the first page so disable the navPrevious button. & First page button
                NavPreviousButton.IsEnabled = false;
                NavPreviousButton.Opacity = 0.25;

                NavFirstPageButton.IsEnabled = false;
                NavFirstPageButton.Opacity = 0.25;
            }
            else
            {
                // If we are not on the first page, enable the navPrevious button. & First page button
                NavPreviousButton.IsEnabled = true;
                NavPreviousButton.Opacity = 1;

                NavFirstPageButton.IsEnabled = true;
                NavFirstPageButton.Opacity = 1;
            }
            // Check to see if we are on the last page.
            if (pageIndex == Pages.Count - 1)
            {
                // If we are on the last page, disable the navNext button. & Last page button
                NavNextButton.IsEnabled = false;
                NavNextButton.Opacity = 0.25;

                NavLastPageButton.IsEnabled = false;
                NavLastPageButton.Opacity = 0.25;
            }
            else
            {
                // If we are not on the last page, enable the navNext button. & Last page button
                NavNextButton.IsEnabled = true;
                NavNextButton.Opacity = 1;

                NavLastPageButton.IsEnabled = true;
                NavLastPageButton.Opacity = 1;
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
        private void UpdateCurrentSelection(object sender, IIsSelectable newSelection)
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

            // Assign this for property editor binding.
            if (newSelection != null)
            {
                if (newSelection.Model is DynamicWizardCell wizardCell)
                {
                    ViewModel.CurrentSelection = wizardCell.Control as IPropertyEditor;
                }
                else
                {
                    ViewModel.CurrentSelection = newSelection.Model as IPropertyEditor;
                }
            }
            else
                ViewModel.CurrentSelection = null;
        }
        private async void CreateNewPage()
        {
            if (WizardModel == null)
                return;

            EditorSection page = new EditorSection(ContentContainer);
            page.OnSelected += UpdateCurrentSelection;
            page.OnWizardUpdated += OnWizardUpdated;

            DynamicWizardSection newSectionModel = new DynamicWizardSection(WizardModel);
            await page.LoadModel(newSectionModel);
            
            page.Model.OrderId = GetNextPageOrderNumber();

            AddPage(page);

            LoadPage(page.Model.OrderId);

            OnWizardUpdated?.Invoke(this, null);
        }
        private void AddPage(EditorSection page)
        {
            if (Pages == null)
                Pages = new List<EditorSection>();

            Pages.Add(page);

            ComboBoxItem comboBoxItem = new ComboBoxItem()
            {
                Text = page.Model.Name,
                Value = page.Model.OrderId
            };

            ViewModel.PageList.Add(comboBoxItem);
        }
        private void LoadNextPage()
        {
            if (CurrentPage != null && Pages != null)
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
            if (CurrentPage != null && Pages != null)
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
            if (CurrentPage != null && Pages != null && WizardModel != null)
            {
                _ = Pages.Remove(CurrentPage);
                ViewModel.DeleteCurrentPage();
                _ = WizardModel.Sections.Remove(CurrentPage.Model as DynamicWizardSection);

                // Wizard Section/Page has been deleted. Update Preview.
                OnWizardUpdated?.Invoke(this, WizardModel);
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
            // TODO: Save Xml
            if (DesignPreview != null)
                DesignPreview.Close();
                
            WindowControl.CloseTopWindow();
        }
        private void DeletePageButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage != null && Pages != null)
            {
                int index = Pages.IndexOf(CurrentPage);

                if (index > -1)
                {
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
        }
        private void NewPageButton_Click(object sender, RoutedEventArgs e)
        {
            CreateNewPage();
        }
        private void NavFirstPageButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPage(0);
        }
        private void NavPreviousButton_Click(object sender, RoutedEventArgs e)
        {
            LoadPreviousPage();
        }
        private void NavNextButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNextPage();
        }
        private void NavLastPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (Pages != null)
                LoadPage(Pages.Count - 1);
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
        private void UpdateWizardDesignPreview()
        {
            if (WizardModel != null)
            {
                OnWizardUpdated?.Invoke(this, WizardModel);
            }
        }
        private void EditProperties_Click(object sender, RoutedEventArgs e)
        {
            if (WizardModel != null)
            {
                WizardCreatorViewModel vm = new WizardCreatorViewModel()
                {
                    WizardName = WizardModel.Name,
                    WizardDescription = WizardModel.Description,
                    WizardTheme = WizardModel.WizardTheme,
                    WizardType = WizardModel.WizardType
                };

                DialogLauncher<WizardCreator> wizardCreator = new DialogLauncher<WizardCreator>(this, ResizeMode.NoResize);
                wizardCreator.Control.LoadExistingData(vm);
                wizardCreator.OnClose += WizardCreator_OnClose;
                if (wizardCreator.Control.ViewModel != null)
                {
                    wizardCreator.Control.ViewModel.LoadCategoryList(WizardModel?.Project?.CreateCategoryList());
                    if (WizardModel?.Category != null)
                    {
                        wizardCreator.Control.ViewModel.SetCategory(WizardModel.Category.Id);
                    }
                }

                wizardCreator.ShowDialog();
            }
        }
        private void WizardCreator_OnClose(object sender, DialogEventArgs e)
        {
            if (sender is DialogLauncher<WizardCreator> wizardCreator && wizardCreator.Control != null && wizardCreator.Control.DialogResult == DialogResult.Accept &&
                wizardCreator.Control.ViewModel != null)
            {
                if (WizardModel != null)
                {
                    WizardModel.LoadFromWizardCreator(wizardCreator.Control.ViewModel);
                }
            }
        }
        private async void OpenPreviewWindow_Click(object sender, RoutedEventArgs e)
        {
            if (DesignPreview == null)
            {
                await LoadDesignPreview();
            }
            
            e.Handled = true;
        }
        private void OnWizardDesignPreviewClose(object sender, EventArgs e)
        {
            //OnWizardUpdated -= DesignPreview.WizardDesignPreview_OnWizardUpdated;
            DesignPreview = null;
        }

        #region Interface

        public event EventHandler OnClosed;
        public void CloseAsync()
        {
            OnClosed?.Invoke(this, new EventArgs());
        }
        public WindowParameters GetWindowParameters()
        {
            WindowParameters windowParameters = new WindowParameters()
            {
                CanResize = true,
                MinHeight = 800,
                MinWidth = 1280
            };

            return windowParameters;
        }

        #endregion

        #region Temp new field item drag

        private Border Source { get; set; }
        private NewControl SelectedControl { get; set; }
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
            TextBlock child = null;
            if (Source != null)
            {
                child = Source.Child as TextBlock;
            }

            ElementType selectedType = ElementType.Table;
            if (child != null)
            {
                switch (child.Text)
                {
                    case "Table":
                        {
                            selectedType = ElementType.Table;
                            break;
                        }
                    case "YesNo":
                        {
                            selectedType = ElementType.YesNo;
                            break;
                        }
                    case "RadioList":
                        {
                            selectedType = ElementType.RadioList;
                            break;
                        }
                    case "Label":
                        {
                            selectedType = ElementType.Label;
                            break;
                        }
                    case "Checkbox":
                        {
                            selectedType = ElementType.CheckBox;
                            break;
                        }
                    case "Button":
                        {
                            selectedType = ElementType.Button;
                            break;
                        }
                    case "DateTime":
                        {
                            selectedType = ElementType.DateTime;
                            break;
                        }
                    case "Textbox":
                        {
                            selectedType = ElementType.TextBox;
                            break;
                        }
                    case "TextboxMultiLine":
                        {
                            selectedType = ElementType.MultiLineTextBox;
                            break;
                        }
                    case "TextboxDouble":
                        {
                            selectedType = ElementType.DoubleTextBox;
                            break;
                        }
                    case "TextboxNumeric":
                        {
                            selectedType = ElementType.NumericTextBox;
                            break;
                        }
                    default:
                        {
                            selectedType = ElementType.Unknown;
                            break;
                        }
                }
            }

            SelectedControl = new NewControl() { ElementType = selectedType };
        }

        #endregion

    }
}
