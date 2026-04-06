using System;

namespace Takeaway_Restaurant_Management_System.Classes.Models
{
    public class Customer
    {
        public int CustomerID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int LoyaltyPoints { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }

        public Customer()
        {
            RegistrationDate = DateTime.Now;
            IsActive = true;
            LoyaltyPoints = 0;
        }

        public Customer(string name, string phone)
        {
            FullName = name;
            Phone = phone;
            RegistrationDate = DateTime.Now;
            IsActive = true;
            LoyaltyPoints = 0;
        }

        public Customer(string name, string phone, string address)
        {
            FullName = name;
            Phone = phone;
            Address = address;
            RegistrationDate = DateTime.Now;
            IsActive = true;
            LoyaltyPoints = 0;
        }

        public override string ToString()
        {
            return $"{FullName} - {Phone}";
        }
    }
}