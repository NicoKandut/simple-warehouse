using System;
using System.Collections.Generic;
using System.Text;

namespace WerhausCore
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Warehouse> Warehouses { get; set; }

        public Owner(int id, string name)
        {
            Id = id;
            Name = name;
            Warehouses = new List<Warehouse>();
        }
        public void AddWarehouse(Warehouse warehouse)
        {
            Warehouses.Add(warehouse);
        }
        public void RemoveWarehouse(Warehouse warehouse)
        {
            Warehouses.Remove(warehouse);
        }
    }
}
