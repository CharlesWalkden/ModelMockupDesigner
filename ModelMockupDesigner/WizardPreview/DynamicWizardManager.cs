using ModelMockupDesigner.Controls;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.WizardPreview
{
    public class DynamicWizardManager : IWizardManager
    {
        private readonly PreviewWizardLayout Ui;

        private DynamicWizard DynamicWizard;

        private DynamicWizardPageLayout CurrentPage;

        public double Width { get; set; } = 1020;
        public double Height { get; set; } = 720;

        public DynamicWizardManager(PreviewWizardLayout ui)
        {
            Ui = ui;
            Ui.OnNextPressed += Ui_OnNextPressed;
            Ui.OnPreviousPressed += Ui_OnPreviousPressed; 
        }
        public async Task Reload()
        {
            if (CurrentPage != null)
            {
                int index = DynamicWizard.Sections.IndexOf(CurrentPage.Template);
                CurrentPage.Unload();
                CurrentPage = null;
                await DisplayPage(index);
            }
            else
            {
                await DisplayPage(0);
            }
        }
        public async Task LoadWizard(IWizardModel wizard)
        {
            if (wizard is DynamicWizard dynamicWizard)
            {
                DynamicWizard = dynamicWizard;

                await DisplayPage(0);
            }
        }

        public static async Task<IWizardPageLayout> BuildWizardPage(IWizardModel wizard, int pageIndex)
        {
            IWizardPageLayout page = null;
            if (wizard is DynamicWizard dynamicWizard)
            {
                DynamicWizardSection section = dynamicWizard.Sections[pageIndex];
                page = new DynamicWizardPageLayout(section);
                await page.Build();
            }

            return page;
        }
        public static async Task<EditorSection> BuildDynamicWizardEditorPage(IWizardModel wizard, int pageIndex)
        {
            EditorSection section = null;
            if (wizard is DynamicWizard dynamicWizard)
            {
                DynamicWizardSection sectionModel = dynamicWizard.Sections[pageIndex];
                section = new EditorSection(null);

                await section.LoadModel(sectionModel);
            }

            return section;
        }
        private async Task DisplayPage(int index)
        {
            DynamicWizardSection page = DynamicWizard.Sections[index];

            DynamicWizardPageLayout pageLayout = new DynamicWizardPageLayout(page);

            CurrentPage = pageLayout;

            await Ui.DisplayPage(pageLayout);

            UpdateNavButtons();
        }
        private async void Ui_OnPreviousPressed(object sender, EventArgs e)
        {
            int index = DynamicWizard.Sections.IndexOf(CurrentPage.Template) - 1;

            await DisplayPage(index);
        }
        private async void Ui_OnNextPressed(object sender, EventArgs e)
        {
            int index = DynamicWizard.Sections.IndexOf(CurrentPage.Template) + 1;

            await DisplayPage(index);
        }
        private void UpdateNavButtons()
        {
            int index = DynamicWizard.Sections.IndexOf(CurrentPage.Template);
            bool showPrevious = false;
            bool showNext = false;
            bool showFinished = false;

            if (index > 0)
            {
                showPrevious = true;
            }

            bool isLastPage = true;
            if (index < DynamicWizard.Sections.Count - 1)
            {
                isLastPage = false;
            }

            if (isLastPage)
                showFinished = true;
            else
                showNext = true;

            Ui.ShowNavButtons(showPrevious, showNext, showFinished);
        }

    }
}
