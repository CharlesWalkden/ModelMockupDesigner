using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
    /// Interaction logic for AthenaTime.xaml
    /// </summary>
    public partial class AthenaTime : UserControl, ICellControl
    {
        public CustomControl ControlModel { get; set; }
        public ElementType ElementType => ControlModel.ElementType;
        public BaseModel Model => ControlModel;
        public bool DisplayGroupbox => ControlModel.DisplayGroupbox;
        public void Unload()
        {
            DataContext = null;
            ControlModel = null;
        }

        private static Color ValidColour = Color.FromArgb(255, 1, 243, 62);
        private static Color InvalidColour = Color.FromArgb(255, 255, 72, 72);

        public AthenaTime(CustomControl customControl)
        {
            Initialise();
            DataContext = customControl;
            ControlModel = customControl;
            //ControlModel.DisplayChanged += ControlModel_DisplayChanged;
            //Unloaded += AthenaTime_Unloaded;

            SetValue("NOW");
            //ShowGroupBox = controlModel.DisplayGroupbox;
            //Title = ControlModel.GroupBoxTitle;
        }

        //private void AthenaTime_Unloaded(object sender, RoutedEventArgs e)
        //{
        //    ControlModel.DisplayChanged -= ControlModel_DisplayChanged;
        //}

        //private void ControlModel_DisplayChanged(object sender, GroupBoxDisplayChangedEventArgs e)
        //{
        //    //ShowGroupBox = e.Display;
        //    Title = e.GroupBoxTitle;
        //    if (e.Display)
        //    {
        //        HorizontalAlignment = e.HorizontalAlignment.ToXaml();
        //        VerticalAlignment = e.VerticalAlignment.ToXaml();
        //    }
        //}

        private void Initialise()
        {

            InitializeComponent();
            increaseTimeBtn.Visibility = Visibility.Visible;
            decreaseTimeBtn.Visibility = Visibility.Visible;
            increaseTimeBtn.Width = 20;
            decreaseTimeBtn.Width = 20;
            LayoutRoot.Margin = new Thickness(5, -8, 5, 0);
            LayoutRoot.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Auto);
            horizontalContainer.Margin = new Thickness(2, 0, 2, 0);

        }
        


        private void increaseTimeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!currentDateTime.HasValue)
                SetValue(DateTime.UtcNow);
            else
            {
                DateTime dt = currentDateTime.Value;
                if (hourTextBox.IsFocused)
                {
                    dt = currentDateTime.Value.AddHours(1);
                }
                else
                {
                    dt = currentDateTime.Value.AddMinutes(1);
                }
                DateTime cutOff = DateTime.UtcNow;
                if (dt > cutOff)
                    SetValue(cutOff);
                else
                    SetValue(dt);
            }
        }

        private void decreaseTimeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!currentDateTime.HasValue)
                SetValue(DateTime.UtcNow);
            else
            {
                DateTime dt = currentDateTime.Value;
                if (hourTextBox.IsFocused)
                {
                    dt = currentDateTime.Value.AddHours(-1);
                }
                else
                {
                    dt = currentDateTime.Value.AddMinutes(-1);
                }
                DateTime cutOff = DateTime.UtcNow.AddHours(-23);
                if (dt < cutOff)
                    SetValue(cutOff);
                else
                    SetValue(dt);
            }
        }

        public string Title
        {
            get;
            set;
        }
        private DateTime? Date;
        public bool SetValue(object o)
        {
            try
            {
                if (o != null && o is DateTime)
                {
                    DateTime dt = ((DateTime)o).ToLocalTime();
                    Date = dt.Date;
                    hourTextBox.Text = dt.Hour.ToString("D2");
                    minuteTextBox.Text = dt.Minute.ToString("D2");

                    IsValidTime();
                }
                else
                {
                    if (o is string && o.Equals("NOW"))
                    {
                        DateTime dt = DateTime.Now;
                        Date = dt.Date;
                        hourTextBox.Text = dt.Hour.ToString("D2");
                        minuteTextBox.Text = dt.Minute.ToString("D2");

                        IsValidTime();
                    }
                    else
                    {
                        hourTextBox.Text = string.Empty;
                        minuteTextBox.Text = string.Empty;
                    }
                }
            }
            catch (Exception)
            {

            }
            return true;
        }
        
        public object GetValue()
        {
            if (currentDateTime != null)
                return currentDateTime.Value;
            return null;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            IsValidTime();
        }

        private bool timeIsGreaterThan23HoursOld;
        private Nullable<DateTime> currentDateTime
        {
            get
            {
                Nullable<DateTime> dt = null;

                try
                {
                    timeIsGreaterThan23HoursOld = false;
                    int hour = 0;
                    if (Int32.TryParse(hourTextBox.Text, out hour))
                    {
                        int minute = 0;
                        if (Int32.TryParse(minuteTextBox.Text, out minute))
                        {
                            if (!Date.HasValue)
                                Date = DateTime.Now.Date;
                            DateTime localTime = new DateTime(Date.Value.Year, Date.Value.Month, Date.Value.Day, hour, minute, 0, DateTimeKind.Local);
                            dt = localTime.ToUniversalTime();

                            // cant enter future times, so must be for yesterday.
                            if (dt > DateTime.UtcNow)
                            {

                                TimeSpan daylightDelta = TimeZone.CurrentTimeZone.GetDaylightChanges(localTime.Year).Delta;
                                //ER26422, if the time is the future and we are at end of DST, then assume its actually 1 hour back
                                if (!localTime.IsDaylightSavingTime() && localTime.Subtract(daylightDelta).IsDaylightSavingTime())
                                {
                                    dt = dt.Value.Subtract(daylightDelta);
                                }
                                else
                                {
                                    dt = dt.Value.AddDays(-1);
                                }
                            }
                            DateTime lowerLimit = DateTime.UtcNow.AddHours(-23);
                            lowerLimit = new DateTime(lowerLimit.Year, lowerLimit.Month, lowerLimit.Day, lowerLimit.Hour, lowerLimit.Minute, 0, DateTimeKind.Utc);
                            // can't enter times > 23 hours ago.
                            if (dt < lowerLimit)
                            {

                                timeIsGreaterThan23HoursOld = true;
                                dt = null;
                            }
                        }
                    }

                }
                catch (Exception)
                {
                }
                return dt;
            }
        }
        private void hourTextBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
                hourTextBox.SelectAll();
                e.Handled = false;
            
        }
        private void minuteTextBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
                minuteTextBox.SelectAll();
                e.Handled = false;
            
        }
        private void hourTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            
                //SetBkColor(Colors.White);
                if (hourTextBox.Text.Length > 0 || minuteTextBox.Text.Length > 0)
                {
                    if (hourTextBox.Text.Length == 2)
                    {
                        minuteTextBox.Focus();
                        minuteTextBox.SelectAll();
                    }

                }

            
        }
        private void minuteTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            
                //SetBkColor(Colors.White);
                if (hourTextBox.Text.Length > 0 || minuteTextBox.Text.Length > 0)
                {

                }
                IsValidTime();
            
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
        private bool IsValidTime()
        {
            //currentDateLabel.Background = Brushes.Transparent;
            currentDateLabel.Background = Brushes.White;
            currentDateLabel.Foreground = Brushes.Black;
            //currentDateLabel.Margin = new Thickness(-40, 0, -40, 0);
            
            bool valid = false;
            int hour = -1;
            int minute = -1;
            if (hourTextBox.Text.Length > 0 || minuteTextBox.Text.Length > 0)
            {
                if (hourTextBox.Text.Length > 0)
                    hour = Convert.ToInt32(hourTextBox.Text);
                if (minuteTextBox.Text.Length > 0)
                    minute = Convert.ToInt32(minuteTextBox.Text);

                //currentDateLabel.Height = Double.NaN;

                if (currentDateTime.HasValue)
                {
                    valid = true;
                    DateTime localTime = currentDateTime.Value.ToLocalTime();
                    bool isDST = localTime.IsDaylightSavingTime();
                    string dstLabel = string.Empty;
                    if (isDST != localTime.AddDays(1).IsDaylightSavingTime() || isDST != localTime.AddDays(-1).IsDaylightSavingTime())
                    {
                        if (isDST)
                        {
                            dstLabel = " (BST)";
                        }
                        else
                        {
                            dstLabel = " (GMT)";
                        }
                    }

                    if (localTime.Date == DateTime.Now.Date)
                    {
                        currentDateLabel.Text = "Today" + dstLabel;
                        currentDateLabel.Margin = new Thickness(0, 1, 0, 0);

                        
                    }
                    else if ((DateTime.Now.Date - localTime.Date).Days == 1)
                    {
                        currentDateLabel.Text = "Yesterday" + dstLabel;
                        currentDateLabel.Margin = new Thickness(0, 1, 0, 0);

                        
                    }
                    else
                    {
                        currentDateLabel.Text = localTime.Date.ToString("dd - MMM - yyyy") + dstLabel;
                    }
                }
                else
                {
                    if (timeIsGreaterThan23HoursOld)
                    {
                        currentDateLabel.Text = "Time cannot be > 23 hours ago";
                        currentDateLabel.Margin = new Thickness(-32, 1, -48, 0);

                        
                    }

                    else
                    {
                        currentDateLabel.Text = "Please enter a Valid Time";
                        currentDateLabel.Margin = new Thickness(-32, 1, -32, 0);

                        

                    }

                    currentDateLabel.Background = Brushes.Red;
                    currentDateLabel.Foreground = Brushes.White;
                }
            }
            else
            {
                // no date set, ignore..
                currentDateLabel.Text = string.Empty;
                //currentDateLabel.Height = 0;
                //currentDateLabel.Margin = new Thickness(0);
            }
            return valid;
        }
        private void SetBkColor(Color col)
        {
            SolidColorBrush brush = new SolidColorBrush(col);
            hourTextBox.Background = brush;
            minuteTextBox.Background = brush;
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            
                TextBox tb = sender as TextBox;
                if (tb != null)
                    tb.Background = new SolidColorBrush(Color.FromArgb(255, 152, 251, 152));
            
        }

        private void textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            
                TextBox tb = sender as TextBox;
                if (tb != null)
                {
                    tb.Background = new SolidColorBrush(Colors.White);
                    if (tb.Text.Length == 1)
                    {
                        tb.Text = "0" + tb.Text;
                    }
                }
            
        }

        
    }
}
