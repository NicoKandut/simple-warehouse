using System;
using System.Collections.Generic;
using System.Text;

namespace WerhausCore
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Location { get; set; }
        public int Capacity { get; set; }
        public Owner Owner { get; set; }
        public List<Product> Products { get; set; }
        public List<Order> Orders { get; set; }
        private int currentCapacity = 0;
        public int CurrentCapacity
        {
            get
            {
                return currentCapacity;
            }
            set
            {
                currentCapacity = value;
            }
        }

        public Warehouse(string name, string description, int location, int capacity, Owner owner, List<Product> products, List<Order> orders)
        {
            Name = name;
            Description = description;
            Location = location;
            Capacity = capacity;
            Owner = owner;
            Products = products;
            Orders = orders;
            currentCapacity = GetCurrentCapacity();
        }
        //public Warehouse(string name, string description, int location, int capacity, Owner owner)
        //{
        //    Name = name;
        //    Description = description;
        //    Location = location;
        //    Capacity = capacity;
        //    Owner = owner;
        //    Products = new List<Product>();
        //    Orders = new List<Order>();
        //    currentCapacity = GetCurrentCapacity();
        //}
        public int GetCurrentCapacity()
        {
            if (Products != null)
            {
                int sum = 0;

                foreach (Product p in Products)
                {
                    sum += p.Space * p.Amount;
                }
                return sum;
            }
            return 0;
        }
        public override string ToString()
        {
            return Name + " with a capacity of " + Capacity + "\n" + Description;
        }
    }
}