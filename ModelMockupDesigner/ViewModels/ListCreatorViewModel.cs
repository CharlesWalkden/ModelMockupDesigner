using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace ModelMockupDesigner.ViewModels
{
    public class ListCreatorViewModel : BaseViewModel
    {
        public CollectionList<ListViewItem> ListOptions { get; set; }

        public ListViewItem CurrentSelection { get; set; }
        
        public string CurrentText { get; set; }

        public ListCreatorViewModel()
        {
            ListOptions = new CollectionList<ListViewItem>();

            AddOptionCommand = new RelayCommand(AddOption);
            RemoveOptionCommand = new RelayCommand(RemoveOption);
        }

        public ICommand AddOptionCommand { get; set; }
        public ICommand RemoveOptionCommand { get; set; }

        public void AddOption()
        {
            if (!string.IsNullOrWhiteSpace(CurrentText))
            {
                ListViewItem listViewItem = new ListViewItem()
                {
                    Content = CurrentText,
                    Foreground = Brushes.White
                };
                ListOptions.Add(listViewItem);
            }
        }
        public void RemoveOption()
        {
            if (CurrentSelection != null && ListOptions.Contains(CurrentSelection))
            {
                ListOptions.Remove(CurrentSelection);
            }
        }
        public List<string> GetListAsString()
        {
            List<string> options = new List<string>();

            foreach (ListViewItem item in ListOptions)
            {
                if (item.Content != null)
                {
                    options.Add(item.Content.ToString());
                }
            }

            return options;
        }
    }
}
