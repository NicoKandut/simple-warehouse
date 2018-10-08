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
        public int space { get; set; }
        public int IdManufacturer { get; set; }

        public ProductBase(int id, string name, string description, double price, int space, int idManufacturer)
        {
            Id = id;
            Name = name;
            Description = description;
            Price = price;
            this.space = space;
            IdManufacturer = idManufacturer;
        }
    }
}
