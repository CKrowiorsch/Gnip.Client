using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

using Krowiorsch.Gnip.Impl;
using Krowiorsch.Gnip.Model;

using NLog;

namespace Krowiorsch.Gnip
{
    /// <summary>
    /// Connects to GnipStreaming Endpoint and try reconnecting
    /// </summary>
    public class ReconnectableHttpStreaming : IHttpStreaming<string>
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        readonly Subject<string> _internalSubject; 

        /// <summary>
        /// Stream of Lines from HttpStreaming
        /// </summary>
        public IObservable<string> Stream { get; set; }

        /// <summary>
        /// Builder to create a WebRequest for this specific stream
        /// </summary>
        protected Func<HttpWebRequest> _webRequestBuilder;
        
        /// <summary>
        /// Token to cancel request
        /// </summary>
        protected CancellationTokenSource _cancellationTokenSource;

        int _currentTimeout = 1000;

        /// <summary>
        /// Initializes the Streamin with endpoint and accesstoken
        /// </summary>
        /// <param name="streamingEndpoint"></param>
        /// <param name="accessToken"></param>
        public ReconnectableHttpStreaming(string streamingEndpoint, GnipAccessToken accessToken)
        {
            Endpoint = new Uri(streamingEndpoint);

            _webRequestBuilder = () => GnipWebRequest.Create(accessToken, streamingEndpoint);
            _cancellationTokenSource = new CancellationTokenSource();

            Stream = _internalSubject = new Subject<string>();
        }

        /// <summary>
        /// Start reading async
        /// </summary>
        public Task ReadAsync()
        {
            var cancellationToken = _cancellationTokenSource.Token;

            return Task.Factory.StartNew(
                () =>
                {
                    while (true)
                    {
                        var clientDisconnectReason = StartObserving(cancellationToken);

                        switch (clientDisconnectReason)
                        {
                            case ClientDisconnectReason.Exception:
                            case ClientDisconnectReason.RemoteDisconnect:
                            case ClientDisconnectReason.EndOfLine:
                                if (_currentTimeout > 60000)
                                    return;

                                Thread.Sleep(_currentTimeout);
                                _currentTimeout *= 2;
                                continue;
                            case ClientDisconnectReason.TaskCancel:
                                return;
                        }
                    }
                },
                cancellationToken);
        }

        public void StopStreaming()
        {
            _cancellationTokenSource.Cancel();
        }

        protected virtual ClientDisconnectReason StartObserving(CancellationToken cancellationToken)
        {
            using (var response = _webRequestBuilder().GetResponse())
            using (var reponseStream = response.GetResponseStream())
            using (var reader = new StreamReader(reponseStream))
            {
                try
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        _currentTimeout = 1000;

                        if (cancellationToken.IsCancellationRequested)
                        {
                            Logger.Info("Gnip Client Canceled");
                            return ClientDisconnectReason.TaskCancel;
                        }

                        if (!string.IsNullOrEmpty(line))
                            _internalSubject.OnNext(line);
                    }
                }
                catch (IOException e)
                {
                    Logger.WarnException(string.Format("Fehler while reading: {0}", e.Message), e);
                    return ClientDisconnectReason.Exception;
                }
                catch (Exception e)
                {
                    Logger.WarnException(string.Format("Fehler while reading: {0}", e.Message), e);
                    return ClientDisconnectReason.Exception;
                }

                return ClientDisconnectReason.Success;
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Endoint
        /// </summary>
        public Uri Endpoint { get; private set; }
    }
}