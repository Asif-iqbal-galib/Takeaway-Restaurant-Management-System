-- =============================================
-- Takeaway Restaurant Management System
-- Database Creation Script
-- =============================================

-- Create Database
CREATE DATABASE TakeawayDB;
GO

USE TakeawayDB;
GO

-- =============================================
-- TABLES
-- =============================================

-- Staff table
CREATE TABLE Staff (
    StaffID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Role NVARCHAR(20) CHECK (Role IN ('Admin', 'Cashier', 'Kitchen', 'Delivery')) NOT NULL,
    Phone NVARCHAR(15),
    Email NVARCHAR(100),
    HireDate DATETIME DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1
);
GO

-- Categories table
CREATE TABLE Categories (
    CategoryID INT PRIMARY KEY IDENTITY(1,1),
    CategoryName NVARCHAR(50) UNIQUE NOT NULL,
    Description NVARCHAR(255),
    DisplayOrder INT DEFAULT 0
);
GO

-- MenuItems table
CREATE TABLE MenuItems (
    ItemID INT PRIMARY KEY IDENTITY(1,1),
    ItemName NVARCHAR(100) NOT NULL,
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID),
    Price DECIMAL(10,2) NOT NULL CHECK (Price > 0),
    Description NVARCHAR(500),
    PreparationTime INT DEFAULT 10,
    IsAvailable BIT DEFAULT 1,
    IsVegetarian BIT DEFAULT 0,
    IsSpicy BIT DEFAULT 0
);
GO

-- Customers table
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY(1,1),
    FullName NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(15) NOT NULL,
    Email NVARCHAR(100),
    Address NVARCHAR(255),
    LoyaltyPoints INT DEFAULT 0,
    RegistrationDate DATETIME DEFAULT GETDATE(),
    IsActive BIT DEFAULT 1
);
GO

-- Orders table
CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY(1,1),
    OrderNumber NVARCHAR(20) UNIQUE NOT NULL,
    CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID),
    CustomerName NVARCHAR(100),
    CustomerPhone NVARCHAR(15),
    DeliveryAddress NVARCHAR(500),
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(10,2) NOT NULL DEFAULT 0,
    Status NVARCHAR(20) CHECK (Status IN ('Pending', 'Preparing', 'Ready', 'Completed', 'Cancelled', 'OutForDelivery', 'Delivered')) DEFAULT 'Pending',
    PaymentMethod NVARCHAR(20) CHECK (PaymentMethod IN ('Cash', 'Card', 'Online', 'Not Paid')) DEFAULT 'Not Paid',
    PaymentStatus NVARCHAR(20) CHECK (PaymentStatus IN ('Paid', 'Unpaid', 'Refunded')) DEFAULT 'Unpaid',
    CreatedBy INT FOREIGN KEY REFERENCES Staff(StaffID),
    AssignedTo INT FOREIGN KEY REFERENCES Staff(StaffID) NULL,
    SpecialInstructions NVARCHAR(500) NULL
);
GO

-- OrderDetails table
CREATE TABLE OrderDetails (
    OrderDetailID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID) ON DELETE CASCADE,
    ItemID INT FOREIGN KEY REFERENCES MenuItems(ItemID),
    ItemName NVARCHAR(100) NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    UnitPrice DECIMAL(10,2) NOT NULL,
    Subtotal AS (Quantity * UnitPrice) PERSISTED,
    SpecialInstructions NVARCHAR(255) NULL
);
GO

-- Payments table
CREATE TABLE Payments (
    PaymentID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
    Amount DECIMAL(10,2) NOT NULL,
    PaymentMethod NVARCHAR(20) CHECK (PaymentMethod IN ('Cash', 'Card', 'Online')),
    PaymentDate DATETIME DEFAULT GETDATE(),
    ReceivedBy INT FOREIGN KEY REFERENCES Staff(StaffID),
    ChangeGiven DECIMAL(10,2) DEFAULT 0,
    CardLastFour CHAR(4) NULL
);
GO

