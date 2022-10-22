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
                    AddTitle(currentSelection.HeaderName, 18);
                }

                // Add all group box properties to edit if it can have one.
                if (currentSelection is IGroupBoxContent boxContent)
                {
                    AddTitle("GroupBox", 12);

                    AddProperty(boxContent, "DisplayGroupbox", "Display"); 
                    AddProperty(boxContent, "GroupBoxTitle", "Title");
                    AddProperty(boxContent, "GroupHorizontalAlignment", "Horizontal");
                    AddProperty(boxContent, "GroupVerticalAlignment", "Vertical");
                }

                // Have this becasue things will have different properties to edit. Elements + controls etc
                Dictionary<string, string> properties = currentSelection.GetEditableProperties();

                if (properties.Count > 0)
                {
                    AddTitle("Control", 12);

                    foreach (KeyValuePair<string,string> property in properties)
                    {
                        PropertyInfo info = currentSelection.GetType().GetProperty(property.Value);
                        if (info != null && info.CanRead && info.CanWrite)
                        {
                            AddProperty(currentSelection, info, property.Key);
                        }
                    }
                }
            }
        }

        private void AddTitle(string title, double fontSize = 16)
        {
            root.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            int rowIndex = root.RowDefinitions.Count - 1;

            TextBlock block = new TextBlock() { Text = title, FontWeight = FontWeights.Bold, FontSize = fontSize, TextDecorations = TextDecorations.Underline, Margin = new Thickness(5, 15, 5, 0), HorizontalAlignment = HorizontalAlignment.Center };
            root.Children.Add(block);
            Grid.SetRow(block, rowIndex);
            Grid.SetColumnSpan(block, 2);
        }
        private void AddProperty(object item, string propertyName, string displayName)
        {
            PropertyInfo propertyInfo = item.GetType().GetProperty(propertyName);
            if (propertyInfo != null)
            {
                AddProperty(item, propertyInfo, displayName);
            }
        }
        private void AddProperty(object item, PropertyInfo propertyInfo, string displayName)
        {
            root.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });

            int rowIndex = root.RowDefinitions.Count - 1;

            TextBlock label = new TextBlock() { Text = displayName, FontSize = 13.333, Margin = new Thickness(5), HorizontalAlignment = HorizontalAlignment.Right, VerticalAlignment = VerticalAlignment.Center };

            root.Children.Add(label);
            Grid.SetRow(label, rowIndex);

            FrameworkElement editingControl = GetEditingElement(item, propertyInfo);

            if (editingControl != null)
            {
                root.Children.Add(editingControl);
                Grid.SetColumn(editingControl, 1);
                Grid.SetRow(editingControl, rowIndex);
            }
        }

        private FrameworkElement GetEditingElement(object item, PropertyInfo property)
        {
            FrameworkElement element;

            if (property.PropertyType.IsEnum)
            {
                ComboBox comboBox = new ComboBox();
                comboBox.HorizontalAlignment = HorizontalAlignment.Left;
                comboBox.Margin = new Thickness(5);
                foreach (var propertyInfo in Enum.GetValues(property.PropertyType))
                {
                    comboBox.Items.Add(propertyInfo);
                }
                Binding binding = new Binding(property.Name) { Source = item, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged};
                comboBox.SetBinding(ComboBox.SelectedItemProperty, binding);

                element = comboBox;
            }
            else if (property.PropertyType == typeof(bool))
            {
                CheckBox checkBox = new CheckBox() { Margin = new Thickness(5) };
                checkBox.HorizontalAlignment = HorizontalAlignment.Left;

                Binding binding = new Binding(property.Name) { Source = item, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                checkBox.SetBinding(CheckBox.IsCheckedProperty, binding);

                element = checkBox;
            }
            else
            {
                TextBox textBox = new TextBox() { Margin = new Thickness(5), Width = 170 };
                textBox.Style = Application.Current.Resources["genericTextBox"] as Style;
                textBox.HorizontalAlignment = HorizontalAlignment.Left;

                Binding binding = new Binding(property.Name) { Source = item, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
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

        private IPropertyEditor currentSelection { get; set; } 

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
