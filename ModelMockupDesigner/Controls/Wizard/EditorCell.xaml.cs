using ModelMockupDesigner.Controls;
using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
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
        public ICellControl? CellControl
        {
            get => cellControl;
            set
            {
                cellControl = value;

                if (value == null)
                {
                    CellModel.Control = value;

                }
                else
                {
                    CellModel.Control = (ICellControl?)value.Model;

                }
            }
        }
        public AthenaGroupBox? GroupBox { get; set; }

        #endregion

        #region Private Properties

        private DynamicWizardCell CellModel { get; set; } 
        private ICellParent cellParent { get; set; }
        private ICellControl? cellControl { get; set; }

        #endregion

        #region Constructor

        public EditorCell(ICellParent parent)
        {
            InitializeComponent();
            cellParent = parent;
        }

        #endregion

        public async Task LoadModel(DynamicWizardCell wizardCell)
        {
            DataContext = wizardCell;
            CellModel = wizardCell;

            if (CellModel.Control != null)
            {
                await AddNewControl(CellModel.Control.ElementType, CellModel.Control);
            }
        }

        public void Unselect()
        {
            Container.Background = Brushes.Transparent;
        }

        public void DeleteControl() 
        {
            if (CellControl != null)
            {
                Delete(CellControl);
            }
        }
        public void Delete(ICellControl cellControl)
        {
            if (CellModel != null && cellControl.Model != null)
            {
                _ = CellModel.Control = null;
                Root.Children.Remove(cellControl as FrameworkElement);
                CellControl = null;
                overlay.Visibility = Visibility.Visible;
                overlay.Background = Brushes.White;
            }
        }
        private void AddCellControl(ICellControl control)
        {
            Root.Children.Add(control as FrameworkElement);
            CellControl = control;
        }
        public async Task AddNewControl(ElementType? elementType, ICellControl? controlModel = null)
        {
            switch (elementType)
            {
                case ElementType.Table:
                    {
                        DynamicWizardTable? wizardTable;
                        if (controlModel == null)
                        {
                            wizardTable = new(CellModel);
                            wizardTable.CreateNew();
                        }
                        else
                        {
                            wizardTable = controlModel as DynamicWizardTable;
                        }

                        if (CellModel != null)
                        {
                            CellModel.Control = wizardTable;
                        }

                        if (wizardTable != null)
                        {
                            wizardTable.DisplayChanged += OnGroupBoxDisplayChanged;
                        }

                        EditorTable editorTable = new(this);
                        editorTable.OnSelected += OnSelected;
                        await editorTable.LoadModel(wizardTable);

                        AddCellControl(editorTable);

                        overlay.Visibility = Visibility.Collapsed;
                        overlay.Background = Brushes.White;

                        break;
                    }
                case ElementType.YesNo:
                    {
                        CustomControl? customControl;

                        if (controlModel == null)
                        {
                            customControl = new(ElementType.YesNo);
                        }
                        else
                        {
                            customControl = controlModel as CustomControl;
                        }

                        if (customControl != null)
                        {
                            customControl.DisplayChanged += OnGroupBoxDisplayChanged;
                        }

                        AthenaYesNoControl athenaYesNoControl = new(customControl);

                        AddCellControl(athenaYesNoControl);

                        overlay.Visibility = Visibility.Visible;
                        overlay.Background = Brushes.Transparent;

                        break;
                    }
                case ElementType.RadioList:
                    {
                        CustomControl? customControl;

                        if (controlModel == null)
                        {
                            customControl = new(ElementType.RadioList);

                            DialogLauncher<ListCreator> listCreator = new DialogLauncher<ListCreator>(this, ResizeMode.NoResize);
                            listCreator.ShowDialog();
                            if (listCreator.DialogResult == DialogResult.Accept)
                            {
                                if (listCreator.Control.ViewModel != null)
                                {
                                    customControl.StoreListOption(listCreator.Control.ViewModel.GetListAsString());
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            customControl = controlModel as CustomControl;
                        }

                        AthenaRadioList athenaRadioList = new AthenaRadioList(customControl);

                        AddCellControl(athenaRadioList);

                        overlay.Visibility = Visibility.Visible;
                        overlay.Background = Brushes.Transparent;

                        break;

                    }
                case ElementType.Label:
                    {
                        CustomControl? customControl;
                        if (controlModel == null)
                        {
                            customControl = new(ElementType.Label);
                        }
                        else
                        {
                            customControl = controlModel as CustomControl;
                        }

                        if (customControl != null)
                        {
                            customControl.DisplayChanged += OnGroupBoxDisplayChanged;
                        }

                        AthenaLabel athenaLabel = new(customControl);

                        AddCellControl(athenaLabel);

                        overlay.Visibility = Visibility.Visible;
                        overlay.Background = Brushes.Transparent;

                        break;
                    }
                case ElementType.TextBox:
                    {
                        break;
                    }
                case ElementType.CheckBoxList:
                    {
                        break;
                    }
                case ElementType.Date:
                    {
                        break;
                    }
                case ElementType.Time:
                    {
                        break;
                    }
                case ElementType.DateTime:
                    {
                        break;
                    }
                case ElementType.ApproxDate:
                    {
                        break;
                    }
                case ElementType.CheckBox:
                    {
                        CustomControl? customControl;
                        if (controlModel == null)
                        {
                            customControl = new(ElementType.CheckBox);
                        }
                        else
                        {
                            customControl = controlModel as CustomControl;
                        }

                        if (customControl != null)
                        {
                            customControl.DisplayChanged += OnGroupBoxDisplayChanged;
                        }

                        AthenaCheckBox athenaCheckBox = new(customControl);

                        AddCellControl(athenaCheckBox);

                        overlay.Visibility = Visibility.Visible;
                        overlay.Background = Brushes.Transparent;

                        break;
                    }
                case ElementType.Image:
                    {
                        break;
                    }
                case ElementType.Button:
                    {
                        break;
                    }
                default:
                    break;
            }
        }

        #region Events

        public EventHandler<IIsSelectable>? OnSelected;
        private void OnGroupBoxDisplayChanged(object? sender, GroupBoxDisplayChangedEventArgs e)
        {
            if (CellControl != null)
            {
                if (e.Display)
                {
                    if (GroupBox == null)
                    {
                        FrameworkElement? control = CellControl as FrameworkElement;
                        if (control != null)
                        {
                            AthenaGroupBox gb = new AthenaGroupBox();
                            gb.Margin = new Thickness(5);
                            gb.Initialise(e);

                            Root.Children.Remove(control);

                            gb.SetContent(control);

                            Root.Children.Add(gb);

                            GroupBox = gb;
                        }
                    }
                }
                else
                {
                    if (GroupBox != null)
                    {
                        GroupBox.RemoveContent((FrameworkElement)CellControl);
                        Root.Children.Remove(GroupBox);
                        GroupBox = null;
                        Root.Children.Add((FrameworkElement)CellControl);
                    }
                }

                GroupBox?.Initialise(e);
            }
            
        }
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

                    if (CellControl != null)
                    {
                        if (MessageBox.Show("Are you sure you want to replace this control?", "Occupied", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
                        {
                            DeleteControl();
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

            if (CellModel?.Parent is not DynamicWizardTable)
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
                            DeleteControl();
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
