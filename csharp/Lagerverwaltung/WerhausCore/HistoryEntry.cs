using System;
using System.Collections.Generic;
using System.Text;

namespace WerhausCore
{
    public class HistoryEntry
    {
        public int stock { get; set; }
        public DateTime Timestamp { get; set; }

        public HistoryEntry(DateTime _date, int _stock)
        {
            stock = _stock;
            Timestamp = _date;
        }
        public override string ToString()
        {
            return Timestamp.ToString() + " | " + stock.ToString();
        }
    }
}
