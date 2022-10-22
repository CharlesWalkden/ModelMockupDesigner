using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        #endregion

        #region Private Properties

        private DynamicWizardSection SectionModel => DataContext as DynamicWizardSection;

        #endregion

        #region Constructor

        public EditorSection()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        public async Task LoadModel(DynamicWizardSection sectionModel)
        {
            DataContext = sectionModel;

            foreach (DynamicWizardColumn wizardColumn in sectionModel.WizardColumns)
            {
                EditorColumn editorColumn = new EditorColumn(this);
                editorColumn.OnSelected += OnSelected;

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
                _ = SectionModel.WizardColumns.Remove((DynamicWizardColumn)child.Model);
                container.Children.Remove(child);
                child.ColumnParent = null;
                UpdateColumnOrderIDs();
            }
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

            container.Children.Add(editorColumn);

            await editorColumn.LoadModel(wizardColumn);
        }
        private void AddColumn(int index, EditorColumn column)  
        {
            if (SectionModel != null)
            {
                SectionModel.WizardColumns.Add(column.Model as DynamicWizardColumn);
                column.SetNewParent(this);
                column.OnSelected += OnSelected;
            }

            if (container.Children.Count == index)
            {
                container.Children.Insert(index - 1, column);
            }
            else
            {
                container.Children.Insert(index, column);
            }
        }
        public int FindIndex(EditorColumn column)
        {
            return container.Children.IndexOf(column);
        }
        public void UpdateColumnOrderIDs() 
        {
            int index = 0;
            foreach (EditorColumn column in container.Children)
            {
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

        #endregion


        #region Events

        public EventHandler<IIsSelectable> OnSelected;
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
