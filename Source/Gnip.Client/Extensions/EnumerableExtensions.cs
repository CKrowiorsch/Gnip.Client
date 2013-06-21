using System;
using System.Collections.Generic;

namespace Krowiorsch.Gnip.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<string[]> GroupInto(this IEnumerable<string> input, Func<string, bool> openingSelector, Func<string, bool> closingSelector)
        {
            var buffer = new List<string>();
            bool isInBuffer = false;

            foreach (var s in input)
            {
                if (openingSelector(s))
                {
                    buffer = new List<string> { s };
                    isInBuffer = true;
                    continue;
                }

                if (closingSelector(s) && isInBuffer)
                {
                    buffer.Add(s);
                    yield return buffer.ToArray();
                    buffer = new List<string>();
                    isInBuffer = false;
                    continue;
                }

                if (isInBuffer)
                {
                    buffer.Add(s);
                }
            }
        }
    }
}