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
    /// Interaction logic for AthenaRadioList.xaml
    /// </summary>
    public partial class AthenaRadioList : UserControl, ICellControl
    {
        public CustomControl ControlModel { get; set; }

        private List<RadioButton> radioButtons = new();
        private List<StackPanel> StackPanels = new List<StackPanel>();
        public AthenaRadioList(CustomControl controlModel)
        {
            InitializeComponent();
            controlModel.DisplayChanged += GroupBox_DisplayChanged;
            controlModel.OnColumnCountChanged += ControlModel_OnColumnCountChanged;
            ControlModel = controlModel;

            if (controlModel.ListOptions != null)
            {
                foreach (string option in controlModel.ListOptions)
                {
                    RadioButton button = new()
                    {
                        Style = Application.Current.Resources["RadioButtonStyle1"] as Style,
                        Content = option
                    };
                    radioButtons.Add(button);
                }
            }

            RefreshListOptions(controlModel.ColumnCount);
            ShowGroupBox = false;
            Title = null;
        }

        private void ControlModel_OnColumnCountChanged(object? sender, int e)
        {
            RefreshListOptions(e);
        }

        private void GroupBox_DisplayChanged(object? sender, GroupBoxDisplayChangedEventArgs e)
        {
            ShowGroupBox = e.Display;
            Title = e.GroupBoxTitle;
            if (e.Display)
            {
                HorizontalAlignment = e.HorizontalAlignment.ToXaml();
                VerticalAlignment = e.VerticalAlignment.ToXaml();
            }
        }
        public void RefreshListOptions(int columnCount)
        {
            StackPanels.Clear();
            listParent.Children.Clear();
            listParent.ColumnDefinitions.Clear();

            int numberOfRows = radioButtons.Count/columnCount;

            if (radioButtons.Count % columnCount > 0)
                numberOfRows++;

            for (int i = 0; i < columnCount; i++)
            {
                AddColumn();
            }

            int currentColumnIndex = 0;
            int columnItemCount = 0; 
            foreach (RadioButton button in radioButtons)
            {
                if (button.Parent is StackPanel panel)
                {
                    panel.Children.Remove(button);
                }

                StackPanels[currentColumnIndex].Children.Add(button);
                columnItemCount++;

                if (columnItemCount >= numberOfRows)
                {
                    columnItemCount = 0;

                    currentColumnIndex++;
                    if (currentColumnIndex == columnCount)
                        currentColumnIndex = 0;
                }
            }
        }
        private void AddColumn()
        {
            listParent.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            StackPanel panel = new StackPanel();
            listParent.Children.Add(panel);

            StackPanels.Add(panel);
            Grid.SetColumn(panel, StackPanels.Count - 1);
        }
        public bool ShowGroupBox
        {
            set
            {
                if (value)
                {
                    groupBoxTitle.Visibility = Visibility.Visible;
                    double offset = 0.0;
                    if (!double.IsNaN(groupBoxTitle.labelBackground.Height))
                    {
                        offset = groupBoxTitle.labelBackground.Height;
                    }
                    layoutRoot.Margin = new Thickness(this.Margin.Left, 2 + offset, this.Margin.Right, this.Margin.Bottom);
                }
                else
                {
                    groupBoxTitle.Visibility = Visibility.Collapsed;
                    layoutRoot.Margin = new Thickness(this.Margin.Left, 2, this.Margin.Right, this.Margin.Bottom);
                }
            }
        }

        public string Title
        {
            get { return groupBoxTitle.Text; }
            set { groupBoxTitle.Text = value; }
        }

        #region Interface

        public ElementType ElementType => ElementType.RadioList;

        public BaseModel? Model => ControlModel;

        public bool DisplayGroupbox => ControlModel.DisplayGroupbox;

        #endregion
    }
}
