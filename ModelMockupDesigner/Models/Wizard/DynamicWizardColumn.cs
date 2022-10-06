using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelMockupDesigner.Models
{
    public class DynamicWizardColumn : BaseModel, IPropertyEditor, IGroupBoxContent
    {
        public DynamicWizardSection? Parent { get; set; }

        public int Order { get; set; } = 0;
        public List<DynamicWizardPanel> WizardPanels { get; set; }

        public string HeaderName => "Column";

        public ElementType ElementType => ElementType.Column;

        
        

        public DynamicWizardColumn(DynamicWizardSection? parent)
        {
            Parent = parent;
            WizardPanels = new List<DynamicWizardPanel>();
        }

        public void CreateNew()
        {
            if (Parent != null)
                Order = Parent.WizardColumns.Count;

            DynamicWizardPanel wizardPanel = new(this);
            wizardPanel.CreateNew();

            WizardPanels.Add(wizardPanel);
        }
        public List<string> GetEditableProperties()
        {
            List<string> properties = new()
            {
                new string("Name")
            };

            return properties;
        }


        #region GroupBox

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

        #endregion

        #region Private

        private bool display { get; set; }
        private string groupBoxTitle { get; set; }
        private VerticalAlignmentTypes groupVerticalAlignment { get; set; }
        private HorizontalAlignmentTypes groupHorizontalAlignment { get; set; }

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
