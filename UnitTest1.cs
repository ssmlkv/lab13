using System;
using lab13;
using lab12__2;
using lab12_4;
using ClassLibrary1;
using lab10;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;

namespace CollectionHandlerTests
{
    [TestClass]
    public class CollectionHandlerTests
    {
        private void TestHandler(object source, CollectionHandlerEventArgs args)
        {
            Assert.AreEqual("TestChange", args.ChangeType);
            Assert.AreEqual(42, args.ChangedItem);
        }

        [TestMethod]
        public void CollectionHandler_Invoke_EventArgsAreCorrect()
        {
            CollectionHandler handler = new CollectionHandler(TestHandler);

            var eventArgs = new CollectionHandlerEventArgs("TestChange", 42);

            handler.Invoke(this, eventArgs);
        }
    }

    [TestClass]
    public class JournalEntryTests
    {
        // Тест для конструктора, чтобы убедиться, что значения свойств правильно устанавливаются
        [TestMethod]
        public void JournalEntry_Constructor_InitializesProperties()
        {
            // Arrange
            string expectedCollectionName = "TestCollection";
            string expectedChangeType = "TestChange";
            string expectedChangedItem = "TestItem";

            // Act
            var journalEntry = new JournalEntry(expectedCollectionName, expectedChangeType, expectedChangedItem);

            // Assert
            Assert.AreEqual(expectedCollectionName, journalEntry.CollectionName);
            Assert.AreEqual(expectedChangeType, journalEntry.ChangeType);
            Assert.AreEqual(expectedChangedItem, journalEntry.ChangedItem);
        }

        [TestMethod]
        public void JournalEntry_Clone_CreatesCopy()
        {
            // Arrange
            var original = new JournalEntry("TestCollection", "TestChange", "TestItem");

            // Act
            var clone = (JournalEntry)original.Clone();

            // Assert
            Assert.AreEqual(original.CollectionName, clone.CollectionName);
            Assert.AreEqual(original.ChangeType, clone.ChangeType);
            Assert.AreEqual(original.ChangedItem, clone.ChangedItem);
            Assert.AreNotSame(original, clone); // Убедимся, что это разные объекты
        }

