﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WerhausCore
{
    public class Order
    {
        public int Id { get; set; }
        public Dictionary<ProductBase, int> Amounts { get; set; }
        public DateTime Timestamp { get; set; }
        public int IdWarehouse { get; set; }
        public Order(int id, Dictionary<ProductBase, int> amounts, DateTime timestamp, int idWarehouse)
        {
            Id = id;
            Amounts = amounts;
            Timestamp = timestamp;
            IdWarehouse = idWarehouse;
        }
    }
}
