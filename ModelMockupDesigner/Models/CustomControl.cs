using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.WizardPreview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelMockupDesigner.Models
{
    public class CustomControl : BaseControlModel, ICellControl
    {
        public event EventHandler<int> OnColumnCountChanged;

        public CustomControl(ElementType elementType, bool scaleDown = false, List<string> listOptions = null)
        {
            ElementType = elementType;
            ListOptions = listOptions;
            ScaleDown = scaleDown;

            if (elementType == ElementType.DateTime || elementType == ElementType.RadioList)
            {
                DisplayGroupbox = true;
                GroupBoxTitle = "TODO: Set GroupBox Title";
            }
        }

        // Text for label
        public string Text
        {
            get => text;
            set
            {
                if (text == value)
                    return;

                text = value;
                OnPropertyChanged(nameof(Text));
            }
        }
        private string text { get; set; }

        public List<string> ListOptions { get; set; }
        // Used for radio lists - default to 1
        public int ColumnCount
        {
            get => columnCount;
            set
            {
                if (columnCount == value)
                    return;
                
                columnCount = value;
                OnPropertyChanged(nameof(ColumnCount));
                OnColumnCountChanged?.Invoke(this, value);
            }
        }
        private int columnCount { get; set; } = 1;

        public int MinimumCharacters
        {
            get => minimumCharacters;
            set
            {
                if (minimumCharacters == value)
                    return;

                minimumCharacters = value;
                OnPropertyChanged(nameof(MinimumCharacters));
                WizardPreviewManager.UpdatePreview();
            }
        }
        private int minimumCharacters { get; set; }
        public int MinimumLines
        {
            get => minimumLines;
            set
            {
                if (minimumLines == value)
                    return;

                minimumLines = value;
                OnPropertyChanged(nameof(MinimumLines));
                WizardPreviewManager.UpdatePreview();
            }
        }
        // Default min lines to 0 which will default to 4.
        private int minimumLines { get; set; } = 0;
        public int DoublePrecision
        {
            get => doublePrecision;
            set
            {
                if (doublePrecision == value)
                    return;

                doublePrecision = value;
                OnPropertyChanged(nameof(DoublePrecision));
                WizardPreviewManager.UpdatePreview();
            }
        }
        private int doublePrecision { get; set; }

        public bool ScaleDown
        {
            get => scaleDown;
            set
            {
                if (scaleDown == value)
                    return;

                scaleDown = value;
                DisplayGroupbox = true;
                GroupBoxTitle = ElementType.ToString();
            }
        }
        private bool scaleDown;

        public override Dictionary<string, string> GetEditableProperties()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("Name", "Name");
            properties.Add("Horizontal", "HorizontalAlignment");
            properties.Add("Vertical", "VerticalAlignment");
            
            if (ElementType == ElementType.RadioList)
            {
                properties.Add("Columns", "ColumnCount");
            }
            if (ElementType == ElementType.Label || ElementType == ElementType.CheckBox || ElementType == ElementType.Button)
            {
                properties.Add("Text", "Text");
            }
            if (ElementType == ElementType.TextBox || ElementType == ElementType.NumericTextBox)
            {
                properties.Add("MinCharacters", "MinimumCharacters");
            }
            if (ElementType == ElementType.MultiLineTextBox)
            {
                properties.Add("MinCharacters", "MinimumCharacters");
                properties.Add("MinLines", "MinimumLines");
            }
            if (ElementType == ElementType.DoubleTextBox)
            {
                properties.Add("MinCharacters", "MinimumCharacters");
                properties.Add("DoublePrecision", "DoublePrecision");
            }

            return properties;
        }
        public void StoreListOption(List<string> listOptions)
        {
            ListOptions = listOptions;
        }
    }
}
