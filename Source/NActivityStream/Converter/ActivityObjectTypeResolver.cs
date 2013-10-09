using System;
using System.Collections.Generic;

using Krowiorsch.Model;

namespace Krowiorsch.Converter
{
    public static class ActivityObjectTypeResolver
    {
        static readonly Dictionary<string, Type> Registry = new Dictionary<string, Type>
        {
            {"note", typeof(ActivityObjectNote)},
        };

        public static Type GetByName(string objectType)
        {
            return Registry[objectType];
        }
    }
}