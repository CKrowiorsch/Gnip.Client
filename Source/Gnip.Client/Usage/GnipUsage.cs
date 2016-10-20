using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Krowiorsch.Gnip.Extensions;
using Newtonsoft.Json;

namespace Krowiorsch.Gnip.Usage
{
    public class GnipUsage : IGnipUsage
    {
        const string Endpoint = "https://gnip-api.twitter.com/metrics/usage/accounts/{ACCOUNT_NAME}.json";

        public ProductUsage[] MonthlyUsagePowertrack(string accountName, string userName, string password)
        {
            var urlBase = Endpoint.Replace("{ACCOUNT_NAME}", accountName);

            var fromDate = DateTime.Today.AddMonths(-6);
            var toDate = DateTime.Today;

            // buildUr
            var url = string.Format("{0}?bucket=month&fromdate={1}&todate={2}", urlBase,
                fromDate.ToString("yyyyMMddHHmm"), toDate.ToString("yyyyMMddHHmm"));

            string result;

            var statuscode = Rest.GetRestResponse("Get", url, userName, password, out result);

            if (statuscode != HttpStatusCode.OK)
                throw new InvalidOperationException("Usage cannot be retrieved");

            var resultDocument = JsonConvert.DeserializeObject<dynamic>(result);

            var twitter = resultDocument.publishers[0];
            var endpoints = twitter.products[0].endpoints;

            var resultList = new List<ProductUsage>();

            foreach (var e in endpoints)
            {
                var p = new ProductUsage
                {
                    Label = e.type + "-" + e.label,
                    Projected = e.projected.activities,
                };

                // read series
                IDictionary<DateTime, int> values = new Dictionary<DateTime, int>();
                foreach (var period in e.used)
                {
                    string time = period.timePeriod;
                    int used = period.activities;

                    values.Add(DateTime.ParseExact(time, "yyyyMMddHHmm", CultureInfo.InvariantCulture), used);
                }

                p.Usage = values;
                resultList.Add(p);
            }

            return resultList.ToArray();
        }
    }
}