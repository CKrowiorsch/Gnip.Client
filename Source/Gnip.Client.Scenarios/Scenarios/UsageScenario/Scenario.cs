using System;
using System.Threading.Tasks;
using Krowiorsch.Gnip.Model;
using Krowiorsch.Gnip.Usage;


namespace Krowiorsch.Gnip.Scenarios.UsageScenario
{
    public class Scenario
    {
        readonly GnipAccessToken _gnipAccessToken;

        public Scenario(GnipAccessToken gnipAccessToken)
        {
            _gnipAccessToken = gnipAccessToken;
        }


        public Task Start()
        {
            IGnipUsage gnipUsage = new GnipUsage();

            Console.WriteLine("Please provide AccountName (Case-Sensitive):");
            var accountName = Console.ReadLine();

            var products = gnipUsage.MonthlyUsagePowertrack(accountName, _gnipAccessToken.Username, _gnipAccessToken.Password);

            WriteResult(products);

            return Task.FromResult(0);
        }


        void WriteResult(ProductUsage[] products)
        {
            foreach (var product in products)
            {
                Console.WriteLine("{0} Label {1} -> Projected:{2}", product.ProductName, product.Label, product.Projected.ToString("N0"));
                foreach (var timeseries in product.Usage)
                {
                    Console.WriteLine("\tMonth:{0} -> Used:{1}", timeseries.Key.ToString("yyyy MM"), timeseries.Value.ToString("N0"));
                }
            }
        }
    }
}