using System;

namespace Takeaway_Restaurant_Management_System.Classes.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int DisplayOrder { get; set; }

        public Category()
        {
        }

        public Category(int id, string name)
        {
            CategoryID = id;
            CategoryName = name;
        }

        public override string ToString()
        {
            return CategoryName;
        }
    }
}