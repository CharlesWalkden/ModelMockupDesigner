using ModelMockupDesigner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.Models
{
    public class ComboBoxItem : BaseViewModel
    {
        public string? Text { get; set; }

        public object? Value { get; set; }

        public override string? ToString()
        {
            if (Text == null || string.IsNullOrEmpty(Text))
            {
                //return "Not Set";
                return Value?.ToString();
            }

            return Text.ToString();
        }
    }
}
