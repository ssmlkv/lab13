using System;
using System;
using System.Text;
using ClassLibrary1;
using System.Collections.Generic;
using lab12_4;

namespace lab13
{
    public class MyObservableCollection<T> : MyCollection<T> where T : IInit, ICloneable, IComparable, new()
    {
        public event CollectionHandler CollectionCountChanged;
        public event CollectionHandler CollectionReferenceChanged;

        public MyObservableCollection() : base() { }

        public MyObservableCollection(int length) : base(length) { }

        public new void Add(T item)
        {
            base.Add(item);
            CollectionCountChanged?.Invoke(this, new CollectionHandlerEventArgs("Added", item));
        }

        public bool Remove(T item)
        {
            bool removed = base.Remove(item);
            if (removed)
            {
                CollectionCountChanged?.Invoke(this, new CollectionHandlerEventArgs("Removed", item));
            }
            return removed;
        }

        //public T this[int index]
        //{
        //    get => base[index];
        //    set
        //    {
        //        T oldItem = base[index];
        //        base[index] = value;
        //        CollectionReferenceChanged?.Invoke(this, new CollectionHandlerEventArgs("Replaced", value));
        //    }
        //}
    }
}