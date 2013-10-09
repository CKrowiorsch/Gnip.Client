using System;
using System.IO;

namespace Krowiorsch.Samples
{
    public class FacebookSamplesProvider
    {
        public string GetComment()
        {
            return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Samples", "Gnip", "Facebook", "Comment1.xml"));
        }
    }
}