using System;
using System.IO;
using System.Net;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

using Krowiorsch.Gnip.Model;

using NLog;

namespace Krowiorsch.Gnip
{
    public class ReconnectableHttpStreaming : IHttpStreaming
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public Subject<string> Stream { get; set; }

        readonly GnipAccessToken _accessToken;
        readonly string _streamingEndpoint;

        CancellationTokenSource _cancellationTokenSource;
        int _currentTimeout = 1000;

        public ReconnectableHttpStreaming(string streamingEndpoint, GnipAccessToken accessToken)
        {
            _streamingEndpoint = streamingEndpoint;
            _accessToken = accessToken;

            Stream = new Subject<string>();
        }

        public Task ReadAsync()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

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

        private ClientDisconnectReason StartObserving(CancellationToken cancellationToken)
        {
            using (var response = (HttpWebResponse)BuildWebRequest().GetResponse())
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
                            Stream.OnNext(line);
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

        private WebRequest BuildWebRequest()
        {
            var request = (HttpWebRequest)WebRequest.Create(_streamingEndpoint);
            request.Method = "GET";

            string username = _accessToken.Username;
            string password = _accessToken.Password;

            var nc = new NetworkCredential(username, password);
            request.Credentials = nc;

            request.PreAuthenticate = true;
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Headers.Add("Accept-Encoding", "gzip");

            return request;
        }
    }
}