using ModelMockupDesigner.Controls;
using ModelMockupDesigner.Models;
using ModelMockupDesigner.WizardPreview.Wizards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ModelMockupDesigner.WizardPreview
{
    public class DynamicWizardSectionLayout : Grid
    {
        public DynamicWizardSection Template { get; set; }

        public DynamicWizardSectionLayout(DynamicWizardSection template)
        {
            Template = template;
        }

        public async Task Build()
        {
            this.Children.Clear();
            this.ColumnDefinitions.Clear();
            // TODO: Update section properties when we have them complete.

            foreach (DynamicWizardColumn column in Template.WizardColumns)
            {
                ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

                DynamicWizardColumnLayout columnLayout = new DynamicWizardColumnLayout(column);
                await columnLayout.Build();

                FrameworkElement control;

                if (column.DisplayGroupbox)
                {
                    AthenaGroupBox groupBox = new AthenaGroupBox();
                    groupBox.Initialise(column);
                    groupBox.SetContent(columnLayout);

                    control = groupBox;
                }
                else
                    control = columnLayout;

                Children.Add(control);
                SetColumn(control, column.Order);
            }

        }
    }
}
