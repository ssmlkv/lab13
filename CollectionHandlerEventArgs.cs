using System;


namespace lab13
{
    public delegate void CollectionHandler(object source, CollectionHandlerEventArgs args);

    public class CollectionHandlerEventArgs : EventArgs
    {
        public string ChangeType { get; set; }
        public object ChangedItem { get; set; }

        public CollectionHandlerEventArgs(string changeType, object changedItem)
        {
            ChangeType = changeType;
            ChangedItem = changedItem;
        }
    }
}

