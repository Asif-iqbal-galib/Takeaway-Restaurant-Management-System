using System;
using Takeaway_Restaurant_Management_System.Classes.Models;

namespace Takeaway_Restaurant_Management_System.Classes.DataStructures
{
    public class TreeNode
    {
        public MenuItemModel Item { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }

        public TreeNode(MenuItemModel item)
        {
            Item = item;
            Left = null;
            Right = null;
        }
    }
}