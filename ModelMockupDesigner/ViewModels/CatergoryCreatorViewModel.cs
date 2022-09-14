using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelMockupDesigner.Enums;

namespace ModelMockupDesigner.ViewModels
{
    public class CatergoryCreatorViewModel : BaseViewModel
    {
        public string? CatergoryName
        {
            get => catergoryName;
            set
            {
                if (catergoryName == value)
                    return;

                catergoryName = value;
                OnPropertyChanged(nameof(CatergoryName));
            }
        }
        public string? CatergoryDescription
        {
            get => catergoryDescription;
            set
            {
                if (catergoryDescription == value)
                    return;

                catergoryDescription = value;
                OnPropertyChanged(nameof(CatergoryDescription));
            }
        }

        #region Private Properties

        private string? catergoryName { get; set; }
        private string? catergoryDescription { get; set; }
        #endregion
    }
}
