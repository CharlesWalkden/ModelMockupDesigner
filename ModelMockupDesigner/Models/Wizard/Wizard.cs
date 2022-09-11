using ModelMockupDesigner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelMockupDesigner.Models.Wizard
{
    public class Wizard : BaseModel
    {
        public string? Description { get; set; }
        public WizardType? WizardType { get; set; }
        public WizardTheme? WizardTheme { get; set; }
        public List<WizardSection> Sections { get; set; }

        public Wizard()
        {
            Sections = new List<WizardSection>();
        }

        public async Task BuildWizard(Guid Id)
        {
            if (Id != Guid.Empty)
            {
                await LoadExistingWizard(Id);
            }
            else
            {
                await CreateNewWizard();
            }
        }
        private async Task LoadExistingWizard(Guid id)
        {
            // Load wizard from cache when we have one.
        }
        private Task CreateNewWizard()
        {
            WizardSection newSection = new(this);
            newSection.CreateNew(); 

            Sections.Add(newSection);

            return Task.CompletedTask;
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
