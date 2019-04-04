using Lagerverwaltung;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WerhausCore;

namespace WerhausCore
{
    public static class Calculation
    {
        static Random rnd = new Random();
        public static async Task<List<List<HistoryEntry>>> getInOutData(Warehouse warehouse)
        {
            try
            {
                List<List<HistoryEntry>> result = new List<List<HistoryEntry>>();
                List<HistoryEntry> inEntries = new List<HistoryEntry>();
                List<HistoryEntry> outEntries = new List<HistoryEntry>();
                List<Order> orders = await Database.getAllOrdersOfWarehouse(warehouse.Id);
                int capacityIn = 0;
                int capacityOut = 0;
                foreach (Order o in orders)
                {
                    if (o.getTotalAmount() < 0)
                    {
                        capacityIn += o.getTotalAmount();
                        outEntries.Add(new HistoryEntry(o.timestamp, o.getTotalAmount()));
                    }
                    else
                    {
                        capacityOut += o.getTotalAmount();
                        inEntries.Add(new HistoryEntry(o.timestamp, o.getTotalAmount()));
                    }
                }
                result.Add(inEntries);
                result.Add(outEntries);
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR", ex.Message + "\n" + ex.StackTrace, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }
        public static async Task<List<HistoryEntry>> getHistographyData(Warehouse warehouse)
        {
            try
            {
                List<HistoryEntry> entries = new List<HistoryEntry>();
                List<Order> orders = await Database.getAllOrdersOfWarehouse(warehouse.Id);
                //List<Order> orders = generateTestData();
                int capacity = 0;
                foreach (Order o in orders)
                {
                    if (o.products.Count > 0)
                    {
                        capacity = capacity + o.getTotalAmount();
                        entries.Add(new HistoryEntry(o.timestamp, capacity));
                    }
                }
                return entries;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR", ex.Message + "\n" + ex.StackTrace, MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }
    }
}
