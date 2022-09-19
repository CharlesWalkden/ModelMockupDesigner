using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.WizardPreview
{
    public class DynamicWizardManager
    {
        private readonly DynamicWizardLayout Ui;

        private DynamicWizard? DynamicWizard;

        private DynamicWizardPageLayout? CurrentPage = null;

        private List<DynamicWizardSection> Pages { get; set; }

        public DynamicWizardManager(DynamicWizardLayout ui)
        {
            Pages = new List<DynamicWizardSection>();
            Ui = ui;
            Ui.OnNextPressed += Ui_OnNextPressed;
            Ui.OnPreviousPressed += Ui_OnPreviousPressed; 
        }

        public async void LoadWizard(DynamicWizard wizard)
        {
            DynamicWizard = wizard;

            Pages.Clear();
            Pages = wizard.Sections;

            await DisplayPage(0);
        }
        private async Task DisplayPage(int index)
        {
            DynamicWizardSection page = Pages[index];

            CurrentPage = await Ui.DisplayPage(page);

            UpdateNavButtons();
        }
        private void Ui_OnPreviousPressed(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Ui_OnNextPressed(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
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
