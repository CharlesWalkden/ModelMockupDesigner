﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelMockupDesigner
{
    public class CollectionList<T> : ObservableCollection<T>
    {
        private bool SuppressNotification = false;

        public CollectionList(IEnumerable<T>? list = null)
        {
            if (list != null)
                AddRange(list);
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (!SuppressNotification)
                base.OnCollectionChanged(e);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (!SuppressNotification)
                base.OnPropertyChanged(e);
        }

        public void AddRange(IEnumerable<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            SuppressNotification = true;

            foreach (T item in list)
            {
                Add(item);
            }
            SuppressNotification = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}
