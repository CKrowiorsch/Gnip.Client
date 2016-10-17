
using Krowiorsch.Gnip.Model;

namespace Krowiorsch.Gnip
{
    /// <summary>
    /// Interface for managing GniopRules
    /// </summary>
    public interface IGnipRulesRepository
    {
        void Clear();

        void Add(Rule[] rules);

        Rule[] List();

        string Endpoint { get; }
    }
}