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
            List<Category> categories = new List<Category>()
            {
                new Category(Guid.Empty){Name = "Intrapartum", Categories = GetSubCategoriesIntrapartum()},
            };

            return categories;
        }
        public static List<Category> GetAntenatalCategories()
        {
            List<Category> categories = new List<Category>()
            {
                new Category(Guid.Empty){Name = "Profile", Categories = GetSubCategoriesProfile()},
                new Category(Guid.Empty){Name = "Consultations & Admissions", Categories = GetSubCategoriesConsultations()},
                new Category(Guid.Empty){Name = "Antenatal", Categories = GetSubCategoriesAntenatal()},
                new Category(Guid.Empty){Name = "Intrapartum", Categories = GetSubCategoriesIntrapartum()},
                new Category(Guid.Empty){Name = "Postnatal", Categories = GetSubCategoriesPostnatal()},
                new Category(Guid.Empty){Name = "Baby(s)"}
            };

            return categories;
        }
        public static List<Category> GetSubCategoriesProfile()
        {
            List<Category> categories = new List<Category>()
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

        public static List<Category> GetSubCategoriesConsultations()
        {
            List<Category> categories = new List<Category>()
            {
                new Category(Guid.Empty){Name = "Helpline Contacts"},
                new Category(Guid.Empty){Name = "Antenatal Consultations"},
                new Category(Guid.Empty){Name = "Hospital Stays & Visits"},
                new Category(Guid.Empty){Name = "Postnatal Consultations"},
            };

            return categories;
        }

        public static List<Category> GetSubCategoriesAntenatal()
        {
            List<Category> categories = new List<Category>()
            {
                new Category(Guid.Empty){Name = "Plan of Care"},
                new Category(Guid.Empty){Name = "Observations"},
                new Category(Guid.Empty){Name = "Specialisms"},
                new Category(Guid.Empty){Name = "Procedures"},
                new Category(Guid.Empty){Name = "Investigations"},
                new Category(Guid.Empty){Name = "Fetal Assessment"},
                new Category(Guid.Empty){Name = "Pregnancy Outcome"},
            };

            return categories;
        }

        public static List<Category> GetSubCategoriesIntrapartum()
        {
            List<Category> categories = new List<Category>()
            {
                new Category(Guid.Empty){Name = "Woman"},
                new Category(Guid.Empty){Name = "Baby"},
                new Category(Guid.Empty){Name = "Reporting"},
            };

            return categories;
        }

        public static List<Category> GetSubCategoriesPostnatal()
        {
            List<Category> categories = new List<Category>()
            {
                new Category(Guid.Empty){Name = "Postnatal Summary"},
                new Category(Guid.Empty){Name = "Observations"},
                new Category(Guid.Empty){Name = "Investigations"},
                new Category(Guid.Empty){Name = "Baby Checks"},
                new Category(Guid.Empty){Name = "Plan of Care"},
                new Category(Guid.Empty){Name = "Checklists"},
                new Category(Guid.Empty){Name = "Discharge From Care"},
            };

            return categories;
        }
    }
}