-- Deliveries table
CREATE TABLE Deliveries (
    DeliveryID INT PRIMARY KEY IDENTITY(1,1),
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
    DeliveryStaffID INT FOREIGN KEY REFERENCES Staff(StaffID),
    AssignedTime DATETIME DEFAULT GETDATE(),
    PickedUpTime DATETIME NULL,
    DeliveredTime DATETIME NULL,
    Status NVARCHAR(20) CHECK (Status IN ('Assigned', 'PickedUp', 'Delivered', 'Failed')) DEFAULT 'Assigned',
    DeliveryNotes NVARCHAR(500) NULL
);
GO

-- Inventory table
CREATE TABLE Inventory (
    ItemID INT PRIMARY KEY IDENTITY(1,1),
    ItemName NVARCHAR(100) NOT NULL,
    Quantity DECIMAL(10,2) NOT NULL DEFAULT 0,
    Unit NVARCHAR(20) NOT NULL,
    ReorderLevel DECIMAL(10,2) NOT NULL DEFAULT 10,
    LastRestocked DATETIME NULL,
    IsActive BIT DEFAULT 1
);
GO

-- =============================================
-- INDEXES
-- =============================================

CREATE INDEX IX_Orders_OrderDate ON Orders(OrderDate);
CREATE INDEX IX_Orders_Status ON Orders(Status);
CREATE INDEX IX_Orders_CustomerID ON Orders(CustomerID);
CREATE INDEX IX_OrderDetails_OrderID ON OrderDetails(OrderID);
CREATE INDEX IX_OrderDetails_ItemID ON OrderDetails(ItemID);
CREATE INDEX IX_MenuItems_CategoryID ON MenuItems(CategoryID);
CREATE INDEX IX_Deliveries_OrderID ON Deliveries(OrderID);
CREATE INDEX IX_Deliveries_Status ON Deliveries(Status);
GO

-- =============================================
-- SAMPLE DATA - STAFF
-- =============================================

INSERT INTO Staff (Username, PasswordHash, FullName, Role, Phone, Email, IsActive) VALUES 
('admin123', 'admin123', 'Admin User', 'Admin', '01711111111', 'admin@appeliano.com', 1),
('cashier1', 'cash123', 'John Cashier', 'Cashier', '01722222222', 'john@appeliano.com', 1),
('kitchen1', 'kitchen123', 'Mike Kitchen', 'Kitchen', '01733333333', 'mike@appeliano.com', 1),
('delivery1', 'delivery123', 'Sam Delivery', 'Delivery', '01744444444', 'sam@appeliano.com', 1);
GO

-- =============================================
-- SAMPLE DATA - CATEGORIES
-- =============================================

INSERT INTO Categories (CategoryName, Description, DisplayOrder) VALUES 
('Burgers', 'Gourmet burgers', 1),
('Pizzas', 'Wood-fired pizzas', 2),
('Chicken', 'Grilled & Fried Chicken', 3),
('Rice', 'Rice & Biryani', 4),
('Beverages', 'Drinks', 5),
('Sides', 'Sides & Dips', 6);
GO

-- =============================================
-- SAMPLE DATA - MENU ITEMS (67 ITEMS)
-- =============================================

DECLARE @BurgersId INT = (SELECT CategoryID FROM Categories WHERE CategoryName = 'Burgers');
DECLARE @PizzasId INT = (SELECT CategoryID FROM Categories WHERE CategoryName = 'Pizzas');
DECLARE @ChickenId INT = (SELECT CategoryID FROM Categories WHERE CategoryName = 'Chicken');
DECLARE @RiceId INT = (SELECT CategoryID FROM Categories WHERE CategoryName = 'Rice');
DECLARE @BeveragesId INT = (SELECT CategoryID FROM Categories WHERE CategoryName = 'Beverages');
DECLARE @SidesId INT = (SELECT CategoryID FROM Categories WHERE CategoryName = 'Sides');

