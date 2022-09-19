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

            for (int i = 0; i < columnModel.WizardPanels.Count; i++)
            {
                container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }

            foreach (DynamicWizardPanel wizardPanel in columnModel.WizardPanels)
            {
                EditorPanel editorPanel = new(this);
                editorPanel.OnSelected += OnSelected;

                container.Children.Add(editorPanel);
                Grid.SetRow(editorPanel, wizardPanel.Order);

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
            }
        }

        private async Task AddPanel()
        {
            if (ColumnModel == null)
                return;

            container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            DynamicWizardPanel wizardPanel = new(ColumnModel);
            wizardPanel.CreateNew();

            ColumnModel.WizardPanels.Add(wizardPanel);

            EditorPanel editorPanel = new(this);
            editorPanel.OnSelected += OnSelected;

            container.Children.Add(editorPanel);
            Grid.SetRow(editorPanel, wizardPanel.Order);

            await editorPanel.LoadModel(wizardPanel);

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
