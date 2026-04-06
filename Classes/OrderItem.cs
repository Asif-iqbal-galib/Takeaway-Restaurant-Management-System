using System;

namespace Takeaway_Restaurant_Management_System.Classes.Models
{
    public class OrderItem
    {
        // Primary Key
        public int OrderDetailID { get; set; }

        // Foreign Keys
        public int OrderID { get; set; }
        public int ItemID { get; set; }

        // Item Details
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }

        // Special Instructions
        public string SpecialInstructions { get; set; }

        // Navigation Properties
        public Order Order { get; set; }
        public MenuItemModel MenuItem { get; set; }

        // Constructor
        public OrderItem()
        {
            Quantity = 1;
        }

        public void CalculateSubtotal()
        {
            Subtotal = Quantity * UnitPrice;
        }

        public override string ToString()
        {
            return $"{Quantity}x {ItemName} - {Subtotal:C}";
        }
    }
}