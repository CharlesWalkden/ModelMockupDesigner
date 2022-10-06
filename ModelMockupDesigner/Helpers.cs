using ModelMockupDesigner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ModelMockupDesigner
{
    public static class Helpers
    {
        public static HorizontalAlignment ToXaml(this HorizontalAlignmentTypes k2Type) { return (HorizontalAlignment)k2Type; }
        public static VerticalAlignment ToXaml(this VerticalAlignmentTypes k2Type) { return (VerticalAlignment)k2Type; }
    }
}
