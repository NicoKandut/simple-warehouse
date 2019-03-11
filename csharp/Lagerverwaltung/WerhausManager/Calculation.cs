using Lagerverwaltung;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using WerhausCore;

namespace WerhausCore
{
    public static class Calculation
    {
        static Random rnd = new Random();
        public static async void getHistographyData(Warehouse warehouse)
        {
            List<HistoryEntry> entries = new List<HistoryEntry>();
            List<Order> orders = await Database.getAllOrdersOfWarehouse(warehouse.Id);
            //List<Order> orders = generateTestData();
            
            int capacity = warehouse.CurrentCapacity;
            entries.Add(new HistoryEntry(DateTime.Now, capacity));         
            foreach (Order o in orders)
            {
                if (o.products.Count > 0)
                {
                    capacity = capacity - o.getTotalAmount();
                    entries.Add(new HistoryEntry(o.timestamp, capacity));
                }
            }
            entries.Reverse();
            outputResult(entries);
        }

        private static void outputResult(List<HistoryEntry> entries)
        {
            string result = "";
            foreach(HistoryEntry h in entries)
            {
                result += h.ToString() + "\n";
            }
            MessageBox.Show(result);
        }
        private static List<Order> generateTestData()
        {
            List<Order> orders = new List<Order>();
            Dictionary<ProductBase, int> amounts = new Dictionary<ProductBase, int>();
            amounts.Add(new ProductBase(1, "name", "desc", 2, 10), 5);
            amounts.Add(new ProductBase(2, "name", "desc", 2, 10), 10);
            amounts.Add(new ProductBase(3, "name", "desc", 2, 10), 6);
            amounts.Add(new ProductBase(4, "name", "desc", 2, 10), 8);
            orders.Add(new Order(1, amounts, DateTime.Now, 1));
            amounts = new Dictionary<ProductBase, int>();
            amounts.Add(new ProductBase(1, "name", "desc", 2, 10), 3);
            orders.Add(new Order(2, amounts, DateTime.Now - new TimeSpan(25, 0, 0), 1));
            amounts = new Dictionary<ProductBase, int>();
            amounts.Add(new ProductBase(1, "name", "desc", 2, 10), -1);
            amounts.Add(new ProductBase(2, "name", "desc", 2, 10), -5);
            orders.Add(new Order(3, amounts, DateTime.Now - new TimeSpan(50, 0, 0), 1));
            amounts = new Dictionary<ProductBase, int>();
            amounts.Add(new ProductBase(3, "name", "desc", 2, 10), 5);
            amounts.Add(new ProductBase(4, "name", "desc", 2, 10), -6);
            orders.Add(new Order(3, amounts, DateTime.Now - new TimeSpan(75, 0, 0), 1));
            amounts = new Dictionary<ProductBase, int>();
            return orders;
        }
    }
}
