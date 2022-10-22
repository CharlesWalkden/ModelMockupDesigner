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
        BaseModel Model { get; }
        void Unselect();
        void DeleteControl();
    }
}
