using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelMockupDesigner.Models
{
    public class DynamicWizard : BaseModel, IWizardModel
    {
        public string? Description { get; set; }
        public WizardType? WizardType { get; set; }
        public WizardTheme? WizardTheme { get; set; }
        public List<DynamicWizardSection> Sections { get; set; }
        public Guid CateogryId { get; set; }
        public Project? Project { get; set; }

        public DynamicWizard()
        {
            Sections = new List<DynamicWizardSection>();
        }

        public Task CreateNew() 
        {
            DynamicWizardSection newSection = new(this);
            newSection.CreateNew(); 

            return Task.CompletedTask;
        }
        public void LoadFromWizardCreator(WizardCreatorViewModel vm)
        {
            Name = vm.WizardName;
            Description = vm.WizardDescription;
            WizardType = vm.WizardType;
            WizardTheme = vm.WizardTheme;

            if (vm.CurrentCategorySelection != null)
                CateogryId = (Guid)vm.CurrentCategorySelection.Value;

            Project ??= vm.Project;
        }

        #region Xml

        public override void LoadFromXml(XmlNode node)
        {

        }
        private void ToXml(XmlWriter writer)
        {

        }
        public override XmlNode? ToXml()
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true
            };
            StringBuilder sb = new StringBuilder();

            using (XmlWriter writer = XmlWriter.Create(sb, settings))
            {
                ToXml(writer);
            }

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(sb.ToString());

            return doc.DocumentElement;
        }

        #endregion
    }
}
