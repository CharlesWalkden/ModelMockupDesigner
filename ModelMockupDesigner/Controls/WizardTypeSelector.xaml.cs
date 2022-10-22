using ModelMockupDesigner.Enums;
using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Interaction logic for WizardTypeSelector.xaml
    /// </summary>
    public partial class WizardTypeSelector : UserControl
    {
        public WizardTypeSelector()
        {
            InitializeComponent();
        }

        private bool SuppressCheckChangeEvent = false;


        private void Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            if (SuppressCheckChangeEvent)
                return;

            if (sender is CheckBox checkbox)
            {
                switch (checkbox.Name)
                {
                    case "Static":
                        {
                            ChangeCheckStatus(WizardType.Static);
                            break;
                        }
                    case "Dynamic":
                        {
                            ChangeCheckStatus(WizardType.Dynamic);
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            WizardType type = WizardType.Dynamic; //Default for now

            if (checkBox != null)
            {
                switch (checkBox.Name)
                {
                    case "Static":
                        {
                            type = WizardType.Static;
                            break;
                        }

                    case "Dynamic":
                        {
                            type = WizardType.Dynamic;
                            break;
                        }
                    default:
                        break;
                }

                if (type == currentType)
                {
                    checkBox.Checked -= Checkbox_Checked;
                    checkBox.IsChecked = true;
                    checkBox.Checked += Checkbox_Checked;
                }
            }
        }

        private void ChangeCheckStatus(WizardType wizardType)
        {
            SuppressCheckChangeEvent = true;

            currentType = wizardType;

            SetCheckStatus(CurrentType);

            CurrentType = wizardType;

            SuppressCheckChangeEvent = false;
        }

        private void SetCheckStatus(WizardType wizardType, bool check = false)
        {
            switch (wizardType)
            {
                case WizardType.Static:
                    {
                        if (check)
                            Static.IsChecked = true;
                        else
                            Static.IsChecked = false;
                        break;
                    }
                case WizardType.Dynamic:
                    {
                        if (check)
                            Dynamic.IsChecked = true;
                        else
                            Dynamic.IsChecked = false;
                        break;
                    }
                default:
                    break;
            }
        }

        public WizardType CurrentType
        {
            get => (WizardType)GetValue(CurrentTypeProperty);
            set
            {
                currentType = value;
                SetValue(CurrentTypeProperty, value);
            }
        }

        private WizardType currentType { get; set; }

        public static readonly DependencyProperty CurrentTypeProperty =
            DependencyProperty.Register(nameof(CurrentType), typeof(WizardType), typeof(WizardTypeSelector), new FrameworkPropertyMetadata(default(WizardType), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CurrentTypePropertyChanged));

        private static object CurrentTypePropertyChanged(DependencyObject d, object value)
        {
            WizardTypeSelector wizardTypeSelector = (WizardTypeSelector)d;
            WizardType type = (WizardType)value;

            if (wizardTypeSelector.currentType != type)
            {
                wizardTypeSelector.SetCheckStatus(type, true);
            }

            return value;
        }
    }
}
