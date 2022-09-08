using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ModelMockupDesigner.Interfaces
{
    public interface IIsSelectable
    {
        bool IsSelected { get; set; }
        BaseModel? Model { get; }
        void Delete(bool unselect);
    }
}
