using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelMockupDesigner.Models
{
    public class WizardSection : BaseModel
    {
        public Wizard Parent { get; set; }

        public List<WizardColumn> WizardColumns { get; set; }

        public WizardSection(Wizard parent)
        {
            Parent = parent;
            Parent.Sections.Add(this);
            WizardColumns = new List<WizardColumn>();
            CreateNew();
        }

        public void CreateNew()
        {
            WizardColumn wizardColumn = new(this);
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
