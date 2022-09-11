﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner.Interfaces
{
    public interface IDialogClient
    {
        event EventHandler<DialogEventArgs> OnClose;
    }
}
