using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
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
        public event EventHandler<int>? OnColumnCountChanged;

        public event EventHandler? OnContentUpdated;
        public CustomControl(ElementType elementType, List<string>? listOptions = null)
        {
            ElementType = elementType;
            ListOptions = listOptions;
        }

        // Text for label
        public string? Text
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
        private string? text { get; set; }

        public List<string>? ListOptions { get; set; }
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

        public int MinimumCharacters { get; set; }
        private int minimumCharacters { get; set; }
        public int MinimumLines { get; set; }
        private int minimumLines { get; set; }
        public int DoublePrecision { get; set; }

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

            return properties;
        }
        public void StoreListOption(List<string> listOptions)
        {
            ListOptions = listOptions;
        }
    }
}
