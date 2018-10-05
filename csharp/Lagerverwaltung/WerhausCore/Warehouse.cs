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
        public List<ProductBase> Products { get; set; }
        public List<Order> Orders { get; set; }
        public Warehouse(string name, string description, int location, int capacity, Owner owner)
        {
            Name = name;
            Description = description;
            Location = location;
            Capacity = capacity;
            Owner = owner;
            Products = new List<ProductBase>();
            Orders = new List<Order>();
        }
        public void AddProduct(Product product)
        {
            Products.Add(product);
        }
        public void AddOrder(Order order)
        {
            Orders.Add(order);
        }
        public void ChangeDescription(string description)
        {
            Description = description;
        }
        public void Order()
        {
            //TODO: call webservice with order
            //update information
        }
    }
}
