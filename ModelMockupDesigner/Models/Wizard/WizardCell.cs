using ModelMockupDesigner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace ModelMockupDesigner.Models.Wizard
{
    public class WizardCell : BaseModel, ICellContent
    {
        public WizardPanel Parent { get; set; }

        public int Row { get; set; }
        public int Column { get; set; }

        public FrameworkElement? Content { get; set; }

        public WizardCell(WizardPanel parent)
        {
            Parent = parent;
        }

        public void CreateNew()
        {

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
