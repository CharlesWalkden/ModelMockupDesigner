﻿using ModelMockupDesigner.Controls;
using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelMockupDesigner.Models
{
    public class DynamicWizardTable : BaseModel, ICellParent, ICellControl, IPropertyEditor, IGroupBoxContent
    {
        public DynamicWizardCell? Parent { get; set; }

        public List<DynamicWizardCell> Cells { get; set; }

        public DynamicWizardTable(DynamicWizardCell? parent)
        {
            Parent = parent;
            Cells = new List<DynamicWizardCell>();
        }

        public void CreateNew()
        {
            DynamicWizardCell wizardCell = new(this);

            Cells.Add(wizardCell);
        }
        public ElementType ElementType => ElementType.Table;
        public string HeaderName => "Table";

        public Dictionary<string, string> GetEditableProperties()
        {
            Dictionary<string, string> properties = new();
            properties.Add("Name", "Name");

            return properties;
        }


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

        #region Xml

        public override void LoadFromXml(XmlNode node)
        {
            throw new NotImplementedException();
        }

        public override XmlNode? ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Not Used

        public void Delete(EditorCell cell)
        {
            throw new NotImplementedException();
        }


        public BaseModel? Model => throw new NotImplementedException();

        

        #endregion
    }
}
