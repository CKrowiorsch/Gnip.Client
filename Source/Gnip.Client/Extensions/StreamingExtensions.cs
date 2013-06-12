using System.Reactive.Linq;
using System;

using Krowiorsch.Gnip.Converter;
using Krowiorsch.Gnip.Model.Data;

namespace Krowiorsch.Gnip.Extensions
{
    public static class StreamingExtensions
    {
        public static IObservable<Activity> ToActivity(this IObservable<string> input)
        {
            return input
                .GroupInto(s => s.StartsWith("<entry"), s => s.Equals("</entry>"))
                .Select(s => ActivityConvert.Deserialize(string.Join(Environment.NewLine, s)));
        }
    }
}