using System;
using System.Collections.Generic;
using System.Text;

namespace WerhausCore
{
    public class ProductBase
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Space { get; set; }

        public ProductBase(int id, string name, string description, double price, int space)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            Space = space;
        }
        public override string ToString()
        {
            return Name + " | " + string.Format("{0:0.00}", Price)+"€";
        }
    }
}
