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

        private List<DynamicWizardSection> Pages { get; set; }

        public DynamicWizardManager(PreviewWizardLayout ui)
        {
            Pages = new List<DynamicWizardSection>();
            Ui = ui;
            Ui.OnNextPressed += Ui_OnNextPressed;
            Ui.OnPreviousPressed += Ui_OnPreviousPressed; 
        }
        public async Task Reload()
        {
            await DisplayPage(0);
        }
        public async Task LoadWizard(IWizardModel wizard)
        {
            if (wizard is DynamicWizard dynamicWizard)
            {
                DynamicWizard = dynamicWizard;

                Pages = dynamicWizard.Sections;

                await DisplayPage(0);
            }
        }
        private async Task DisplayPage(int index)
        {
            DynamicWizardSection page = Pages[index];

            DynamicWizardPageLayout pageLayout = new DynamicWizardPageLayout(page);

            CurrentPage = pageLayout;

            await Ui.DisplayPage(pageLayout);

            UpdateNavButtons();
        }
        private async void Ui_OnPreviousPressed(object sender, EventArgs e)
        {
            int index = Pages.IndexOf(CurrentPage.Template) - 1;

            await DisplayPage(index);
        }
        private async void Ui_OnNextPressed(object sender, EventArgs e)
        {
            int index = Pages.IndexOf(CurrentPage.Template) + 1;

            await DisplayPage(index);
        }
        private void UpdateNavButtons()
        {
            int index = Pages.IndexOf(CurrentPage.Template);
            bool showPrevious = false;
            bool showNext = false;
            bool showFinished = false;

            if (index > 0)
            {
                showPrevious = true;
            }

            bool isLastPage = true;
            if (index < Pages.Count - 1)
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
