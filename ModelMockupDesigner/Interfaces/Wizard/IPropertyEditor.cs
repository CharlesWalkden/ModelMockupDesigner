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
        string HeaderName { get; }
        ElementType ElementType { get; }
        Dictionary<string, string> GetEditableProperties();
    }
}
