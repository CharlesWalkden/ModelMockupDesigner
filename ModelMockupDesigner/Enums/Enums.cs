using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.Enums
{
    public enum WizardType
    {
        Static,
        Dynamic
    }
    public enum WizardTheme
    {
        V5,
        V6,
        V7
    }
    public enum ProjectTemplate
    {
        FullAntenatal,
        IntrapartumOnly
    }

    public enum ElementType
    {
        Textblock,
        Textbox,
        SingleList,
        MultiList,
        Checkbox,
        Image,
        Combobox,
        Grid,
        Page,
        Cell,
        Table,
        Panel,
        Column,
        Section,
        YesNo
    }
    public enum DialogResult
    {
        None, Cancel, OK, Yes, No, Accept
    }
}
