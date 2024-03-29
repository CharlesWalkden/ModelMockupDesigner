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
        public void Unload()
        {
            Template = null;
            foreach (FrameworkElement element in Children)
            {
                if (element is DynamicWizardPanelLayout panel)
                {
                    panel.Unload();
                }
                else if (element is AthenaGroupBox groupBox)
                {
                    DynamicWizardPanelLayout groupBoxPanel = (DynamicWizardPanelLayout)groupBox.GetContent();
                    groupBoxPanel.Unload();
                    groupBox.Unload();
                }
            }
        }
        public async Task Build()
        {
            this.Children.Clear();
            bool first = true;
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

                    if (!first)
                    {
                        groupBox.Margin = new Thickness(0, 10, 0, 0);
                    }

                    control = groupBox;
                }
                else
                {
                    control = panelLayout;
                    if (!first)
                    {
                        control.Margin = new Thickness(0, 10, 0, 0);
                    }
                    else
                    {
                        control.Margin = new Thickness(0);
                    }
                }

                this.Children.Add(control);

                first = false;
            }
        }
    }
}
