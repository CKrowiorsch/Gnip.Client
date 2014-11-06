using System.Collections.Generic;

namespace Krowiorsch.Gnip.Model.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class EntryAuthor
    {
        /// <summary> Name of the Author </summary>
        public string Name { get; set; }

        /// <summary> Link zum Author </summary>
        public string Link { get; set; }

        /// <summary> Statistics Informations </summary>
        public IDictionary<string, long>  Statistics { get; set; }

        /// <summary> bekannte Einträge für Autorhenstatistiken </summary>
        public class KnownAuthorStatistics
        {
            /// <summary> Count of Following</summary>
            public const string FollowingCount = "Following";

            /// <summary> Count of Followers </summary>
            public const string FollowedByCount = "FollowedBy";
        }
    }
}