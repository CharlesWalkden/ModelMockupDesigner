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

        private DynamicWizard DynamicWizard;
        public DynamicWizardManager(DynamicWizardLayout ui)
        {
            Ui = ui;
            Ui.OnNextPressed += Ui_OnNextPressed;
            Ui.OnPreviousPressed += Ui_OnPreviousPressed; 
        }

        public void LoadWizard(DynamicWizard wizard)
        {

        }
        private async void DisplayPage(int index)
        {

        }
        private void Ui_OnPreviousPressed(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Ui_OnNextPressed(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
