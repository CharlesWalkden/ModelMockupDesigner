using ModelMockupDesigner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.Interfaces
{
    public interface IPropertyEditor
    {
        public string HeaderName { get; }
        public ElementType ElementType { get; }
        public List<string> GetEditableProperties();
    }
}
