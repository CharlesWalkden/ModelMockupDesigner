using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelMockupDesigner.Models
{
    public class BaseControlModel : BaseModel, ICellControl
    {
        #region GroupBox

        public bool ShowGroupBox { get; set; } = true;
        public string? GroupBoxTitle { get; set; }
        public HorizontalAlignmentTypes GroupBoxHorizontalAlignment { get; set; }
        public VerticalAlignmentTypes GroupBoxVerticalAlignment { get; set; }

        #endregion

        #region Content

        public HorizontalAlignmentTypes HorizontalAlignment { get; set; }
        public VerticalAlignmentTypes VerticalAlignment { get; set; }

        #endregion



        #region Interface

        public ElementType ElementType { get; set; } = ElementType.Unknown;

        public BaseModel? Model => this;

        #endregion

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
