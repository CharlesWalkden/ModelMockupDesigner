using ModelMockupDesigner.Interfaces;
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

namespace ModelMockupDesigner.Controls.Wizard.Fields
{
    /// <summary>
    /// Interaction logic for AthenaGroupBox.xaml
    /// </summary>
    public partial class AthenaGroupBox : UserControl
    {
        public AthenaGroupBox()
        {
            InitializeComponent();
        }

        public string Text
        {
            get => label.Text;
            set
            {
                label.Text = value;
                if (string.IsNullOrEmpty(value))
                {
                    labelBackground.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 255, 255, 255));
                }
                else
                {
                    labelBackground.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 36, 164, 200));
                }
            }
        }
        public void SetContent(FrameworkElement element)
        {
            mainContent.Children.Add(element);
            Grid.SetRow(element, 1);
        }
        public void RemoveContent(FrameworkElement element)
        {
            mainContent.Children.Remove(element);
        }
        public void Initialise(IGroupBoxContent content)
        {
            VerticalAlignment = content.GroupVerticalAlignment.ToXaml();
            HorizontalAlignment = content.GroupHorizontalAlignment.ToXaml();
            Text = content.GroupBoxTitle;
        }
    }
}
