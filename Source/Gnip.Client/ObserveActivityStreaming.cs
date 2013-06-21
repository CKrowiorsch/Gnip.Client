﻿using System;

using Krowiorsch.Gnip.Model;
using Krowiorsch.Gnip.Model.Data;

using Krowiorsch.Gnip.Extensions;

namespace Krowiorsch.Gnip
{
    /// <summary>
    /// Stream observes all new Activities on endpoint
    /// </summary>
    public class ObserveActivityStreaming : ReconnectableHttpStreaming, IHttpStreaming<Activity>
    {
        /// <summary>
        /// created a stream on specific endpoint
        /// </summary>
        /// <param name="streamingEndpoint"></param>
        /// <param name="accessToken"></param>
        public ObserveActivityStreaming(string streamingEndpoint, GnipAccessToken accessToken)
            : base(streamingEndpoint, accessToken)
        {
            Endpoint = new Uri(streamingEndpoint);
            Stream = base.Stream.ToActivity();
        }

        /// <summary>
        /// stream of activities
        /// </summary>
        public IObservable<Activity> Stream { get; set; }

        /// <summary>
        /// Endoint
        /// </summary>
        public Uri Endpoint { get; private set; }
    }
}