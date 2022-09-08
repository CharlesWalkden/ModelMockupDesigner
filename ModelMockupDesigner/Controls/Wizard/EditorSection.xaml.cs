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
    /// Interaction logic for EditorSection.xaml
    /// </summary>
    public partial class EditorSection : UserControl, IIsSelectable
    {
        #region Public Properties

        public BaseModel? Model => SectionModel;
        public bool IsSelected { get; set; }

        #endregion

        #region Private Properties

        private WizardSection? SectionModel => DataContext as WizardSection;

        private readonly List<EditorColumn> Columns = new List<EditorColumn>();

        #endregion

        #region Constructor

        public EditorSection()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        public async Task LoadModel(WizardSection sectionModel)
        {
            DataContext = sectionModel;

            Grid container = new();
            for (int i = 0; i < sectionModel.WizardColumns.Count; i++)
            {
                container.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            }

            foreach (WizardColumn wizardColumn in sectionModel.WizardColumns)
            {
                EditorColumn editorColumn = new();

                container.Children.Add(editorColumn);
                Grid.SetColumn(editorColumn, wizardColumn.Order - 1);

                await editorColumn.LoadModel(wizardColumn);

            }

            Root.Children.Add(container);
        }

        public void Delete(bool unselect)
        {

        }


        #endregion

        #region Override Events

        protected override void OnPreviewDragEnter(DragEventArgs e)
        {
            this.AllowDrop = true;
            base.OnPreviewDragEnter(e);
            newColumn.Visibility = Visibility.Visible;
            newRow.Visibility = Visibility.Visible;
        }

        protected override void OnPreviewDragLeave(DragEventArgs e)
        {

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {

        }

        #endregion

        #region Events

        public EventHandler<IIsSelectable> OnSelected;
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //HeaderStackPanel.Background = Brushes.Red;
            //HeaderTextBlock.Foreground = Brushes.White;
            //HeaderTextBlock.Background = Brushes.Transparent;
            //Border.Fill = Brushes.Yellow;

            //EditorActions.UpdateSelection(this);
        }
        private void Control_DragEnter(object sender, DragEventArgs e)
        {
            //if (sender is Grid target)
            //{
            //    target.Background = Brushes.LightBlue;
            //}
        }
        private void Control_DragLeave(object sender, DragEventArgs e)
        {
            //if (sender is Grid target)
            //{
            //    target.Background = Brushes.Transparent;
            //}
        }
        private void Control_DragOver(object sender, DragEventArgs e)
        {
            //if (e.Data.GetDataPresent(typeof(NewControl)))
            //{
            //    e.Effects = DragDropEffects.Copy;
            //}
            //else
            //{
            //    e.Effects = DragDropEffects.None;
            //}
        }
        private void Control_Drop(object sender, DragEventArgs e)
        {
            //int column = 0;
            //int row = 0;
            //NewControl newControl = e.Data.GetData(typeof(NewControl)) as NewControl;

            //if (sender == newColumn)
            //{
            //    column = Root.ColumnDefinitions.Count;

            //    if (newControl != null)
            //    {
            //        AddNewColumn(column, newControl);
            //    }
            //    else
            //    {
            //        AddNewColumn(column);
            //    }

            //}
            //if (sender == newRow)
            //{
            //    row = Root.RowDefinitions.Count;

            //    if (newControl != null)
            //    {
            //        AddNewRow(row, newControl);
            //    }
            //    else
            //    {
            //        AddNewRow(row);
            //    }
            //}
            //newColumn.Visibility = Visibility.Collapsed;
            //newRow.Visibility = Visibility.Collapsed;
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

        }

        #endregion
    }
}
