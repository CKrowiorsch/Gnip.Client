using System;
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

        readonly GnipAccessToken _accessToken;
        readonly string _streamingEndpoint;

        protected Func<HttpWebRequest> WebRequestBuilder;
        protected CancellationTokenSource CancellationTokenSource;

        int _currentTimeout = 1000;

        /// <summary>
        /// Initializes the Streamin with endpoint and accesstoken
        /// </summary>
        /// <param name="streamingEndpoint"></param>
        /// <param name="accessToken"></param>
        public ReconnectableHttpStreaming(string streamingEndpoint, GnipAccessToken accessToken)
        {
            _streamingEndpoint = streamingEndpoint;
            _accessToken = accessToken;

            WebRequestBuilder = () => GnipWebRequest.Create(accessToken, streamingEndpoint);
            CancellationTokenSource = new CancellationTokenSource();

            Stream = _internalSubject = new Subject<string>();
        }

        /// <summary>
        /// Start reading async
        /// </summary>
        public Task ReadAsync()
        {
            var cancellationToken = CancellationTokenSource.Token;

            return Task.Factory.StartNew(
                () =>
                {
                    while (true)
                    {
                        Logger.Info(string.Format("Connect to {0}", _streamingEndpoint));
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
            CancellationTokenSource.Cancel();
        }

        protected virtual ClientDisconnectReason StartObserving(CancellationToken cancellationToken)
        {
            using (var response = WebRequestBuilder().GetResponse())
            using (var reponseStream = response.GetResponseStream())
            using (var reader = new StreamReader(reponseStream))
            {
                try
                {
                    Logger.Debug(string.Format("ResponseStream Opened:{0}", _streamingEndpoint));

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
            CancellationTokenSource.Cancel();
        }
    }
}