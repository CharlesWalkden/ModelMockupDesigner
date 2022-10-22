using ModelMockupDesigner.Data;
using ModelMockupDesigner.Models;
using ModelMockupDesigner.ViewModels;
using ModelMockupDesigner.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Markup.Localizer;
using ModelMockupDesigner.Interfaces;
using System.Runtime.CompilerServices;

namespace ModelMockupDesigner.Models
{
    public class Project : BaseModel
    {
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string CustomerName { get; set; }
        public ProjectTemplate ProjectTemplate { get; set; }
        public List<IWizardModel> AllWizards { get; set; }
        public int WizardCount { get => AllWizards.Count; }
        public List<Category> Categories { get; set; }
        public List<IWizardModel> LoneWizards { get; set; }   
        public DateTime LastAccess { get; set; }
        public Project(ProjectCreatorViewModel creatorModel)
        {
            AllWizards = new List<IWizardModel>();
            LoneWizards = new List<IWizardModel>();
            ProjectTemplate = creatorModel.ProjectTemplate;

            switch (ProjectTemplate)
            {
                case ProjectTemplate.FullAntenatal:
                    {
                        Categories = DataStore.GetAntenatalCategories();
                        break;
                    }
                case ProjectTemplate.IntrapartumOnly:
                    {
                        Categories = DataStore.GetIntrapartumCategories();
                        break;
                    }
                default:
                    Categories = new List<Category>();
                    break;
            }

            ProjectName = creatorModel.ProjectName;
            ProjectDescription = creatorModel.ProjectDescription;
            CustomerName = creatorModel.CustomerName;
            LastAccess = DateTime.Now;
        }
        public List<ComboBoxItem> CreateCategoryList()
        {
            List<ComboBoxItem> categoryList = new List<ComboBoxItem>()
            {
                new ComboBoxItem()
                {
                    Text = "",
                    Value = null
                }
            };

            if (Categories != null)
            {
                foreach (Category category in Categories)
                {
                    ComboBoxItem comboBox = new ComboBoxItem()
                    {
                        Text = category.Name,
                        Value = category
                    };
                    categoryList.Add(comboBox);

                    if (category.Categories != null && category.Categories.Count > 0)
                    {
                        categoryList.AddRange(category.GetCategoryList());
                    }
                }
            }

            return categoryList;
        }

        // This will be used when we load the project from Xml
        public void AddWizardsToCategories() 
        {
            if (Categories != null)
            {
                foreach (IWizardModel wizard in AllWizards)
                {
                    if (wizard.Category != null)
                    {
                        wizard.Category.AddWizard(wizard);
                    }
                    else
                    {
                        LoneWizards.Add(wizard);
                    }
                }
            }
        }

        #region XML

        public override void LoadFromXml(XmlNode node)
        {
            throw new NotImplementedException();
        }

        public override XmlNode ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
