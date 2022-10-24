using ModelMockupDesigner.Controls;
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
    public class DynamicWizardTable : BaseModel, ICellParent, ICellControl, IPropertyEditor
    {
        public DynamicWizardCell Parent { get; set; }

        public List<DynamicWizardCell> Cells { get; set; }

        public DynamicWizardTable(DynamicWizardCell parent)
        {
            Parent = parent;
            Cells = new List<DynamicWizardCell>();
        }

        public void CreateNew()
        {
            DynamicWizardCell wizardCell = new DynamicWizardCell(this);

            Cells.Add(wizardCell);
        }
        public ElementType ElementType => ElementType.Table;
        public string HeaderName => "Table";

        public Dictionary<string, string> GetEditableProperties()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("Name", "Name");

            return properties;
        }

        public BaseModel Model => this;

        #region Xml

        public override void LoadFromXml(XmlNode node)
        {
            throw new NotImplementedException();
        }

        public override XmlNode ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Not Used

        public void Delete(EditorCell cell)
        {
            throw new NotImplementedException();
        }

       

        #endregion
    }
}
