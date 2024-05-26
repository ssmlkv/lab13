using System;
using lab12__2;
using ClassLibrary1;
using lab10;
using System.Text;

namespace lab13
{
    public class JournalEntry : ICloneable, IComparable, IInit
    {
        public string CollectionName { get; set; }
        public string ChangeType { get; set; }
        public string ChangedItem { get; set; }

        public JournalEntry() { }

        public JournalEntry(string collectionName, string changeType, string changedItem)
        {
            CollectionName = collectionName;
            ChangeType = changeType;
            ChangedItem = changedItem;
        }

        public override string ToString()
        {
            return $"Collection: {CollectionName}, ChangeType: {ChangeType}, ChangedItem: {ChangedItem}";
        }

        public object Clone()
        {
            return new JournalEntry(CollectionName, ChangeType, ChangedItem);
        }

        public int CompareTo(object obj)
        {
            if (obj is JournalEntry other)
                return String.Compare(CollectionName, other.CollectionName, StringComparison.Ordinal);
            return -1;
        }

        public void Init()
        {
            CollectionName = "Default Collection";
            ChangeType = "Added";
            ChangedItem = "Default Item";
        }

        public void RandomInit()
        {
            Random rnd = new Random();
            CollectionName = $"Collection {rnd.Next(1, 10)}";
            ChangeType = rnd.Next(0, 2) == 0 ? "Added" : "Removed";
            ChangedItem = $"Item {rnd.Next(1, 100)}";
        }
    }

    public class Journal
    {
        private MyHashTable<JournalEntry> entries;

        public Journal()
        {
            entries = new MyHashTable<JournalEntry>();
        }

        public void AddEntry(object source, CollectionHandlerEventArgs args)
        {
            string collectionName = (source as MyObservableCollection<Carriage>)?.ToString() ?? "Unknown Collection";
            var entry = new JournalEntry(collectionName, args.ChangeType, args.ChangedItem.ToString());
            entries.AddItem(entry);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var entry in entries)
            {
                sb.AppendLine(entry.ToString());
            }
            return sb.ToString();
        }
    }
}

