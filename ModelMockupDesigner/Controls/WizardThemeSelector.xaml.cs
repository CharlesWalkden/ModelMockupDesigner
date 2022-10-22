using ModelMockupDesigner.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
    /// Interaction logic for WizardThemeSelector.xaml
    /// </summary>
    public partial class WizardThemeSelector : UserControl
    {
        public WizardThemeSelector()
        {
            InitializeComponent();
        }

        private bool SuppressCheckChangeEvent = false; 

        private void Checkbox_Checked(object sender, RoutedEventArgs e)
        {
            if (SuppressCheckChangeEvent)
                return;

            if (sender is CheckBox checkBox)
            {
                switch (checkBox.Name)
                {
                    case "V5":
                        {
                            ChangeCheckStatus(WizardTheme.V5);
                            break;
                        }
                    case "V6":
                        {
                            ChangeCheckStatus(WizardTheme.V6);
                            break;
                        }
                    case "V7":
                        {
                            ChangeCheckStatus(WizardTheme.V7);
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
            WizardTheme theme = WizardTheme.V7; // Default for now

            if (checkBox != null)
            {
                switch (checkBox.Name)
                {
                    case "V5":
                        {
                            theme = WizardTheme.V5;
                            break;
                        }
                    case "V6":
                        {
                            theme = WizardTheme.V6;
                            break;
                        }
                    case "V7":
                        {
                            theme = WizardTheme.V7;
                            break;
                        }
                    default:
                        break;
                }

                if (theme == currentTheme)
                {
                    checkBox.Checked -= Checkbox_Checked;
                    checkBox.IsChecked = true;
                    checkBox.Checked += Checkbox_Checked;
                }
            }
        }

        private void ChangeCheckStatus(WizardTheme wizardTheme)
        {
            SuppressCheckChangeEvent = true;

            currentTheme = wizardTheme;

            SetCheckStatus(CurrentTheme);

            CurrentTheme = wizardTheme;

            SuppressCheckChangeEvent = false;
        }

        private void SetCheckStatus(WizardTheme wizardTheme, bool check = false)
        {
            switch (wizardTheme)
            {
                case WizardTheme.V5:
                    {
                        if (check)
                            V5.IsChecked = true;
                        else
                            V5.IsChecked = false;
                        break;
                    }
                case WizardTheme.V6:
                    {
                        if (check)
                            V6.IsChecked = true;
                        else
                            V6.IsChecked = false;
                        break;
                    }
                case WizardTheme.V7:
                    {
                        if (check)
                            V7.IsChecked = true;
                        else
                            V7.IsChecked = false;
                        break;
                    }
                default:
                    break;
            }
        }

        public WizardTheme CurrentTheme
        {
            get => (WizardTheme)GetValue(CurrentThemeProperty);
            set
            {
                currentTheme = value;
                SetValue(CurrentThemeProperty, value);
            }
        }
        private WizardTheme currentTheme { get; set; }


        public static readonly DependencyProperty CurrentThemeProperty =
            DependencyProperty.Register(nameof(CurrentTheme), typeof(WizardTheme), typeof(WizardThemeSelector), new FrameworkPropertyMetadata(default(WizardTheme), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CurrentThemePropertyChanged));

        private static object CurrentThemePropertyChanged(DependencyObject d, object value)
        {
            WizardThemeSelector wizardThemeSelector = (WizardThemeSelector)d;
            WizardTheme theme = (WizardTheme)value;

            if (wizardThemeSelector.currentTheme != theme)
            {
                wizardThemeSelector.SetCheckStatus(theme, true);
            }

            return value;
        }
    }
}