        // Тест для метода CompareTo
        [TestMethod]
        public void JournalEntry_CompareTo_SameCollectionName_ReturnsZero()
        {
            // Arrange
            var entry1 = new JournalEntry("TestCollection", "Change1", "Item1");
            var entry2 = new JournalEntry("TestCollection", "Change2", "Item2");

            // Act
            int result = entry1.CompareTo(entry2);

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void JournalEntry_CompareTo_DifferentCollectionName_ReturnsNonZero()
        {
            // Arrange
            var entry1 = new JournalEntry("ACollection", "Change1", "Item1");
            var entry2 = new JournalEntry("BCollection", "Change2", "Item2");

            // Act
            int result = entry1.CompareTo(entry2);

            // Assert
            Assert.AreNotEqual(0, result);
            Assert.IsTrue(result < 0); // "ACollection" < "BCollection"
        }

        [TestMethod]
        public void JournalEntry_CompareTo_NonJournalEntry_ReturnsNegativeOne()
        {
            // Arrange
            var entry = new JournalEntry("TestCollection", "Change1", "Item1");
            var notAnEntry = new object();

            // Act
            int result = entry.CompareTo(notAnEntry);

            // Assert
            Assert.AreEqual(-1, result);
        }

        // Тест для метода Init
        [TestMethod]
        public void JournalEntry_Init_SetsDefaultValues()
        {
            // Arrange
            var journalEntry = new JournalEntry("Initial Collection", "Initial Change", "Initial Item");

            // Act
            journalEntry.Init();

            // Assert
            Assert.AreEqual("Default Collection", journalEntry.CollectionName);
            Assert.AreEqual("Added", journalEntry.ChangeType);
            Assert.AreEqual("Default Item", journalEntry.ChangedItem);
        }

        // Тест для метода RandomInit
        [TestMethod]
        public void JournalEntry_RandomInit_SetsRandomValues()
        {
            // Arrange
            var journalEntry = new JournalEntry("Initial Collection", "Initial Change", "Initial Item");

            // Act
            journalEntry.RandomInit();

            // Assert
            StringAssert.StartsWith(journalEntry.CollectionName, "Collection ");
            Assert.IsTrue(journalEntry.ChangeType == "Added" || journalEntry.ChangeType == "Removed");
            StringAssert.StartsWith(journalEntry.ChangedItem, "Item ");
        }

        // Дополнительный тест для RandomInit, чтобы проверить диапазоны значений
        [TestMethod]
        public void JournalEntry_RandomInit_SetsValuesWithinExpectedRange()
        {
            // Arrange
            var journalEntry = new JournalEntry("Initial Collection", "Initial Change", "Initial Item");

            // Act
            journalEntry.RandomInit();

            // Assert
            var collectionNumber = int.Parse(journalEntry.CollectionName.Replace("Collection ", ""));
            var itemNumber = int.Parse(journalEntry.ChangedItem.Replace("Item ", ""));

            Assert.IsTrue(collectionNumber >= 1 && collectionNumber <= 9);
            Assert.IsTrue(itemNumber >= 1 && itemNumber <= 99);
        }

        [TestMethod]
        public void Journal_Constructor_InitializesJournal()
        {
            // Act
            var journal = new Journal();

            // Assert
            Assert.IsNotNull(journal);
        }

        [TestMethod]
        public void Journal_AddEntry_AddsJournalEntry()
        {
            // Arrange
            var journal = new Journal();
            var collection = new MyObservableCollection<Carriage>();
            var args = new CollectionHandlerEventArgs("Added", "TestItem");

            // Act
            journal.AddEntry(collection, args);

            // Assert
            var journalField = typeof(Journal).GetField("journal", BindingFlags.NonPublic | BindingFlags.Instance);
            var journalTable = (MyHashTable<JournalEntry>)journalField.GetValue(journal);
            var journalItems = journalTable.table;

            foreach (var item in journalItems)
            {
                Console.WriteLine(item != null ? item.ToString() : "null");
            }

            var addedEntry = journalItems.FirstOrDefault();
            Assert.IsNotNull(addedEntry, "No entry was added to the journal.");
            Assert.AreEqual("MyObservableCollection", addedEntry.CollectionName);
        }

        [TestMethod]
        public void Journal_AddEntry_WithUnknownCollection_AddsJournalEntry()
        {
            // Arrange
            var journal = new Journal();
            var args = new CollectionHandlerEventArgs("Removed", "UnknownItem");

            // Act
            journal.AddEntry(null, args);

            // Assert
            var journalField = typeof(Journal).GetField("journal", BindingFlags.NonPublic | BindingFlags.Instance);
            var journalTable = (MyHashTable<JournalEntry>)journalField.GetValue(journal);
            var journalItems = journalTable.table;

            foreach (var item in journalItems)
            {
                Console.WriteLine(item != null ? item.ToString() : "null");
            }

            var addedEntry = journalItems.FirstOrDefault();
            Assert.IsNotNull(addedEntry, "No entry was added to the journal.");
        }
    }
    [TestClass]
    public class MyObservableCollectionTests
    {
        [TestMethod]
        public void MyObservableCollection_Add_AddsItem_AndRaisesEvent()
        {
            // Arrange
            var collection = new MyObservableCollection<Carriage>();
            bool eventRaised = false;
            var carriage = new Carriage(new IdNumber(1), 1, 100); // Создание экземпляра вагона

            // Подписка на событие CollectionCountChanged
            collection.CollectionCountChanged += (sender, args) =>
            {
                eventRaised = true;
                Assert.AreEqual("Added", args.ChangeType); // Проверка типа изменения
                Assert.AreEqual(carriage, args.ChangedItem); // Проверка добавленного элемента
            };

            // Act
            collection.Add(carriage);

            // Assert
            Assert.IsTrue(eventRaised, "The CollectionCountChanged event was not raised.");
            Assert.IsTrue(collection.Contains(carriage), "The item was not added to the collection.");
        }

        [TestMethod]
        public void MyObservableCollection_Remove_RemovesItem_AndRaisesEvent()
        {
            // Arrange
            var collection = new MyObservableCollection<Carriage>();
            bool eventRaised = false;
            var carriage = new Carriage(new IdNumber(1), 1, 100); // Создание экземпляра вагона
            collection.Add(carriage); // Добавление элемента для удаления

            // Подписка на событие CollectionCountChanged
            collection.CollectionCountChanged += (sender, args) =>
            {
                eventRaised = true;
                Assert.AreEqual("Removed", args.ChangeType); // Проверка типа изменения
                Assert.AreEqual(carriage, args.ChangedItem); // Проверка удаленного элемента
            };

            // Act
            collection.Remove(carriage);

            // Assert
            Assert.IsTrue(eventRaised, "The CollectionCountChanged event was not raised.");
            Assert.IsFalse(collection.Contains(carriage), "The item was not removed from the collection.");
        }
    }
}
