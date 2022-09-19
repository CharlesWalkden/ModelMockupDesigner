using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelMockupDesigner.Models
{
    public class DynamicWizardSection : BaseModel
    {
        public DynamicWizard Parent { get; set; }

        public List<DynamicWizardColumn> WizardColumns { get; set; }

        public DynamicWizardSection(DynamicWizard parent)
        {
            Parent = parent;
            Parent.Sections.Add(this);
            WizardColumns = new List<DynamicWizardColumn>();
            CreateNew();
        }

        public void CreateNew()
        {
            DynamicWizardColumn wizardColumn = new(this);
            wizardColumn.CreateNew();

            WizardColumns.Add(wizardColumn);
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
