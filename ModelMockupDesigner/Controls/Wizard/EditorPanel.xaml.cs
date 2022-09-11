using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using ModelMockupDesigner.Models.Wizard;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for EditorPanel.xaml
    /// </summary>
    public partial class EditorPanel : UserControl, IIsSelectable, ICellParent
    {
        #region Public Properties

        public BaseModel? Model { get => PanelModel; }
        public EditorColumn PanelParent { get => panelParent; set => panelParent = value; }
        public ElementType ElementType { get => ElementType.Panel; }

        #endregion

        #region Private Properties

        private WizardPanel? PanelModel { get; set; } 
        private EditorColumn panelParent { get; set; }

        #endregion

        #region Constructor

        public EditorPanel(EditorColumn parent)
        {
            InitializeComponent();
            panelParent = parent;
        }

        #endregion

        #region Methods

        public async Task LoadModel(WizardPanel panelModel)
        {
            DataContext = panelModel;
            PanelModel = panelModel;

            if (panelModel.Cells.Count == 0)
                return;

            int maxRow = panelModel.Cells.Max(x => x.Row);
            int maxColumn = panelModel.Cells.Max(x => x.Column);

            for (int r = 0; r <= maxRow; r++)
            {
                container.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Auto) });
                for (int c = 0; c <= maxColumn; c++)
                {
                    container.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Auto) });
                }
            }

            foreach (WizardCell cellContent in panelModel.Cells)
            {
                EditorCell editorCell = new(this);
                editorCell.OnSelected += OnSelected;

                container.Children.Add(editorCell);
                Grid.SetColumn(editorCell, cellContent.Column);
                Grid.SetRow(editorCell, cellContent.Row);

                await editorCell.LoadModel(cellContent);
            }
        }
        public void Delete()
        {
            PanelParent.Delete(this);
        }
        public void Delete(EditorCell cell)
        {
            if (PanelModel != null && cell.Model != null)
            {
                _ = PanelModel.Cells.Remove((WizardCell)cell.Model);
                container.Children.Remove(cell);
            }
        }

        public void Unselect()
        {
            HeaderStackPanel.Background = Brushes.Transparent;
            HeaderTextBlock.Foreground = Application.Current.Resources["PanelRedBrush"] as SolidColorBrush;
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

        
        private async Task AddCell(int column, int row, ElementType? elementType = null)
        {
            WizardCell wizardCell = new(PanelModel) { Column = column, Row = row};

            PanelModel?.Cells.Add(wizardCell);

            EditorCell editorCell = new(this);
            editorCell.OnSelected += OnSelected;
            if (elementType != null)
            {
                await editorCell.AddNewControl(elementType);
            }

            container.Children.Add(editorCell);
            Grid.SetColumn(editorCell,column);
            Grid.SetRow(editorCell, row);

            await editorCell.LoadModel(wizardCell);
        }
        private async Task AddColumn(NewControl? newControl = null)
         {
            if (PanelModel == null)
                return;

            container.ColumnDefinitions.Add(new ColumnDefinition() { MinWidth = 25, Width = new GridLength(1, GridUnitType.Auto) });

            int column = PanelModel.Cells.Max(x => x.Column);
            int row = PanelModel.Cells.Max(x => x.Row);

            for (int r = 0; r <= row; r++)
            {
                if (newControl != null)
                {
                    await AddCell(column + 1, r, newControl.ElementType);
                    newControl = null;
                }
                else
                {
                    await AddCell(column + 1, r);
                }
            }
        }
        private async Task AddRow(NewControl? newControl = null)
        {
            if (PanelModel == null)
                return;

            container.RowDefinitions.Add(new RowDefinition() { MinHeight = 25, Height = new GridLength(1, GridUnitType.Auto) });

            int column = PanelModel.Cells.Max(x => x.Column);
            int row = PanelModel.Cells.Max(x => x.Row);

            for (int c = 0; c <= column; c++)
            {
                if (newControl != null)
                {
                    await AddCell(c, row + 1, newControl.ElementType);
                    newControl = null;
                }
                else
                {
                    await AddCell(c, row + 1);
                }
            }
            
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
            HeaderStackPanel.Background = Application.Current.Resources["PanelRedBrush"] as SolidColorBrush;
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
        private async void Control_Drop(object sender, DragEventArgs e)
        {
            NewControl? newControl = e.Data.GetData(typeof(NewControl)) as NewControl;

            if (sender == newColumn)
            {
                if (newControl != null)
                {
                    await AddColumn(newControl);
                }

            }
            if (sender == newRow)
            {
                if (newControl != null)
                {
                    await AddRow(newControl);
                }
            }
            newColumn.Visibility = Visibility.Collapsed;
            newRow.Visibility = Visibility.Collapsed;
        }
        private void HeaderStackPanel_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem menuItem = new() { Header = "Delete Panel" };
            menuItem.Click += MenuItem_Click;
            contextMenu.Items.Add(menuItem);

            menuItem = new() { Header = "Add Column to Panel" };
            menuItem.Click += MenuItem_Click;
            contextMenu.Items.Add(menuItem);

            menuItem = new() { Header = "Add Row to Panel" };
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
                    case "Delete Panel":
                        {
                            Delete();
                            break;
                        }
                    case "Add Column to Panel":
                        {
                            await AddColumn();
                            break;
                        }
                    case "Add Row to Panel":
                        {
                            await AddRow();
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
