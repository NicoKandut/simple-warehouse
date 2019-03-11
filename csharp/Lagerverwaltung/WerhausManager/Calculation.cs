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
        public static async void getHistographyData(int warehouseId)
        {
            List<HistoryEntry> entries = new List<HistoryEntry>();
            List<Order> orders = await Database.getAllOrdersOfWarehouse(warehouseId);
            Warehouse warehouse = await Database.getWarehouseAsync(warehouseId);
            int capacity = warehouse.CurrentCapacity;
            entries.Add(new HistoryEntry(DateTime.Now, capacity));
            foreach (Order o in orders)
            {
                capacity = capacity - o.getTotalAmount();
                entries.Add(new HistoryEntry(o.Timestamp, capacity));
            }
            entries.Reverse();
            MessageBox.Show(entries[0].ToString() + "\n" + entries[1].ToString() + "\n" + entries[2].ToString() + "\n" + entries[3].ToString() + "\n");
        }
    }
}
