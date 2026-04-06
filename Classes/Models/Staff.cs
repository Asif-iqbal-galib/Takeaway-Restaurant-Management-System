using System;

namespace Takeaway_Restaurant_Management_System.Classes.Models
{
    public class Staff
    {
        public int StaffID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public Staff()
        {
            HireDate = DateTime.Now;
            IsActive = true;
        }

        public override string ToString()
        {
            return $"{FullName} ({Role})";
        }

        public bool IsAdmin => Role == "Admin";
        public bool IsCashier => Role == "Cashier";
        public bool IsKitchen => Role == "Kitchen";
        public bool IsDelivery => Role == "Delivery";
    }
}