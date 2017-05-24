using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;

namespace XPlat
{
    public class LinkChecker
    {
        public static IEnumerable<string> GetLinks(string page)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(page);
            var links = htmlDocument.DocumentNode.SelectNodes("//a[@href]")
                                .Select(n => n.GetAttributeValue("href", string.Empty))
                                .Where(l => !String.IsNullOrEmpty(l))
                                .Where(l => l.StartsWith("http", StringComparison.CurrentCultureIgnoreCase));

            return links;
        }

        public static IEnumerable<LinkCheckResult> CheckLinks(IEnumerable<string> links)
        {
            var all = Task.WhenAll(links.Select(CheckLinkAsync));
            return all.Result;
        }

        private static async Task<LinkCheckResult> CheckLinkAsync(string link)
        {
            var result = new LinkCheckResult();

            result.Link = link;
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Head, link);
                try
                {
                    var response = await client.SendAsync(request);
                    result.Problem = response.IsSuccessStatusCode
                        ? null
                        : response.StatusCode.ToString();
                    return result;
                }
                catch (HttpRequestException e)
                {
                    result.Problem = e.Message;
                    return result;
                }
            }
        }
    }

    public class LinkCheckResult
    {
        public bool Exists => String.IsNullOrWhiteSpace(Problem);
        public bool IsMissing => !Exists;
        public string Problem { get; set; }
        public string Link { get; set; }
    }
}