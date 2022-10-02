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

namespace ModelMockupDesigner.Controls
{
    /// <summary>
    /// Interaction logic for RecentProjectListItem.xaml
    /// </summary>
    public partial class RecentProjectListItem : UserControl
    {
        public RecentProjectListItem()
        {
            InitializeComponent();
        }

        private void mainBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                if (border.DataContext is RecentProjectListItemViewModel vm)
                {
                    if (vm.Selected)
                        return;
                }
                if (border.Child != null)
                {
                    if (border.Child is Grid grid)
                    {
                        grid.Opacity = 0.5;
                    }
                }
            }
        }

        private void mainBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                if (border.Child != null)
                {
                    if (border.Child is Grid grid)
                    {
                        grid.Opacity = 1;
                    }
                }
            }
        }
    }
}
