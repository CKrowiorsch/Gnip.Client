using System;
using System.Threading.Tasks;

namespace Krowiorsch.Gnip
{
    /// <summary> Interface for HttpStreaming  </summary>
    public interface IHttpStreaming<T> : IDisposable
    {
        /// <summary> Stream of Lines from HttpStreaming </summary>
        IObservable<T> Stream { get; set; }

        /// <summary> Start reading async </summary>
        Task ReadAsync();

        /// <summary> Stops reading data </summary>
        void StopStreaming();

        /// <summary> the connected endpoint </summary>
        Uri Endpoint { get; }
    }
}