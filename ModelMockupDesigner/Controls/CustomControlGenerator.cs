﻿using ModelMockupDesigner.Controls;
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
                case ElementType.DateTime:
                    {
                        controlElement = new AthenaDateTime(control);
                        break;
                    }
                case ElementType.Date:
                    {
                        controlElement = new AthenaDate(control);
                        break;
                    }
                case ElementType.ApproxDate:
                    {
                        controlElement = new AthenaApproxDate(control);
                        break;
                    }
                case ElementType.Time:
                    {
                        controlElement = new AthenaTime(control);
                        break;
                    }
                case ElementType.RadioList:
                    {
                        controlElement = new AthenaRadioList(control);
                        break;
                    }
                case ElementType.Button:
                case ElementType.CheckBox:
                    {
                        controlElement = new AthenaCheckBox(control);
                        break;
                    }
                case ElementType.Label:
                    {
                        controlElement = new AthenaLabel(control);
                        break;
                    }
                case ElementType.TextBox:
                case ElementType.MultiLineTextBox:
                case ElementType.NumericTextBox:
                case ElementType.DoubleTextBox:
                    {
                        controlElement = new AthenaTextBox(control);
                        break;
                    }
            }

            return Task.FromResult(controlElement);
        }
    }
}
