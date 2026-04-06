using System;

namespace Takeaway_Restaurant_Management_System.Classes.Models
{
    public class MenuItemModel
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int PreparationTime { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsSpicy { get; set; }

        public MenuItemModel()
        {
            IsAvailable = true;
            PreparationTime = 10;
        }

        public MenuItemModel(int id, string name, decimal price, string category)
        {
            ItemID = id;
            Name = name;
            Price = price;
            CategoryName = category;
            IsAvailable = true;
            PreparationTime = 10;
        }

        public override string ToString()
        {
            return $"{Name} - {Price:C}";
        }

        public string GetFormattedPrice()
        {
            return Price.ToString("C");
        }
    }
}