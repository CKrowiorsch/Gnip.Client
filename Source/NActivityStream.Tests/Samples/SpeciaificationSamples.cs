using System;
using System.IO;

namespace Krowiorsch.Samples
{
    public class SpeciaificationSamplesProvider
    {
        public string GetMinmal()
        {
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Samples", "Specifications", "Minimal.json"));
        }
    }
}