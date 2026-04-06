using System;

namespace Takeaway_Restaurant_Management_System.Classes.Models
{
    public class Inventory
    {
        public int ItemID { get; set; }
        public string ItemName { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } // kg, liter, piece, box, etc.
        public decimal ReorderLevel { get; set; }
        public DateTime? LastRestocked { get; set; }
        public bool IsActive { get; set; }

        public Inventory()
        {
            IsActive = true;
            Quantity = 0;
            ReorderLevel = 10;
        }

        public bool IsLowStock
        {
            get { return Quantity <= ReorderLevel; }
        }

        public override string ToString()
        {
            return $"{ItemName} - {Quantity} {Unit}";
        }

        public string GetStatusDisplay()
        {
            if (IsLowStock)
                return "⚠️ LOW STOCK";
            else
                return "✅ OK";
        }
    }
}