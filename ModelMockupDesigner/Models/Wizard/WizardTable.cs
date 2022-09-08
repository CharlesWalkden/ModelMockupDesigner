using ModelMockupDesigner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelMockupDesigner.Models.Wizard
{
    public class WizardTable : BaseModel, ICellContent
    {
        public WizardPanel Parent { get; set; }


        public int Row { get; set; }
        public int Column { get; set; }
        public List<WizardCell> Cells { get; set; }

        public WizardTable(WizardPanel parent)
        {
            Parent = parent;
            Cells = new List<WizardCell>();
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
