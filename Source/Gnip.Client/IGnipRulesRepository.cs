
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

        void DeleteByTag(string tag);

        Rule[] List();

        string Endpoint { get; }
    }
}