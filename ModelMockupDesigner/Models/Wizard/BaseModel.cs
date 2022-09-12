using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ModelMockupDesigner.Models
{
    public abstract class BaseModel : INotifyPropertyChanged
    {
        public string? Name
        {
            get => name;
            set
            {
                if (name == value)
                    return;

                name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public int OrderId
        {
            get => orderId;
            set
            {
                if (orderId == value)
                    return;

                orderId = value;
                OnPropertyChanged(nameof(OrderId));
            }
        }
        private int orderId { get; set; } = 0;
        private string? name { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged = (sender, e) => { };

        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public abstract void LoadFromXml(XmlNode node);
        public abstract XmlNode? ToXml(); 
    }
}
