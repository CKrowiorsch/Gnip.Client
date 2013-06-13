using System;

namespace Krowiorsch.Gnip.Extensions
{
    public static class StringExtensions
    {
         public static string ToSingleLine(this string input)
         {
             return input.Replace(Environment.NewLine, string.Empty);
         }
    }
}