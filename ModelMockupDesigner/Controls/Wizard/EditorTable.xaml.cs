using ModelMockupDesigner.Enums;
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
    /// Interaction logic for EditorTable.xaml
    /// </summary>
    public partial class EditorTable : UserControl, IIsSelectable, ICellParent
    {
        #region Public Properties

        public BaseModel? Model { get => TableModel; }
        public EditorCell TableParent { get => tableParent; set => tableParent = value; }
        public ElementType ElementType { get => ElementType.Table; }

        #endregion

        #region Private Properties

        private WizardTable? TableModel { get; set; }
        private EditorCell tableParent { get; set; }
        

        #endregion

        public EditorTable(EditorCell parent)
        {
            InitializeComponent();
            tableParent = parent;
        }

        #region Methods

        public async Task LoadModel(WizardTable tableModel) 
        {
            DataContext = tableModel;
            TableModel = tableModel;

            Grid container = new();

            if (tableModel.Cells.Count == 0)
                return;

            int maxRow = tableModel.Cells.Max(x => x.Row);
            int maxColumn = tableModel.Cells.Max(x => x.Column);

            for (int r = 0; r < maxRow; r++)
            {
                container.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                for (int c = 0; c < maxColumn; c++)
                {
                    container.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                }
            }

            foreach (WizardCell cell in tableModel.Cells)
            {
                EditorCell editorCell = new(this);
                editorCell.OnSelected += OnSelected;

                container.Children.Add(editorCell);
                Grid.SetColumn(editorCell, cell.Column);
                Grid.SetRow(editorCell, cell.Row);

                await editorCell.LoadModel(cell);
            }

            Root.Children.Add(container);

        }

        public void Delete()
        {

        }
        public void Unselect()
        {
            HeaderStackPanel.Background = Brushes.Transparent;
            HeaderTextBlock.Foreground = Brushes.Green;
            HeaderTextBlock.Background = Brushes.White;
            Border.Fill = Brushes.Transparent;
        }

        public void HideNewRowColumn()
        {
            //Hide new column text and make background transparent
            newColumn.Visibility = Visibility.Collapsed;
            newColumn.Background = Brushes.Transparent;

            // Hide new row text and make background transparent
            newRow.Visibility = Visibility.Collapsed;
            newRow.Background = Brushes.Transparent;
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
            base.OnPreviewDragLeave(e);
            HideNewRowColumn();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            HideNewRowColumn();
        }

        #endregion
        
        #region Events

        public EventHandler<IIsSelectable>? OnSelected;
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HeaderStackPanel.Background = Brushes.Green;
            HeaderTextBlock.Foreground = Brushes.White;
            HeaderTextBlock.Background = Brushes.Transparent;
            Border.Fill = Brushes.Yellow;

            OnSelected?.Invoke(this, this);
        }
        private void Control_DragEnter(object sender, DragEventArgs e)
        {
            if (sender is Grid target)
            {
                target.Background = Brushes.LightBlue;
            }
        }
        private void Control_DragLeave(object sender, DragEventArgs e)
        {
            if (sender is Grid target)
            {
                target.Background = Brushes.Transparent;
            }
        }
        private void Control_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(NewControl)))
            {
                e.Effects = DragDropEffects.Copy;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
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
