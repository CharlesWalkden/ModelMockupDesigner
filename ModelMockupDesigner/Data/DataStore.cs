using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.Data
{
    public static class DataStore
    {
        public static List<Category> GetIntrapartumCategories()
        {
            List<Category> categories = new List<Category>();

            return categories;
        }
        public static List<Category> GetAntenatalCategories()
        {
            List<Category> categories = new()
            {
                new Category(Guid.Empty){Name = "Profile", Categories = GetSubCategoriesProfile()},
                new Category(Guid.Empty){Name = "Consultations & Admissions"},
                new Category(Guid.Empty){Name = "Antenatal"},
                new Category(Guid.Empty){Name = "Intrapartum"},
                new Category(Guid.Empty){Name = "Postnatal"}
            };

            return categories;
        }
        public static List<Category> GetSubCategoriesProfile()
        {
            List<Category> categories = new()
            {
                new Category(Guid.Empty){Name = "Woman's Details"},
                new Category(Guid.Empty){Name = "Obstetric Profile"},
                new Category(Guid.Empty){Name = "Medical Profile"},
                new Category(Guid.Empty){Name = "Social Profile"},
                new Category(Guid.Empty){Name = "Screening Profile"},
                new Category(Guid.Empty){Name = "Plan of Care"},
                new Category(Guid.Empty){Name = "Communications"},
                new Category(Guid.Empty){Name = "Pregnancy Outcome"}
            };

            return categories;
        }
    }
}
