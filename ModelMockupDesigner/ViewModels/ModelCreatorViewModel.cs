﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelMockupDesigner.Enums;

namespace ModelMockupDesigner.ViewModels
{
    public class ModelCreatorViewModel : BaseViewModel
    {
        public string? ProjectName
        {
            get => projectName;
            set
            {
                if (projectName == value)
                    return;

                projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }
        public string? ProjectDescription
        {
            get => projectDescription;
            set
            {
                if (projectDescription == value)
                    return;

                projectDescription = value;
                OnPropertyChanged(nameof(ProjectDescription));
            }
        }

        public string? CustomerName
        {
            get => customerName;
            set
            {
                if (customerName == value)
                    return;

                customerName = value;
                OnPropertyChanged(nameof(CustomerName));
            }
        }

        #region Private Properties

        private string? projectName { get; set; }
        private string? projectDescription { get; set; }
        private string? customerName { get; set; }
        #endregion
    }
}
