using ModelMockupDesigner.Models;
using ModelMockupDesigner.WizardPreview;
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

namespace ModelMockupDesigner
{
    /// <summary>
    /// Interaction logic for DynamicWizardLayout.xaml
    /// </summary>
    public partial class DynamicWizardLayout : UserControl
    {
        public event EventHandler OnPreviousPressed;
        public event EventHandler OnNextPressed;

        private DynamicWizardManager Manager;
        public DynamicWizardLayout()
        {
            InitializeComponent();
        }

        public async void LoadWizard(DynamicWizard wizard)
        {
            Manager.LoadWizard(wizard);
        }

        public async void DisplayPage(DynamicWizardSection page)
        {

        }
    }
}
