using System;
using ClassLibrary1;
using lab10;

namespace lab13
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Step 1: Creating and displaying collections...");
            var collection1 = new MyObservableCollection<Carriage>(5);
            var collection2 = new MyObservableCollection<Carriage>(5);

            Console.WriteLine("Collection 1 after initialization:");
            DisplayCollection(collection1);
            Console.WriteLine("Collection 2 after initialization:");
            DisplayCollection(collection2);

            Console.WriteLine("\nStep 2: Creating journals and subscribing to events...");
            var journal1 = new Journal();
            var journal2 = new Journal();

            collection1.CollectionCountChanged += journal1.AddEntry;
            collection1.CollectionReferenceChanged += journal1.AddEntry;
            collection2.CollectionReferenceChanged += journal2.AddEntry;

            Console.WriteLine("Journals subscribed to events.\n");

            Console.WriteLine("Step 3: Adding items to collections...");
            var carriage1 = new Carriage(new IdNumber(1), 1, 100);
            var carriage2 = new Carriage(new IdNumber(2), 2, 110);
            var carriage3 = new Carriage(new IdNumber(3), 3, 120);

            collection1.Add(carriage1);
            collection1.Add(carriage2);
            collection1.Add(carriage3);

            collection2.Add(new Carriage(new IdNumber(4), 4, 130));
            collection2.Add(new Carriage(new IdNumber(5), 5, 140));

            Console.WriteLine("Collection 1 after adding items:");
            DisplayCollection(collection1);
            Console.WriteLine("Collection 2 after adding items:");
            DisplayCollection(collection2);

            Console.WriteLine("\nStep 4: Removing items from collections...");
            collection1.Remove(carriage2);

            Console.WriteLine("Collection 1 after removing an item:");
            DisplayCollection(collection1);

            Console.WriteLine("\nStep 5: Modifying items in collections...");
            collection2[0] = new Carriage(new IdNumber(6), 6, 150);

            Console.WriteLine("Collection 2 after modifying an item:");
            DisplayCollection(collection2);

            Console.WriteLine("\nStep 6: Displaying journal entries...");
            Console.WriteLine("Journal 1 entries:");
            Console.WriteLine(journal1);
            Console.WriteLine("Journal 2 entries:");
            Console.WriteLine(journal2);
        }

        static void DisplayCollection<T>(MyObservableCollection<T> collection) where T : IInit, ICloneable, IComparable, new()
        {
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }
    }
}
