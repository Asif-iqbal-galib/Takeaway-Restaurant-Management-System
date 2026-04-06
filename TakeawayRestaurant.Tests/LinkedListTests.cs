using Microsoft.VisualStudio.TestTools.UnitTesting;
using Takeaway_Restaurant_Management_System.Classes.DataStructures;
using Takeaway_Restaurant_Management_System.Classes.Models;
using System;
using System.Collections.Generic;

namespace TakeawayRestaurant.Tests
{
    [TestClass]
    public class LinkedListTests
    {
        private OrderLinkedList list;
        private Order order1;
        private Order order2;
        private Order order3;

        [TestInitialize]
        public void Setup()
        {
            list = new OrderLinkedList();
            order1 = new Order
            {
                OrderID = 1,
                OrderNumber = "ORD-001",
                CustomerName = "John Doe",
                Status = "Pending",
                OrderDate = new DateTime(2026, 4, 1, 10, 0, 0)
            };
            order2 = new Order
            {
                OrderID = 2,
                OrderNumber = "ORD-002",
                CustomerName = "Jane Smith",
                Status = "Preparing",
                OrderDate = new DateTime(2026, 4, 1, 11, 0, 0)
            };
            order3 = new Order
            {
                OrderID = 3,
                OrderNumber = "ORD-003",
                CustomerName = "Bob Johnson",
                Status = "Ready",
                OrderDate = new DateTime(2026, 4, 2, 9, 0, 0)
            };
        }

        [TestMethod]
        public void AddOrder_EmptyList_AddsFirstOrder()
        {
            list.AddOrder(order1);
            Assert.AreEqual(1, list.Count);
            List<Order> allOrders = list.GetAllOrders();
            Assert.AreEqual(1, allOrders.Count);
        }

        [TestMethod]
        public void AddOrder_MultipleOrders_IncreasesCount()
        {
            list.AddOrder(order1);
            list.AddOrder(order2);
            list.AddOrder(order3);
            Assert.AreEqual(3, list.Count);
        }

        [TestMethod]
        public void GetOrderById_ExistingOrder_ReturnsOrder()
        {
            list.AddOrder(order1);
            list.AddOrder(order2);
            Order result = list.GetOrderById(2);
            Assert.IsNotNull(result);
            Assert.AreEqual("ORD-002", result.OrderNumber);
        }

        [TestMethod]
        public void GetOrderById_NonExistingOrder_ReturnsNull()
        {
            list.AddOrder(order1);
            Order result = list.GetOrderById(99);
            Assert.IsNull(result);
        }

        [TestMethod]
        public void RemoveOrder_ExistingOrder_RemovesItem()
        {
            list.AddOrder(order1);
            list.AddOrder(order2);
            int countBefore = list.Count;
            bool result = list.RemoveOrder(2);
            Assert.IsTrue(result);
            Assert.AreEqual(countBefore - 1, list.Count);
            Assert.IsNull(list.GetOrderById(2));
        }

        [TestMethod]
        public void RemoveOrder_NonExistingOrder_ReturnsFalse()
        {
            list.AddOrder(order1);
            bool result = list.RemoveOrder(99);
            Assert.IsFalse(result);
            Assert.AreEqual(1, list.Count);
        }

        [TestMethod]
        public void GetOrdersByDate_ReturnsCorrectOrders()
        {
            list.AddOrder(order1);
            list.AddOrder(order2);
            list.AddOrder(order3);
            List<Order> april1Orders = list.GetOrdersByDate(new DateTime(2026, 4, 1));
            Assert.AreEqual(2, april1Orders.Count);
        }

        [TestMethod]
        public void GetOrdersByStatus_ReturnsCorrectOrders()
        {
            list.AddOrder(order1);
            list.AddOrder(order2);
            List<Order> pending = list.GetOrdersByStatus("Pending");
            Assert.AreEqual(1, pending.Count);
        }

        [TestMethod]
        public void GetPendingOrders_ReturnsPendingAndPreparing()
        {
            list.AddOrder(order1);
            list.AddOrder(order2);
            list.AddOrder(order3);
            List<Order> pending = list.GetPendingOrders();
            Assert.AreEqual(2, pending.Count);
        }

        [TestMethod]
        public void UpdateOrderStatus_ExistingOrder_UpdatesStatus()
        {
            list.AddOrder(order1);
            bool result = list.UpdateOrderStatus(1, "Preparing");
            Assert.IsTrue(result);
            Order updated = list.GetOrderById(1);
            Assert.AreEqual("Preparing", updated.Status);
        }

        [TestMethod]
        public void Clear_RemovesAllOrders()
        {
            list.AddOrder(order1);
            list.AddOrder(order2);
            list.Clear();
            Assert.AreEqual(0, list.Count);
        }

        [TestMethod]
        public void AddOrder_NullOrder_ThrowsException()
        {
            try
            {
                list.AddOrder(null);
                Assert.Fail("Expected exception was not thrown");
            }
            catch (ArgumentNullException)
            {
                // Expected exception - test passes
            }
        }
    }
}