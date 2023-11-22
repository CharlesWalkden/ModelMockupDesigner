using ModelMockupDesigner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.Data
{
    public class ElementDefinition
    {
        public bool Required { get; set; }
        public bool Scale { get; set; }
        public DesignGroup DesignGroup { get; set; }
        public List<ElementType> InterchangeableTypes { get; set; }
    }

    public enum DesignGroup
    {
        Fields,
        Layout
    }
}
