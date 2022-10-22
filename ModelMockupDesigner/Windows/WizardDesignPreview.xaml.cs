using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModelMockupDesigner
{
    /// <summary>
    /// Interaction logic for WizardDesignPreview.xaml
    /// </summary>
    public partial class WizardDesignPreview : UserControl, IDialogClient
    {
        public PreviewWizardLayout PreviewWizardLayout; 

        public WizardDesignPreview()
        {
            InitializeComponent();
        }

        public async void WizardDesignPreview_OnWizardUpdated(object sender, DynamicWizard e)
        {
            if (PreviewWizardLayout == null)
                await LoadWizard(e);
            else
            {
                await PreviewWizardLayout.Reload();
            }
        }

        public async Task LoadWizard(IWizardModel wizard)
        {
            PreviewWizardLayout = new PreviewWizardLayout(wizard.WizardType);
            await PreviewWizardLayout.LoadWizard(wizard);

            root.Children.Add(PreviewWizardLayout);
        }

        #region Interface

        public event EventHandler<DialogEventArgs> OnClose;

        public void Close()
        {
            OnClose?.Invoke(this, new DialogEventArgs() { Result = Enums.DialogResult.None});
        }

        #endregion
    }
}
