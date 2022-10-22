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
    

    public abstract class BaseControlModel : BaseModel, ICellControl, IPropertyEditor
    {
        #region Content

        public HorizontalAlignmentTypes HorizontalAlignment
        {
            get => horizontalAlignment;
            set
            {
                if (horizontalAlignment == value)
                    return;

                horizontalAlignment = value;
                OnPropertyChanged(nameof(HorizontalAlignment));
            }
        }
        private HorizontalAlignmentTypes horizontalAlignment { get; set; }
        public VerticalAlignmentTypes VerticalAlignment
        {
            get => verticalAlignment;
            set
            {
                if (verticalAlignment == value)
                    return;

                verticalAlignment = value;
                OnPropertyChanged(nameof(VerticalAlignment));
            }
        }
        private VerticalAlignmentTypes verticalAlignment { get; set; }

        #endregion

        #region Interface

        public ElementType ElementType { get; set; }

        public BaseModel Model => this;

        public string HeaderName => ElementType.ToString();

        #endregion

        #region Xml
        public override void LoadFromXml(XmlNode node)
        {
            throw new NotImplementedException();
        }

        public override XmlNode ToXml()
        {
            throw new NotImplementedException();
        }

        public abstract Dictionary<string, string> GetEditableProperties();

        #endregion

    }
}
