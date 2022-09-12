using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using ModelMockupDesigner.Models.Wizard;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for EditorSection.xaml
    /// </summary>
    public partial class EditorSection : UserControl, IIsSelectable
    {
        #region Public Properties

        public BaseModel Model => SectionModel;

        #endregion

        #region Private Properties

#pragma warning disable CS8603 // Will always have a value
        private WizardSection SectionModel => DataContext as WizardSection;
#pragma warning restore CS8603


        #endregion

        #region Constructor

        public EditorSection()
        {
            InitializeComponent();
        }

        #endregion

        #region Methods

        public async Task LoadModel(WizardSection sectionModel)
        {
            DataContext = sectionModel;

            for (int i = 0; i <= sectionModel.WizardColumns.Count; i++)
            {
                container.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
            }

            foreach (WizardColumn wizardColumn in sectionModel.WizardColumns)
            {
                EditorColumn editorColumn = new(this);
                editorColumn.OnSelected += OnSelected;

                container.Children.Add(editorColumn);
                Grid.SetColumn(editorColumn, wizardColumn.Order);

                await editorColumn.LoadModel(wizardColumn);

            }
        }

        public void Unselect()
        {
            HeaderStackPanel.Background = Brushes.Transparent;
            HeaderTextBlock.Foreground = Application.Current.Resources["SectionPinkBrush"] as SolidColorBrush;
            HeaderTextBlock.Background = Brushes.White;
            Border.Fill = Brushes.Transparent;
        }
        public void Delete()
        {

        }
        public void Delete(EditorColumn child)
        {
            if (SectionModel != null && child.Model != null)
            {
                _ = SectionModel.WizardColumns.Remove((WizardColumn)child.Model);
                container.Children.Remove(child);
            }
        }

        private async Task AddColumn()
        {
            if (SectionModel == null)
                return;

            container.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            WizardColumn wizardColumn = new(SectionModel);
            wizardColumn.CreateNew();

            SectionModel.WizardColumns.Add(wizardColumn);

            EditorColumn editorColumn = new(this);
            editorColumn.OnSelected += OnSelected;

            container.Children.Add(editorColumn);
            Grid.SetColumn(editorColumn, wizardColumn.Order);

            await editorColumn.LoadModel(wizardColumn);
        }

        #endregion

       
        #region Events

        public EventHandler<IIsSelectable>? OnSelected;
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HeaderStackPanel.Background = Application.Current.Resources["SectionPinkBrush"] as SolidColorBrush;
            HeaderTextBlock.Foreground = Brushes.White;
            HeaderTextBlock.Background = Brushes.Transparent;
            Border.Fill = Brushes.Yellow;

            OnSelected?.Invoke(this, this);
        }
        private void HeaderStackPanel_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            ContextMenu contextMenu = new ContextMenu();

            MenuItem menuItem = new MenuItem() { Header = "Add New Column" };
            menuItem.Click += MenuItem_Click;
            contextMenu.Items.Add(menuItem);

            contextMenu.IsOpen = true;

            HeaderStackPanel.ContextMenu = contextMenu;

            e.Handled = true;
        }
        private async void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem)
            {
                switch (menuItem.Header)
                {
                    case "Add New Column":
                        {
                            await AddColumn();
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}
