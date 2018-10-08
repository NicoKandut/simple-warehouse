using System;
using System.Collections.Generic;
using System.Text;

namespace WerhausCore
{
    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ProductBase> ProducedProducts { get; set; }

        public Manufacturer(int id, string name, string description)
        {
            Id = id;
            Name = name;
            Description = description;
            ProducedProducts = new List<ProductBase>();
        }
        public void changeDescription(string description)
        {
            Description = description;
        }
        public void addProducedProduct(ProductBase product)
        {
            ProducedProducts.Add(product);
        }
    }
}
