using System;
using System.Collections.Generic;
using System.Text;

namespace WerhausCore
{
    public class Product : ProductBase
    {
        public double Amount { get; set; }
        public Product(int id, string name, string description, double price, int space, int idManufacturer, int amount) : base(id, name, description, price, space, idManufacturer)
        {
            Amount = amount;
        }
        public double calculateCosts()
        {
            return Amount*Price;
        }
    }
}
