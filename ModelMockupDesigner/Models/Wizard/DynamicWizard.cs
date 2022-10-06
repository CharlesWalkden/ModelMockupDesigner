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
        public Category? Category
        {
            get => category;
            set
            {
                // If we had a previous category, remove the wizard from it
                if (category != null)
                {
                    category.DeleteWizard(this);
                }
                // If we didnt have a previous category, remove the wizard from the lone wizard list.
                if (category == null)
                {
                    Project?.LoneWizards.Remove(this);
                }
                category = value;
                if (category != null)
                {
                    category?.AddWizard(this);
                }
                else
                {
                    Project?.LoneWizards.Add(this);
                }
            }
        }
        private Category? category { get; set; }
        public Project? Project { get; set; }

        public DynamicWizard()
        {
            Sections = new List<DynamicWizardSection>();
        }

        public Task CreateNew() 
        {
            DynamicWizardSection newSection = new(this);

            return Task.CompletedTask;
        }
        public void LoadFromWizardCreator(WizardCreatorViewModel vm)
        {
            Name = vm.WizardName;
            Description = vm.WizardDescription;
            WizardType = vm.WizardType;
            WizardTheme = vm.WizardTheme;

            if (vm.CurrentCategorySelection != null)
                Category = (Category)vm.CurrentCategorySelection.Value;

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
