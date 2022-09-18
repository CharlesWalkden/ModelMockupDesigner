using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
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
    /// Interaction logic for WizardSelector.xaml
    /// </summary>
    public partial class WizardSelector : UserControl, IWindowStack
    {
        public WizardSelectorViewModel? ViewModel { get => DataContext as WizardSelectorViewModel; }

        public TreeViewItem? CurrentSelection
        {
            get => currentSelection;
            set
            {
                currentSelection = value;

                if (value != null && value is WizardTreeViewItem wizard)
                {
                    // Show preview.
                }
            }
        }
        private TreeViewItem? currentSelection { get; set; }

        public WizardSelector(Project project)
        {
            InitializeComponent();
            WizardSelectorViewModel viewModel = new(this)
            {
                Model = project
            };
            viewModel.OnListUpdated += OnListUpdated_RefreshTreeView;

            DataContext = viewModel;

            SetupTreeView();
        }

        private void SetupTreeView()
        {
            mainTreeView.Items.Clear();

            if (ViewModel != null && ViewModel.Model != null)
            {
                foreach (Category category in ViewModel.Model.Categories)
                {
                    CategoryTreeViewItem categoryTreeViewItem = new(null, category);

                    mainTreeView.Items.Add(categoryTreeViewItem);
                }
            }
        }

        public void RefreshTreeView()
        {
            SetupTreeView();
        }


        #region Interface

        public event EventHandler? OnClosed;

        public void CloseAsync()
        {
            OnClosed?.Invoke(this, new EventArgs());
        }

        #endregion

        private void OnListUpdated_RefreshTreeView(object? sender, EventArgs e)
        {
            SetupTreeView();
        }
        private void mainTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeViewItem treeViewItem)
            {
                CurrentSelection = treeViewItem;
            }
            else
            {
                CurrentSelection = null;
            }
        }

        private void createButton_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void createButton_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Button button)
            {
                button.ContextMenu.DataContext = button.DataContext;
                button.ContextMenu.IsOpen = true;
            }
        }
    }

    public class CategoryTreeViewItem : TreeViewItem
    {
        public Category? CategoryParent { get; set; }
        public Category Category { get; private set; }

        public CategoryTreeViewItem(Category? parent, Category category)
        {
            CategoryParent = parent;
            Category = category;
            Header = category.Name;
            HeaderTemplate = Application.Current.Resources["treeFolder"] as DataTemplate;
            Foreground = Brushes.White;

            if (category.IsExpanded)
            {
                IsExpanded = true;
            }
            Expanded += OnExpandedEvent;
            Collapsed += OnCollapsedEvent;

            if (Category.Categories != null)
            {
                foreach (Category subCategory in Category.Categories)
                {
                    CategoryTreeViewItem categoryTreeView = new(Category, subCategory);
                    
                    if (subCategory.IsExpanded)
                    {
                        categoryTreeView.IsExpanded = true;
                    }

                    categoryTreeView.Expanded += OnExpandedEvent;
                    categoryTreeView.Collapsed += OnCollapsedEvent;

                    this.Items.Add(categoryTreeView);
                }
            }

            if (Category.Wizards != null)
            {
                foreach (Wizard wizard in Category.Wizards)
                {
                    WizardTreeViewItem treeViewItem = new(wizard);
                    this.Items.Add(treeViewItem);
                }
            }
        }

        public void OnExpandedEvent(object sender, RoutedEventArgs e) 
        {
            if (sender is CategoryTreeViewItem treeViewItem)
            {
                treeViewItem.Category.IsExpanded = true;
            }
        }
        public void OnCollapsedEvent(object sender, RoutedEventArgs e)
        {
            if (sender is CategoryTreeViewItem treeViewItem)
            {
                treeViewItem.Category.IsExpanded = false;
            }
        }

        public bool DeleteFromParent()  
        {
            if (CategoryParent != null)
            {
                if (CategoryParent.Categories != null)
                {
                    CategoryParent.Categories.Remove(Category);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }

    public class WizardTreeViewItem : TreeViewItem
    {
        public Wizard Wizard { get; private set; }

        public WizardTreeViewItem(Wizard wizard)
        {
            Wizard = wizard;
            if (string.IsNullOrWhiteSpace(wizard.Name))
            {
                Header = "Wizard name not set";
            }
            else
            {
                Header = wizard.Name;
            }

            HeaderTemplate = Application.Current.Resources["treeWizardAthena"] as DataTemplate;
            Foreground = Brushes.White;
        }
    }
}
