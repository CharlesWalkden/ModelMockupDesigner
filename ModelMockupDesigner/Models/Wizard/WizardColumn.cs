using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelMockupDesigner.Models
{
    public class WizardColumn : BaseModel
    {
        public WizardSection? Parent { get; set; }

        public int Order { get; set; } = 0;
        public List<WizardPanel> WizardPanels { get; set; }  

        public WizardColumn(WizardSection? parent)
        {
            Parent = parent;
            WizardPanels = new List<WizardPanel>();
        }

        public void CreateNew()
        {
            if (Parent != null)
                Order = Parent.WizardColumns.Count;

            WizardPanel wizardPanel = new(this);
            wizardPanel.CreateNew();

            WizardPanels.Add(wizardPanel);
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
