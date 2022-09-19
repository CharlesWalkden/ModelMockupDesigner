using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelMockupDesigner.Models
{
    public class Category : BaseModel
    {
        public Guid Id { get; set; }
        public string? Description { get; set; }

        public List<Category>? Categories { get; set; }
        public List<DynamicWizard>? Wizards { get; set; }

        public bool IsExpanded { get; set; } = false;

        public Category(Guid id)
        {
            if (id == Guid.Empty)
            {
                Id = Guid.NewGuid();
            }
            else
            {
                Id = id;
            }

            Categories = new();
            Wizards = new();

        }
        public void Load(List<DynamicWizard> wizards, List<Category>? categories = null)
        {
            Wizards = wizards;
            Categories = categories;
        }
        public void AddWizard(DynamicWizard wizard)
        {
            if (Wizards != null)
            {
                Wizards.Add(wizard);
            }
        }
        public bool AddWizardToCategory(DynamicWizard wizard)
        {
            bool added = false;
            if (Categories != null)
            {
                foreach(Category category in Categories)
                {
                    if (category.Id == wizard.CateogryId)
                    {
                        category.AddWizard(wizard);
                        break;
                    }
                    if (category.Categories != null)
                    {
                        if (category.AddWizardToCategory(wizard))
                        {
                            break;
                        }
                    }
                }
            }

            return added;
        }
        public List<ComboBoxItem> GetCategoryList()
        {
            List<ComboBoxItem> categoryList = new List<ComboBoxItem>();
            if (Categories != null)
            {
                foreach (Category category in Categories)
                {
                    ComboBoxItem comboBoxItem = new()
                    {
                        Text = category.Name,
                        Value = category.Id
                    };
                    categoryList.Add(comboBoxItem);

                    if (category.Categories != null && category.Categories.Count > 0)
                    {
                        categoryList.AddRange(category.GetCategoryList());
                    }
                }
            }

            return categoryList;
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
