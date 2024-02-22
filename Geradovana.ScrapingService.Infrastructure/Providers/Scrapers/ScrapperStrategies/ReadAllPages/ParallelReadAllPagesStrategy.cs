using Geradovana.ScrapingService.Infrastructure.Extensions;
using Geradovana.ScrapingService.Infrastructure.Interfaces;
using HtmlAgilityPack;
using System.Net.Http;

namespace Geradovana.ScrapingService.Infrastructure.Providers.Scrapers.ScrapperStrategies.ReadAllPages
{
    public class ParallelReadAllPagesStrategy : IReadAllPagesStrategy
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ParallelReadAllPagesStrategy(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Product[]> ReadAllPages(string requestUrl, int pageCount)
        {
            var listOfGetProductsTasks = new List<Task<HtmlDocument>>();

            using var httpClient = _httpClientFactory.CreateClient();

            for (int i = 1; i <= pageCount; i++)
            {
                var pageRequestUrl = $"{requestUrl}?page={i}";
                var getProductTask = httpClient.GetHtmlDocument(pageRequestUrl);
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