-- BURGERS (12 items)
INSERT INTO MenuItems (ItemName, CategoryID, Price, Description, PreparationTime) VALUES 
('Classic Beef Burger', @BurgersId, 8.99, 'Beef patty with lettuce, tomato, mayo', 12),
('Cheeseburger', @BurgersId, 9.49, 'Beef patty with cheese', 12),
('Double Cheeseburger', @BurgersId, 11.99, 'Two beef patties with double cheese', 15),
('Chicken Fillet Burger', @BurgersId, 8.99, 'Crispy chicken fillet with mayo', 12),
('Spicy Chicken Burger', @BurgersId, 9.49, 'Spicy chicken fillet with jalapeños', 12),
('Grilled Chicken Burger', @BurgersId, 9.99, 'Grilled chicken breast with herbs', 12),
('Zinger Tower Burger', @BurgersId, 10.99, 'Zinger fillet with hash brown', 12),
('Doner Kebab Burger', @BurgersId, 9.99, 'Doner meat with chilli sauce', 12),
('Veggie Burger', @BurgersId, 7.99, 'Plant-based patty with avocado', 10),
('Fish Burger', @BurgersId, 8.99, 'Crispy fish fillet with tartar sauce', 12),
('BBQ Bacon Burger', @BurgersId, 10.99, 'Beef patty with bacon and BBQ sauce', 12),
('Peri Peri Chicken Burger', @BurgersId, 9.99, 'Peri peri chicken with spicy mayo', 12);

-- PIZZAS (12 items)
INSERT INTO MenuItems (ItemName, CategoryID, Price, Description, PreparationTime) VALUES 
('Margherita', @PizzasId, 12.99, 'Tomato sauce, mozzarella, basil', 15),
('Pepperoni', @PizzasId, 13.99, 'Tomato sauce, mozzarella, pepperoni', 15),
('Hawaiian', @PizzasId, 13.99, 'Ham, pineapple, mozzarella', 15),
('BBQ Chicken', @PizzasId, 14.99, 'BBQ sauce, chicken, red onions', 15),
('Meat Feast', @PizzasId, 14.99, 'Pepperoni, ham, sausage, beef', 15),
('Veggie Supreme', @PizzasId, 13.99, 'Mushrooms, peppers, onions, olives', 15),
('Chicken Supreme', @PizzasId, 15.99, 'Chicken, peppers, onions', 15),
('Spicy Hot', @PizzasId, 14.99, 'Pepperoni, jalapeños, chilli', 15),
('Four Cheese', @PizzasId, 14.99, 'Mozzarella, cheddar, parmesan, gorgonzola', 15),
('Chicken & Mushroom', @PizzasId, 14.99, 'Chicken, mushrooms, garlic sauce', 15),
('Tandoori Chicken Pizza', @PizzasId, 15.99, 'Tandoori chicken, onions, peppers', 15),
('Doner Pizza', @PizzasId, 14.99, 'Doner meat, onions, chilli sauce', 15);

-- CHICKEN (11 items)
INSERT INTO MenuItems (ItemName, CategoryID, Price, Description, PreparationTime) VALUES 
('1/4 Grilled Chicken', @ChickenId, 5.99, 'Quarter grilled chicken with salad', 15),
('1/2 Grilled Chicken', @ChickenId, 9.99, 'Half grilled chicken with salad', 20),
('Full Grilled Chicken', @ChickenId, 16.99, 'Full grilled chicken with salad', 25),
('Peri Peri Wings (6pcs)', @ChickenId, 6.99, 'Spicy peri peri chicken wings', 12),
('Chicken Strips (4pcs)', @ChickenId, 5.99, 'Crispy chicken strips', 8),
('2pcs Chicken & Chips', @ChickenId, 7.99, '2 pieces fried chicken with chips', 12),
('3pcs Chicken Meal', @ChickenId, 9.99, '3 pieces fried chicken with chips & drink', 12),
('Chicken Wings Meal (6pcs)', @ChickenId, 8.99, '6 chicken wings with chips & drink', 12),
('Chicken Nuggets Meal (9pcs)', @ChickenId, 7.99, '9 chicken nuggets with chips & drink', 10),
('Chicken Strip Meal (4pcs)', @ChickenId, 8.99, '4 chicken strips with chips & drink', 10),
('Family Chicken Bucket (8pcs)', @ChickenId, 24.99, '8 pieces chicken with chips & 2 drinks', 20);

