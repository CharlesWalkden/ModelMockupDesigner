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
    /// Interaction logic for WizardEditor.xaml
    /// </summary>
    public partial class WizardEditor : UserControl
    {
        public WizardEditor()
        {
            InitializeComponent();
            Editor editor = new Editor(new());
            editor.LoadEditor(Guid.Empty);

            root.Children.Add(editor);
        }
    }
}
