using Microsoft.VisualStudio.TestTools.UnitTesting;
using Takeaway_Restaurant_Management_System.Classes.Models;

namespace TakeawayRestaurant.Tests
{
    [TestClass]
    public class ModelTests
    {
        [TestMethod]
        public void MenuItemModel_Constructor_SetsProperties()
        {
            var item = new MenuItemModel(1, "Burger", 10.99m, "Burgers");
            Assert.AreEqual(1, item.ItemID);
            Assert.AreEqual("Burger", item.Name);
            Assert.AreEqual(10.99m, item.Price);
            Assert.AreEqual("Burgers", item.CategoryName);
        }

        [TestMethod]
        public void MenuItemModel_ToString_ReturnsFormattedString()
        {
            var item = new MenuItemModel(1, "Burger", 10.99m, "Burgers");
            Assert.AreEqual("Burger - $10.99", item.ToString());
        }

        [TestMethod]
        public void Order_Constructor_SetsDefaultValues()
        {
            var order = new Order();
            Assert.IsNotNull(order.Items);
            Assert.AreEqual("Pending", order.Status);
            Assert.AreEqual("Unpaid", order.PaymentStatus);
        }

        [TestMethod]
        public void Order_CalculateTotal_SumsItems()
        {
            var order = new Order();
            order.Items.Add(new OrderItem { Quantity = 2, UnitPrice = 10.99m });
            order.Items.Add(new OrderItem { Quantity = 1, UnitPrice = 5.99m });
            order.CalculateTotal();
            Assert.AreEqual(27.97m, order.TotalAmount);
        }

        [TestMethod]
        public void Customer_Constructor_SetsDefaultValues()
        {
            var customer = new Customer();
            Assert.IsTrue(customer.IsActive);
            Assert.AreEqual(0, customer.LoyaltyPoints);
        }

        [TestMethod]
        public void Customer_ToString_ReturnsNameAndPhone()
        {
            var customer = new Customer("John Doe", "01711111111");
            Assert.AreEqual("John Doe - 01711111111", customer.ToString());
        }

        [TestMethod]
        public void Staff_RoleProperties_ReturnCorrectBooleans()
        {
            var admin = new Staff { Role = "Admin" };
            var cashier = new Staff { Role = "Cashier" };
            Assert.IsTrue(admin.IsAdmin);
            Assert.IsTrue(cashier.IsCashier);
        }

        [TestMethod]
        public void OrderItem_CalculateSubtotal_ReturnsCorrectTotal()
        {
            var item = new OrderItem { Quantity = 3, UnitPrice = 5.99m };
            item.CalculateSubtotal();
            Assert.AreEqual(17.97m, item.Subtotal);
        }

        [TestMethod]
        public void Delivery_Constructor_SetsDefaultValues()
        {
            var delivery = new Delivery();
            Assert.AreEqual("Assigned", delivery.Status);
        }

        [TestMethod]
        public void Inventory_IsLowStock_ReturnsTrueWhenBelowReorderLevel()
        {
            var item = new Inventory { Quantity = 5, ReorderLevel = 10 };
            Assert.IsTrue(item.IsLowStock);
        }

        [TestMethod]
        public void Inventory_IsLowStock_ReturnsFalseWhenAboveReorderLevel()
        {
            var item = new Inventory { Quantity = 20, ReorderLevel = 10 };
            Assert.IsFalse(item.IsLowStock);
        }
    }
}