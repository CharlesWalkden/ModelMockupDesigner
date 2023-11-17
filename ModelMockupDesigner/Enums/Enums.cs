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
        NotSet,
        FullAntenatal,
        IntrapartumOnly
    }

    public enum ElementType
    {
        Unknown,
        Label,  

        TextBox, 
        NumericTextBox,
        MultiLineTextBox,
        DoubleTextBox,

        RadioList, 
        CheckBoxList, 
        DropDownList, 
        Date,
        Time, 
        YesNo, 
        CheckBox,  
        Image,  
        DateTime, 
        ApproxDate, 
        ApproxDateTextBox,
        TextSuggestion, 
        Button, 

        GroupBox,

        Combobox,
        Grid,
        Page,
        Cell,
        Table,
        Panel,
        Column,
        Section
    }
    public enum DialogResult
    {
        None, Cancel, OK, Yes, No, Accept
    }
    public enum ImagePosition
    {
        None,
        Above,
        Below,
        Left,
        Right
    }
    public enum ContentAlignment
    {
        // Summary:
        //     Content is vertically aligned at the top, and horizontally aligned on the
        //     left.
        TopLeft = 1,
        //
        // Summary:
        //     Content is vertically aligned at the top, and horizontally aligned at the
        //     center.
        TopCenter = 2,
        //
        // Summary:
        //     Content is vertically aligned at the top, and horizontally aligned on the
        //     right.
        TopRight = 4,
        //
        // Summary:
        //     Content is vertically aligned in the middle, and horizontally aligned on
        //     the left.
        MiddleLeft = 16,
        //
        // Summary:
        //     Content is vertically aligned in the middle, and horizontally aligned at
        //     the center.
        MiddleCenter = 32,
        //
        // Summary:
        //     Content is vertically aligned in the middle, and horizontally aligned on
        //     the right.
        MiddleRight = 64,
        //
        // Summary:
        //     Content is vertically aligned at the bottom, and horizontally aligned on
        //     the left.
        BottomLeft = 256,
        //
        // Summary:
        //     Content is vertically aligned at the bottom, and horizontally aligned at
        //     the center.
        BottomCenter = 512,
        //
        // Summary:
        //     Content is vertically aligned at the bottom, and horizontally aligned on
        //     the right.
        BottomRight = 1024,
    }
    public enum HorizontalAlignmentTypes
    {
        //
        // Summary:
        //     An element aligned to the left of the layout slot for the parent element.
        Left = 0,
        //
        // Summary:
        //     An element aligned to the center of the layout slot for the parent element.
        Center = 1,
        //
        // Summary:
        //     An element aligned to the right of the layout slot for the parent element.
        Right = 2,
        //
        // Summary:
        //     An element stretched to fill the entire layout slot of the parent element.
        Stretch = 3
    }


    public enum VerticalAlignmentTypes
    {
        //
        // Summary:
        //     The child element is aligned to the top of the parent's layout slot.
        Top = 0,
        //
        // Summary:
        //     The child element is aligned to the center of the parent's layout slot.
        Center = 1,
        //
        // Summary:
        //     The child element is aligned to the bottom of the parent's layout slot.
        Bottom = 2,
        //
        // Summary:
        //     The child element stretches to fill the parent's layout slot.
        Stretch = 3
    }
}
