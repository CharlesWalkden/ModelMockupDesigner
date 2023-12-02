using ModelMockupDesigner.Data;
using ModelMockupDesigner.Enums;
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

namespace ModelMockupDesigner.Controls
{
    /// <summary>
    /// Interaction logic for ControlSelector.xaml
    /// </summary>
    public partial class ControlSelector : UserControl, IDialogClient
    {
        public event EventHandler<DialogEventArgs> OnClose;

        public ControlSelectorViewModel ViewModel { get => DataContext as ControlSelectorViewModel; }

        public ControlSelector()
        {
            InitializeComponent();
            DataContext = new ControlSelectorViewModel();
            Loaded += ControlSelector_Loaded;
            Unloaded += ControlSelector_Unloaded;
        }

        private void ControlSelector_Unloaded(object sender, RoutedEventArgs e)
        {
            OnClose?.Invoke(sender, new DialogEventArgs());
        }

        private async void ControlSelector_Loaded(object sender, RoutedEventArgs e)
        {
            await BuildAndDisplayControls();
        }

        private async Task BuildAndDisplayControls()
        {
            ElementDefinition elementDefinition = DataStore.AllControls[ViewModel.OriginalElement];
            foreach (ElementType element in elementDefinition.InterchangeableTypes)
            {

                Border controlBorder = new Border();
                controlBorder.BorderBrush = new SolidColorBrush(Color.FromRgb(50, 62, 73));
                controlBorder.Background = Brushes.Transparent;
                controlBorder.BorderThickness = new Thickness(2);
                controlBorder.Margin = new Thickness(2);
                controlBorder.Padding = new Thickness(2);
                controlBorder.Opacity = 0.5;

                controlBorder.MouseEnter += ControlBorder_MouseEnter;
                controlBorder.MouseDown += ControlBorder_MouseDown;
                controlBorder.MouseLeave += ControlBorder_MouseLeave;

                FrameworkElement control = await CustomControlGenerator.GetControl(new Models.CustomControl(element));
                if (control != null)
                {
                    controlBorder.Child = control;
                    controlBorder.Child.IsEnabled = false;
                    // TODO: Wrap the control, make it look pretty.
                    root.Children.Add(controlBorder);
                }
            }
        }

        private void ControlBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                border.Opacity = 0.5;
            }
        }

        private void ControlBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Border border)
            {
                FrameworkElement control = border.Child as FrameworkElement;
                CustomControl model = control.DataContext as CustomControl;

                ViewModel.SelectedControl = model.ElementType;
                OnClose?.Invoke(this, new DialogEventArgs() { Result = DialogResult.Accept});
            }
        }

        private void ControlBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Border border)
            {
                border.Opacity = 1;
            }
        }
    }
}
