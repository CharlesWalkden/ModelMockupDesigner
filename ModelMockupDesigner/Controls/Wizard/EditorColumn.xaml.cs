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
using System.Xml.Linq;

namespace ModelMockupDesigner.Controls
{
    /// <summary>
    /// Interaction logic for EditorColumn.xaml
    /// </summary>
    public partial class EditorColumn : UserControl, IIsSelectable
    {
        #region Public Properties

        public BaseModel? Model { get => ColumnModel; }
        public EditorSection ColumnParent { get => columnParent; set => columnParent = value; }

        #endregion

        #region Private Properties

        private DynamicWizardColumn? ColumnModel => DataContext as DynamicWizardColumn;
        private EditorSection columnParent { get; set; } 
 
        #endregion

        #region Constructor

        public EditorColumn(EditorSection parent)
        {
            InitializeComponent();
            columnParent = parent;
        }

        public void SetNewParent(EditorSection section) 
        {
            columnParent = section;
        }

        #endregion

        #region Methods

        public async Task LoadModel(DynamicWizardColumn columnModel)
        {
            DataContext = columnModel;

            foreach (DynamicWizardPanel wizardPanel in columnModel.WizardPanels)
            {
                EditorPanel editorPanel = new(this);
                editorPanel.OnSelected += OnSelected;

                container.Children.Add(editorPanel);

                await editorPanel.LoadModel(wizardPanel);
            }
        }
        public void Unselect()
        {
            HeaderStackPanel.Background = Brushes.Transparent;
            HeaderTextBlock.Foreground = Application.Current.Resources["ColumnGrayBrush"] as SolidColorBrush;
            HeaderTextBlock.Background = Brushes.White;
            Border.Fill = Brushes.Transparent;
        }

        public void DeleteControl()
        {
            ColumnParent.Delete(this);
        }
        public void Delete(EditorPanel child)
        {
            if (ColumnModel != null && child.Model != null)
            {
                _ = ColumnModel.WizardPanels.Remove((DynamicWizardPanel)child.Model);
                container.Children.Remove(child);
                child.PanelParent = null;
                UpdatePanelOrderIDs();
            }
        }

        private async Task AddPanel()
        {
            if (ColumnModel == null)
                return;

            DynamicWizardPanel wizardPanel = new(ColumnModel);
            wizardPanel.CreateNew();

            ColumnModel.WizardPanels.Add(wizardPanel);

            EditorPanel editorPanel = new(this);
            editorPanel.OnSelected += OnSelected;

            container.Children.Add(editorPanel);

            await editorPanel.LoadModel(wizardPanel);
        }
        private void AddPanel(int index, EditorPanel panel)
        {
            if (ColumnModel != null)
            {
                ColumnModel.WizardPanels.Add(panel.Model as DynamicWizardPanel);
                panel.SetNewParent(this);
                panel.OnSelected += OnSelected;
            }

            if (container.Children.Count == 0)
            {
                container.Children.Add(panel);
            }
            else if (container.Children.Count == index)
            {
                container.Children.Insert(index -1, panel);
            }
            else
            {
                container.Children.Insert(index,panel);
            }
        }
        public void AddPanelAtIndex(int index, EditorPanel panel)
        {
            AddPanel(index, panel);
            UpdatePanelOrderIDs();
        }
        public int FindIndex(EditorPanel panel)
        {
            return container.Children.IndexOf(panel);
        }
        public void UpdatePanelOrderIDs()
        {
            int index = 0;
            foreach (EditorPanel panel in container.Children)
            {
                if (panel.Model != null)
                {
                    panel.Model.OrderId = index;
                    index++;
                }
            }
        }

        private int GetIndex()
        {
            return ColumnParent.FindIndex(this);
        }
        private void AddColumnAtIndex(int index, EditorColumn column)
        {
            ColumnParent.AddColumnAtIndex(index, column);
        }
        
        #endregion

        #region Events

        public EventHandler<IIsSelectable>? OnSelected;
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HeaderStackPanel.Background = Application.Current.Resources["ColumnGrayBrush"] as SolidColorBrush;
            HeaderTextBlock.Foreground = Brushes.White;
            HeaderTextBlock.Background = Brushes.Transparent;
            Border.Fill = Brushes.Yellow;

            OnSelected?.Invoke(this, this);
        }
        private void HeaderStackPanel_Drop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("column") && !e.Data.GetDataPresent("panel"))
            {
                e.Effects = DragDropEffects.None;
            }
            else
            {
                if (e.Data.GetDataPresent("column"))
                {
                    try
                    {
                        if (e.Data.GetData("column") is EditorColumn column && column != this)
                        {
                            int destinationIndex = GetIndex();
                            if (destinationIndex == column.Model?.OrderId + 1 && column.ColumnParent == this.ColumnParent)
                            {
                                // This means we are dragging to the column to the right which is basically just putting it in the same place it's currently in.
                                return;
                            }
                            column.DeleteControl();
                            AddColumnAtIndex(destinationIndex, column);
                        }
                    }
                    catch { }
                }
                else if (e.Data.GetDataPresent("panel"))
                {
                    try
                    {
                        if (e.Data.GetData("panel") is EditorPanel panel)
                        {
                            if (panel.PanelParent == this && panel.Model?.OrderId == 0)
                            {
                                // Return here are we are already in the position we are dragging to.
                                return;
                            }

                            panel.DeleteControl();
                            AddPanelAtIndex(0, panel);
                        }
                    }
                    catch { }
                }
            }
            
        }
        private void HeaderStackPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject dragObject = new DataObject("column", this);

                DragDrop.DoDragDrop(this, dragObject, DragDropEffects.Move);
            }
        }
        private void HeaderStackPanel_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem menuItem = new MenuItem() { Header = "Delete Column" };
            menuItem.Click += MenuItem_Click;
            contextMenu.Items.Add(menuItem);

            menuItem = new MenuItem() { Header = "Add New Panel" };
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
                    case "Delete Column":
                        {
                            DeleteControl();
                            break;
                        }
                    case "Add New Panel":
                        {
                            await AddPanel();
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
