using ModelMockupDesigner.Controls;
using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelMockupDesigner.Models.Wizard
{
    public class WizardPanel : BaseModel, ICellParent
    {
        public WizardColumn Parent { get; set; }

        public int Order { get; set; } = 0;
        public List<WizardCell> Cells { get; set; }

        public WizardPanel(WizardColumn parent)
        {
            Parent = parent;
            Cells = new List<WizardCell>();
        }

        public void CreateNew()
        {
            Order = Parent.WizardPanels.Count + 1;

            WizardCell wizardCell = new(this);

            Cells.Add(wizardCell);
        }

        public ElementType ElementType { get => ElementType.Panel; }

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

        #region Not Used

        public void Delete(EditorCell cell)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
