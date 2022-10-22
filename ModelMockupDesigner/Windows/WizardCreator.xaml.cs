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
using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.ViewModels;

namespace ModelMockupDesigner
{
    /// <summary>
    /// Interaction logic for WizardCreator.xaml
    /// </summary>
    public partial class WizardCreator : UserControl, IDialogClient
    {
        #region Interface

        public event EventHandler<DialogEventArgs> OnClose;

        #endregion

        public WizardCreatorViewModel ViewModel { get => DataContext as WizardCreatorViewModel; }
        public DialogResult DialogResult { get; set; }
        public WizardCreator()
        {
            InitializeComponent();

            WizardCreatorViewModel wizardCreatorViewModel = new WizardCreatorViewModel();

            DataContext = wizardCreatorViewModel;
        }
        public void LoadExistingData(WizardCreatorViewModel existingWizardData)
        {
            DataContext = existingWizardData;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Accept;
            OnClose?.Invoke(this, new DialogEventArgs(DialogResult));
        }

        private void Close_Click(object sender, RoutedEventArgs e) 
        {
            DialogResult = DialogResult.Cancel;
            OnClose?.Invoke(this, new DialogEventArgs(DialogResult));
        }
    }
}
