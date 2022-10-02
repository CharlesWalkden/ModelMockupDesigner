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
            if (container.Children.Count == index)
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
