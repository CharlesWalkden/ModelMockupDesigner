using ModelMockupDesigner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelMockupDesigner.Models.Wizard
{
    public class WizardPanel : BaseModel
    {
        public WizardColumn Parent { get; set; }

        public int Order { get; set; } = 1;
        public List<ICellContent> Cells { get; set; }

        public WizardPanel(WizardColumn parent)
        {
            Parent = parent;
            Cells = new List<ICellContent>();
        }

        public void CreateNew()
        {
            Order = Parent.WizardPanels.Count + 1;

            WizardCell wizardCell = new(this);
            wizardCell.CreateNew(); // Dont think I need this.

            Cells.Add(wizardCell);
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
