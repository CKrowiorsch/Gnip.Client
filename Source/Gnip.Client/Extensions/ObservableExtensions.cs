using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace Krowiorsch.Gnip.Extensions
{
    public static class ObservableExtensions
    {
        public static IObservable<string[]> GroupInto(this IObservable<string> input, Func<string, bool> openingSelector, Func<string, bool> closingSelector)
        {
            var buffer = new List<string>();
            bool isInBuffer = false;

            return Observable.Create<string[]>(observer =>
            {
                return input.Subscribe(s =>
                {
                    if (openingSelector(s))
                    {
                        buffer = new List<string> { s };
                        isInBuffer = true;
                        return;
                    }

                    if (closingSelector(s) && isInBuffer)
                    {
                        buffer.Add(s);
                        observer.OnNext(buffer.ToArray());
                        buffer = new List<string>();
                        isInBuffer = false;
                        return;
                    }

                    if (isInBuffer)
                    {
                        buffer.Add(s);
                    }
                });
            });
        }
    }
}