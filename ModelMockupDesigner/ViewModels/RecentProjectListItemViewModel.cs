using ModelMockupDesigner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ModelMockupDesigner.ViewModels
{
    public class RecentProjectListItemViewModel : BaseViewModel
    {
        public Project? ProjectModel { get; set; }
        public bool Selected
        {
            get => selected;
            set
            {
                selected = value;
                OnPropertyChanged(nameof(Selected));
            }
        }
        private bool selected { get; set; }

        public event EventHandler? OnSelected;
        public ICommand SelectCommand { get; set; }

        public RecentProjectListItemViewModel()
        {
            SelectCommand = new RelayCommand(SelectedCommandMethod);
        }

        private void SelectedCommandMethod()
        {
            OnSelected?.Invoke(this, new EventArgs());
        }
    }
}
