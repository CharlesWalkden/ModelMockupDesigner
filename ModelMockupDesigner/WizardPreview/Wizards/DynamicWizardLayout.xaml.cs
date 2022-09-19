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
            Manager = new DynamicWizardManager(this);
        }

        public async Task LoadWizard(DynamicWizard wizard)
        {
            Manager.LoadWizard(wizard);
        }

        public async Task<DynamicWizardPageLayout> DisplayPage(DynamicWizardSection pageModel)
        {
            DynamicWizardPageLayout? newPage = null;
            container.Children.Clear();

            if (pageModel != null)
            {
                newPage = new(pageModel);
                await newPage.Build();

                container.Children.Add(newPage);
            }

            return newPage;
        }

        public void ShowNavButtons(bool showPrevious, bool showNext, bool showFinished)
        {
            if (showPrevious)
            {
                previousButton.Visibility = Visibility.Visible;
            }
            else
            {
                previousButton.Visibility = Visibility.Collapsed;
            }

            if (showNext)
            {
                nextButton.Visibility = Visibility.Visible;
            }
            else
            {
                nextButton.Visibility = Visibility.Collapsed;
            }

            if (showFinished)
            {
                finishButton.Visibility = Visibility.Visible;
            }
            else
            {
                finishButton.Visibility = Visibility.Collapsed;
            }
        }
    }
}
