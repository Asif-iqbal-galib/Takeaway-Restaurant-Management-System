using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using Takeaway_Restaurant_Management_System.Classes.Models;

namespace Takeaway_Restaurant_Management_System.Classes.Database
{
    public class DatabaseManager
    {
        private static DatabaseManager instance;
        private string connectionString;

        public static DatabaseManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new DatabaseManager();
                return instance;
            }
        }

        private DatabaseManager()
        {
            connectionString = ConfigurationManager.ConnectionStrings["TakeawayDB"].ConnectionString;
        }

        // =============================================
        // GENERAL DATABASE METHODS
        // =============================================

        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        conn.Open();
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }

        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        // =============================================
        // DASHBOARD STATS METHODS
        // =============================================

        public int GetTodayOrdersCount()
        {
            string query = "SELECT COUNT(*) FROM Orders WHERE CAST(OrderDate AS DATE) = CAST(GETDATE() AS DATE) AND PaymentStatus = 'Paid'";
            object result = ExecuteScalar(query);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        public decimal GetTodayRevenue()
        {
            string query = "SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders WHERE CAST(OrderDate AS DATE) = CAST(GETDATE() AS DATE) AND PaymentStatus = 'Paid'";
            object result = ExecuteScalar(query);
            return result != null ? Convert.ToDecimal(result) : 0;
        }

        public int GetPendingOrdersCount()
        {
            string query = "SELECT COUNT(*) FROM Orders WHERE Status IN ('Pending', 'Preparing')";
            object result = ExecuteScalar(query);
            return result != null ? Convert.ToInt32(result) : 0;
        }

        // =============================================
        // REPORT METHODS
        // =============================================

        public DataTable GetDailySalesReport(DateTime date)
        {
            string query = @"SELECT 
                                FORMAT(o.OrderDate, 'HH:mm') as Time,
                                o.OrderNumber,
                                o.CustomerName,
                                o.TotalAmount,
                                o.PaymentMethod,
                                s.FullName as Cashier
                             FROM Orders o
                             INNER JOIN Staff s ON o.CreatedBy = s.StaffID
                             WHERE CAST(o.OrderDate AS DATE) = @Date
                             AND o.PaymentStatus = 'Paid'
                             ORDER BY o.OrderDate";

            SqlParameter[] parameters = { new SqlParameter("@Date", date.Date) };
            return ExecuteQuery(query, parameters);
        }

        public DataTable GetPopularItemsReport(DateTime startDate, DateTime endDate)
        {
            string query = @"SELECT 
                                mi.ItemName,
                                c.CategoryName,
                                SUM(od.Quantity) as TotalQuantity,
                                COUNT(DISTINCT o.OrderID) as TimesOrdered,
                                SUM(od.Subtotal) as TotalRevenue
                             FROM OrderDetails od
                             INNER JOIN Orders o ON od.OrderID = o.OrderID
                             INNER JOIN MenuItems mi ON od.ItemID = mi.ItemID
                             INNER JOIN Categories c ON mi.CategoryID = c.CategoryID
                             WHERE CAST(o.OrderDate AS DATE) BETWEEN @StartDate AND @EndDate
                             AND o.PaymentStatus = 'Paid'
                             GROUP BY mi.ItemName, c.CategoryName
                             ORDER BY TotalQuantity DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@StartDate", startDate.Date),
                new SqlParameter("@EndDate", endDate.Date)
            };
            return ExecuteQuery(query, parameters);
        }

        public DataTable GetStaffPerformanceReport(DateTime startDate, DateTime endDate)
        {
            string query = @"SELECT 
                                s.FullName,
                                s.Role,
                                COUNT(DISTINCT o.OrderID) as OrdersProcessed,
                                ISNULL(SUM(o.TotalAmount), 0) as TotalSales
                             FROM Staff s
                             LEFT JOIN Orders o ON s.StaffID = o.CreatedBy
                                 AND CAST(o.OrderDate AS DATE) BETWEEN @StartDate AND @EndDate
                                 AND o.PaymentStatus = 'Paid'
                             WHERE s.IsActive = 1
                             GROUP BY s.FullName, s.Role
                             ORDER BY TotalSales DESC";

            SqlParameter[] parameters = {
                new SqlParameter("@StartDate", startDate.Date),
                new SqlParameter("@EndDate", endDate.Date)
            };
            return ExecuteQuery(query, parameters);
        }

        // =============================================
        // MENU ITEMS METHODS
        // =============================================

        public List<MenuItemModel> GetMenuItems()
        {
            List<MenuItemModel> items = new List<MenuItemModel>();
            string query = @"SELECT mi.ItemID, mi.ItemName, mi.Price, mi.Description, 
                                    mi.PreparationTime, mi.IsAvailable, mi.IsVegetarian, mi.IsSpicy,
                                    c.CategoryID, c.CategoryName
                             FROM MenuItems mi
                             INNER JOIN Categories c ON mi.CategoryID = c.CategoryID
                             WHERE mi.IsAvailable = 1
                             ORDER BY c.DisplayOrder, mi.ItemName";
            DataTable dt = ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                items.Add(new MenuItemModel
                {
                    ItemID = Convert.ToInt32(row["ItemID"]),
                    Name = row["ItemName"].ToString(),
                    Price = Convert.ToDecimal(row["Price"]),
                    Description = row["Description"].ToString(),
                    PreparationTime = Convert.ToInt32(row["PreparationTime"]),
                    IsAvailable = Convert.ToBoolean(row["IsAvailable"]),
                    IsVegetarian = Convert.ToBoolean(row["IsVegetarian"]),
                    IsSpicy = Convert.ToBoolean(row["IsSpicy"]),
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    CategoryName = row["CategoryName"].ToString()
                });
            }
            return items;
        }

        public List<Category> GetCategories()
        {
            List<Category> categories = new List<Category>();
            string query = "SELECT CategoryID, CategoryName, Description, DisplayOrder FROM Categories ORDER BY DisplayOrder";
            DataTable dt = ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                categories.Add(new Category
                {
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    CategoryName = row["CategoryName"].ToString(),
                    Description = row["Description"].ToString(),
                    DisplayOrder = Convert.ToInt32(row["DisplayOrder"])
                });
            }
            return categories;
        }

        public int AddMenuItem(MenuItemModel item)
        {
            string query = @"INSERT INTO MenuItems (ItemName, CategoryID, Price, Description, PreparationTime, IsAvailable, IsVegetarian, IsSpicy) 
                            VALUES (@ItemName, @CategoryID, @Price, @Description, @PrepTime, @IsAvailable, @IsVeg, @IsSpicy);
                            SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@ItemName", item.Name),
                new SqlParameter("@CategoryID", item.CategoryID),
                new SqlParameter("@Price", item.Price),
                new SqlParameter("@Description", item.Description ?? ""),
                new SqlParameter("@PrepTime", item.PreparationTime),
                new SqlParameter("@IsAvailable", item.IsAvailable),
                new SqlParameter("@IsVeg", item.IsVegetarian),
                new SqlParameter("@IsSpicy", item.IsSpicy)
            };

            return Convert.ToInt32(ExecuteScalar(query, parameters));
        }

        public int UpdateMenuItem(MenuItemModel item)
        {
            string query = @"UPDATE MenuItems SET 
                            ItemName = @ItemName,
                            CategoryID = @CategoryID,
                            Price = @Price,
                            Description = @Description,
                            PreparationTime = @PrepTime,
                            IsAvailable = @IsAvailable,
                            IsVegetarian = @IsVeg,
                            IsSpicy = @IsSpicy
                            WHERE ItemID = @ItemID";

            SqlParameter[] parameters = {
                new SqlParameter("@ItemID", item.ItemID),
                new SqlParameter("@ItemName", item.Name),
                new SqlParameter("@CategoryID", item.CategoryID),
                new SqlParameter("@Price", item.Price),
                new SqlParameter("@Description", item.Description ?? ""),
                new SqlParameter("@PrepTime", item.PreparationTime),
                new SqlParameter("@IsAvailable", item.IsAvailable),
                new SqlParameter("@IsVeg", item.IsVegetarian),
                new SqlParameter("@IsSpicy", item.IsSpicy)
            };

            return ExecuteNonQuery(query, parameters);
        }

        public int DeleteMenuItem(int itemId)
        {
            string query = "DELETE FROM MenuItems WHERE ItemID = @ItemID";
            SqlParameter[] parameters = { new SqlParameter("@ItemID", itemId) };
            return ExecuteNonQuery(query, parameters);
        }

        // =============================================
        // STAFF METHODS
        // =============================================

        public Staff AuthenticateStaff(string username, string password)
        {
            string query = "SELECT StaffID, Username, FullName, Role, Phone, Email FROM Staff WHERE Username = @Username AND PasswordHash = @Password AND IsActive = 1";
            SqlParameter[] parameters = {
                new SqlParameter("@Username", username),
                new SqlParameter("@Password", password)
            };
            DataTable result = ExecuteQuery(query, parameters);
            if (result.Rows.Count > 0)
            {
                DataRow row = result.Rows[0];
                return new Staff
                {
                    StaffID = Convert.ToInt32(row["StaffID"]),
                    Username = row["Username"].ToString(),
                    FullName = row["FullName"].ToString(),
                    Role = row["Role"].ToString(),
                    Phone = row["Phone"].ToString(),
                    Email = row["Email"].ToString()
                };
            }
            return null;
        }

        public List<Staff> GetAllStaff()
        {
            List<Staff> staffList = new List<Staff>();
            string query = "SELECT StaffID, Username, PasswordHash, FullName, Role, Phone, Email, HireDate, IsActive FROM Staff ORDER BY FullName";
            DataTable dt = ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                staffList.Add(new Staff
                {
                    StaffID = Convert.ToInt32(row["StaffID"]),
                    Username = row["Username"].ToString(),
                    PasswordHash = row["PasswordHash"].ToString(),
                    FullName = row["FullName"].ToString(),
                    Role = row["Role"].ToString(),
                    Phone = row["Phone"].ToString(),
                    Email = row["Email"].ToString(),
                    HireDate = Convert.ToDateTime(row["HireDate"]),
                    IsActive = Convert.ToBoolean(row["IsActive"])
                });
            }
            return staffList;
        }

        public int AddStaff(Staff staff)
        {
            string query = @"INSERT INTO Staff (Username, PasswordHash, FullName, Role, Phone, Email, HireDate, IsActive) 
                            VALUES (@Username, @Password, @FullName, @Role, @Phone, @Email, @HireDate, @IsActive);
                            SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@Username", staff.Username),
                new SqlParameter("@Password", staff.PasswordHash),
                new SqlParameter("@FullName", staff.FullName),
                new SqlParameter("@Role", staff.Role),
                new SqlParameter("@Phone", staff.Phone ?? ""),
                new SqlParameter("@Email", staff.Email ?? ""),
                new SqlParameter("@HireDate", staff.HireDate),
                new SqlParameter("@IsActive", staff.IsActive)
            };

            return Convert.ToInt32(ExecuteScalar(query, parameters));
        }

        public int UpdateStaff(Staff staff)
        {
            string query = @"UPDATE Staff SET 
                            Username = @Username,
                            FullName = @FullName,
                            Role = @Role,
                            Phone = @Phone,
                            Email = @Email,
                            IsActive = @IsActive
                            WHERE StaffID = @StaffID";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@StaffID", staff.StaffID),
                new SqlParameter("@Username", staff.Username),
                new SqlParameter("@FullName", staff.FullName),
                new SqlParameter("@Role", staff.Role),
                new SqlParameter("@Phone", staff.Phone ?? ""),
                new SqlParameter("@Email", staff.Email ?? ""),
                new SqlParameter("@IsActive", staff.IsActive)
            };

            if (!string.IsNullOrEmpty(staff.PasswordHash))
            {
                query = @"UPDATE Staff SET 
                        Username = @Username,
                        PasswordHash = @Password,
                        FullName = @FullName,
                        Role = @Role,
                        Phone = @Phone,
                        Email = @Email,
                        IsActive = @IsActive
                        WHERE StaffID = @StaffID";
                parameters.Add(new SqlParameter("@Password", staff.PasswordHash));
            }

            return ExecuteNonQuery(query, parameters.ToArray());
        }

        public int SetStaffActiveStatus(int staffId, bool isActive)
        {
            string query = "UPDATE Staff SET IsActive = @IsActive WHERE StaffID = @StaffID";
            SqlParameter[] parameters = {
                new SqlParameter("@StaffID", staffId),
                new SqlParameter("@IsActive", isActive)
            };
            return ExecuteNonQuery(query, parameters);
        }

        // =============================================
        // CUSTOMER METHODS
        // =============================================

        public List<Customer> GetAllCustomers()
        {
            List<Customer> customers = new List<Customer>();
            string query = "SELECT CustomerID, FullName, Phone, Email, Address, LoyaltyPoints, RegistrationDate FROM Customers ORDER BY FullName";
            DataTable dt = ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                customers.Add(new Customer
                {
                    CustomerID = Convert.ToInt32(row["CustomerID"]),
                    FullName = row["FullName"].ToString(),
                    Phone = row["Phone"].ToString(),
                    Email = row["Email"].ToString(),
                    Address = row["Address"].ToString(),
                    LoyaltyPoints = Convert.ToInt32(row["LoyaltyPoints"]),
                    RegistrationDate = Convert.ToDateTime(row["RegistrationDate"])
                });
            }
            return customers;
        }

        public List<Customer> SearchCustomers(string searchTerm)
        {
            List<Customer> customers = new List<Customer>();
            string query = @"SELECT CustomerID, FullName, Phone, Email, Address, LoyaltyPoints 
                            FROM Customers 
                            WHERE (FullName LIKE @Search OR Phone LIKE @Search) 
                            ORDER BY FullName";

            SqlParameter[] parameters = {
                new SqlParameter("@Search", $"%{searchTerm}%")
            };

            DataTable dt = ExecuteQuery(query, parameters);
            foreach (DataRow row in dt.Rows)
            {
                customers.Add(new Customer
                {
                    CustomerID = Convert.ToInt32(row["CustomerID"]),
                    FullName = row["FullName"].ToString(),
                    Phone = row["Phone"].ToString(),
                    Email = row["Email"].ToString(),
                    Address = row["Address"].ToString(),
                    LoyaltyPoints = Convert.ToInt32(row["LoyaltyPoints"])
                });
            }
            return customers;
        }

        public int AddCustomer(Customer customer)
        {
            string query = @"INSERT INTO Customers (FullName, Phone, Email, Address) 
                            VALUES (@FullName, @Phone, @Email, @Address);
                            SELECT SCOPE_IDENTITY();";
            SqlParameter[] parameters = {
                new SqlParameter("@FullName", customer.FullName),
                new SqlParameter("@Phone", customer.Phone),
                new SqlParameter("@Email", customer.Email ?? ""),
                new SqlParameter("@Address", customer.Address ?? "")
            };
            return Convert.ToInt32(ExecuteScalar(query, parameters));
        }

        public int UpdateCustomer(Customer customer)
        {
            string query = @"UPDATE Customers SET FullName = @FullName, Phone = @Phone, 
                            Email = @Email, Address = @Address WHERE CustomerID = @CustomerID";
            SqlParameter[] parameters = {
                new SqlParameter("@CustomerID", customer.CustomerID),
                new SqlParameter("@FullName", customer.FullName),
                new SqlParameter("@Phone", customer.Phone),
                new SqlParameter("@Email", customer.Email ?? ""),
                new SqlParameter("@Address", customer.Address ?? "")
            };
            return ExecuteNonQuery(query, parameters);
        }

        // =============================================
        // INVENTORY METHODS
        // =============================================

        public List<Inventory> GetInventory()
        {
            List<Inventory> items = new List<Inventory>();
            try
            {
                string query = "SELECT ItemID, ItemName, Quantity, Unit, ReorderLevel, LastRestocked FROM Inventory ORDER BY ItemName";
                DataTable dt = ExecuteQuery(query);
                foreach (DataRow row in dt.Rows)
                {
                    items.Add(new Inventory
                    {
                        ItemID = Convert.ToInt32(row["ItemID"]),
                        ItemName = row["ItemName"].ToString(),
                        Quantity = Convert.ToDecimal(row["Quantity"]),
                        Unit = row["Unit"].ToString(),
                        ReorderLevel = Convert.ToDecimal(row["ReorderLevel"]),
                        LastRestocked = row["LastRestocked"] != DBNull.Value ? Convert.ToDateTime(row["LastRestocked"]) : (DateTime?)null
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetInventory: {ex.Message}");
            }
            return items;
        }

        public int UpdateInventoryQuantity(int itemId, decimal quantity, bool isAdd)
        {
            string query;
            if (isAdd)
                query = "UPDATE Inventory SET Quantity = Quantity + @Quantity, LastRestocked = GETDATE() WHERE ItemID = @ItemID";
            else
                query = "UPDATE Inventory SET Quantity = Quantity - @Quantity WHERE ItemID = @ItemID AND Quantity >= @Quantity";

            SqlParameter[] parameters = {
                new SqlParameter("@ItemID", itemId),
                new SqlParameter("@Quantity", quantity)
            };
            return ExecuteNonQuery(query, parameters);
        }

        public int AddInventoryItem(Inventory item)
        {
            string query = @"INSERT INTO Inventory (ItemName, Quantity, Unit, ReorderLevel) 
                            VALUES (@ItemName, @Quantity, @Unit, @ReorderLevel);
                            SELECT SCOPE_IDENTITY();";
            SqlParameter[] parameters = {
                new SqlParameter("@ItemName", item.ItemName),
                new SqlParameter("@Quantity", item.Quantity),
                new SqlParameter("@Unit", item.Unit),
                new SqlParameter("@ReorderLevel", item.ReorderLevel)
            };
            return Convert.ToInt32(ExecuteScalar(query, parameters));
        }

        // =============================================
        // ORDER METHODS
        // =============================================

        public List<Order> GetOrders()
        {
            List<Order> orders = new List<Order>();
            string query = @"SELECT OrderID, OrderNumber, CustomerName, CustomerPhone, 
                                    DeliveryAddress, OrderDate, TotalAmount, Status, 
                                    PaymentMethod, PaymentStatus, CreatedBy, AssignedTo
                             FROM Orders ORDER BY OrderDate DESC";

            DataTable dt = ExecuteQuery(query);

            if (dt == null || dt.Rows.Count == 0)
                return orders;

            foreach (DataRow row in dt.Rows)
            {
                Order order = new Order();

                if (row["OrderID"] != DBNull.Value)
                    order.OrderID = Convert.ToInt32(row["OrderID"]);

                if (row["OrderNumber"] != DBNull.Value)
                    order.OrderNumber = row["OrderNumber"].ToString();
                else
                    order.OrderNumber = "N/A";

                if (row["CustomerName"] != DBNull.Value)
                    order.CustomerName = row["CustomerName"].ToString();
                else
                    order.CustomerName = "Walk-in Customer";

                if (row["CustomerPhone"] != DBNull.Value)
                    order.CustomerPhone = row["CustomerPhone"].ToString();
                else
                    order.CustomerPhone = "";

                if (row["DeliveryAddress"] != DBNull.Value)
                    order.DeliveryAddress = row["DeliveryAddress"].ToString();
                else
                    order.DeliveryAddress = "";

                if (row["OrderDate"] != DBNull.Value)
                    order.OrderDate = Convert.ToDateTime(row["OrderDate"]);
                else
                    order.OrderDate = DateTime.Now;

                if (row["TotalAmount"] != DBNull.Value)
                    order.TotalAmount = Convert.ToDecimal(row["TotalAmount"]);
                else
                    order.TotalAmount = 0;

                if (row["Status"] != DBNull.Value)
                    order.Status = row["Status"].ToString();
                else
                    order.Status = "Pending";

                if (row["PaymentMethod"] != DBNull.Value)
                    order.PaymentMethod = row["PaymentMethod"].ToString();
                else
                    order.PaymentMethod = "Not Paid";

                if (row["PaymentStatus"] != DBNull.Value)
                    order.PaymentStatus = row["PaymentStatus"].ToString();
                else
                    order.PaymentStatus = "Unpaid";

                if (row["CreatedBy"] != DBNull.Value)
                    order.CreatedBy = Convert.ToInt32(row["CreatedBy"]);

                if (row["AssignedTo"] != DBNull.Value)
                    order.AssignedTo = Convert.ToInt32(row["AssignedTo"]);

                orders.Add(order);
            }

            return orders;
        }

        public List<OrderItem> GetOrderDetails(int orderId)
        {
            List<OrderItem> items = new List<OrderItem>();

            try
            {
                string query = @"SELECT OrderDetailID, OrderID, ItemName, Quantity, UnitPrice, Subtotal
                                 FROM OrderDetails 
                                 WHERE OrderID = @OrderID";

                SqlParameter[] parameters = { new SqlParameter("@OrderID", orderId) };
                DataTable dt = ExecuteQuery(query, parameters);

                if (dt != null)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        OrderItem item = new OrderItem();

                        if (row["OrderDetailID"] != DBNull.Value)
                            item.OrderDetailID = Convert.ToInt32(row["OrderDetailID"]);

                        if (row["OrderID"] != DBNull.Value)
                            item.OrderID = Convert.ToInt32(row["OrderID"]);

                        if (row["ItemName"] != DBNull.Value)
                            item.ItemName = row["ItemName"].ToString();
                        else
                            item.ItemName = "Unknown";

                        if (row["Quantity"] != DBNull.Value)
                            item.Quantity = Convert.ToInt32(row["Quantity"]);
                        else
                            item.Quantity = 0;

                        if (row["UnitPrice"] != DBNull.Value)
                            item.UnitPrice = Convert.ToDecimal(row["UnitPrice"]);
                        else
                            item.UnitPrice = 0;

                        if (row["Subtotal"] != DBNull.Value)
                            item.Subtotal = Convert.ToDecimal(row["Subtotal"]);
                        else
                            item.Subtotal = item.Quantity * item.UnitPrice;

                        items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetOrderDetails: {ex.Message}");
            }

            return items;
        }

        public int UpdateOrderStatus(int orderId, string status)
        {
            string query = "UPDATE Orders SET Status = @Status WHERE OrderID = @OrderID";
            SqlParameter[] parameters = {
                new SqlParameter("@Status", status),
                new SqlParameter("@OrderID", orderId)
            };
            return ExecuteNonQuery(query, parameters);
        }

        public int AssignDeliveryStaff(int orderId, int staffId)
        {
            string query = "UPDATE Orders SET AssignedTo = @StaffID WHERE OrderID = @OrderID";
            SqlParameter[] parameters = {
                new SqlParameter("@StaffID", staffId),
                new SqlParameter("@OrderID", orderId)
            };
            return ExecuteNonQuery(query, parameters);
        }

        public List<Order> GetDeliveryOrders()
        {
            List<Order> orders = new List<Order>();
            string query = @"SELECT OrderID, OrderNumber, CustomerName, CustomerPhone, 
                                    DeliveryAddress, OrderDate, TotalAmount, Status
                             FROM Orders 
                             WHERE DeliveryAddress IS NOT NULL 
                             AND DeliveryAddress != '' 
                             AND Status = 'Pending'
                             AND AssignedTo IS NULL
                             ORDER BY OrderDate";

            DataTable dt = ExecuteQuery(query);
            foreach (DataRow row in dt.Rows)
            {
                orders.Add(new Order
                {
                    OrderID = Convert.ToInt32(row["OrderID"]),
                    OrderNumber = row["OrderNumber"].ToString(),
                    CustomerName = row["CustomerName"].ToString(),
                    CustomerPhone = row["CustomerPhone"].ToString(),
                    DeliveryAddress = row["DeliveryAddress"].ToString(),
                    OrderDate = Convert.ToDateTime(row["OrderDate"]),
                    TotalAmount = Convert.ToDecimal(row["TotalAmount"]),
                    Status = row["Status"].ToString()
                });
            }
            return orders;
        }

        public int CreateOrder(Order order, List<OrderItem> items)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    string orderQuery = @"INSERT INTO Orders (OrderNumber, CustomerName, CustomerPhone, DeliveryAddress, 
                                          TotalAmount, Status, PaymentMethod, PaymentStatus, CreatedBy, OrderDate)
                                          OUTPUT INSERTED.OrderID
                                          VALUES (@OrderNumber, @CustomerName, @CustomerPhone, @DeliveryAddress, 
                                          @TotalAmount, @Status, @PaymentMethod, @PaymentStatus, @CreatedBy, @OrderDate)";

                    SqlCommand cmdOrder = new SqlCommand(orderQuery, conn, transaction);
                    cmdOrder.Parameters.AddWithValue("@OrderNumber", order.OrderNumber);
                    cmdOrder.Parameters.AddWithValue("@CustomerName", order.CustomerName ?? "");
                    cmdOrder.Parameters.AddWithValue("@CustomerPhone", order.CustomerPhone ?? "");
                    cmdOrder.Parameters.AddWithValue("@DeliveryAddress", order.DeliveryAddress ?? "");
                    cmdOrder.Parameters.AddWithValue("@TotalAmount", order.TotalAmount);
                    cmdOrder.Parameters.AddWithValue("@Status", order.Status);
                    cmdOrder.Parameters.AddWithValue("@PaymentMethod", order.PaymentMethod);
                    cmdOrder.Parameters.AddWithValue("@PaymentStatus", order.PaymentStatus);
                    cmdOrder.Parameters.AddWithValue("@CreatedBy", order.CreatedBy);
                    cmdOrder.Parameters.AddWithValue("@OrderDate", DateTime.Now);

                    int orderId = (int)cmdOrder.ExecuteScalar();

                    foreach (var item in items)
                    {
                        string detailQuery = @"INSERT INTO OrderDetails (OrderID, ItemName, Quantity, UnitPrice)
                                               VALUES (@OrderID, @ItemName, @Quantity, @UnitPrice)";

                        SqlCommand cmdDetail = new SqlCommand(detailQuery, conn, transaction);
                        cmdDetail.Parameters.AddWithValue("@OrderID", orderId);
                        cmdDetail.Parameters.AddWithValue("@ItemName", item.ItemName);
                        cmdDetail.Parameters.AddWithValue("@Quantity", item.Quantity);
                        cmdDetail.Parameters.AddWithValue("@UnitPrice", item.UnitPrice);

                        cmdDetail.ExecuteNonQuery();
                    }

                    transaction.Commit();
                    return orderId;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        // =============================================
        // DELIVERY METHODS
        // =============================================

        public List<Delivery> GetDeliveriesByStaff(int staffId)
        {
            List<Delivery> deliveries = new List<Delivery>();
            try
            {
                string query = @"SELECT d.DeliveryID, d.OrderID, d.DeliveryStaffID, d.AssignedTime,
                                        d.PickedUpTime, d.DeliveredTime, d.Status,
                                        o.OrderNumber, o.CustomerName, o.CustomerPhone, o.DeliveryAddress
                                 FROM Deliveries d
                                 INNER JOIN Orders o ON d.OrderID = o.OrderID
                                 WHERE d.DeliveryStaffID = @StaffID
                                 ORDER BY d.AssignedTime DESC";

                SqlParameter[] parameters = { new SqlParameter("@StaffID", staffId) };
                DataTable dt = ExecuteQuery(query, parameters);

                foreach (DataRow row in dt.Rows)
                {
                    deliveries.Add(new Delivery
                    {
                        DeliveryID = Convert.ToInt32(row["DeliveryID"]),
                        OrderID = Convert.ToInt32(row["OrderID"]),
                        DeliveryStaffID = Convert.ToInt32(row["DeliveryStaffID"]),
                        AssignedTime = Convert.ToDateTime(row["AssignedTime"]),
                        PickedUpTime = row["PickedUpTime"] != DBNull.Value ? Convert.ToDateTime(row["PickedUpTime"]) : (DateTime?)null,
                        DeliveredTime = row["DeliveredTime"] != DBNull.Value ? Convert.ToDateTime(row["DeliveredTime"]) : (DateTime?)null,
                        Status = row["Status"].ToString(),
                        OrderNumber = row["OrderNumber"].ToString(),
                        CustomerName = row["CustomerName"].ToString(),
                        CustomerPhone = row["CustomerPhone"].ToString(),
                        DeliveryAddress = row["DeliveryAddress"].ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetDeliveriesByStaff: {ex.Message}");
            }
            return deliveries;
        }

        public int UpdateDeliveryStatus(int deliveryId, string status, DateTime? time = null)
        {
            string timeField = "";
            if (status == "PickedUp")
                timeField = ", PickedUpTime = @Time";
            else if (status == "Delivered")
                timeField = ", DeliveredTime = @Time";

            string query = $"UPDATE Deliveries SET Status = @Status {timeField} WHERE DeliveryID = @DeliveryID";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@Status", status),
                new SqlParameter("@DeliveryID", deliveryId)
            };
            if (time.HasValue)
                parameters.Add(new SqlParameter("@Time", time.Value));

            return ExecuteNonQuery(query, parameters.ToArray());
        }

        public int CreateDelivery(Delivery delivery)
        {
            string query = @"INSERT INTO Deliveries (OrderID, DeliveryStaffID, Status, AssignedTime)
                            VALUES (@OrderID, @StaffID, 'Assigned', @AssignedTime);
                            SELECT SCOPE_IDENTITY();";

            SqlParameter[] parameters = {
                new SqlParameter("@OrderID", delivery.OrderID),
                new SqlParameter("@StaffID", delivery.DeliveryStaffID),
                new SqlParameter("@AssignedTime", DateTime.Now)
            };

            return Convert.ToInt32(ExecuteScalar(query, parameters));
        }
    }
}