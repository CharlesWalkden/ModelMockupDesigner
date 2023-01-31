using ModelMockupDesigner.Controls;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ModelMockupDesigner.WizardPreview
{
    public class DynamicWizardPageLayout : StackPanel, IWizardPageLayout
    {
        public DynamicWizardSection Template { get; set; }

        public DynamicWizardPageLayout(DynamicWizardSection template)
        {
            Template = template;
        }
        public void Unload()
        {
            Template = null;
            foreach (FrameworkElement element in Children)
            {
                if (element is DynamicWizardSectionLayout section)
                {
                    section.Unload();
                }
                else if (element is AthenaGroupBox groupBox)
                {
                    DynamicWizardSectionLayout groupBoxSection = (DynamicWizardSectionLayout)groupBox.GetContent();
                    groupBoxSection.Unload();
                    groupBox.Unload();
                }
            }
        }
        public async Task Build() 
        {
            this.Children.Clear();

            DynamicWizardSectionLayout sectionLayout = new DynamicWizardSectionLayout(Template);
            await sectionLayout.Build();

            FrameworkElement control;

            if (Template.DisplayGroupbox)
            {
                AthenaGroupBox groupBox = new AthenaGroupBox();
                groupBox.Initialise(Template);
                groupBox.SetContent(sectionLayout);

                control = groupBox;
            }
            else
                control = sectionLayout;

            this.Children.Add(control);
        }
    }
}
