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

        public void Start()
        {
            Logger.Debug("Start Scenario");
            var rulesRepository = new HttpGnipRulesRepository(_baseEndpoint, _gnipAccessToken);

            Logger.Info("Vor Scenario ...");

            var list = rulesRepository.List();

            //foreach (var rule in rulesRepository.List())
            //{
            //    Logger.Info("Rule:{0}", rule.Value);
            //}

            var rules = new[]
                {
                    new Rule {Value = "007seven"}
                };

            Logger.Info("Adding ...");
            rulesRepository.Add(rules);


            //Logger.Info("Stand (after Add):");
            //foreach (var rule in rulesRepository.List())
            //{
            //    Logger.Info("Rule:{0}", rule.Value);
            //}

            Logger.Info("Cleanup ...");
            rulesRepository.Delete(rules);


            Logger.Info("Stand (after cleanup):");
            foreach (var rule in rulesRepository.List())
            {
                Logger.Info("Rule:{0}", rule.Value);
            }

            
        }
    }
}