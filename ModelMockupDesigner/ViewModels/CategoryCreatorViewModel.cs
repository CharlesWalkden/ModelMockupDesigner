using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelMockupDesigner.Enums;

namespace ModelMockupDesigner.ViewModels
{
    public class CategoryCreatorViewModel : BaseViewModel
    {
        public string CategoryName
        {
            get => categoryName;
            set
            {
                if (categoryName == value)
                    return;

                categoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }
        public string CategoryDescription
        {
            get => categoryDescription;
            set
            {
                if (categoryDescription == value)
                    return;

                categoryDescription = value;
                OnPropertyChanged(nameof(CategoryDescription));
            }
        }

        public List<string> ValidateData()
        {
            List<string> remaining = new List<string>();

            if (string.IsNullOrWhiteSpace(CategoryName))
            {
                remaining.Add(nameof(CategoryName));
            }

            if (string.IsNullOrWhiteSpace(CategoryDescription))
            {
                remaining.Add(nameof(CategoryDescription));
            }

            return remaining;
        }

        #region Private Properties

        private string categoryName { get; set; }
        private string categoryDescription { get; set; } 
        #endregion
    }
}
