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
    public class CustomControl : BaseControlModel, ICellControl
    {
        
        public CustomControl(ElementType elementType)
        {
            ElementType = elementType;
        }

        // Used for radion lists - default to 1
        public int ColumnCount { get; set; } = 1;


    }
}
