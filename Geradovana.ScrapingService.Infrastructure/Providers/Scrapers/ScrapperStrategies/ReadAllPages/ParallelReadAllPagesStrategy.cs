using Geradovana.ScrapingService.Infrastructure.Extensions;
using Geradovana.ScrapingService.Infrastructure.Interfaces;
using HtmlAgilityPack;

namespace Geradovana.ScrapingService.Infrastructure.Providers.Scrapers.ScrapperStrategies.ReadAllPages
{
    internal class ParallelReadAllPagesStrategy : IReadAllPagesStrategy
    {
        public async Task<Product[]> ReadAllPages(string requestUrl, int pageCount)
        {
            var listOfGetProductsTasks = new List<Task<HtmlDocument>>();

            for (int i = 1; i <= pageCount; i++)
            {
                var pageRequestUrl = $"{requestUrl}?page={i}";
                var getProductTask = HtmlDocumentProvider.GetHtmlDocument(pageRequestUrl);
                listOfGetProductsTasks.Add(getProductTask);
            }

            await Task.WhenAll(listOfGetProductsTasks);

            var products = new List<Product>();

            Parallel.ForEach(listOfGetProductsTasks, pageTask =>
            {
                var page = pageTask.Result;
                var pageProducts = page.DocumentNode.ParseProducts();
                lock (products)
                {
                    products.AddRange(pageProducts);
                }
            });

            return products.ToArray();
        }
    }
}