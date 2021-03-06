﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using Krowiorsch.Gnip.Events;
using Krowiorsch.Gnip.Impl;
using Krowiorsch.Gnip.Model;
using Krowiorsch.Gnip.Model.Data;
using Krowiorsch.Gnip.Extensions;

namespace Krowiorsch.Gnip
{
    /// <summary> replay of activities </summary>
    public class ReplayActivityHttpStreaming : IHttpStreaming<Activity>, IProcessEvents
    {
        readonly CancellationTokenSource _cancellationTokenSource;

        readonly GnipAccessToken _accessToken;

        DateTime _startDate;
        DateTime _stopDate;
        readonly Subject<Activity> _internalSubject;
        readonly Subject<ProcessingEventBase> _internalProcessing;
        
        /// <summary> Stream of activities </summary>
        public IObservable<Activity> Stream { get; set; }

        public ReplayActivityHttpStreaming(string endpoint, GnipAccessToken accessToken, DateTime startDate)
        {
            _accessToken = accessToken;
            _startDate = startDate;
            _stopDate = DateTime.MaxValue;
            _cancellationTokenSource = new CancellationTokenSource();

            Endpoint = new Uri(endpoint);
            Stream = _internalSubject = new Subject<Activity>();
            Processing = _internalProcessing = new Subject<ProcessingEventBase>();
        }
        

        /// <summary> starts async reading </summary>
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

                    activities = reader.ReadLines().ToActivityOrNull().Where(a => a != null).OrderBy(t => t.Published).ToArray();

                    var newIds = new List<string>();

                    foreach (var activity in activities)
                    {
                        activity.Received = DateTime.Now;

                        _internalSubject.OnNext(activity);
                        newIds.Add(activity.Id);
                        _startDate = activity.Published;
                    }

                }
                while (activities.Count() == reader.MaxCount);
            });
        }

        /// <summary> stop reading </summary>
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