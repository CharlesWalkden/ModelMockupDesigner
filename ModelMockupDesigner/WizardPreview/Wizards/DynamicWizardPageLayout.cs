using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ModelMockupDesigner.WizardPreview
{
    public class DynamicWizardPageLayout : StackPanel
    {
        public DynamicWizardSection Template { get; set; }

        public DynamicWizardPageLayout(DynamicWizardSection template)
        {
            Template = template;
        }

        public async Task Build() 
        {
            this.Children.Clear();

            DynamicWizardSectionLayout sectionLayout = new(Template);
            await sectionLayout.Build();

            this.Children.Add(sectionLayout);
        }
    }
}
