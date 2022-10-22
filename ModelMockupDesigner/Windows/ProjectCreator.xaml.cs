using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
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

namespace ModelMockupDesigner.Windows
{
    /// <summary>
    /// Interaction logic for ProjectCreator.xaml
    /// </summary>
    public partial class ProjectCreator : UserControl, IDialogClient 
    {
        #region Interface

        public event EventHandler<DialogEventArgs> OnClose;

        #endregion

        public ProjectCreatorViewModel ViewModel { get => DataContext as ProjectCreatorViewModel; }
        public DialogResult DialogResult { get; set; }

        public ProjectCreator()
        {
            InitializeComponent();

            ProjectCreatorViewModel viewModel = new ProjectCreatorViewModel();

            DataContext = viewModel;
        }

        private void Accept_Click(object sender, RoutedEventArgs e) 
        {
            List<string> mandatoryFields = ViewModel?.ValidateData() ?? new List<string>();

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < mandatoryFields.Count; i++)
            {
                sb.Append(mandatoryFields[i] + " ,");
            }

            if (mandatoryFields.Count > 0)
            {
                MessageBox.Show($"The following fields have not been recorded: {sb.ToString()}");
            }
            else
            {
                DialogResult = DialogResult.Accept;
                OnClose?.Invoke(this, new DialogEventArgs(DialogResult));
            }

        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            OnClose?.Invoke(this, new DialogEventArgs(DialogResult));
        }
    }
}
