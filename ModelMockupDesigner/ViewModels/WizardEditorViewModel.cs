﻿using ModelMockupDesigner.Models.Wizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ModelMockupDesigner.ViewModels
{
    public class WizardEditorViewModel : BaseViewModel
    {
        public string? WizardName
        {
            get => wizardName;
            set
            {
                if (wizardName == value)
                    return;

                wizardName = value;
                OnPropertyChanged(nameof(WizardName));
            }
        }
        
        public ComboBoxItem? CurrentPage
        {
            get => currentPage;
            set
            {
                if (currentPage != null && value != null && currentPage.Value == value.Value)
                    return;

                currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
        }
        private ComboBoxItem? currentPage { get; set; }

        public CollectionList<ComboBoxItem> PageList { get; set; }

        public WizardEditorViewModel()
        {
            PageList = new CollectionList<ComboBoxItem>();
        }

        public void SetCurrentPage(int order) => CurrentPage = PageList.FirstOrDefault(x => (int)x.Value == order);
        public void DeleteCurrentPage()
        {
            if (CurrentPage != null)
            {
                PageList.Remove(CurrentPage);
            }
        }
        public void UpdatePageTitle(string title)
        {
            if (CurrentPage != null)
            {
                ComboBoxItem comboBoxItem = CurrentPage;

                CurrentPage = null;

                comboBoxItem.Text = title;

                CurrentPage = comboBoxItem;
            }
        }

        public void OrderPageList()
        {
            PageList = new CollectionList<ComboBoxItem>(PageList.OrderBy(x => (int)x.Value));
        }


        #region Private Properties

        private string? wizardName { get; set; }

        #endregion

    }

    /// <summary>
    /// Class to use for combobox items within the page selector control.
    /// </summary>
    public class ComboBoxItem : BaseViewModel
    {
        public string? Text { get; set; }

#pragma warning disable CS8618 // This object will always have a value when used!.
        public object Value { get; set; }
#pragma warning restore CS8618 

        public override string ToString()
        {
            if (Text == null || string.IsNullOrEmpty(Text))
            {
                //return "Not Set";
                return Value.ToString();
            }

            return Text.ToString();
        }
    }
}
