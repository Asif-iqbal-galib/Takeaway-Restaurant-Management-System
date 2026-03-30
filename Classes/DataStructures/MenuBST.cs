using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Takeaway_Restaurant_Management_System.Classes.Models;

namespace Takeaway_Restaurant_Management_System.Classes.DataStructures
{
    /// <summary>
    /// Custom Binary Search Tree for Menu Items
    /// Time Complexity: O(log n) average, O(n) worst case
    /// No STL collections used - pure custom implementation
    /// </summary>
    public class MenuBST
    {
        private TreeNode root;
        private int count;

        public MenuBST()
        {
            root = null;
            count = 0;
        }

        public int Count { get { return count; } }

        /// <summary>
        /// Insert a new menu item into the BST
        /// Time Complexity: O(log n) average, O(n) worst case
        /// </summary>
        public void Insert(MenuItemModel item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            root = InsertRecursive(root, item);
            count++;
        }

        private TreeNode InsertRecursive(TreeNode node, MenuItemModel item)
        {
            if (node == null)
                return new TreeNode(item);

            // Compare by name (case-insensitive)
            int comparison = string.Compare(item.Name, node.Item.Name, StringComparison.OrdinalIgnoreCase);

            if (comparison < 0)
                node.Left = InsertRecursive(node.Left, item);
            else if (comparison > 0)
                node.Right = InsertRecursive(node.Right, item);
            else
                throw new InvalidOperationException($"Item with name '{item.Name}' already exists.");

            return node;
        }

        /// <summary>
        /// Search for an item by name
        /// Time Complexity: O(log n) average, O(n) worst case
        /// </summary>
        public MenuItemModel SearchByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return SearchRecursive(root, name);
        }

        private MenuItemModel SearchRecursive(TreeNode node, string name)
        {
            if (node == null)
                return null;

            int comparison = string.Compare(name, node.Item.Name, StringComparison.OrdinalIgnoreCase);

            if (comparison == 0)
                return node.Item;
            else if (comparison < 0)
                return SearchRecursive(node.Left, name);
            else
                return SearchRecursive(node.Right, name);
        }

        /// <summary>
        /// Search for an item by ID
        /// Time Complexity: O(n) - must traverse tree
        /// </summary>
        public MenuItemModel SearchById(int itemId)
        {
            return SearchByIdRecursive(root, itemId);
        }

        private MenuItemModel SearchByIdRecursive(TreeNode node, int itemId)
        {
            if (node == null)
                return null;

            if (node.Item.ItemID == itemId)
                return node.Item;

            MenuItemModel leftResult = SearchByIdRecursive(node.Left, itemId);
            if (leftResult != null)
                return leftResult;

            return SearchByIdRecursive(node.Right, itemId);
        }

        /// <summary>
        /// Delete an item by ID
        /// Time Complexity: O(log n) average, O(n) worst case
        /// </summary>
        public bool Delete(int itemId)
        {
            int oldCount = count;
            root = DeleteRecursive(root, itemId);
            return count < oldCount;
        }

        private TreeNode DeleteRecursive(TreeNode node, int itemId)
        {
            if (node == null)
                return null;

            if (itemId < node.Item.ItemID)
                node.Left = DeleteRecursive(node.Left, itemId);
            else if (itemId > node.Item.ItemID)
                node.Right = DeleteRecursive(node.Right, itemId);
            else
            {
                // Case 1: No child (leaf node)
                if (node.Left == null && node.Right == null)
                {
                    count--;
                    return null;
                }
                // Case 2: One child
                else if (node.Left == null)
                {
                    count--;
                    return node.Right;
                }
                else if (node.Right == null)
                {
                    count--;
                    return node.Left;
                }
                // Case 3: Two children
                else
                {
                    // Find inorder successor (smallest in right subtree)
                    TreeNode successor = FindMin(node.Right);
                    node.Item = successor.Item;
                    node.Right = DeleteRecursive(node.Right, successor.Item.ItemID);
                }
            }

            return node;
        }

        private TreeNode FindMin(TreeNode node)
        {
            while (node.Left != null)
                node = node.Left;
            return node;
        }

        /// <summary>
        /// Get all items in sorted order (in-order traversal)
        /// Time Complexity: O(n)
        /// </summary>
        public List<MenuItemModel> GetAllItems()
        {
            List<MenuItemModel> items = new List<MenuItemModel>();
            InOrderTraversal(root, items);
            return items;
        }

        private void InOrderTraversal(TreeNode node, List<MenuItemModel> items)
        {
            if (node != null)
            {
                InOrderTraversal(node.Left, items);
                items.Add(node.Item);
                InOrderTraversal(node.Right, items);
            }
        }

        /// <summary>
        /// Get items by category
        /// Time Complexity: O(n)
        /// </summary>
        public List<MenuItemModel> GetItemsByCategory(string category)
        {
            List<MenuItemModel> allItems = GetAllItems();
            return allItems.FindAll(item => item.CategoryName.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Get available items only
        /// Time Complexity: O(n)
        /// </summary>
        public List<MenuItemModel> GetAvailableItems()
        {
            List<MenuItemModel> allItems = GetAllItems();
            return allItems.FindAll(item => item.IsAvailable);
        }
    }
}