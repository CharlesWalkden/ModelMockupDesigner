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
    /// Interaction logic for EditorCell.xaml
    /// </summary>
    public partial class EditorCell : UserControl, IIsSelectable
    {
        #region Public Properties

        public BaseModel? Model { get => CellModel; }
        public ICellParent CellParent { get => cellParent; set => cellParent = value; }

        #endregion

        #region Private Properties

        private WizardCell? CellModel { get; set; } 
        private ICellParent cellParent { get; set; }

        #endregion

        #region Constructor

        public EditorCell(ICellParent parent)
        {
            InitializeComponent();
            cellParent = parent;
        }

        #endregion


        public async Task LoadModel(WizardCell wizardCell)
        {
            DataContext = wizardCell;
            CellModel = wizardCell;

            // TODO: Create whatever control we need and then add it to UI.
        }

        public void Unselect()
        {
            Container.Background = Brushes.Transparent;
        }

        public void Delete()
        {
            CellParent.Delete(this);
        }
        public void Delete(ICellControl cellControl)
        {
            if (CellModel != null && cellControl.Model != null)
            {
                _ = CellModel.Control = null;
                Root.Children.Remove(cellControl as FrameworkElement);
                overlay.Visibility = Visibility.Visible;
            }
        }

        public async Task AddNewControl(ElementType? elementType)
        {
            switch (elementType)
            {
                case ElementType.Table:
                    {
                        WizardTable wizardTable = new(CellModel);
                        wizardTable.CreateNew();

                        if (CellModel != null)
                        {
                            CellModel.Control = wizardTable;
                        }

                        EditorTable editorTable = new(this);
                        editorTable.OnSelected += OnSelected;
                        await editorTable.LoadModel(wizardTable);

                        Root.Children.Add(editorTable);
                        overlay.Visibility = Visibility.Collapsed;
                        overlay.Background = Brushes.White;

                        break;
                    }
                default:
                    break;
            }
        }

        #region Events

        public EventHandler<IIsSelectable>? OnSelected;
        

        private void Control_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(NewControl)))
            {
                if (e.Data.GetData(typeof(NewControl)) is NewControl newControl)
                {
                    if (newControl.ElementType == ElementType.Table && CellParent.ElementType == ElementType.Table)
                    {
                        e.Effects = DragDropEffects.None;
                        return;
                    }

                    if (Root.Children.Count > 1)
                    {
                        overlay.Background = Brushes.Transparent;
                    }
                    else
                    {
                        overlay.Background = Brushes.LightBlue;
                    }
                }
            }
        }
        private void Control_DragLeave(object sender, DragEventArgs e)
        {
            if (Root.Children.Count > 1)
            {
                overlay.Background = Brushes.Transparent;
            }
            else
            {
                overlay.Background = Brushes.White;
            }
        }
        private async void Control_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(NewControl)))
            {
                if (e.Data.GetData(typeof(NewControl)) is NewControl newControl)
                {
                    if (newControl.ElementType == ElementType.Table && CellParent.ElementType == ElementType.Table)
                    {
                        // don't allow drop, we dont want table within a table.
                        return;
                    }

                    if (Root.Children.Count > 1)
                    {
                        if (MessageBox.Show("Are you sure you want to replace this control?", "Occupied", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                        {
                            Delete();
                            await AddNewControl(newControl.ElementType);
                        }
                    }
                    else
                    {
                        await AddNewControl(newControl.ElementType);
                    }
                }

            }
            overlay.Background = Brushes.Transparent;

            // TESTING - This is here to collapse the overlay so we can interact with the control for testing.
            //overlay.Visibility = Visibility.Collapsed;
        }

        private void Control_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Root.Children.Count > 1)
            {
                //CustomControl customControl = ((Control)Root.Children[1]).DataContext as CustomControl;

                //EditorActions.OpenPropertyEditor(customControl as IFrameworkElement);

                // Old code, replace with new property editor.
            }

            Container.Background = Brushes.Yellow;

            OnSelected?.Invoke(this, this);
        }

        private void Overlay_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ContextMenu contextMenu = new ContextMenu();

            if (Root.Children.Count > 1)
            {
                MenuItem menuItem = new MenuItem() { Header = "Delete" };
                menuItem.Click += MenuItem_Click;
                contextMenu.Items.Add(menuItem);

            }

            if (CellModel?.Parent is not WizardTable)
            {
                MenuItem item = new() { Header = "Add Table" };
                item.Click += MenuItem_Click;
                contextMenu.Items.Add(item);
            }

            if (contextMenu.Items.Count > 0)
            {
                contextMenu.IsOpen = true;
            }

            overlay.ContextMenu = contextMenu;

            e.Handled = true;
        }

        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                switch (menuItem.Header)
                {
                    case "Add Table":
                        {
                            await AddNewControl(ElementType.Table);
                            break;
                        }
                    case "Delete":
                        {
                            Delete();
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
