﻿using System.Threading.Tasks;

using Krowiorsch.Gnip.Impl;
using Krowiorsch.Gnip.Model;

using NLog;

namespace Krowiorsch.Gnip.Scenarios.RulesScenario
{
    public class Scenario
    {
        static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        readonly GnipAccessToken _gnipAccessToken;
        readonly string _baseEndpoint;

        public Scenario(GnipAccessToken gnipAccessToken, string baseEndpoint)
        {
            _gnipAccessToken = gnipAccessToken;
            _baseEndpoint = baseEndpoint;
        }

        public Task Start()
        {
            Logger.Debug("Start Scenario");
            var rulesRepository = new HttpGnipRulesRepository(_baseEndpoint, _gnipAccessToken);

            return Task.Factory.StartNew(() =>
            {
                foreach (var rule in rulesRepository.List())
                {
                    Logger.Info(string.Format("Rule:{0}", rule.Value));
                }
            });
        }
    }
}