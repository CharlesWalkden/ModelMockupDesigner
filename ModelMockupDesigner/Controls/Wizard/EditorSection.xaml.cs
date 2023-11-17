using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
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

namespace ModelMockupDesigner.Controls
{
    /// <summary>
    /// Interaction logic for EditorSection.xaml
    /// </summary>
    public partial class EditorSection : UserControl, IIsSelectable
    {
        #region Public Properties

        public BaseModel Model => SectionModel;
        public AthenaGroupBox GroupBox { get; set; }

        #endregion

        #region Private Properties

        private DynamicWizardSection SectionModel => DataContext as DynamicWizardSection;
        private Grid Container { get; set; }

        #endregion

        #region Constructor

        public EditorSection(Grid container)
        {
            InitializeComponent();
            Container = container;
        }

        #endregion

        #region Methods

        public async Task LoadModel(DynamicWizardSection sectionModel)
        {
            sectionModel.DisplayChanged += OnGroupBoxDisplayChanged;
            DataContext = sectionModel;

            foreach (DynamicWizardColumn wizardColumn in sectionModel.WizardColumns)
            {
                EditorColumn editorColumn = new EditorColumn(this);
                editorColumn.OnSelected += OnSelected;
                editorColumn.OnWizardUpdated += OnWizardUpdated;

                container.Children.Add(editorColumn);

                await editorColumn.LoadModel(wizardColumn);
            }
        }
        public void Unselect()
        {
            HeaderStackPanel.Background = Brushes.Transparent;
            HeaderTextBlock.Foreground = Application.Current.Resources["SectionPinkBrush"] as SolidColorBrush;
            HeaderTextBlock.Background = Brushes.White;
            Border.Fill = Brushes.Transparent;
        }
        public void DeleteControl()
        {

        }
        public void Delete(EditorColumn child)
        {
            if (SectionModel != null && child.Model != null)
            {
                child.OnSelected -= OnSelected;
                _ = SectionModel.WizardColumns.Remove((DynamicWizardColumn)child.Model);
                if (child.Model.DisplayGroupbox)
                {
                    container.Children.Remove(child.GroupBox);
                }
                else
                {
                    container.Children.Remove(child);
                }
                child.ColumnParent = null;
                UpdateColumnOrderIDs();

                OnWizardUpdated?.Invoke(this, null);
            }
        }
        public void RemoveFromUI(FrameworkElement element)
        {
            container.Children.Remove(element);
        }
        private async Task AddColumn()
        {
            if (SectionModel == null)
                return;

            DynamicWizardColumn wizardColumn = new DynamicWizardColumn(SectionModel);
            wizardColumn.CreateNew();

            SectionModel.WizardColumns.Add(wizardColumn);

            EditorColumn editorColumn = new EditorColumn(this);
            editorColumn.OnSelected += OnSelected;
            editorColumn.OnWizardUpdated += OnWizardUpdated;

            container.Children.Add(editorColumn);

            await editorColumn.LoadModel(wizardColumn);

            OnWizardUpdated?.Invoke(this, null);
        }
        private void AddColumn(int index, EditorColumn column)  
        {
            if (SectionModel != null)
            {
                SectionModel.WizardColumns.Add(column.Model as DynamicWizardColumn);
                column.SetNewParent(this);
                column.OnSelected += OnSelected;
                column.OnWizardUpdated += OnWizardUpdated;
            }
            FrameworkElement columnOrGroupBox = column;
            if (column.Model.DisplayGroupbox)
            {
                columnOrGroupBox = column.GroupBox;
            }

            if (container.Children.Count == index)
            {
                container.Children.Insert(index - 1, columnOrGroupBox);
            }
            else
            {
                container.Children.Insert(index, columnOrGroupBox);
            }

            OnWizardUpdated?.Invoke(this, null);
        }
        public int FindIndex(FrameworkElement element)
        {
            return container.Children.IndexOf(element);
        }
        public void UpdateColumnOrderIDs() 
        {
            int index = 0;
            foreach (FrameworkElement element in container.Children)
            {
                EditorColumn column = null;

                if (element is EditorColumn)
                {
                    column = element as EditorColumn;
                }
                else if (element is AthenaGroupBox box)
                {
                    column = box.GetContent() as EditorColumn;
                }

                if (column.Model != null) 
                {
                    column.Model.OrderId = index;
                    index++;
                }
            }
        }
        public void AddColumnAtIndex(int index, EditorColumn column) 
        {
            AddColumn(index, column);
            UpdateColumnOrderIDs();
        }
        public void AddElementAtIndex(int index, FrameworkElement element)
        {
            if (container.Children.Count == 0)
            {
                container.Children.Add(element);
            }
            else if (container.Children.Count == index)
            {
                container.Children.Insert(index - 1, element);
            }
            else
            {
                container.Children.Insert(index, element);
            }

            OnWizardUpdated?.Invoke(this, null);
        }

        #endregion

        #region Events

        public EventHandler<IIsSelectable> OnSelected;
        public event EventHandler<DynamicWizard> OnWizardUpdated;
        private void OnGroupBoxDisplayChanged(object sender, GroupBoxDisplayChangedEventArgs e)
        {
            if (e.Display)
            {
                if (GroupBox == null)
                {
                    AthenaGroupBox gb = new AthenaGroupBox();
                    //gb.Margin = new Thickness(5);
                    gb.Initialise(e);
                    Container.Children.Remove(this);
                    gb.SetContent(this);
                    Container.Children.Add(gb);

                    GroupBox = gb;
                }
            }
            else
            {
                if (GroupBox != null)
                {
                    GroupBox.RemoveContent(this);
                    Container.Children.Remove(GroupBox);
                    Container.Children.Add(this);
                    GroupBox = null;
                }
            }

            GroupBox?.Initialise(e);

            OnWizardUpdated?.Invoke(this, null);
        }
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HeaderStackPanel.Background = Application.Current.Resources["SectionPinkBrush"] as SolidColorBrush;
            HeaderTextBlock.Foreground = Brushes.White;
            HeaderTextBlock.Background = Brushes.Transparent;
            Border.Fill = Brushes.Yellow;

            OnSelected?.Invoke(this, this);
        }
        private void HeaderStackPanel_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem menuItem = new MenuItem() { Header = "Add New Column" };
            menuItem.Click += MenuItem_Click;
            contextMenu.Items.Add(menuItem);

            contextMenu.IsOpen = true;

            HeaderStackPanel.ContextMenu = contextMenu;

            e.Handled = true;
        }
        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                switch (menuItem.Header)
                {
                    case "Add New Column":
                        {
                            await AddColumn();
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}
