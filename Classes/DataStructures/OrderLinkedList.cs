using System;
using System.Collections.Generic;
using Takeaway_Restaurant_Management_System.Classes.Models;

namespace Takeaway_Restaurant_Management_System.Classes.DataStructures
{
    public class OrderLinkedList
    {
        private Node head;
        private Node tail;
        private int count;

        public OrderLinkedList()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public int Count { get { return count; } }

        public void AddOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            Node newNode = new Node(order);

            if (head == null)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                tail = newNode;
            }
            count++;
        }

        public bool RemoveOrder(int orderId)
        {
            if (head == null) return false;

            if (head.Order.OrderID == orderId)
            {
                head = head.Next;
                if (head == null) tail = null;
                count--;
                return true;
            }

            Node current = head;
            Node previous = null;

            while (current != null && current.Order.OrderID != orderId)
            {
                previous = current;
                current = current.Next;
            }

            if (current == null) return false;

            previous.Next = current.Next;
            if (current == tail) tail = previous;
            count--;
            return true;
        }

        public Order GetOrderById(int orderId)
        {
            Node current = head;
            while (current != null)
            {
                if (current.Order.OrderID == orderId)
                    return current.Order;
                current = current.Next;
            }
            return null;
        }

        public List<Order> GetOrdersByDate(DateTime date)
        {
            List<Order> result = new List<Order>();
            Node current = head;
            while (current != null)
            {
                if (current.Order.OrderDate.Date == date.Date)
                    result.Add(current.Order);
                current = current.Next;
            }
            return result;
        }

        public List<Order> GetOrdersByStatus(string status)
        {
            List<Order> result = new List<Order>();
            Node current = head;
            while (current != null)
            {
                if (current.Order.Status == status)
                    result.Add(current.Order);
                current = current.Next;
            }
            return result;
        }

        public List<Order> GetPendingOrders()
        {
            List<Order> result = new List<Order>();
            Node current = head;
            while (current != null)
            {
                if (current.Order.Status == "Pending" || current.Order.Status == "Preparing")
                    result.Add(current.Order);
                current = current.Next;
            }
            return result;
        }

        public bool UpdateOrderStatus(int orderId, string newStatus)
        {
            Order order = GetOrderById(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                return true;
            }
            return false;
        }

        public List<Order> GetAllOrders()
        {
            List<Order> result = new List<Order>();
            Node current = head;
            while (current != null)
            {
                result.Add(current.Order);
                current = current.Next;
            }
            return result;
        }

        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }
    }
}