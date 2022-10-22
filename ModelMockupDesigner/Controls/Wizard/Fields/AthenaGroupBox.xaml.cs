using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ModelMockupDesigner.Controls
{
    /// <summary>
    /// Interaction logic for AthenaGroupBox.xaml
    /// </summary>
    public partial class AthenaGroupBox : UserControl, ICellControl
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
        private FrameworkElement ContentControl { get; set; }
        public ElementType ElementType => ElementType.GroupBox;

        public BaseModel Model => ContentControl?.DataContext as BaseModel;
        public bool DisplayGroupbox { get => false; } 
        public void SetContent(FrameworkElement element)
        {
            if (element != null)
            {
                ContentControl = element;
                mainContent.Children.Add(element);
                Grid.SetRow(element, 1);
            }
        }
        public void RemoveContent(FrameworkElement element)
        {
            ContentControl = null;
            mainContent.Children.Remove(element);
        }
        public void Initialise(IGroupBoxContent content)
        {
            VerticalAlignment = content.GroupVerticalAlignment.ToXaml();
            HorizontalAlignment = content.GroupHorizontalAlignment.ToXaml();
            Text = content.GroupBoxTitle;
        }
        public void Initialise(GroupBoxDisplayChangedEventArgs e)
        {
            VerticalAlignment = e.VerticalAlignment.ToXaml();
            HorizontalAlignment = e.HorizontalAlignment.ToXaml();
            Text = e.GroupBoxTitle;
        }
    }
}
