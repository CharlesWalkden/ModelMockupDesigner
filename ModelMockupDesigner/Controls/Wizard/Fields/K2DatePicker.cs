using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ModelMockupDesigner.Controls
{
    public class K2DatePicker : DatePicker
    {
        public K2DatePicker()
        {
            // KeyboardNavigation.SetTabNavigation(this, KeyboardNavigationMode.None);
            AddHandler(FrameworkElement.LoadedEvent, new RoutedEventHandler(ControlIsLoaded));
        }

        private void ControlIsLoaded(object sender, RoutedEventArgs e)
        {
            var box = GetChildOfType<DatePickerTextBox>(this);
            if (box != null)
            {
                // foreach (string str in box.Template.)
                ContentControl partWatermark = box.Template.FindName("PART_Watermark", box) as ContentControl;
                if (partWatermark != null)
                {
                    TextBlock block = new TextBlock();
                    block.Foreground = Brushes.Gray;
                    block.Text = "dd/mm/yyyy";
                    partWatermark.Content = block;
                }

            }
        }
        public static T GetChildOfType<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }
    }
    public class DatePickerDateFormat
    {
        public static readonly DependencyProperty DateFormatProperty = DependencyProperty.RegisterAttached("DateFormat", typeof(string), typeof(DatePickerDateFormat), new PropertyMetadata(OnDateFormatChanged));

        public static string GetDateFormat(DependencyObject dobj)
        {
            return (string)dobj.GetValue(DateFormatProperty);
        }

        public static void SetDateFormat(DependencyObject dobj, string value)
        {
            dobj.SetValue(DateFormatProperty, value);
        }

        private static void OnDateFormatChanged(DependencyObject dobj, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = (DatePicker)dobj;

            datePicker.Dispatcher.BeginInvoke(new Action<DatePicker>(ApplyDateFormat), new object[] { datePicker });
            //Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action<DatePicker>(ApplyDateFormat), datePicker);
        }

        private static void ApplyDateFormat(DatePicker datePicker)
        {
            var binding = new Binding("SelectedDate")
            {
                RelativeSource = new RelativeSource { AncestorType = typeof(DatePicker) },
                Converter = new DatePickerDateTimeConverter(),
                ConverterParameter = new Tuple<DatePicker, string>(datePicker, GetDateFormat(datePicker))
            };

            datePicker.ApplyTemplate();
            var textBox = GetTemplateTextBox(datePicker);
            if (textBox != null)
            {
                textBox.SetBinding(TextBox.TextProperty, binding);
#if WPF
                textBox.PreviewKeyDown -= TextBoxOnPreviewKeyDown;
                textBox.PreviewKeyDown += TextBoxOnPreviewKeyDown;
#else
                textBox.KeyDown -= TextBoxOnPreviewKeyDown;
                textBox.KeyDown += TextBoxOnPreviewKeyDown;
#endif
            }

            datePicker.CalendarOpened -= DatePickerOnCalendarOpened;
            datePicker.CalendarOpened += DatePickerOnCalendarOpened;
            datePicker.SelectedDateChanged -= DatePickerOnSelectedDateChanged;
            datePicker.SelectedDateChanged += DatePickerOnSelectedDateChanged;
        }

        private static TextBox GetTemplateTextBox(DependencyObject obj)
        {
            TextBox tb = null;
            if (obj != null)
            {
#if WPF
                Control control = obj as Control;
                if (control != null)
                    tb = (TextBox)control.Template.FindName("PART_TextBox", control);
#else
                int count = System.Windows.Media.VisualTreeHelper.GetChildrenCount(obj);
                for (int i = 0; i < count; i++)
                {
                    DependencyObject childControl = System.Windows.Media.VisualTreeHelper.GetChild(obj, i);
                    System.Windows.Controls.Primitives.DatePickerTextBox el = childControl as System.Windows.Controls.Primitives.DatePickerTextBox;
                    if (el != null && el.Name == "TextBox")
                    {
                        tb = el;
                        break;
                    }
                    else
                    {
                        tb = GetTemplateTextBox(childControl);
                    }
                }
#endif
            }
            return tb;
        }

        private static DatePicker GetTemplateDatePicker(FrameworkElement control)
        {
            DatePicker datePicker = null;
            if (control != null)
            {
#if WPF
                datePicker = control.TemplatedParent as DatePicker;
#else
                FrameworkElement el = System.Windows.Media.VisualTreeHelper.GetParent(control) as FrameworkElement;
                if (el != null)
                {
                    if (el is DatePicker)
                        datePicker = el as DatePicker;
                    else
                        datePicker = GetTemplateDatePicker(el);
                }
#endif
            }
            return datePicker;
        }

        private static void TextBoxOnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            /* DatePicker subscribes to its TextBox's KeyDown event to set its SelectedDate if Key.Return was
             * pressed. When this happens its text will be the result of its internal date parsing until it
             * loses focus or another date is selected. A workaround is to stop the KeyDown event bubbling up
             * and handling setting the DatePicker.SelectedDate. */

            e.Handled = true;

            var textBox = (TextBox)sender;
            var datePicker = GetTemplateDatePicker(textBox);
            var dateStr = textBox.Text;
            var formatStr = GetDateFormat(datePicker);
            datePicker.SelectedDate = DatePickerDateTimeConverter.StringToDateTime(datePicker, formatStr, dateStr);
        }

        private static void DatePickerOnCalendarOpened(object sender, RoutedEventArgs e)
        {
            /* When DatePicker's TextBox is not focused and its Calendar is opened by clicking its calendar button
             * its text will be the result of its internal date parsing until its TextBox is focused and another
             * date is selected. A workaround is to set this string when it is opened. */

            var datePicker = (DatePicker)sender;
            var textBox = GetTemplateTextBox(datePicker);
            var formatStr = GetDateFormat(datePicker);
            if (datePicker.SelectedDate.HasValue)
            {
                textBox.Text = DatePickerDateTimeConverter.DateTimeToString(formatStr, datePicker.SelectedDate);
            }
        }

        private static void DatePickerOnSelectedDateChanged(object sender, RoutedEventArgs e)
        {
            var datePicker = (DatePicker)sender;
            var textBox = GetTemplateTextBox(datePicker);
            var formatStr = GetDateFormat(datePicker);
            if (datePicker.SelectedDate.HasValue)
            {
                textBox.Text = DatePickerDateTimeConverter.DateTimeToString(formatStr, datePicker.SelectedDate);
            }
        }

        private class DatePickerDateTimeConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var formatStr = ((Tuple<DatePicker, string>)parameter).Item2;
                var selectedDate = (DateTime?)value;
                return DateTimeToString(formatStr, selectedDate);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                var tupleParam = ((Tuple<DatePicker, string>)parameter);
                var dateStr = (string)value;
                return StringToDateTime(tupleParam.Item1, tupleParam.Item2, dateStr);
            }

            public static string DateTimeToString(string formatStr, DateTime? selectedDate)
            {
                return selectedDate.HasValue ? selectedDate.Value.ToString(formatStr) : null;
            }

            public static DateTime? StringToDateTime(DatePicker datePicker, string formatStr, string dateStr)
            {
                DateTime date;
                var canParse = DateTime.TryParseExact(dateStr, formatStr, CultureInfo.CurrentCulture, DateTimeStyles.None, out date);

                if (!canParse)
                    canParse = DateTime.TryParse(dateStr, CultureInfo.CurrentCulture, DateTimeStyles.None, out date);

                return canParse ? date : datePicker.SelectedDate;
            }
        }
    }
}
