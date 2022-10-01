using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.Interfaces
{
    public interface IWizardModel
    {
        string? Name { get; set; }
        WizardType? WizardType { get; }
        Category? Category { get; set; }
    }
}
