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

namespace ModelMockupDesigner.Controls
{
    /// <summary>
    /// Interaction logic for ListCreator.xaml
    /// </summary>
    public partial class ListCreator : UserControl, IDialogClient
    {
        #region Interface

        public event EventHandler<DialogEventArgs>? OnClose;

        #endregion

        public ListCreatorViewModel? ViewModel { get => DataContext as ListCreatorViewModel; }
        public DialogResult DialogResult { get; set; }
        public ListCreator()
        {
            InitializeComponent();
            DataContext = new ListCreatorViewModel();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Accept;
            OnClose?.Invoke(this, new DialogEventArgs(DialogResult));
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            OnClose?.Invoke(this, new DialogEventArgs(DialogResult));
        }

    }
}
