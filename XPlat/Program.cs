using System;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace XPlat
{
    class Program
    {
        static void Main(string[] args)
        {
            var appSettings = new AppSettings(args);
            var outputSettings = appSettings.OutputSettings;

            Console.WriteLine($"Saving report to {outputSettings.ReportFilePath}.");
            Directory.CreateDirectory(outputSettings.ReportDirectory);
            var client = new HttpClient();
            var body = client.GetStringAsync(appSettings.Site);
            Console.WriteLine(body.Result);

			Console.WriteLine();
			Console.WriteLine("Links:");
            var links = LinkChecker.GetLinks(body.Result);
            links.ToList().ForEach(Console.WriteLine);
            // write out links
            //File.WriteAllLines(outputPath, links);
            var checkedLinks = LinkChecker.CheckLinks(links);

            using (var file = File.CreateText(outputSettings.ReportFilePath))
            {
                foreach (var link in checkedLinks.OrderBy(l => l.Exists))
                {
                    var status = link.IsMissing ? "missing" : "OK";
                    file.WriteLine($"{status} - {link.Link}");
                }
            }
        }
    }
}
