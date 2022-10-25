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
    /// Interaction logic for AthenaDateTime.xaml
    /// </summary>
    public partial class AthenaDateTime : UserControl, ICellControl
    {
        public CustomControl ControlModel { get; set; }
        public ElementType ElementType => ControlModel.ElementType;
        public BaseModel Model => ControlModel;
        public bool DisplayGroupbox => ControlModel.DisplayGroupbox;

        private static Color ValidColour = Color.FromArgb(255, 1, 243, 62);
        private static Color InvalidColour = Color.FromArgb(255, 255, 72, 72);

        public AthenaDateTime(CustomControl controlModel) 
        {
            InitializeComponent();
            DataContext = controlModel;
            ControlModel = controlModel;
            ControlModel.DisplayChanged += GroupBox_DisplayChanged;
            Unloaded += AthenaDateTime_Unloaded;

            datePicker.LostFocus += new RoutedEventHandler(datePicker_LostFocus);
            datePicker.GotFocus += new RoutedEventHandler(datePicker_GotFocus);

            hourTextBox.LostFocus += new RoutedEventHandler(datePicker_LostFocus);
            hourTextBox.GotFocus += new RoutedEventHandler(datePicker_GotFocus);
            minuteTextBox.LostFocus += new RoutedEventHandler(datePicker_LostFocus);
            minuteTextBox.GotFocus += new RoutedEventHandler(datePicker_GotFocus);

            SetToDefaultValue();
            ShowGroupBox = controlModel.DisplayGroupbox;
            Title = controlModel.GroupBoxTitle;
        }

        private void AthenaDateTime_Unloaded(object sender, RoutedEventArgs e)
        {
            ControlModel.DisplayChanged -= GroupBox_DisplayChanged;
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

        private bool defaultToNow = true;
        private Nullable<DateTime> currentDateTime
        {
            get
            {
                Nullable<DateTime> dt = null;

                try
                {
                    int hour = 0;
                    int minute = 0;

                    if (datePicker.SelectedDate.HasValue && int.TryParse(hourTextBox.Text, out hour) && int.TryParse(minuteTextBox.Text, out minute))
                    {
                        dt = new DateTime(datePicker.SelectedDate.Value.Year, datePicker.SelectedDate.Value.Month, datePicker.SelectedDate.Value.Day, hour, minute, 0, DateTimeKind.Local).ToUniversalTime();
                    }
                }
                catch (Exception)
                {
                }
                return dt;
            }
        }
        private string lastHourText = string.Empty;
        private int lastHourSelStart = 0;
        private string lastMinText = string.Empty;
        private int lastMinSelStart = 0;
        
        private bool IsValidKeyPress(Key key)
        {
            return ((key >= Key.D0 && key <= Key.D9) || (key >= Key.NumPad0 && key <= Key.NumPad9) || key == Key.Back || key == Key.Delete || key == Key.Tab);
        }
        private bool IsValidTime()
        {
            int hour = -1;
            int minute = -1;
            if (hourTextBox.Text.Length > 0)
                hour = Convert.ToInt32(hourTextBox.Text);
            if (minuteTextBox.Text.Length > 0)
                minute = Convert.ToInt32(minuteTextBox.Text);
            return (hour >= 0 && hour < 24 && minute >= 0 && minute < 60);
        }
        private void SetBkColor(Color col)
        {
            SolidColorBrush brush = new SolidColorBrush(col);
            hourTextBox.Background = brush;
            minuteTextBox.Background = brush;
        }
        public bool SetValue(object o)
        {
            SetBkColor(Colors.White);
            if (o != null && o is DateTime)
            {
                if ((hourTextBox.Text == string.Empty && minuteTextBox.Text == string.Empty) || (hourTextBox.Text != string.Empty && minuteTextBox.Text != string.Empty))
                {

                    DateTime dt = ((DateTime)o).ToLocalTime();

                    if (datePicker.SelectedDate != dt.Date)
                        datePicker.SelectedDate = dt.Date;

                    if ((hourTextBox.Text.TrimStart('0') == string.Empty) || (hourTextBox.Text.TrimStart('0') != string.Empty && hourTextBox.Text.TrimStart('0') != dt.Hour.ToString()))
                        hourTextBox.Text = dt.Hour.ToString("D2");

                    if ((minuteTextBox.Text.TrimStart('0') == string.Empty) || (minuteTextBox.Text.TrimStart('0') != string.Empty && minuteTextBox.Text.TrimStart('0') != dt.Minute.ToString()))
                        minuteTextBox.Text = dt.Minute.ToString("D2");
                }
            }
            else
            {
                SetToDefaultValue();
            }

            return true;
        }
        private void SetToDefaultValue()
        {
            if (defaultToNow)
            {
                SetValue(DateTime.Now);
            }
            else
            {
                datePicker.SelectedDate = null;
                hourTextBox.Text = string.Empty;
                minuteTextBox.Text = string.Empty;
            }
        }
        public object GetValue()
        {
            if (currentDateTime != null)
                return currentDateTime.Value;
            return null;
        }

        #region Events
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
        void datePicker_GotFocus(object sender, RoutedEventArgs e)
        {

            {
                Control control = sender as Control;
                if (control != null)
                {
                    if (control is TextBox)
                        SetBkColor(Color.FromArgb(255, 152, 251, 152));
                    else
                        control.Background = new SolidColorBrush(Color.FromArgb(255, 152, 251, 152));
                }
            }
        }
        void datePicker_LostFocus(object sender, RoutedEventArgs e)
        {

            {
                Control control = sender as Control;
                if (control != null)
                {
                    TextBox tb = control as TextBox;
                    if (tb != null)
                    {
                        SetBkColor(Colors.White);
                        if (tb.Text.Length == 1)
                        {
                            tb.Text = "0" + tb.Text;
                        }

                    }
                    else
                        control.Background = new SolidColorBrush(Colors.White);
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            datePicker.SelectedDate = DateTime.Now.Date;
            hourTextBox.Text = DateTime.Now.Hour.ToString("D2");
            minuteTextBox.Text = DateTime.Now.Minute.ToString("D2");
        }
        private void hourTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (hourTextBox.Text.Length > 0)
            {
                bool ok = false;
                int min = 0;
                if (Int32.TryParse(hourTextBox.Text, out min))
                {
                    if (min >= 0 && min < 24)
                        ok = true;
                }
                if (!ok)
                {
                    hourTextBox.TextChanged -= hourTextBox_TextChanged;
                    hourTextBox.Text = lastHourText;
                    hourTextBox.TextChanged += hourTextBox_TextChanged;
                    hourTextBox.SelectionStart = lastHourSelStart;
                    return;
                }
            }
            lastHourText = hourTextBox.Text;
            lastHourSelStart = hourTextBox.SelectionStart;
        }
        private void minuteTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (minuteTextBox.Text.Length > 0)
            {
                bool ok = false;
                int min = 0;
                if (Int32.TryParse(minuteTextBox.Text, out min))
                {
                    if (min >= 0 && min < 60)
                        ok = true;
                }
                if (!ok)
                {
                    minuteTextBox.TextChanged -= minuteTextBox_TextChanged;
                    minuteTextBox.Text = lastMinText;
                    minuteTextBox.TextChanged += minuteTextBox_TextChanged;
                    minuteTextBox.SelectionStart = lastMinSelStart;
                    return;
                }
            }
            lastMinText = minuteTextBox.Text;
            lastMinSelStart = minuteTextBox.SelectionStart;
        }
        private void hourTextBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetBkColor(Colors.White);
            hourTextBox.SelectAll();
            e.Handled = false;
        }
        private void minuteTextBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SetBkColor(Colors.White);
            minuteTextBox.SelectAll();
            e.Handled = false;
        }
        private void hourTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {
                SetBkColor(Colors.White);
                if (hourTextBox.Text.Length > 0 || minuteTextBox.Text.Length > 0)
                {
                    if (hourTextBox.Text.Length == 2)
                    {
                        minuteTextBox.Focus();
                        minuteTextBox.SelectAll();
                    }
                    if (IsValidTime())
                        SetBkColor(ValidColour);
                    else
                        SetBkColor(InvalidColour);

                }
            }
        }
        private void minuteTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
            {

                SetBkColor(Colors.White);
                if (hourTextBox.Text.Length > 0 || minuteTextBox.Text.Length > 0)
                {
                    if (IsValidTime())
                        SetBkColor(ValidColour);
                    else
                        SetBkColor(InvalidColour);

                }
            }
        }
        private void hourTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (hourTextBox.Text.Length > 0)
            {
                int hour = Convert.ToInt32(hourTextBox.Text);
                hourTextBox.Text = String.Format("{0:00}", hour);
            }
            else if (hourTextBox.Text == string.Empty)
            {
                minuteTextBox.Text = string.Empty;
                SetToDefaultValue();
            }
        }
        private void minuteTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (minuteTextBox.Text.Length > 0)
            {
                int minute = Convert.ToInt32(minuteTextBox.Text);
                minuteTextBox.Text = String.Format("{0:00}", minute);
            }
            else if (minuteTextBox.Text == string.Empty)
            {
                hourTextBox.Text = string.Empty;
                SetToDefaultValue();
            }
        }
        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!IsValidKeyPress(e.Key))
                e.Handled = true;
        }


        #endregion
    }
}
