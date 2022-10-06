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

        //private double columnWidth = 0;
        public AthenaRadioList(List<string> listOptions, CustomControl controlModel)
        {
            InitializeComponent();
            controlModel.DisplayChanged += ControlModel_DisplayChanged; 
            ControlModel = controlModel;

            if (listOptions != null)
            {
                foreach (string option in listOptions)
                {
                    RadioButton button = new()
                    {
                        Style = Application.Current.Resources["RadioButtonStyle1"] as Style,
                        Content = option
                    };
                    radioButtons.Add(button);
                    //button.Measure(new Size(1000.0, 1000.0));
                    //if (button.DesiredSize.Width > columnWidth)
                    //    columnWidth = button.DesiredSize.Width;
                }
            }
            ShowGroupBox = true;
            this.SizeChanged += new SizeChangedEventHandler(AthenaCheckboxList_SizeChanged);
            ControlModel = controlModel;
        }

        private void ControlModel_DisplayChanged(object? sender, GroupBoxDisplayChangedEventArgs e)
        {
            ShowGroupBox = e.Display;
            Title = e.GroupBoxTitle;
        }

        void AthenaCheckboxList_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int columnCount = 1;

            List<StackPanel> stackPanels = new List<StackPanel>();
            listParent.ColumnDefinitions.Clear();
            for (int i = 0; i< columnCount; i++)
            {
                AddColumn(ref stackPanels);
            }

            int currentColumnIndex = 0;
            foreach (RadioButton button in radioButtons)
            {
                if (button.Parent is StackPanel panel)
                {
                    panel.Children.Remove(button);
                }
                stackPanels[currentColumnIndex].Children.Add(button);
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

        public BaseModel? Model => new CustomControl(ElementType);

        #endregion
    }
}
