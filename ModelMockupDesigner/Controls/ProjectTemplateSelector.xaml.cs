using ModelMockupDesigner.Enums;
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
    public partial class ProjectTemplateSelector : UserControl
    {
        public ProjectTemplateSelector()
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
                    case "FullAntenatal":
                        {
                            ChangeCheckStatus(ProjectTemplate.FullAntenatal);
                            break;
                        }
                    case "IntrapartumOnly":
                        {
                            ChangeCheckStatus(ProjectTemplate.IntrapartumOnly);
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void Checkbox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox? checkBox = sender as CheckBox;
            ProjectTemplate template = ProjectTemplate.FullAntenatal; //Default for now

            if (checkBox != null)
            {
                switch (checkBox.Name)
                {
                    case "FullAntenatal":
                        {
                            template = ProjectTemplate.FullAntenatal;
                            break;
                        }

                    case "IntrapartumOnly":
                        {
                            template = ProjectTemplate.IntrapartumOnly;
                            break;
                        }
                    default:
                        break;
                }

                if (template == currentTemplate)
                {
                    checkBox.Checked -= Checkbox_Checked;
                    checkBox.IsChecked = true;
                    checkBox.Checked += Checkbox_Checked;
                }
            }
        }

        private void ChangeCheckStatus(ProjectTemplate projectTemplate)
        {
            SuppressCheckChangeEvent = true;

            currentTemplate = projectTemplate;

            SetCheckStatus(CurrentTemplate);

            CurrentTemplate = projectTemplate;

            SuppressCheckChangeEvent = false;
        }

        private void SetCheckStatus(ProjectTemplate projectTemplate, bool check = false)
        {
            switch (projectTemplate)
            {
                case ProjectTemplate.FullAntenatal:
                    {
                        if (check)
                            FullAntenatal.IsChecked = true;
                        else
                            FullAntenatal.IsChecked = false;
                        break;
                    }
                case ProjectTemplate.IntrapartumOnly:
                    {
                        if (check)
                            IntrapartumOnly.IsChecked = true;
                        else
                            IntrapartumOnly.IsChecked = false;
                        break;
                    }
                default:
                    break;
            }
        }

        public ProjectTemplate CurrentTemplate
        {
            get => (ProjectTemplate)GetValue(CurrentTemplateProperty);
            set
            {
                currentTemplate = value;
                SetValue(CurrentTemplateProperty, value);
            }
        }

        private ProjectTemplate currentTemplate { get; set; }

        public static readonly DependencyProperty CurrentTemplateProperty =
            DependencyProperty.Register(nameof(CurrentTemplate), typeof(ProjectTemplate), typeof(ProjectTemplateSelector), new FrameworkPropertyMetadata(default(ProjectTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CurrentTemplatePropertyChanged));

        private static object CurrentTemplatePropertyChanged(DependencyObject d, object value)
        {
            ProjectTemplateSelector projectTemplateSelector = (ProjectTemplateSelector)d;
            ProjectTemplate template = (ProjectTemplate)value;

            if (projectTemplateSelector.currentTemplate != template)
            {
                projectTemplateSelector.SetCheckStatus(template, true);
            }

            return value;
        }
    }
}

