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

namespace ModelMockupDesigner.Controls
{
    public class AthenaTextBox : TextBox, ICellControl
    {
        public CustomControl ControlModel { get; set; }
        public AthenaTextBox(CustomControl customControl)
        {
            ControlModel = customControl;
            this.TextChanged += AthenaTextBox_TextChanged;
        }

        private string? lastText;
        private int lastSelStart = 0;
        public string? Filter { get; set; }
        private void AthenaTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool error = false;
            if (!String.IsNullOrEmpty(Filter) && this.Text.Length > 0)
            {
                foreach (char c in this.Text)
                {
                    if (!Filter.Contains(c.ToString()))
                    {
                        error = true;
                    }
                }

            }

            // Make sure its a valid number
            if (!string.IsNullOrEmpty(this.Text) && this.Text != "-")
            {
                if (ControlModel.ElementType == ElementType.DoubleTextBox)
                {
                    if (this.Text != ".")
                    {
                        double d = 0.0;
                        if (!double.TryParse(this.Text, out d))
                        {
                            error = true;
                        }
                    }
                }
                else if (ControlModel.ElementType == ElementType.NumericTextBox)
                {
                    int i = 0;
                    if (!int.TryParse(this.Text, out i))
                    {
                        error = true;
                    }
                }
            }

            if (error)
            {
                this.TextChanged -= AthenaTextBox_TextChanged;
                this.Text = lastText;
                this.TextChanged += AthenaTextBox_TextChanged;
                this.SelectionStart = lastSelStart;
                return;
            }

            lastText = this.Text;
            lastSelStart = this.SelectionStart;
        }
        private void UpdateMinimumSize()
        {
            if (ControlModel.MinimumCharacters > 0)
            {
                string str = string.Empty;
                for (int i = 0; i < ControlModel.MinimumCharacters; i++)
                {
                    str += "W";
                }
                string lines = str;
                for (int i = 1; i < MinLines; i++)
                {
                    lines += Environment.NewLine + str;
                }
                string temp = this.Text;
                this.Text = lines;
                this.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                this.MinWidth = this.DesiredSize.Width;
                if (MinLines > 1)
                    this.MinHeight = this.DesiredSize.Height;
                this.Text = temp;
            }
        }
        public ElementType ElementType
        {
            get => ControlModel.ElementType;
            set
            {
                //VerticalContentAlignment = VerticalAlignment.Center;
                Filter = null;
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                //Alignment = ContentAlignment.MiddleLeft;
                AcceptsReturn = false;
                switch (ControlModel.ElementType)
                {
                    case ElementType.TextBox:
                        // Prevent the control from increasing in size as text is typed into it
                        this.TextWrapping = System.Windows.TextWrapping.NoWrap;
                        break;
                    case ElementType.NumericTextBox:
                        //Alignment = ContentAlignment.MiddleRight;
                        Filter = "01234567890-";
                        break;
                    case ElementType.MultiLineTextBox:
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        base.AcceptsReturn = true;
                        //VerticalContentAlignment = VerticalAlignment.Top;
                        break;
                    case ElementType.DoubleTextBox:
                        //Alignment = ContentAlignment.MiddleRight;
                        Filter = "01234567890.-";
                        break;
                }
            }
        }
        public BaseModel? Model => ControlModel;
    }
}
