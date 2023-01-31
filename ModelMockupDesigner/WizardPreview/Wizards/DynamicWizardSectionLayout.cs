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
        public void Unload()
        {
            Template = null;
            foreach (FrameworkElement element in Children)
            {
                if (element is DynamicWizardColumnLayout column)
                {
                    column.Unload();
                }
                else if (element is AthenaGroupBox groupBox)
                {
                    DynamicWizardColumnLayout groupBoxColumn = (DynamicWizardColumnLayout)groupBox.GetContent();
                    groupBoxColumn.Unload();
                    groupBox.Unload();
                }
            }
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
