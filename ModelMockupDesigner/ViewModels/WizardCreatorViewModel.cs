using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelMockupDesigner.Enums;

namespace ModelMockupDesigner.ViewModels
{
    public class WizardModel : BaseViewModel
    {
        public string? WizardName
        {
            get => wizardName;
            set
            {
                if (wizardName == value)
                    return;

                wizardName = value;
                OnPropertyChanged(nameof(WizardName));
            }
        }
        public string? WizardDescription
        {
            get => wizardDescription;
            set
            {
                if (wizardDescription == value)
                    return;

                wizardDescription = value;
                OnPropertyChanged(nameof(WizardDescription));
            }
        }
        public WizardType WizardType
        {
            get => wizardType;
            set
            {
                if (wizardType == value)
                    return;

                wizardType = value;
                OnPropertyChanged(nameof(WizardType));
            }
        }

        #region Private Properties

        private string? wizardName { get; set; }
        private string? wizardDescription { get; set; }
        private WizardType wizardType { get; set; }

        #endregion

        public WizardModel()
        {

        }
    }
}
