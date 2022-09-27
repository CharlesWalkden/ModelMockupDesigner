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
    public class DynamicWizardColumn : BaseModel, IPropertyEditor
    {
        public DynamicWizardSection? Parent { get; set; }

        public int Order { get; set; } = 0;
        public List<DynamicWizardPanel> WizardPanels { get; set; }

        public string HeaderName => "Column";

        public ElementType ElementType => ElementType.Column;

        public DynamicWizardColumn(DynamicWizardSection? parent)
        {
            Parent = parent;
            WizardPanels = new List<DynamicWizardPanel>();
        }

        public void CreateNew()
        {
            if (Parent != null)
                Order = Parent.WizardColumns.Count;

            DynamicWizardPanel wizardPanel = new(this);
            wizardPanel.CreateNew();

            WizardPanels.Add(wizardPanel);
        }
        public List<string> GetEditableProperties()
        {
            List<string> properties = new()
            {
                new string("Name")
            };

            return properties;
        }


        #region Xml

        public override void LoadFromXml(XmlNode node)
        {
            throw new NotImplementedException();
        }

        public override XmlNode? ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
