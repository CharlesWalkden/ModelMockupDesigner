using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using ModelMockupDesigner.Models.Wizard;
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

namespace ModelMockupDesigner.Controls
{
    /// <summary>
    /// Interaction logic for EditorColumn.xaml
    /// </summary>
    public partial class EditorColumn : UserControl, IIsSelectable
    {
        #region Public Properties

        public BaseModel? Model { get => ColumnModel; }
        public bool IsSelected { get; set; }

        #endregion

        #region Private Properties

        private WizardColumn? ColumnModel => DataContext as WizardColumn;

        private readonly List<EditorPanel> Panels = new List<EditorPanel>(); 

        #endregion

        #region Constructor

        public EditorColumn()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        public async Task LoadModel(WizardColumn columnModel)
        {
            DataContext = columnModel;

            Grid container = new();
            for (int i = 0; i < columnModel.WizardPanels.Count; i++)
            {
                container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }

            foreach (WizardPanel wizardPanel in columnModel.WizardPanels)
            {
                EditorPanel editorPanel = new();

                container.Children.Add(editorPanel);
                Grid.SetRow(editorPanel, wizardPanel.Order - 1);

                await editorPanel.LoadModel(wizardPanel);
            }

            Root.Children.Add(container);
        }

        public void Delete(bool unselect)
        {

        }
        private void DeleteAllContent()
        {
            foreach (FrameworkElement element in Root.Children)
            {
                if (element is EditorPanel panel)
                {
                    panel.Delete(false);
                }
            }
        }


        #endregion


        #region Events

        public EventHandler<IIsSelectable> OnSelected;
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HeaderStackPanel.Background = Brushes.Red;
            HeaderTextBlock.Foreground = Brushes.White;
            HeaderTextBlock.Background = Brushes.Transparent;
            Border.Fill = Brushes.Yellow;

            //EditorActions.UpdateSelection(this);
        }
        private void HeaderStackPanel_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem menuItem = new MenuItem() { Header = "Delete all controls" };
            menuItem.Click += MenuItem_Click;
            contextMenu.Items.Add(menuItem);

            contextMenu.IsOpen = true;

            HeaderStackPanel.ContextMenu = contextMenu;

            e.Handled = true;
        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            DeleteAllContent();
        }

        #endregion
    }
}
