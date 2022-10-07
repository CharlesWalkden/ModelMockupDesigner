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

namespace ModelMockupDesigner.Controls.Wizard.Fields
{
    /// <summary>
    /// Interaction logic for AthenaRadioList.xaml
    /// </summary>
    public partial class AthenaRadioList : UserControl, ICellControl
    {
        public CustomControl ControlModel { get; set; }

        private List<RadioButton> radioButtons = new();

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
            ShowGroupBox = true;
            RefreshListOptions(controlModel.ColumnCount);
        }

        private void ControlModel_OnColumnCountChanged(object? sender, int e)
        {
            RefreshListOptions(e);
        }

        private void GroupBox_DisplayChanged(object? sender, GroupBoxDisplayChangedEventArgs e)
        {
            ShowGroupBox = e.Display;
            Title = e.GroupBoxTitle;
        }
        public void RefreshListOptions(int columnCount)
        {
            listParent.Children.Clear();

            int itemsPerColumn = radioButtons.Count/columnCount;
            int remainder = radioButtons.Count%columnCount;
            if (remainder > 0)
                itemsPerColumn++;

            List<StackPanel> stackPanels = new List<StackPanel>();
            listParent.ColumnDefinitions.Clear();
            for (int i = 0; i < columnCount; i++)
            {
                AddColumn(ref stackPanels);
            }

            int currentColumnIndex = 0;
            int currentColumnCount = 0;
            foreach (RadioButton button in radioButtons)
            {
                if (button.Parent is StackPanel panel)
                {
                    panel.Children.Remove(button);
                }

                if (currentColumnCount == itemsPerColumn)
                {
                    currentColumnIndex++;
                    currentColumnCount = 0;
                }

                stackPanels[currentColumnIndex].Children.Add(button);
                currentColumnCount++;
                
            }
        }
        private void AddColumn(ref List<StackPanel> stackPanels)
        {
            listParent.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            StackPanel panel = new StackPanel();
            listParent.Children.Add(panel);

            stackPanels.Add(panel);
            Grid.SetColumn(panel, stackPanels.Count - 1);
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

        #endregion
    }
}
