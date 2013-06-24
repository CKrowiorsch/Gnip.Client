using System;

using Krowiorsch.Gnip.Events;

namespace Krowiorsch.Gnip
{
    public interface IProcessEvents
    {
        IObservable<ProcessingEventBase> Processing { get; }
    }
}