using ModelMockupDesigner.Controls;
using ModelMockupDesigner.Models.Wizard;
using ModelMockupDesigner.ViewModels;
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
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : UserControl
    {
        public Wizard? WizardModel { get; set; }

        public Editor(WizardEditorViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        public async Task LoadEditor(Guid identifier)
        {
            await CreateWizardModel(identifier);

            await LoadUI();
            
        }
        private async Task CreateWizardModel(Guid wizardId)
        {
            WizardModel = new Wizard();

            // Add events if needed.

            await WizardModel.BuildWizard(wizardId);
        }
        private async Task LoadUI()
        {
            ContentContainer.Children.Clear();

            if (WizardModel == null)
                return;

            foreach (WizardSection wizardSection in WizardModel.Sections)
            {
                EditorSection editorSection = new EditorSection();
                // Add events when created.

                if (ContentContainer.Children.Count == 0)
                {
                    // Adding first page
                    ContentContainer.Children.Add(editorSection);
                    await editorSection.LoadModel(wizardSection);
                }
            }
        }

    }
}
