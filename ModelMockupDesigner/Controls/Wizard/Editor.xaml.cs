﻿using ModelMockupDesigner.Controls;
using ModelMockupDesigner.Enums;
using ModelMockupDesigner.Interfaces;
using ModelMockupDesigner.Models;
using ModelMockupDesigner.Models.Wizard;
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

namespace ModelMockupDesigner
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor : UserControl
    {
        public Wizard? WizardModel { get; set; }

        public IIsSelectable? CurrentSelection { get; set; }

        public Editor(WizardEditorViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }

        public async Task LoadEditor(Guid identifier)
        {
            await CreateWizardModel(identifier);

            await LoadUI();
            
        }
        private async Task CreateWizardModel(Guid wizardId)
        {
            WizardModel = new Wizard();

            // Add events if needed.

            await WizardModel.BuildWizard(wizardId);
        }
        private async Task LoadUI()
        {
            ContentContainer.Children.Clear();

            if (WizardModel == null)
                return;

            foreach (WizardSection wizardSection in WizardModel.Sections)
            {
                EditorSection editorSection = new();
                editorSection.OnSelected += UpdateCurrentSelection;

                if (ContentContainer.Children.Count == 0)
                {
                    // Adding first page
                    ContentContainer.Children.Add(editorSection);
                    await editorSection.LoadModel(wizardSection);
                }
            }
        }

        private void UpdateCurrentSelection(object? sender, IIsSelectable newSelection)
        {
            if (CurrentSelection != null)
            {
                if (CurrentSelection.Equals(newSelection))
                {
                    return;
                }
                else
                {
                    CurrentSelection.Unselect();
                }
            }

            CurrentSelection = newSelection;
            
        }

        #region Temp new field item drag

        private Border? Source { get; set; }
        private NewControl? SelectedControl { get; set; }
        private void Border_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (Source != null && SelectedControl != null)
                    DragDrop.DoDragDrop(Source, SelectedControl, DragDropEffects.All);
            }
        }

        private void Border_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Source = (Border)sender;
            SelectedControl = new NewControl() {ElementType = ElementType.Table}; 
        }

        #endregion
    }
}