-- RICE (9 items)
INSERT INTO MenuItems (ItemName, CategoryID, Price, Description, PreparationTime) VALUES 
('Chicken Fried Rice', @RiceId, 6.99, 'Fried rice with chicken', 10),
('Egg Fried Rice', @RiceId, 5.99, 'Fried rice with egg', 8),
('Vegetable Fried Rice', @RiceId, 5.99, 'Fried rice with mixed vegetables', 8),
('Chicken Biryani', @RiceId, 8.99, 'Spicy chicken biryani', 15),
('Lamb Biryani', @RiceId, 9.99, 'Spicy lamb biryani', 15),
('Prawn Biryani', @RiceId, 10.99, 'Spicy prawn biryani', 15),
('Special Mixed Biryani', @RiceId, 11.99, 'Chicken, lamb & prawn biryani', 15),
('Pilau Rice', @RiceId, 3.99, 'Fragrant pilau rice', 8),
('Plain Boiled Rice', @RiceId, 2.99, 'Steamed white rice', 5);

-- BEVERAGES (9 items)
INSERT INTO MenuItems (ItemName, CategoryID, Price, Description, PreparationTime) VALUES 
('Coca-Cola', @BeveragesId, 2.50, '330ml can', 2),
('Diet Coke', @BeveragesId, 2.50, '330ml can', 2),
('Pepsi', @BeveragesId, 2.50, '330ml can', 2),
('Fanta', @BeveragesId, 2.50, '330ml can', 2),
('Sprite', @BeveragesId, 2.50, '330ml can', 2),
('Mineral Water', @BeveragesId, 1.99, '500ml bottle', 2),
('Fruit Juice', @BeveragesId, 3.50, 'Fresh fruit juice', 3),
('Milkshake', @BeveragesId, 4.50, 'Chocolate, strawberry or vanilla', 5),
('Energy Drink', @BeveragesId, 3.00, 'Energy boost', 2);

-- SIDES (8 items)
INSERT INTO MenuItems (ItemName, CategoryID, Price, Description, PreparationTime) VALUES 
('Chips / Fries', @SidesId, 3.50, 'Crispy fries', 6),
('Cheesy Chips', @SidesId, 4.50, 'Fries with melted cheese', 8),
('Onion Rings', @SidesId, 4.00, 'Crispy onion rings', 6),
('Garlic Bread', @SidesId, 3.00, 'Toasted garlic bread', 5),
('Coleslaw', @SidesId, 2.50, 'Fresh creamy coleslaw', 2),
('Garlic Mayo Dip', @SidesId, 1.00, 'Garlic mayonnaise dip', 1),
('BBQ Dip', @SidesId, 1.00, 'BBQ sauce dip', 1),
('Chilli Dip', @SidesId, 1.00, 'Spicy chilli dip', 1);

-- =============================================
-- TOTAL: 12+12+11+9+9+8 = 61 MENU ITEMS
-- =============================================

-- =============================================
-- SAMPLE DATA - INVENTORY
-- =============================================

