using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.WizardPreview
{
    public class WizardPreviewManager
    {
        private static WizardDesignPreview WizardPreview { get; set; }
        public async static Task LoadWizardPreview(IWizardModel wizardModel) 
        {
            DialogLauncher<WizardDesignPreview> designPreview = new DialogLauncher<WizardDesignPreview>(WindowControl.GetTopWindow(), System.Windows.ResizeMode.NoResize, true);
            WizardPreview = designPreview.Control;
            await WizardPreview.LoadWizard(wizardModel);
            designPreview.Show();
        }

        public async static Task UpdatePreview(IWizardModel wizardModel)
        {
            if (WizardPreview.PreviewWizardLayout == null)
            {
                if (wizardModel != null)
                {
                    await WizardPreview.LoadWizard(wizardModel);
                }
                
            }
            else
            {
                await WizardPreview.PreviewWizardLayout.Reload();
            }
        }

        public async static Task UpdatePreview()
        {
            await WizardPreview.PreviewWizardLayout.Reload();
        }

        public static void ClosePreview()
        {
            WizardPreview.Close();
        }

        public static bool IsClosed => WizardPreview == null;
    }
}
