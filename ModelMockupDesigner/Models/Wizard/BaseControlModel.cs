using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelMockupDesigner.Models
{
    public class GroupBoxDisplayChangedEventArgs :EventArgs
    {
        public bool Display { get; set; }
        public string? GroupBoxTitle { get; set; }
        public HorizontalAlignmentTypes HorizontalAlignment { get; set; }
        public VerticalAlignmentTypes VerticalAlignment { get; set; }  
    }

    public abstract class BaseControlModel : BaseModel, ICellControl, IPropertyEditor, IGroupBoxContent
    {
        #region GroupBox

        public event EventHandler<GroupBoxDisplayChangedEventArgs> DisplayChanged;

        public bool Display
        {
            get => display;
            set
            {
                if (display == value)
                    return;

                display = value;
                OnPropertyChanged(nameof(Display));
                DisplayChanged?.Invoke(this, new GroupBoxDisplayChangedEventArgs() { Display = value, GroupBoxTitle = GroupBoxTitle, HorizontalAlignment = GroupHorizontalAlignment, VerticalAlignment = GroupVerticalAlignment });
            }
        }
        public string GroupBoxTitle
        {
            get => groupBoxTitle;
            set
            {
                if (groupBoxTitle == value)
                    return;

                groupBoxTitle = value;
                OnPropertyChanged(nameof(GroupBoxTitle));
                DisplayChanged?.Invoke(this, new GroupBoxDisplayChangedEventArgs() { Display = Display, GroupBoxTitle = value, HorizontalAlignment = GroupHorizontalAlignment, VerticalAlignment = GroupVerticalAlignment });
            }
        }
        public HorizontalAlignmentTypes GroupHorizontalAlignment
        {
            get => groupHorizontalAlignment;
            set
            {
                if (groupHorizontalAlignment == value)
                    return;

                groupHorizontalAlignment = value;
                OnPropertyChanged(nameof(GroupHorizontalAlignment));
                DisplayChanged?.Invoke(this, new GroupBoxDisplayChangedEventArgs() { Display = Display, GroupBoxTitle = GroupBoxTitle, HorizontalAlignment = value, VerticalAlignment = GroupVerticalAlignment });
            }
        }
        public VerticalAlignmentTypes GroupVerticalAlignment
        {
            get => groupVerticalAlignment;
            set
            {
                if (groupVerticalAlignment == value)
                    return;

                groupVerticalAlignment = value;
                OnPropertyChanged(nameof(GroupVerticalAlignment));
                DisplayChanged?.Invoke(this, new GroupBoxDisplayChangedEventArgs() { Display = Display, GroupBoxTitle = GroupBoxTitle, HorizontalAlignment = GroupHorizontalAlignment, VerticalAlignment = value });
            }
        }

        #endregion

        #region Private

        private bool display { get; set; }
        private string groupBoxTitle { get; set; }
        private HorizontalAlignmentTypes groupHorizontalAlignment { get; set; }
        private VerticalAlignmentTypes groupVerticalAlignment { get; set; }

        #endregion

        #region Content

        public HorizontalAlignmentTypes HorizontalAlignment
        {
            get => horizontalAlignment;
            set
            {
                if (horizontalAlignment == value)
                    return;

                horizontalAlignment = value;
                OnPropertyChanged(nameof(HorizontalAlignment));
            }
        }
        private HorizontalAlignmentTypes horizontalAlignment { get; set; }
        public VerticalAlignmentTypes VerticalAlignment
        {
            get => verticalAlignment;
            set
            {
                if (verticalAlignment == value)
                    return;

                verticalAlignment = value;
                OnPropertyChanged(nameof(VerticalAlignment));
            }
        }
        private VerticalAlignmentTypes verticalAlignment { get; set; }

        #endregion

        #region Interface

        public ElementType ElementType { get; set; }

        public BaseModel? Model => this;

        public string HeaderName => ElementType.ToString();

        #endregion

        #region Xml
        public override void LoadFromXml(XmlNode node)
        {
            throw new NotImplementedException();
        }

        public override XmlNode? ToXml()
        {
            throw new NotImplementedException();
        }

        public abstract Dictionary<string, string> GetEditableProperties();

        #endregion

    }
}
