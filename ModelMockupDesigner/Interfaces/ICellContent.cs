using ModelMockupDesigner.Controls;
using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Models;
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
        List<DynamicWizardCell> Cells { get; set; }
        void Delete(EditorCell cell);
    }
}
