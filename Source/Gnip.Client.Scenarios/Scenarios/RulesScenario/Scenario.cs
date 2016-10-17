using System.Threading.Tasks;

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
                Logger.Info("Vor Scenario ...");

                foreach (var rule in rulesRepository.List())
                {
                    Logger.Info("Rule:{0}", rule.Value);
                }

                var rules = new[]
                {
                    new Rule {Tag = "Test", Value = "CKrowiorsch"}
                };

                Logger.Info("Adding ...");
                rulesRepository.Add(rules);


                Logger.Info("Stand (after Add):");
                foreach (var rule in rulesRepository.List())
                {
                    Logger.Info("Rule:{0}", rule.Value);
                }

                Logger.Info("Cleanup ...");
                rulesRepository.Delete(rules);


                Logger.Info("Stand (after cleanup):");
                foreach (var rule in rulesRepository.List())
                {
                    Logger.Info("Rule:{0}", rule.Value);
                }
            });
        }
    }
}