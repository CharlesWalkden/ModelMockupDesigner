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
    public class GroupBoxDisplayChangedEventArgs : EventArgs
    {
        public bool Display { get; set; }
        public string GroupBoxTitle { get; set; }
        public HorizontalAlignmentTypes HorizontalAlignment { get; set; }
        public VerticalAlignmentTypes VerticalAlignment { get; set; }
    }

    public abstract class BaseModel : INotifyPropertyChanged, IGroupBoxContent
    {
        public string Name
        {
            get => name;
            set
            {
                if (name == value)
                    return;

                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public int OrderId
        {
            get => orderId;
            set
            {
                if (orderId == value)
                    return;

                orderId = value;
                OnPropertyChanged(nameof(OrderId));
            }
        }
        private int orderId { get; set; } = 0;
        private string name { get; set; }

        #region GroupBox

        public event EventHandler<GroupBoxDisplayChangedEventArgs> DisplayChanged;
        public bool DisplayGroupbox
        {
            get => displayGroupbox;
            set
            {
                if (displayGroupbox == value)
                    return;

                displayGroupbox = value;
                OnPropertyChanged(nameof(DisplayGroupbox));
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
                DisplayChanged?.Invoke(this, new GroupBoxDisplayChangedEventArgs() { Display = DisplayGroupbox, GroupBoxTitle = value, HorizontalAlignment = GroupHorizontalAlignment, VerticalAlignment = GroupVerticalAlignment });
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
                DisplayChanged?.Invoke(this, new GroupBoxDisplayChangedEventArgs() { Display = DisplayGroupbox, GroupBoxTitle = GroupBoxTitle, HorizontalAlignment = value, VerticalAlignment = GroupVerticalAlignment });
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
                DisplayChanged?.Invoke(this, new GroupBoxDisplayChangedEventArgs() { Display = DisplayGroupbox, GroupBoxTitle = GroupBoxTitle, HorizontalAlignment = GroupHorizontalAlignment, VerticalAlignment = value });
            }
        }

        #endregion

        #region Private

        private bool displayGroupbox { get; set; }
        private string groupBoxTitle { get; set; }
        private HorizontalAlignmentTypes groupHorizontalAlignment { get; set; }
        private VerticalAlignmentTypes groupVerticalAlignment { get; set; }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public abstract void LoadFromXml(XmlNode node);
        public abstract XmlNode ToXml(); 
    }
}
