using Microsoft.VisualStudio.TestTools.UnitTesting;
using Takeaway_Restaurant_Management_System.Classes.Database;
using Takeaway_Restaurant_Management_System.Classes.Models;

namespace TakeawayRestaurant.Tests
{
    [TestClass]
    public class DatabaseTests
    {
        private DatabaseManager db;

        [TestInitialize]
        public void Setup()
        {
            db = DatabaseManager.Instance;
        }

        [TestMethod]
        public void DatabaseConnection_ShouldSucceed()
        {
            var result = db.ExecuteQuery("SELECT 1");
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Rows.Count > 0);
        }

        [TestMethod]
        public void GetMenuItems_ReturnsItems()
        {
            var items = db.GetMenuItems();
            Assert.IsNotNull(items);
        }

        [TestMethod]
        public void GetCategories_ReturnsCategories()
        {
            var categories = db.GetCategories();
            Assert.IsNotNull(categories);
        }

        [TestMethod]
        public void AuthenticateStaff_ValidCredentials_ReturnsStaff()
        {
            var staff = db.AuthenticateStaff("admin123", "admin123");
            Assert.IsNotNull(staff);
            Assert.AreEqual("admin123", staff.Username);
        }

        [TestMethod]
        public void AuthenticateStaff_InvalidCredentials_ReturnsNull()
        {
            var staff = db.AuthenticateStaff("wronguser", "wrongpass");
            Assert.IsNull(staff);
        }

        [TestMethod]
        public void GetOrders_ReturnsOrders()
        {
            var orders = db.GetOrders();
            Assert.IsNotNull(orders);
        }

        [TestMethod]
        public void GetTodayOrdersCount_ReturnsNumber()
        {
            int count = db.GetTodayOrdersCount();
            Assert.IsTrue(count >= 0);
        }

        [TestMethod]
        public void GetTodayRevenue_ReturnsDecimal()
        {
            decimal revenue = db.GetTodayRevenue();
            Assert.IsTrue(revenue >= 0);
        }

        [TestMethod]
        public void GetPendingOrdersCount_ReturnsNumber()
        {
            int count = db.GetPendingOrdersCount();
            Assert.IsTrue(count >= 0);
        }

        [TestMethod]
        public void GetAllStaff_ReturnsStaffList()
        {
            var staff = db.GetAllStaff();
            Assert.IsNotNull(staff);
        }
    }
}