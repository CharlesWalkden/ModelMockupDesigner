using ModelMockupDesigner.Controls;
using ModelMockupDesigner.Enums;
using ModelMockupDesigner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.Interfaces
{
    public interface ICellParent 
    {
        ElementType ElementType { get; }
        void Delete(EditorCell cell);
    }
}
