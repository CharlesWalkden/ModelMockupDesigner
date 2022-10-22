using ModelMockupDesigner.Controls;
using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ModelMockupDesigner.Controls
{
    public class CustomControlGenerator
    {
        static public Task<FrameworkElement> GetControl(CustomControl control)
        {
            FrameworkElement controlElement = null;

            switch (control.ElementType)
            {
                case ElementType.YesNo:
                    {
                        controlElement = new AthenaYesNoControl(control);
                        break;
                    }
            }

            return Task.FromResult(controlElement);
        }
    }
}
