using System;
using System.Collections.Generic;
using System.Linq;

using Krowiorsch.Model;

namespace Krowiorsch.Converter
{
    public static class ActivityObjectTypeResolver
    {
        static readonly Dictionary<string, Type> Registry = new Dictionary<string, Type>
        {
            {"note", typeof(ActivityObjectNote)},
            {"comment", typeof(ActivityObjectComment)},
            {"activity", typeof(ActivityObjectActivity)}
        };

        public static Type GetByName(string objectType)
        {
            var cleanedObjectType = objectType;

            if (cleanedObjectType.StartsWith("http://activitystrea.ms"))
                cleanedObjectType = new Uri(cleanedObjectType).Segments.Last();

            return Registry.ContainsKey(cleanedObjectType) ? Registry[cleanedObjectType] : typeof(ActivityObject);
        }
    }
}