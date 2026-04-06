using Microsoft.VisualStudio.TestTools.UnitTesting;
using Takeaway_Restaurant_Management_System.Classes.DataStructures;
using Takeaway_Restaurant_Management_System.Classes.Models;
using System;

namespace TakeawayRestaurant.Tests
{
    [TestClass]
    public class BSTTests
    {
        private MenuBST bst;
        private MenuItemModel item1;
        private MenuItemModel item2;
        private MenuItemModel item3;

        [TestInitialize]
        public void Setup()
        {
            bst = new MenuBST();
            item1 = new MenuItemModel(1, "Burger", 10.99m, "Burgers");
            item2 = new MenuItemModel(2, "Pizza", 15.99m, "Pizzas");
            item3 = new MenuItemModel(3, "Fries", 3.99m, "Sides");
        }

        [TestMethod]
        public void Insert_EmptyTree_AddsRoot()
        {
            int initialCount = bst.Count;
            bst.Insert(item1);
            Assert.AreEqual(initialCount + 1, bst.Count);
            MenuItemModel found = bst.SearchByName("Burger");
            Assert.IsNotNull(found);
        }

        [TestMethod]
        public void Insert_MultipleItems_IncreasesCount()
        {
            bst.Insert(item1);
            bst.Insert(item2);
            bst.Insert(item3);
            Assert.AreEqual(3, bst.Count);
        }

        [TestMethod]
        public void Insert_DuplicateName_ThrowsException()
        {
            bst.Insert(item1);
            MenuItemModel duplicate = new MenuItemModel(6, "Burger", 12.99m, "Burgers");

            try
            {
                bst.Insert(duplicate);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (InvalidOperationException)
            {
                // Expected exception - test passes
            }
        }

        [TestMethod]
        public void SearchByName_ExistingItem_ReturnsItem()
        {
            bst.Insert(item1);
            bst.Insert(item2);
            MenuItemModel result = bst.SearchByName("Pizza");
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ItemID);
        }

        [TestMethod]
        public void SearchByName_NonExistingItem_ReturnsNull()
        {
            bst.Insert(item1);
            MenuItemModel result = bst.SearchByName("Pasta");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void SearchById_ExistingItem_ReturnsItem()
        {
            bst.Insert(item1);
            bst.Insert(item2);
            MenuItemModel result = bst.SearchById(2);
            Assert.IsNotNull(result);
            Assert.AreEqual("Pizza", result.Name);
        }

        [TestMethod]
        public void SearchById_NonExistingItem_ReturnsNull()
        {
            bst.Insert(item1);
            MenuItemModel result = bst.SearchById(99);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Delete_LeafNode_RemovesItem()
        {
            bst.Insert(item1);
            bst.Insert(item2);
            bst.Insert(item3);
            int countBefore = bst.Count;
            bool result = bst.Delete(3);
            Assert.IsTrue(result);
            Assert.AreEqual(countBefore - 1, bst.Count);
            Assert.IsNull(bst.SearchById(3));
        }

        [TestMethod]
        public void Delete_NonExistingItem_ReturnsFalse()
        {
            bst.Insert(item1);
            bool result = bst.Delete(99);
            Assert.IsFalse(result);
            Assert.AreEqual(1, bst.Count);
        }

        [TestMethod]
        public void GetAllItems_ReturnsSortedItems()
        {
            bst.Insert(item3);
            bst.Insert(item1);
            bst.Insert(item2);
            var items = bst.GetAllItems();
            Assert.AreEqual(3, items.Count);
            Assert.AreEqual("Burger", items[0].Name);
            Assert.AreEqual("Fries", items[1].Name);
            Assert.AreEqual("Pizza", items[2].Name);
        }

        [TestMethod]
        public void GetAvailableItems_ReturnsOnlyAvailable()
        {
            bst.Insert(item1);
            bst.Insert(item2);
            item2.IsAvailable = false;
            var available = bst.GetAvailableItems();
            Assert.AreEqual(1, available.Count);
            Assert.AreEqual("Burger", available[0].Name);
        }

        [TestMethod]
        public void GetItemsByCategory_ReturnsCorrectItems()
        {
            bst.Insert(item1);
            bst.Insert(item3);
            var sides = bst.GetItemsByCategory("Sides");
            Assert.AreEqual(1, sides.Count);
            Assert.AreEqual("Fries", sides[0].Name);
        }
    }
}