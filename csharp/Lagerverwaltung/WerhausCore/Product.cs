using System;
using System.Collections.Generic;
using System.Text;

namespace WerhausCore
{
    public class Product : ProductBase
    {
        public int Amount { get; set; }
        public Product(int id, string name, string description, double price, int space, int amount) : base(id, name, description, price, space)
        {
            Amount = amount;
        }
        public double calculateCosts()
        {
            return Amount*Price;
        }
        public override string ToString()
        {
            return Name + " | " + string.Format("{0:0.00}", Price)+"€";
        }
    }
}
