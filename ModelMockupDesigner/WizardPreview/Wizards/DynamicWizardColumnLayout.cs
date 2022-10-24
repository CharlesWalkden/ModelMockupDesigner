﻿using ModelMockupDesigner.Controls;
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
    public class DynamicWizardColumnLayout : StackPanel
    {
        public DynamicWizardColumn Template { get; set; }

        public DynamicWizardColumnLayout(DynamicWizardColumn template)
        {
            Template = template;
        }

        public async Task Build()
        {
            this.Children.Clear();

            foreach (DynamicWizardPanel panel in Template.WizardPanels)
            {
                DynamicWizardPanelLayout panelLayout = new DynamicWizardPanelLayout(panel);
                await panelLayout.Build();

                FrameworkElement control;

                if (panel.DisplayGroupbox)
                {
                    AthenaGroupBox groupBox = new AthenaGroupBox();
                    groupBox.Initialise(panel);
                    groupBox.SetContent(panelLayout);

                    control = groupBox;
                }
                else
                    control = panelLayout;

                this.Children.Add(control);
            }
        }
    }
}
