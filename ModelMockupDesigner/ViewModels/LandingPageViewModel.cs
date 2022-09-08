using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ModelMockupDesigner.ViewModels
{
    public class LandingPageViewModel
    {

        public LandingPageViewModel()
        {
            NewWizardCommand = new RelayCommand(NewWizard);
            LoadWizardCommand = new RelayCommand(LoadWizard);
            ExportWizardCommand = new RelayCommand(ExportWizard);
        }

        public ICommand NewWizardCommand { get; set; }
        public ICommand LoadWizardCommand { get; set; }
        public ICommand ExportWizardCommand { get; set; }

        private void NewWizard()
        {

        }
        private void LoadWizard()
        {

        }
        private void ExportWizard()
        {

        }
    }
}
