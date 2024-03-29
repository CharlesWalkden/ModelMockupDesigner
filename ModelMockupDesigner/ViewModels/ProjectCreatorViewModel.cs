﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ModelMockupDesigner.Enums;

namespace ModelMockupDesigner.ViewModels
{
    public class ProjectCreatorViewModel : BaseViewModel 
    {
        public ProjectCreatorViewModel()
        {
            // Set default for binding.
            ProjectTemplate = ProjectTemplate.FullAntenatal;
        }
        public string ProjectName
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
        public string ProjectDescription
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
        public string CustomerName
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
        public ProjectTemplate ProjectTemplate
        {
            get => projectTemplate;
            set
            {
                if (projectTemplate == value)
                    return;

                projectTemplate = value;
                OnPropertyChanged(nameof(ProjectTemplate));
            }
        }
        public List<string> ValidateData()
        {
            List<string> remaining = new List<string>();

            if (string.IsNullOrWhiteSpace(ProjectName))
            {
                remaining.Add(nameof(ProjectName));
            }

            if (string.IsNullOrWhiteSpace(CustomerName))
            {
                remaining.Add(nameof(CustomerName));
            }

            return remaining;
        }

        #region Private Properties

        private string projectName { get; set; }
        private string projectDescription { get; set; }
        private string customerName { get; set; }
        private ProjectTemplate projectTemplate { get; set; }

        #endregion
    }
}
