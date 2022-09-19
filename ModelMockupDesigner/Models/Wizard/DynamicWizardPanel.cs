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
    public class DynamicWizardPanel : BaseModel, ICellParent 
    {
        public DynamicWizardColumn Parent { get; set; }

        public int Order { get; set; } = 0;
        public List<DynamicWizardCell> Cells { get; set; }

        public DynamicWizardPanel(DynamicWizardColumn parent)
        {
            Parent = parent;
            Cells = new List<DynamicWizardCell>();
        }

        public void CreateNew()
        {
            Order = Parent.WizardPanels.Count + 1;

            DynamicWizardCell wizardCell = new(this);

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
