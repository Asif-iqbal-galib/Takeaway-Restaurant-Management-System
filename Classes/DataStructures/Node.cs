using Takeaway_Restaurant_Management_System.Classes.Models;

namespace Takeaway_Restaurant_Management_System.Classes.DataStructures
{
    public class Node
    {
        public Order Order { get; set; }
        public Node Next { get; set; }

        public Node(Order order)
        {
            Order = order;
            Next = null;
        }
    }
}