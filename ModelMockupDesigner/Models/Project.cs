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

namespace ModelMockupDesigner.Models
{
    public class Project : BaseModel
    {
        public string? ProjectName { get; set; }
        public string? ProjectDescription { get; set; }
        public string? CustomerName { get; set; }
        public ProjectTemplate ProjectTemplate { get; set; }
        public List<Category> Categories { get; set; }
        public Project(ProjectCreatorViewModel creatorModel)
        {
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
                    Categories = new();
                    break;
            }

            ProjectName = creatorModel.ProjectName;
            ProjectDescription = creatorModel.ProjectDescription;
            CustomerName = creatorModel.CustomerName;
        }

        #region XML

        public override void LoadFromXml(XmlNode node)
        {
            throw new NotImplementedException();
        }

        public override XmlNode? ToXml()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
