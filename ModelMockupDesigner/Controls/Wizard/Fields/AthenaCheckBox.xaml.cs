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
    /// Interaction logic for AthenaCheckBox.xaml
    /// </summary>
    public partial class AthenaCheckBox : UserControl, ICellControl
    {
        
        public AthenaCheckBox(CustomControl controlModel)
        {
            InitializeComponent();

            DataContext = controlModel;

            if (controlModel.ElementType == ElementType.CheckBox)
            {
                button.Visibility = Visibility.Collapsed;
            }
        }

        public ElementType ElementType => throw new NotImplementedException();

        public BaseModel? Model => DataContext as CustomControl;
    }
}
