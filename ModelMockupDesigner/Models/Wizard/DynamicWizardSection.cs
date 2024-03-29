﻿using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelMockupDesigner.Models
{
    public class DynamicWizardSection : BaseModel, IPropertyEditor
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
            DynamicWizardColumn wizardColumn = new DynamicWizardColumn(this);
            wizardColumn.CreateNew();

            WizardColumns.Add(wizardColumn);
        }
        public Dictionary<string, string> GetEditableProperties()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>
            {
                { "Name", "Name" }
            };

            return properties;
        }

        #region Xml

        public override void LoadFromXml(XmlNode node)
        {
            throw new NotImplementedException();
        }

        public override XmlNode ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
