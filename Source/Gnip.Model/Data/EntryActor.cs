using System.Collections.Generic;

namespace Krowiorsch.Gnip.Model.Data
{
    public class EntryActor
    {
        public EntryActor()
        {
            Statistics = new Dictionary<string, long>();
        }

        /// <summary></summary>
        public string Id { get; set; }

        /// <summary> Statistics Informations </summary>
        public IDictionary<string, long> Statistics { get; set; }

        /// <summary>
        /// Die Herkunft des Autoren
        /// </summary>
        public string Herkunft { get; set; }

        /// <summary>
        /// Die Sprache des Autoren
        /// </summary>
        public string Sprache { get; set; }

        /// <summary> bekannte Einträge für Autorhenstatistiken </summary>
        public class KnownActorStatistics
        {
            /// <summary> Count of Following</summary>
            public const string FollowingCount = "Following";

            /// <summary> Count of Followers </summary>
            public const string FollowedByCount = "FollowedBy";
        }
    }
}