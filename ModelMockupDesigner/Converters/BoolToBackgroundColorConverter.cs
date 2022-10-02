using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ModelMockupDesigner.Converters
{
    public class BoolToBackgroundColorConverter : BaseValueConverter<BoolToBackgroundColorConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var converter = new BrushConverter();

            if ((bool)value)
            {
                var brush = (Brush)converter.ConvertFromString("Orange");
                return brush;
            }
            else
            {
                var brush = Application.Current.Resources["EditorDarkGrayBrush"] ;
                return brush;
            }
            
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
