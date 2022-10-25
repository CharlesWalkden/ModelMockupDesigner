using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
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
    public partial class PreviewWizardLayout : UserControl
    {
        public event EventHandler OnPreviousPressed;
        public event EventHandler OnNextPressed;

        private readonly IWizardManager Manager;

        public PreviewWizardLayout(WizardType wizardType)
        {
            InitializeComponent();
            
            switch (wizardType)
            {
                case WizardType.Static:
                    {
                        break;
                    }
                case WizardType.Dynamic:
                    {
                        Manager = new DynamicWizardManager(this);
                        break;
                    }
            }
        }

        public async Task Reload()
        {
            if (Manager != null)
                await Manager.Reload();
        }

        public async Task LoadWizard(IWizardModel wizardModel) 
        {
            if (Manager != null)
                await Manager.LoadWizard(wizardModel);
        }

        public async Task DisplayPage(IWizardPageLayout pageLayout)
        {
            wizardContainer.Children.Clear();
            

            if (pageLayout != null)
                await pageLayout.Build();

            if (pageLayout is FrameworkElement frameworkElement)
                wizardContainer.Children.Add(frameworkElement);

        }
        public double GetHeight()
        {
            return Manager.Height;
        }
        public double GetWidth()
        {
            return Manager.Width;
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
        private void previousButton_Click(object sender, RoutedEventArgs e)
        {
            OnPreviousPressed?.Invoke(this, new EventArgs());
        }
        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            OnNextPressed?.Invoke(this, new EventArgs());
        }
    }
}
