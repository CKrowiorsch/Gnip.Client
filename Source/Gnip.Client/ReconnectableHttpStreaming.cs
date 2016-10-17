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

        /// <summary> Stream of Lines from HttpStreaming </summary>
        public IObservable<string> Stream { get; set; }

        /// <summary> Builder to create a WebRequest for this specific stream </summary>
        protected Func<HttpWebRequest> _webRequestBuilder;
        
        /// <summary> Token to cancel request </summary>
        protected CancellationTokenSource _cancellationTokenSource;

        /// <summary> Count of reconnect </summary>
        protected int ReconnectCount { get; set; }
        
        /// <summary> der Initale Timeout </summary>
        protected TimeSpan InitalTimeout { get; set; }

        protected int _currentReconnectCount;

        TimeSpan _currentTimeout;

        /// <summary> Initializes the Streamin with endpoint and accesstoken </summary>
        public ReconnectableHttpStreaming(string streamingEndpoint, GnipAccessToken accessToken, int reconnectCount = 8)
        {
            Endpoint = new Uri(streamingEndpoint);

            _webRequestBuilder = () => GnipWebRequest.Create(accessToken, streamingEndpoint);
            _cancellationTokenSource = new CancellationTokenSource();

            Stream = _internalSubject = new Subject<string>();

            ReconnectCount = _currentReconnectCount = reconnectCount;
            InitalTimeout = _currentTimeout = TimeSpan.FromSeconds(1);
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
                        var clientDisconnectReason = StartObserving(cancellationToken, () =>
                        {
                            _currentReconnectCount = ReconnectCount;
                            _currentTimeout = InitalTimeout;
                        });

                        switch (clientDisconnectReason)
                        {
                            case ClientDisconnectReason.Exception:
                            case ClientDisconnectReason.RemoteDisconnect:
                            case ClientDisconnectReason.EndOfLine:

                                _currentReconnectCount--;

                                if(_currentReconnectCount <= 0)
                                {
                                    Logger.Warn("Stream reconnection failed ... abort");
                                    return;
                                }

                                Logger.Info("Stream interrupted ... trying reconnection in {0} sec", _currentTimeout.TotalSeconds);
                                Thread.Sleep(_currentTimeout);
                                _currentTimeout = _currentTimeout.Add(_currentTimeout);         // increase timeout 
                                continue;

                            case ClientDisconnectReason.TaskCancel:
                                return;
                            case ClientDisconnectReason.Success:
                                return;
                        }
                    }
                },
                cancellationToken);
        }

        /// <summary> cancels streaming </summary>
        public void StopStreaming()
        {
            _cancellationTokenSource.Cancel();
        }

        /// <summary> Observe the Stream </summary>
        protected virtual ClientDisconnectReason StartObserving(CancellationToken cancellationToken, Action onRead)
        {
            using (var response = _webRequestBuilder().GetResponse())
            using (var reponseStream = response.GetResponseStream())
            using (var reader = new StreamReader(reponseStream))
            {
                try
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLineAsync().Result;

                        if (cancellationToken.IsCancellationRequested)
                        {
                            Logger.Info("Gnip Client Canceled");
                            return ClientDisconnectReason.TaskCancel;
                        }

                        if(!string.IsNullOrEmpty(line))
                        {
                            _currentReconnectCount = ReconnectCount;

                            if(onRead != null)
                                onRead();

                            _internalSubject.OnNext(line);
                        }
                            
                    }
                }
                catch (IOException e)
                {
                    Logger.Warn(e, string.Format("Fehler while reading: {0}", e.Message));
                    return ClientDisconnectReason.Exception;
                }
                catch (Exception e)
                {
                    Logger.Warn(e, string.Format("Fehler while reading: {0}", e.Message));
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