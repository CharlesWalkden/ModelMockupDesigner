using ModelMockupDesigner.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.Converters
{
    public class ToXamlConverter : BaseValueConverter<ToXamlConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is HorizontalAlignmentTypes horizontal)
            {
                return horizontal.ToXaml();
            }
            else if (value is VerticalAlignmentTypes vertical)
            {
                return vertical.ToXaml();
            }
            else
            {
                return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
