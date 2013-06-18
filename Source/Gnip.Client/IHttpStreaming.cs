using System;
using System.Reactive.Subjects;
using System.Threading.Tasks;

namespace Krowiorsch.Gnip
{
    public interface IHttpStreaming : IDisposable
    {
        Subject<string> Stream { get; set; }

        Task ReadAsync();
    }
}