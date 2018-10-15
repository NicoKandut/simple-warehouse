using System;
using System.Collections.Generic;
using System.Text;

namespace WerhausCore
{
    public class Owner
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<Warehouse> Warehouses { get; set; }

        public Owner(int id, string name, string password, List<Warehouse> warehouses)
        {
            Id = id;
            Name = name;
            Password = password;
            if (warehouses == null)
                Warehouses = new List<Warehouse>();
            else
                Warehouses = warehouses;
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
