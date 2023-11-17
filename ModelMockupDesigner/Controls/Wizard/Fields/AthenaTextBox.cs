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
using System.Windows.Media;

namespace ModelMockupDesigner.Controls
{
    public class AthenaTextBox : TextBox, ICellControl
    {
        public CustomControl ControlModel { get; set; }
        public AthenaTextBox(CustomControl customControl)
        {
            ControlModel = customControl;
            TextChanged += AthenaTextBox_TextChanged;
            GotFocus += AthenaTextBox_GotFocus;
            LostFocus += AthenaTextBox_LostFocus;

            Initialise();
        }
        public AthenaTextBox()
        {
            Initialise(ElementType.ApproxDateTextBox);
        }

        private void Initialise()
        {
            Initialise(ControlModel.ElementType);
        }
        private void Initialise(ElementType elementType)
        {
            switch (elementType)
            {
                case ElementType.DoubleTextBox:
                case ElementType.NumericTextBox:
                    {
                        MinWidth = 50;
                        break;
                    }
                case ElementType.MultiLineTextBox:
                    {
                        MinWidth = 300;
                        if (ControlModel.MinimumLines == 0)
                        {
                            MinLines = 4;
                        }
                        else
                        {
                            MinLines = ControlModel.MinimumLines;
                        }
                        break;
                    }
                case ElementType.ApproxDateTextBox:
                    {
                        MinWidth = 60;
                        break;
                    }
                default:
                    {
                        MinWidth = 200;
                        break;
                    }
            }

            UpdateMinimumSize();
        }

        private void CustomControl_OnControlUpdated(object sender, EventArgs e)
        {
            if (ControlModel.ElementType == ElementType.MultiLineTextBox)
            {
                if (ControlModel.MinimumLines == 0)
                {
                    MinLines = 4;
                }
                else
                {
                    MinLines = ControlModel.MinimumLines;
                }
            }
            Initialise();
        }

        private string lastText;
        private int lastSelStart = 0;
        public string Filter { get; set; }
        private void AthenaTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            bool error = false;
            if (!string.IsNullOrEmpty(Filter) && Text.Length > 0)
            {
                foreach (char c in Text)
                {
                    if (!Filter.Contains(c.ToString()))
                    {
                        error = true;
                    }
                }

            }

            // Make sure its a valid number
            if (!string.IsNullOrEmpty(this.Text) && Text != "-")
            {
                if (ControlModel.ElementType == ElementType.DoubleTextBox)
                {
                    if (Text != ".")
                    {
                        double d = 0.0;
                        if (!double.TryParse(Text, out d))
                        {
                            error = true;
                        }
                    }
                }
                else if (ControlModel.ElementType == ElementType.NumericTextBox)
                {
                    int i = 0;
                    if (!int.TryParse(Text, out i))
                    {
                        error = true;
                    }
                }
            }

            if (error)
            {
                TextChanged -= AthenaTextBox_TextChanged;
                Text = lastText;
                TextChanged += AthenaTextBox_TextChanged;
                SelectionStart = lastSelStart;
                return;
            }

            lastText = Text;
            lastSelStart = SelectionStart;
        }
        private void AthenaTextBox_LostFocus(object sender, RoutedEventArgs e)
        {

            Background = new SolidColorBrush(Colors.White);

        }
        private void AthenaTextBox_GotFocus(object sender, RoutedEventArgs e)
        {

            Background = new SolidColorBrush(Color.FromArgb(255, 152, 251, 152));

        }
        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            UpdateMinimumSize();
        }
        
        private void UpdateMinimumSize()
        {
            if (ControlModel == null)
                return;

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
                TextChanged -= AthenaTextBox_TextChanged;

                string temp = Text;
                Text = lines;
                
                Measure(new Size(1, 1));
                Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

                MinWidth = DesiredSize.Width;
                if (MinLines > 0)
                    MinHeight = DesiredSize.Height;
                Text = temp;

                TextChanged += AthenaTextBox_TextChanged;
            }
        }
        public ElementType ElementType
        {
            get => ControlModel?.ElementType ?? ElementType.Unknown;
            set
            {
                //VerticalContentAlignment = VerticalAlignment.Center;
                Filter = null;
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                //Alignment = ContentAlignment.MiddleLeft;
                AcceptsReturn = false;

                ElementType elementType = ControlModel?.ElementType ?? value;
                switch (elementType)
                {
                    case ElementType.TextBox:
                        // Prevent the control from increasing in size as text is typed into it
                        TextWrapping = System.Windows.TextWrapping.NoWrap;
                        break;
                    case ElementType.NumericTextBox:
                        //Alignment = ContentAlignment.MiddleRight;
                        Filter = "01234567890-";
                        break;
                    case ElementType.MultiLineTextBox:
                        VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                        AcceptsReturn = true;
                        VerticalContentAlignment = VerticalAlignment.Top;
                        break;
                    case ElementType.DoubleTextBox:
                        //Alignment = ContentAlignment.MiddleRight;
                        Filter = "01234567890.-";
                        break;
                }
            }
        }
        public BaseModel Model => ControlModel;
        public bool DisplayGroupbox => ControlModel.DisplayGroupbox;
        public void Unload()
        {
            ControlModel = null;
        }
    }
}
