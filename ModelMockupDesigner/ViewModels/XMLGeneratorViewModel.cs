using ModelMockupDesigner.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.ViewModels
{
    internal class XMLGeneratorViewModel : BaseViewModel
    {
        #region StringTextboxes
        public string FieldNameEntered { get; set; }

        public string FieldTypeDropdown { get; set; }

        public string DatabaseNameEntered { get; set; }

        public string SizeTextboxEntered { get; set; }

        public string UnitsTextboxEntered { get; set; }

        public string ListItemsEntered { get; set; }

        public string XMLOutputTextbox { get; set; }
        #endregion
        public List<string> EditorFieldList = new List<string>();
    }
}
