using ModelMockupDesigner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.ViewModels
{
    public class ControlSelectorViewModel : BaseViewModel
    {
        public ElementType OriginalElement { get; set; }
        public ElementType SelectedControl { get; set; }

    }
}
