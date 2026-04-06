using System;
using System.Collections.Generic;

namespace Takeaway_Restaurant_Management_System.Classes.Models
{
    public class Order
    {
        // Primary Key
        public int OrderID { get; set; }

        // Order Identification
        public string OrderNumber { get; set; }

        // Customer Information
        public int? CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string DeliveryAddress { get; set; }

        // Order Details
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // Pending, Preparing, Ready, Completed, Cancelled

        // Payment Information
        public string PaymentMethod { get; set; } // Cash, Card, Online
        public string PaymentStatus { get; set; } // Paid, Unpaid, Refunded

        // Staff Information
        public int CreatedBy { get; set; } // Cashier who took the order
        public int? AssignedTo { get; set; } // Delivery staff ID

        // Additional Information
        public string SpecialInstructions { get; set; }
        public DateTime? EstimatedDeliveryTime { get; set; }

        // Navigation Properties
        public List<OrderItem> Items { get; set; }
        public Staff Cashier { get; set; }
        public Staff DeliveryStaff { get; set; }

        // Constructor
        public Order()
        {
            Items = new List<OrderItem>();
            OrderDate = DateTime.Now;
            Status = "Pending";
            PaymentStatus = "Unpaid";
        }

        // Helper Methods
        public void CalculateTotal()
        {
            decimal subtotal = 0;
            foreach (var item in Items)
            {
                if (item != null)
                {
                    subtotal += item.Subtotal;
                }
            }
            TotalAmount = subtotal;
        }

        public string GetStatusDisplay()
        {
            if (Status == "Pending")
                return "⏳ Pending";
            else if (Status == "Preparing")
                return "👨‍🍳 Preparing";
            else if (Status == "Ready")
                return "✅ Ready";
            else if (Status == "OutForDelivery")
                return "🚚 Out for Delivery";
            else if (Status == "Delivered")
                return "📦 Delivered";
            else if (Status == "Completed")
                return "🎉 Completed";
            else if (Status == "Cancelled")
                return "❌ Cancelled";
            else
                return Status;
        }

        public string GetPaymentStatusDisplay()
        {
            if (PaymentStatus == "Paid")
                return "✅ Paid";
            else if (PaymentStatus == "Unpaid")
                return "⚠️ Unpaid";
            else if (PaymentStatus == "Refunded")
                return "💰 Refunded";
            else
                return PaymentStatus;
        }

        public override string ToString()
        {
            return $"{OrderNumber} - {CustomerName} - {TotalAmount:C}";
        }

        public string GetOrderSummary()
        {
            string summary = $"Order: {OrderNumber}\n";
            summary += $"Customer: {CustomerName}\n";
            summary += $"Date: {OrderDate:dd/MM/yyyy HH:mm}\n";
            summary += $"Items: {Items.Count}\n";
            summary += $"Total: {TotalAmount:C}\n";
            summary += $"Status: {GetStatusDisplay()}\n";
            summary += $"Payment: {GetPaymentStatusDisplay()}";
            return summary;
        }
    }
}