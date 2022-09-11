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
    public class WizardTable : BaseModel, ICellParent, ICellControl
    {
        public WizardCell? Parent { get; set; }

        public List<WizardCell> Cells { get; set; }

        public WizardTable(WizardCell? parent)
        {
            Parent = parent;
            Cells = new List<WizardCell>();
        }

        public void CreateNew()
        {
            WizardCell wizardCell = new(this);

            Cells.Add(wizardCell);
        }
        public ElementType ElementType { get => ElementType.Table; }

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
        public BaseModel? Model => throw new NotImplementedException();

        #endregion
    }
}
