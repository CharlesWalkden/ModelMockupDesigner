using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
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
    /// Interaction logic for AthenaDate.xaml
    /// </summary>
    public partial class AthenaDate : UserControl, ICellControl
    {
        public CustomControl ControlModel { get; set; }
        public ElementType ElementType => ControlModel.ElementType;
        public BaseModel Model => ControlModel;
        public bool DisplayGroupbox => ControlModel.DisplayGroupbox;
        public void Unload()
        {
            if (ControlModel != null)
            {
                ControlModel.DisplayChanged -= ControlModel_DisplayChanged;
            }
            DataContext = null;
            ControlModel = null;
        }
        public AthenaDate(CustomControl customControl)
        {
            InitializeComponent();
            DataContext = customControl;
            ControlModel = customControl;
            ControlModel.DisplayChanged += ControlModel_DisplayChanged;
            Unloaded += AthenaDate_Unloaded;

            SetValue("NOW");
            ShowGroupBox = customControl.DisplayGroupbox;
            Title = customControl.GroupBoxTitle;

        }

        private void AthenaDate_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ControlModel != null)
            {
                ControlModel.DisplayChanged -= ControlModel_DisplayChanged;
            }
        }

        private void ControlModel_DisplayChanged(object sender, GroupBoxDisplayChangedEventArgs e)
        {
            ShowGroupBox = e.Display;
            Title = e.GroupBoxTitle;
            if (e.Display)
            {
                HorizontalAlignment = e.HorizontalAlignment.ToXaml();
                VerticalAlignment = e.VerticalAlignment.ToXaml();
            }
        }

        public bool ShowGroupBox
        {
            set
            {
                if (value)
                    titleText.Visibility = Visibility.Visible;
                else
                    titleText.Visibility = Visibility.Collapsed;
            }
        }
        private bool showTodayButton = true;
        public bool ShowTodayButton
        {
            set
            {
                showTodayButton = value;
                todayButton.Visibility = System.Windows.Visibility.Collapsed;
                if (showTodayButton)
                    todayButton.Visibility = System.Windows.Visibility.Visible;
            }
        }
        public string Title
        {
            get { return titleText.Text; }
            set { titleText.Text = value; }
        }
        
        public bool Enabled
        {
            get
            {
                return this.IsEnabled;
            }
            set
            {
                if (IsEnabled != value)
                {
                    IsEnabled = value;
                }
            }
        }
        
        
        public bool SetValue(object o)
        {
            if (o is DateTime)
            {
                dataPicker.SelectedDate = ((DateTime)o).ToLocalTime();
            }
            else if (o is string && ((string)o == "NOW"))
            {
                dataPicker.SelectedDate = DateTime.Now;
            }
            else
            {
                dataPicker.SelectedDate = null;
            }
            return true;
        }
        
        
        public object GetValue()
        {
            try
            {
                if (dataPicker.SelectedDate.HasValue)
                    return new DateTime(dataPicker.SelectedDate.Value.Year, dataPicker.SelectedDate.Value.Month, dataPicker.SelectedDate.Value.Day, 0, 0, 0, DateTimeKind.Utc);
            }
            catch (Exception)
            {
            }
            return null;
        }
        
        private void dataPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dataPicker.SelectedDate = null; // Force the ValueChanged event to be raised whenever the button gets clicked
            dataPicker.SelectedDate = DateTime.Now.Date;

        }
    }
}
