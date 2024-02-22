using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geradovana.ScrapingService.Infrastructure.Providers
{
    internal static class HtmlDocumentProvider
    {
        public async static Task<HtmlDocument> GetHtmlDocument(this HttpClient client, string url)
        {
            var html = await client.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            return htmlDoc;
        }
    }
}