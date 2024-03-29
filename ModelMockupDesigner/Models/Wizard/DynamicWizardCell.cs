﻿using ModelMockupDesigner.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace ModelMockupDesigner.Models
{
    public class DynamicWizardCell : BaseModel 
    {
        public ICellParent Parent { get; set; }

        public int Row { get; set; } = 0;
        public int Column { get; set; } = 0;

        public ICellControl Control { get; set; }

        public DynamicWizardCell(ICellParent parent)
        {
            Parent = parent;
        }

        #region Xml

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
