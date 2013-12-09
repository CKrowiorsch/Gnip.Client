using System;
using System.IO;

namespace Krowiorsch.Samples
{
    public class TwitterSamplesProvider
    {
        public string GetGnipTwitter()
        {
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Samples", "Gnip", "Twitter", "Sample1.json"));
        }

        public string GetGnipSpecific(string specificName)
        {
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Samples", "Gnip", "Twitter", string.Format("specific_{0}.json", specificName)));
        } 
    }
}