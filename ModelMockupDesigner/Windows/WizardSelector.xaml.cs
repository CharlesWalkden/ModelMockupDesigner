using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
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
    /// Interaction logic for WizardSelector.xaml
    /// </summary>
    public partial class WizardSelector : UserControl, IWindowStack
    {
        public WizardSelector(Project project)
        {
            InitializeComponent();
            WizardSelectorViewModel viewModel = new()
            {
                Model = project
            };

            DataContext = viewModel;
        }




        #region Interface

        public event EventHandler? OnClosed;

        public void CloseAsync()
        {
            OnClosed?.Invoke(this, new EventArgs());
        }

        #endregion

    }
}
