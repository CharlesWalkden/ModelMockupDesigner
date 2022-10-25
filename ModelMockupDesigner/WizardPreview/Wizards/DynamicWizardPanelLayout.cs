using ModelMockupDesigner.Controls;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ModelMockupDesigner.WizardPreview.Wizards
{
    public class DynamicWizardPanelLayout : Grid
    {
        // This will also be used for Tables.

        public ICellParent Template { get; set; }

        public DynamicWizardPanelLayout(ICellParent template)
        {
            Template = template;
        }

        public async Task Build()
        {
            Children.Clear();
            ColumnDefinitions.Clear();
            RowDefinitions.Clear();

            int maxRow = Template.Cells.Max(x => x.Row);
            int maxColumn = Template.Cells.Max(x => x.Column);
            
            for (int i = 0; i <= maxRow; i++)
            {
                RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            }
            for (int c = 0; c <= maxColumn; c++)
            {
                ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            }

            foreach (DynamicWizardCell cell in Template.Cells)
            {
                FrameworkElement control = null;

                if (cell.Control is DynamicWizardTable)
                {
                    DynamicWizardPanelLayout table = new DynamicWizardPanelLayout((ICellParent)cell.Control.Model);
                    await table.Build();
                    control = table;
                    
                }
                else if (cell.Control is CustomControl customControl)
                {
                    control = CustomControlGenerator.GetControl(customControl).Result;
                }

                if (control != null)
                {
                    if (cell.Control.DisplayGroupbox &&
                        cell.Control.ElementType != Enums.ElementType.DateTime && cell.Control.ElementType != Enums.ElementType.RadioList)
                    {
                        AthenaGroupBox groupBox = new AthenaGroupBox();
                        groupBox.Margin = new Thickness(5);
                        groupBox.Initialise(cell.Control.Model);
                        groupBox.SetContent(control);

                        control = groupBox;
                    }

                    // Added this so controls will line up when groupboxes are next to each other.
                    if (cell.Control.DisplayGroupbox && (cell.Control.ElementType == Enums.ElementType.DateTime || cell.Control.ElementType == Enums.ElementType.RadioList))
                    {
                        control.Margin = new Thickness(5);
                    }

                    this.Children.Add(control);
                    Grid.SetColumn(control, cell.Column);
                    Grid.SetRow(control, cell.Row);
                }
            }
        }
    }
}
