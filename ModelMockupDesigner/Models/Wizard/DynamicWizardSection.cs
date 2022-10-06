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
    public class DynamicWizardSection : BaseModel, IPropertyEditor, IGroupBoxContent
    {
        public DynamicWizard Parent { get; set; }

        public List<DynamicWizardColumn> WizardColumns { get; set; }

        public string HeaderName => "Section";

        public ElementType ElementType => ElementType.Section;

        public DynamicWizardSection(DynamicWizard parent)
        {
            Parent = parent;
            Parent.Sections.Add(this);
            WizardColumns = new List<DynamicWizardColumn>();
            CreateNew();
        }

        public void CreateNew()
        {
            DynamicWizardColumn wizardColumn = new(this);
            wizardColumn.CreateNew();

            WizardColumns.Add(wizardColumn);
        }
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
    }
}
