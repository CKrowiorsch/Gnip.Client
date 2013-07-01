using System;
using System.IO;

namespace Krowiorsch.Gnip.Samples
{
    public static class XmlSamplesProvider
    {
         public static string GetCommentWithUniCode()
         {
             return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Samples", "Note", "Note2_withUnicode.xml"));
         }

         public static string GetCommentWithMultipleRules()
         {
             return File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Samples", "Comment", "Comment_with_multipleRules.xml"));
         }
    }
}