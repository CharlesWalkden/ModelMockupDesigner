using ModelMockupDesigner.Converters;
using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace ModelMockupDesigner.Controls
{
    public class AthenaLabel : Label, ICellControl
    {
        public CustomControl ControlModel { get; set; }

        protected TextBlock TextBlock;

        public AthenaLabel(CustomControl controlModel)
        {
            ControlModel = controlModel;

            this.FontSize = 13.333;
            this.Background = new SolidColorBrush(Colors.Transparent);
            TextBlock = new TextBlock();
            TextBlock.TextWrapping = System.Windows.TextWrapping.Wrap;

            if (string.IsNullOrWhiteSpace(ControlModel.Text))
            {
                ControlModel.Text = "TODO: Set Text";
            }

            // Setup binding for the label text.
            Binding textBinding = new Binding(nameof(ControlModel.Text)) { Source = ControlModel, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged};
            TextBlock.SetBinding(TextBlock.TextProperty, textBinding);

            // Binding for horizontal alignment
            Binding horizontalBinding = new Binding(nameof(ControlModel.HorizontalAlignment)) { Source = ControlModel, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Converter = new ToXamlConverter() };
            SetBinding(Label.HorizontalAlignmentProperty, horizontalBinding);

            // Binding for vertical alignment
            Binding verticalBinding = new Binding(nameof(ControlModel.VerticalAlignment)) { Source = ControlModel, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged, Converter = new ToXamlConverter() };
            SetBinding(Label.VerticalAlignmentProperty, verticalBinding);

            this.Content = TextBlock;
            TextBlock.Margin = new System.Windows.Thickness(0, -2, 0, -2);
            TextBlock.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 60, 60, 60));
        }

        #region Interface

        public ElementType ElementType => ElementType.Label;

        public BaseModel? Model => ControlModel;

        public bool DisplayGroupbox => ControlModel.DisplayGroupbox;

        #endregion
    }
}
