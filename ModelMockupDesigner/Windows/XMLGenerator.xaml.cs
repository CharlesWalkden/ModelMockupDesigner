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
    /// Interaction logic for XMLGenerator.xaml
    /// </summary>
    public partial class XMLGenerator : UserControl
    {
        public XMLGenerator()
        {
            InitializeComponent();
        }
        private void LoadFieldsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearFieldsButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GenerateSelected_Click(object sender, RoutedEventArgs e)
        {

        }
        private void GenerateAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ClearOutputButton_Click(object sender, RoutedEventArgs e)
        {
            XMLOutputTextbox = null;
        }


    }
}
