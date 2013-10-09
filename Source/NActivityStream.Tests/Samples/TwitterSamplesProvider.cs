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
    }
}