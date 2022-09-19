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
        public WizardDesignPreview()
        {
            InitializeComponent();
        }

        public async void LoadWizard(DynamicWizard wizard)
        {

        }

        #region Interface

        public event EventHandler<DialogEventArgs>? OnClose;

        public void Close()
        {
            OnClose?.Invoke(this, new() { Result = Enums.DialogResult.None});
        }

        #endregion
    }
}
