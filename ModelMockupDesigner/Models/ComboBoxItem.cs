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

#pragma warning disable CS8618 // This object will always have a value when used!.
        public object Value { get; set; }
#pragma warning restore CS8618 

        public override string ToString()
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
