using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.Interfaces
{
    public interface IGroupBoxContent
    {
        bool DisplayGroupbox { get; set; } 
        string GroupBoxTitle { get; set; }
        HorizontalAlignmentTypes GroupHorizontalAlignment { get; set; } 
        VerticalAlignmentTypes GroupVerticalAlignment { get; set; }
        event EventHandler<GroupBoxDisplayChangedEventArgs> DisplayChanged;
    }
}
