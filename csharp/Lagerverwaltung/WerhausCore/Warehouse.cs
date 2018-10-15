using System;
using System.Collections.Generic;
using System.Text;

namespace WerhausCore
{
    public class Warehouse
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Location { get; set; }
        public int Capacity { get; set; }
        public Owner Owner { get; set; }
        public List<Product> Products { get; set; }
        public List<Order> Orders { get; set; }
        //public int currentCapacity
        //{
        //    get
        //    {
        //        return GetCurrentCapacity();
        //    }
        //    set
        //    {
        //        currentCapacity = value;
        //    }
        //} 

                public Warehouse(string name, string description, int location, int capacity, Owner owner)
        {
            Name = name;
            Description = description;
            Location = location;
            Capacity = capacity;
            Owner = owner;
            Products = new List<Product>();
            Orders = new List<Order>();
            GetCurrentCapacity();
        }
        public void AddProduct(Product product)
        {
            Products.Add(product);
        }
        public int GetCurrentCapacity()
        {
            int sum = 0;
            foreach (Product p in Products)
            {
                sum += p.space * p.Amount;
            }
            //currentCapacity = sum;
            return sum;
        }
        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }
        public void ChangeDescription(string description)
        {
            Description = description;
        }
        public override string ToString()
        {
            return Name + " with a capacity of " + Capacity + "\n" + Description;
        }
    }
}
