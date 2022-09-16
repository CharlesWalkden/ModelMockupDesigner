using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Models;

namespace ModelMockupDesigner.ViewModels
{
    public class WizardCreatorViewModel : BaseViewModel
    {
        public CollectionList<ComboBoxItem> CategoryList { get; set; }
        public ComboBoxItem? CurrentCategorySelection { get; set; }
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
        public WizardTheme WizardTheme
        {
            get => wizardTheme;
            set
            {
                if (wizardTheme == value)
                    return;

                wizardTheme = value;
                OnPropertyChanged(nameof(WizardTheme));
            }
        }

        #region Private Properties

        private string? wizardName { get; set; }
        private string? wizardDescription { get; set; }
        private WizardType wizardType { get; set; }
        private WizardTheme wizardTheme { get; set; }

        #endregion

        public WizardCreatorViewModel()
        {
            CategoryList = new();
        }

        public void LoadCategoryList(List<ComboBoxItem> categoryList)
        {
            CategoryList.AddRange(categoryList);
        }
        public void SetCategory(Guid categoryId)
        {
            CurrentCategorySelection = CategoryList.FirstOrDefault(x => (Guid)x.Value == categoryId);
        }
    }
}
