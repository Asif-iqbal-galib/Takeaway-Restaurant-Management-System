using Microsoft.VisualStudio.TestTools.UnitTesting;
using Takeaway_Restaurant_Management_System.Classes.Database;
using Takeaway_Restaurant_Management_System.Classes.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TakeawayRestaurant.Tests
{
    [TestClass]
    public class IntegrationTests
    {
        private DatabaseManager db;

        [TestInitialize]
        public void Setup()
        {
            db = DatabaseManager.Instance;
        }

        [TestMethod]
        public void CreateOrder_WithItems_ShouldCreateOrder()
        {
            Order order = new Order
            {
                OrderNumber = $"TEST-{DateTime.Now:yyyyMMddHHmmss}",
                CustomerName = "Test Customer",
                CustomerPhone = "01700000000",
                CreatedBy = 1,
                PaymentMethod = "Cash"
            };

            List<OrderItem> items = new List<OrderItem>
            {
                new OrderItem { ItemName = "Burger", Quantity = 2, UnitPrice = 10.99m },
                new OrderItem { ItemName = "Fries", Quantity = 1, UnitPrice = 3.99m }
            };

            order.TotalAmount = 10.99m * 2 + 3.99m;

            int orderId = db.CreateOrder(order, items);
            var orders = db.GetOrders();
            var createdOrder = orders.Find(o => o.OrderID == orderId);
            var orderItems = db.GetOrderDetails(orderId);

            Assert.IsTrue(orderId > 0);
            Assert.IsNotNull(createdOrder);
            Assert.AreEqual(2, orderItems.Count);
        }

        [TestMethod]
        public void AddAndGetCustomer_ShouldWork()
        {
            Customer customer = new Customer
            {
                FullName = "Integration Test Customer",
                Phone = "01799999999",
                Email = "test@email.com",
                Address = "Test Address"
            };

            int newId = db.AddCustomer(customer);
            var customers = db.GetAllCustomers();
            var found = customers.Find(c => c.CustomerID == newId);

            Assert.IsTrue(newId > 0);
            Assert.IsNotNull(found);
            Assert.AreEqual("Integration Test Customer", found.FullName);
        }

        [TestMethod]
        public void UpdateCustomer_ChangesInformation()
        {
            int newId = db.AddCustomer(new Customer { FullName = "Update Test", Phone = "01666666666" });
            var customers = db.GetAllCustomers();
            var customer = customers.Find(c => c.CustomerID == newId);
            customer.FullName = "Updated Name";

            int rows = db.UpdateCustomer(customer);
            var updatedCustomers = db.GetAllCustomers();
            var updated = updatedCustomers.Find(c => c.CustomerID == newId);

            Assert.AreEqual(1, rows);
            Assert.IsNotNull(updated);
            Assert.AreEqual("Updated Name", updated.FullName);
        }

        [TestMethod]
        public void GetDailySalesReport_ReturnsData()
        {
            var report = db.GetDailySalesReport(DateTime.Today);
            Assert.IsNotNull(report);
        }

        [TestMethod]
        public void GetPopularItemsReport_ReturnsData()
        {
            var report = db.GetPopularItemsReport(DateTime.Today.AddDays(-30), DateTime.Today);
            Assert.IsNotNull(report);
        }

        [TestMethod]
        public void GetStaffPerformanceReport_ReturnsData()
        {
            var report = db.GetStaffPerformanceReport(DateTime.Today.AddDays(-30), DateTime.Today);
            Assert.IsNotNull(report);
        }
    }
}