using Takeaway_Restaurant_Management_System.Classes.Models;

namespace Takeaway_Restaurant_Management_System.Classes.Utilities
{
    public static class CurrentUser
    {
        public static int UserID { get; set; }
        public static string Username { get; set; }
        public static string FullName { get; set; }
        public static string Role { get; set; }

        public static bool IsAdmin => Role == "Admin";
        public static bool IsCashier => Role == "Cashier";
        public static bool IsKitchen => Role == "Kitchen";
        public static bool IsDelivery => Role == "Delivery";

        public static void Clear()
        {
            UserID = 0;
            Username = null;
            FullName = null;
            Role = null;
        }
    }
}
