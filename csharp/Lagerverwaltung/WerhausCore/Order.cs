using System;
using System.Collections.Generic;
using System.Text;

namespace WerhausCore
{
    public class Order
    {
        public int id { get; set; }
        public Dictionary<ProductBase, int> products { get; set; }
        public DateTime timestamp { get; set; }
        public int IdWarehouse { get; set; }
        public Order(int id, Dictionary<ProductBase, int> amounts, DateTime timestamp, int idWarehouse)
        {
            this.id = id;
            products = amounts;
            this.timestamp = timestamp;
            IdWarehouse = idWarehouse;
        }
        public Order()
        {
            products = new Dictionary<ProductBase, int>();
        }
        public int getTotalAmount()
        {
            int result = 0;
            foreach(KeyValuePair<ProductBase, int> p in products)
            {
                result += p.Value;
            }
            return result;
        }
    }
}
