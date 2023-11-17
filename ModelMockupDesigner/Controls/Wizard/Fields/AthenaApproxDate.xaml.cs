using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for AthenaApproxDate.xaml
    /// </summary>
    public partial class AthenaApproxDate : UserControl, ICellControl
    {
        public CustomControl ControlModel { get; set; }
        public ElementType ElementType => ControlModel.ElementType;
        public BaseModel Model => ControlModel;
        public bool DisplayGroupbox => ControlModel.DisplayGroupbox;
        public void Unload()
        {
            if (ControlModel != null)
            {
                ControlModel.DisplayChanged -= GroupBox_DisplayChanged;
            }
            DataContext = null;
            ControlModel = null;
        }
        public AthenaApproxDate(CustomControl controlModel)
        {
            InitializeComponent();

            DataContext = controlModel;
            ControlModel = controlModel;
            ControlModel.DisplayChanged += GroupBox_DisplayChanged;
            Unloaded += AthenaApproxDate_Unloaded;



#if !SILVERLIGHT
            monthComboBox.IsEditable = false;
#endif
            monthComboBox.SelectedIndex = 0;

            List<string> months = new List<string>();
            months.Add("");
            for (int m = 1; m < 13; m++)
            {
                months.Add(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m));
            }
            monthComboBox.ItemsSource = months;

            yearTextBox.ElementType = ElementType.NumericTextBox;
            dayTextBox.ElementType = ElementType.NumericTextBox;

            // Bedside config
            //rd1.Height = new GridLength(0, GridUnitType.Auto);
            //rd2.Height = new GridLength(0, GridUnitType.Auto);
            //monthComboBox.Style = Application.Current.Resources["ComboBoxStyleTouch"] as Style;
            //monthComboBox.ItemContainerStyle = Application.Current.Resources["ComboBoxItemStyleTouch"] as Style;

            //dayLabel.Margin = new Thickness(-15, 0, -15, 0);
            //monthLabel.Margin = new Thickness(-15, 0, -15, 0);
            //yearLabel.Margin = new Thickness(-15, 0, -15, 0);
            //grid.Margin = new Thickness(0);


            ShowGroupBox = controlModel.DisplayGroupbox;
            Title = controlModel.GroupBoxTitle;
        }

        private void AthenaApproxDate_Unloaded(object sender, RoutedEventArgs e)
        {
            if (ControlModel != null)
            {
                ControlModel.DisplayChanged -= GroupBox_DisplayChanged;
            }
        }
        private void GroupBox_DisplayChanged(object sender, GroupBoxDisplayChangedEventArgs e)
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
        public string Title
        {
            get { return titleText.Text; }
            set { titleText.Text = value; }
        }
        public bool SetValue(object o)
        {
            SetBkColor(null);
            if (o is DateTime)
            {
                DateTime dt = ((DateTime)o).ToLocalTime();

                if (dt.Second == 30)
                    dayTextBox.Text = dt.Day.ToString();
                else
                    dayTextBox.Text = string.Empty;

                if (dt.Minute == 30)
                    monthComboBox.SelectedIndex = dt.Month;
                else
                    monthComboBox.SelectedIndex = 0;

                yearTextBox.Text = dt.Year.ToString();
            }
            else
            {
                dayTextBox.Text = string.Empty;
                monthComboBox.SelectedIndex = 0;
                yearTextBox.Text = string.Empty;
            }
            return true;
        }
        
        public object GetValue()
        {
            try
            {
                bool? valid = ValidateDate();
                if (valid.HasValue && valid.Value)
                {
                    int seconds = 0;
                    int minutes = 0;
                    int day = 0;
                    int.TryParse(dayTextBox.Text, out day);
                    int month = monthComboBox.SelectedIndex;

                    if (yearTextBox.Text.Length == 4)
                    {
                        int year = 0;
                        int.TryParse(yearTextBox.Text, out year);

                        if (day > 0)
                            seconds = 30;
                        else
                            day = 1;

                        if (month > 0)
                            minutes = 30;
                        else
                            month = 1;

                        return new DateTime(year, month, day, 12, minutes, seconds, DateTimeKind.Utc);
                    }
                }
            }
            catch (Exception)
            {
            }
            return null;
        }
        
        private bool? ValidateDate()
        {
            bool? valid = null;

            try
            {
                int year = -1;
                if (yearTextBox.Text.Length == 4)
                    int.TryParse(yearTextBox.Text, out year);

                int month = monthComboBox.SelectedIndex;

                int day = -1;
                if (dayTextBox.Text.Length > 0)
                    int.TryParse(dayTextBox.Text, out day);

                if (year != -1 && month > 0 && day == -1)
                {
                    day = 1;
                }
                else if (year != -1 && month == 0 && day == -1)
                {
                    month = 1;
                    day = 1;
                }

                if (year != -1 || month != 0 || day != -1)
                {
                    // Exception thrown if not a valid date
                    DateTime dt = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
                    valid = true;
                }
            }
            catch
            {
                valid = false;
            }

            SetBkColor(valid);

            return valid;
        }
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsValidKeyPress(e.Key))
                e.Handled = true;
        }
        private bool IsValidKeyPress(Key key)
        {
            return ((key >= Key.D0 && key <= Key.D9) || (key >= Key.NumPad0 && key <= Key.NumPad9) || key == Key.Back || key == Key.Delete);
        }
        private void SetBkColor(bool? valid)
        {
            Color validColour = Color.FromArgb(255, 1, 243, 62);
            Color invalidColour = Color.FromArgb(255, 255, 72, 72);
            Color col = Colors.White;

            if (valid.HasValue)
            {
                if (valid.Value)
                    col = validColour;
                else
                    col = invalidColour;
            }

            SolidColorBrush brush = new SolidColorBrush(col);
            dayTextBox.Background = brush;
            yearTextBox.Background = brush;
        }
    }
}
