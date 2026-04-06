using System;

namespace Takeaway_Restaurant_Management_System.Classes.Models
{
    public class Delivery
    {
        public int DeliveryID { get; set; }
        public int OrderID { get; set; }
        public int DeliveryStaffID { get; set; }
        public DateTime AssignedTime { get; set; }
        public DateTime? PickedUpTime { get; set; }
        public DateTime? DeliveredTime { get; set; }
        public string Status { get; set; } // Assigned, PickedUp, Delivered, Failed

        // Order details for display
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string DeliveryAddress { get; set; }
        public string DeliveryNotes { get; set; }

        public Delivery()
        {
            AssignedTime = DateTime.Now;
            Status = "Assigned";
        }

        public string GetStatusDisplay()
        {
            if (Status == "Assigned")
                return "📋 Assigned";
            else if (Status == "PickedUp")
                return "📦 Picked Up";
            else if (Status == "Delivered")
                return "✅ Delivered";
            else if (Status == "Failed")
                return "❌ Failed";
            else
                return Status;
        }
    }
}