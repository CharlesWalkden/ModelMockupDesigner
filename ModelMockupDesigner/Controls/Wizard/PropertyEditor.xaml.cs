using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for PropertyEditor.xaml
    /// </summary>
    public partial class PropertyEditor : UserControl
    {
        public PropertyEditor()
        {
            InitializeComponent();
        }
        private void Clear()
        {
            root.RowDefinitions.Clear();
            root.Children.Clear();
        }
        private void Build(IPropertyEditor currentSelection) 
        {
            Clear();

            if (currentSelection != null)
            {
                if (!string.IsNullOrEmpty(currentSelection.HeaderName))
                {
                    AddTitle(currentSelection.HeaderName);
                }

                List<string> properties = currentSelection.GetEditableProperties();

                foreach (string property in properties)
                {
                    PropertyInfo? info = currentSelection.GetType().GetProperty(property);
                    if (info != null && info.CanRead && info.CanWrite)
                    {
                        AddProperty(currentSelection, info);
                    }
                }
            }

        }

        private void AddTitle(string title)
        {
            root.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            int rowIndex = root.RowDefinitions.Count - 1;

            TextBlock block = new() { Text = title, FontWeight = FontWeights.Bold, FontSize = 16, TextDecorations = TextDecorations.Underline, Margin = new Thickness(5, 15, 5, 0), HorizontalAlignment = HorizontalAlignment.Center };
            root.Children.Add(block);
            Grid.SetRow(block, rowIndex);
            Grid.SetColumnSpan(block, 2);
        }
        private void AddProperty(object item, PropertyInfo propertyInfo)
        {
            root.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            int rowIndex = root.RowDefinitions.Count - 1;

            TextBlock label = new() { Text = propertyInfo.Name, Margin = new Thickness(5), HorizontalAlignment = HorizontalAlignment.Right };

            root.Children.Add(label);
            Grid.SetRow(label, rowIndex);

            FrameworkElement? editingControl = GetEditingElement(item, propertyInfo);

            if (editingControl != null)
            {
                root.Children.Add(editingControl);
                Grid.SetColumn(editingControl, 1);
                Grid.SetRow(editingControl, rowIndex);
            }

        }

        private FrameworkElement? GetEditingElement(object item, PropertyInfo property)
        {
            FrameworkElement? element = null;

            if (property.PropertyType == typeof(string))
            {
                TextBox textBox = new() { Margin = new Thickness(5), Width = 200 };

                Binding binding = new(property.Name) { Source = item, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                textBox.SetBinding(TextBox.TextProperty, binding);

                element = textBox;
            }

            return element;
        }


        #region DependencyProperty

        public IPropertyEditor CurrentSelection
        {
            get => (IPropertyEditor)GetValue(CurrentSelectionProperty);
            set
            {
                currentSelection = value;
                SetValue(CurrentSelectionProperty, value);
            }
        }

        private IPropertyEditor? currentSelection { get; set; } 

        public static readonly DependencyProperty CurrentSelectionProperty =
            DependencyProperty.Register(nameof(CurrentSelection), typeof(IPropertyEditor), typeof(PropertyEditor), new FrameworkPropertyMetadata(default(IPropertyEditor), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, null, CurrentSelectionPropertyChanged));

        private static object CurrentSelectionPropertyChanged(DependencyObject d, object value)
        {
            PropertyEditor propertyEditor = (PropertyEditor)d;
            IPropertyEditor currentSelection = (IPropertyEditor)value;

            if (propertyEditor != null && currentSelection != null)
            {
                propertyEditor.Build(currentSelection);
            }
            else if (propertyEditor != null)
            {
                propertyEditor.Clear();
            }

            return value;
        }

        #endregion
    }
}
