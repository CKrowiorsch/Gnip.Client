using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

using C5;

using Krowiorsch.Gnip.Impl;
using Krowiorsch.Gnip.Model;
using Krowiorsch.Gnip.Model.Data;
using Krowiorsch.Gnip.Extensions;

namespace Krowiorsch.Gnip
{
    public class ReplayActivityHttpStreaming : IHttpStreaming<Activity>
    {
        readonly CancellationTokenSource _cancellationTokenSource;

        readonly GnipAccessToken _accessToken;
        readonly string _endpoint;

        readonly SortedArray<string> _idsAlreadySeen = new SortedArray<string>();

        readonly DateTime _startDate;
        DateTime _stopDate;
        readonly Subject<Activity> _internalSubject;
        public IObservable<Activity> Stream { get; set; }

        public ReplayActivityHttpStreaming(string endpoint, GnipAccessToken accessToken, DateTime startDate)
        {
            _accessToken = accessToken;
            _endpoint = endpoint;
            _startDate = startDate;
            _stopDate = DateTime.MaxValue;
            _cancellationTokenSource = new CancellationTokenSource();

            Stream = _internalSubject = new Subject<Activity>();
        }
        

        public Task ReadAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                Activity[] activities;
                GnipReplayReader reader;

                do
                {
                    reader = new GnipReplayReader(_endpoint, _accessToken, _startDate, _stopDate);
                    activities = reader.ReadLines().ToActivity().ToArray();

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
    }
}