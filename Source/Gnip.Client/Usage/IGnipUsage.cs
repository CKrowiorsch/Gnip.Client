using System;
using System.Collections.Generic;

namespace Krowiorsch.Gnip.Usage
{
    public interface IGnipUsage
    {
        ProductUsage[] MonthlyUsagePowertrack(string accountName, string userName, string password);
    }


    public class ProductUsage
    {
        public string Label { get; set; }

        public IDictionary<DateTime, int> Usage { get; set; } 

        public int Projected { get; set; }
    }
}