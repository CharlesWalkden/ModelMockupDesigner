using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.Data
{
    public static class DataStore
    {
        public static Dictionary<ElementType, ElementDefinition> AllControls = new Dictionary<ElementType, ElementDefinition>()
        {
            {ElementType.ApproxDate, new ElementDefinition(){Required=true,Scale=true,DesignGroup = DesignGroup.Fields,
                InterchangeableTypes = new List<ElementType>()
                {
                    ElementType.Date,
                    ElementType.DateTime,
                    ElementType.Time
                }
            }},
            {ElementType.Date,  new ElementDefinition(){Required=true,Scale=true,DesignGroup = DesignGroup.Fields,
                InterchangeableTypes = new List<ElementType>()
                {
                    ElementType.ApproxDate,
                    ElementType.DateTime,
                    ElementType.Time
                }
            }},
            {ElementType.DateTime,  new ElementDefinition(){Required=true,Scale=true,DesignGroup = DesignGroup.Fields,
                InterchangeableTypes = new List<ElementType>()
                {
                    ElementType.Date,
                    ElementType.Time,
                    ElementType.ApproxDate
                }
            }},
            {ElementType.CheckBox,  new ElementDefinition(){Required=true,Scale=true,DesignGroup = DesignGroup.Fields} },
            {ElementType.CheckBoxList,  new ElementDefinition(){Required=true,Scale=true,DesignGroup = DesignGroup.Fields} },
            {ElementType.DropDownList,  new ElementDefinition(){Required=true,Scale=true,DesignGroup = DesignGroup.Fields} },
            {ElementType.YesNo,  new ElementDefinition(){Required=true,Scale=true,DesignGroup = DesignGroup.Fields} },
            {ElementType.TextBox,  new ElementDefinition(){Required=true,Scale=true,DesignGroup = DesignGroup.Fields} },
            {ElementType.RadioList,  new ElementDefinition(){Required=true,Scale=true,DesignGroup = DesignGroup.Fields} },
            {ElementType.Time,  new ElementDefinition(){Required=true,Scale=true,DesignGroup = DesignGroup.Fields} },
            {ElementType.Grid, new ElementDefinition(){Required=true,Scale=true,DesignGroup = DesignGroup.Layout} },
            {ElementType.Label, new ElementDefinition(){Required=true,Scale=true,DesignGroup = DesignGroup.Layout} }
        };
    
        public static Dictionary<ElementType, List<ElementType>> GetControlGroups()
        {
            Dictionary<ElementType, List<ElementType>> controls = new Dictionary<ElementType, List<ElementType>>()
            {
                {ElementType.YesNo, new List<ElementType>()
                    {
                        ElementType.CheckBox,
                    } 
                },
                { ElementType.Date, new List<ElementType>()
                    {
                        ElementType.DateTime,
                        ElementType.ApproxDate,
                        ElementType.ApproxDateTextBox,
                        ElementType.Time
                    }
                },
                {ElementType.TextBox, new List<ElementType>()
                    {
                        ElementType.TextSuggestion,
                        ElementType.MultiLineTextBox,
                        ElementType.NumericTextBox,
                    }
                },
                {ElementType.RadioList, new List<ElementType>()
                    {
                        ElementType.CheckBoxList
                    }
                }

            };

            return controls;
        }

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
