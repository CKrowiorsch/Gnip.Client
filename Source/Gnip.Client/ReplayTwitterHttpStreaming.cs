using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

using C5;

using Krowiorsch.Converter;
using Krowiorsch.Gnip.Events;
using Krowiorsch.Gnip.Impl;
using Krowiorsch.Gnip.Model;
using Krowiorsch.Model;

namespace Krowiorsch.Gnip
{
    public class ReplayTwitterHttpStreaming : IHttpStreaming<Activity>
    {
readonly CancellationTokenSource _cancellationTokenSource;

        readonly GnipAccessToken _accessToken;

        readonly SortedArray<string> _idsAlreadySeen = new SortedArray<string>();

        readonly DateTime _startDate;
        DateTime _stopDate;
        readonly Subject<Activity> _internalSubject;
        readonly Subject<ProcessingEventBase> _internalProcessing;
        
        /// <summary>
        /// Stream of activities
        /// </summary>
        public IObservable<Activity> Stream { get; set; }

        public ReplayTwitterHttpStreaming(string endpoint, GnipAccessToken accessToken, DateTime startDate)
        {
            _accessToken = accessToken;
            _startDate = startDate;
            _stopDate = DateTime.MaxValue;
            _cancellationTokenSource = new CancellationTokenSource();

            Endpoint = new Uri(endpoint);
            Stream = _internalSubject = new Subject<Activity>();
            Processing = _internalProcessing = new Subject<ProcessingEventBase>();
        }
        

        public Task ReadAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                Activity[] activities;
                GnipReplayReader reader;

                do
                {
                    reader = new GnipReplayReader(Endpoint.ToString(), _accessToken, _startDate, _stopDate)
                    {
                        OnExecuteRequest = request => _internalProcessing.OnNext(new ExecuteWebRequest { Url = request.RequestUri })
                    };

                    activities = reader.ReadLines().Select(GnipActivityConvert.FromJson).ToArray();

                    var newIds = new List<string>();

                    foreach (var activity in activities.Where(activity => !_idsAlreadySeen.Contains(activity.Id)))
                    {
                        _internalSubject.OnNext(activity);
                        newIds.Add(activity.Id);
                        _stopDate = activity.Published;
                    }

                    _idsAlreadySeen.AddAll(newIds);
                }
                while (activities.Count() == reader.MaxCount);
            });
        }

        public void StopStreaming()
        {
            _cancellationTokenSource.Cancel();
        }

        public void Dispose()
        {

        }

        /// <summary>
        /// Endoint
        /// </summary>
        public Uri Endpoint { get; private set; }

        public IObservable<ProcessingEventBase> Processing { get; private set; }
    }
}