using ModelMockupDesigner.Models.Wizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ModelMockupDesigner.ViewModels
{
    public class WizardEditorViewModel : BaseViewModel
    {
        public string? WizardName { get; set; }
        public Wizard? WizardModel
        {
            get => wizardModel;
            set
            {
                if (wizardModel == value)
                    return;

                wizardModel = value;

                WizardName = wizardModel?.Name;
            }
        }

        public ComboBoxItem? CurrentPage { get; set; }

        public CollectionList<ComboBoxItem> PageList { get; set; }

        #region Private Properties

        private Wizard? wizardModel { get; set; }

        #endregion

        public WizardEditorViewModel()
        {
            PageList = new CollectionList<ComboBoxItem>();
        }

    }



    /// <summary>
    /// Class to use for combobox items within the page selector control.
    /// </summary>
    public class ComboBoxItem : BaseViewModel
    {
        public string? Text { get; set; }

        public object? Value { get; set; }

        public override string ToString()
        {
            if (Text == null || string.IsNullOrEmpty(Text))
            {
                return "Not Set";
            }

            return Text.ToString();
        }
    }
}
