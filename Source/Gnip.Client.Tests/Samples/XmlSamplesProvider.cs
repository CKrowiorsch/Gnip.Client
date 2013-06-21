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
    }
}