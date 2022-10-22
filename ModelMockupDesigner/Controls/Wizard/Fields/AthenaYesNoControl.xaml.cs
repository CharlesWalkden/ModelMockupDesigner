using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModelMockupDesigner.Controls
{
    /// <summary>
    /// Interaction logic for BooleanButton.xaml
    /// </summary>
    public partial class AthenaYesNoControl : UserControl, ICellControl
    {
        public BaseModel Model { get => ControlModel; }

        public ElementType ElementType => ElementType.YesNo;

        public bool DisplayGroupbox => ControlModel?.DisplayGroupbox ?? false;

        private CustomControl ControlModel => DataContext as CustomControl;

        public AthenaYesNoControl(CustomControl customControlModel)
        {
            InitializeComponent();
            DataContext = customControlModel;
        }
    }
}