INSERT INTO Inventory (ItemName, Quantity, Unit, ReorderLevel) VALUES 
('Chicken Breast', 50, 'kg', 10),
('Beef Mince', 30, 'kg', 8),
('Cheese', 25, 'kg', 5),
('Lettuce', 100, 'piece', 20),
('Tomatoes', 40, 'kg', 10),
('Onions', 60, 'kg', 15),
('Flour', 80, 'kg', 20),
('Burger Buns', 150, 'piece', 30),
('Coca-Cola', 200, 'can', 50),
('Fries', 100, 'kg', 25),
('Pizza Dough', 50, 'piece', 15),
('Mozzarella', 30, 'kg', 8),
('Pepperoni', 20, 'kg', 5),
('Chicken Wings', 40, 'kg', 10),
('Rice', 100, 'kg', 25);
GO

-- =============================================
-- SAMPLE DATA - CUSTOMERS
-- =============================================

INSERT INTO Customers (FullName, Phone, Email, Address) VALUES 
('John Doe', '01711111111', 'john@email.com', '123 Main Street, London'),
('Jane Smith', '01722222222', 'jane@email.com', '456 Park Road, London'),
('Bob Johnson', '01733333333', 'bob@email.com', '789 Lake Avenue, London');
GO

-- =============================================
-- SAMPLE DATA - ORDERS
-- =============================================

INSERT INTO Orders (OrderNumber, CustomerName, CustomerPhone, TotalAmount, Status, PaymentMethod, PaymentStatus, CreatedBy, OrderDate) VALUES 
('ORD-20260406-001', 'John Doe', '01711111111', 25.97, 'Completed', 'Cash', 'Paid', 1, GETDATE()),
('ORD-20260406-002', 'Jane Smith', '01722222222', 42.50, 'Pending', 'Card', 'Paid', 1, GETDATE()),
('ORD-20260406-003', 'Bob Johnson', '01733333333', 15.99, 'Preparing', 'Cash', 'Paid', 2, GETDATE());
GO

-- =============================================
-- SAMPLE DATA - ORDER DETAILS
-- =============================================

INSERT INTO OrderDetails (OrderID, ItemName, Quantity, UnitPrice) VALUES 
(1, 'Classic Beef Burger', 2, 8.99),
(1, 'Chips / Fries', 1, 3.99),
(2, 'Margherita', 1, 12.99),
(2, 'Coca-Cola', 2, 2.50),
(2, 'Garlic Bread', 1, 3.00),
(3, 'Chicken Biryani', 1, 8.99),
(3, 'Pepsi', 1, 2.50),
(3, 'Onion Rings', 1, 4.00);
GO

-- =============================================
-- VERIFICATION
-- =============================================

PRINT '========================================';
PRINT 'Database created successfully!';
PRINT '========================================';
PRINT 'Staff: ' + CAST((SELECT COUNT(*) FROM Staff) AS VARCHAR) + ' records';
PRINT 'Categories: ' + CAST((SELECT COUNT(*) FROM Categories) AS VARCHAR) + ' records';
PRINT 'Menu Items: ' + CAST((SELECT COUNT(*) FROM MenuItems) AS VARCHAR) + ' records';
PRINT 'Customers: ' + CAST((SELECT COUNT(*) FROM Customers) AS VARCHAR) + ' records';
PRINT 'Orders: ' + CAST((SELECT COUNT(*) FROM Orders) AS VARCHAR) + ' records';
PRINT 'OrderDetails: ' + CAST((SELECT COUNT(*) FROM OrderDetails) AS VARCHAR) + ' records';
PRINT 'Inventory: ' + CAST((SELECT COUNT(*) FROM Inventory) AS VARCHAR) + ' records';
PRINT '========================================';
PRINT '';
PRINT 'Login Credentials:';
PRINT '  Admin:    admin123 / admin123';
PRINT '  Cashier:  cashier1 / cash123';
PRINT '  Kitchen:  kitchen1 / kitchen123';
PRINT '  Delivery: delivery1 / delivery123';
PRINT '========================================';
GO