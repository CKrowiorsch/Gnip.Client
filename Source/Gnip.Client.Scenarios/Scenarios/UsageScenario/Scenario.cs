using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Krowiorsch.Gnip.Model;
using Krowiorsch.Gnip.Usage;
using NLog.Internal;

namespace Krowiorsch.Gnip.Scenarios.UsageScenario
{
    public class Scenario
    {
        readonly GnipAccessToken _gnipAccessToken;
        private readonly string _accountName;

        public Scenario(GnipAccessToken gnipAccessToken, string accountName)
        {
            _gnipAccessToken = gnipAccessToken;
            _accountName = accountName;
        }


        public Task Start()
        {
            IGnipUsage gnipUsage = new GnipUsage();

            var products = gnipUsage.MonthlyUsagePowertrack(_accountName, _gnipAccessToken.Username, _gnipAccessToken.Password);

            WriteResult(products);

            return Task.FromResult(0);
        }


        void WriteResult(ProductUsage[] products)
        {
            foreach (var product in products)
            {
                Console.WriteLine("{0} -> Projected:{1}", product.Label, product.Projected.ToString("N0"));
                foreach (var timeseries in product.Usage)
                {
                    Console.WriteLine("\tMonth:{0} -> Used:{1}", timeseries.Key.ToString("yyyy MM"), timeseries.Value.ToString("N0"));
                }
            }
        }
    }
}