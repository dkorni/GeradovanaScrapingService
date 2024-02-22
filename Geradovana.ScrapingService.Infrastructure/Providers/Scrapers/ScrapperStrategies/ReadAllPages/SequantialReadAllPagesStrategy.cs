using Geradovana.ScrapingService.Infrastructure.Extensions;
using Geradovana.ScrapingService.Infrastructure.Interfaces;

namespace Geradovana.ScrapingService.Infrastructure.Providers.Scrapers.ScrapperStrategies.ReadAllPages
{
    public class SequantialReadAllPagesStrategy : IReadAllPagesStrategy
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SequantialReadAllPagesStrategy(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Product[]> ReadAllPages(string requestUrl, int pageCount)
        {
            List<Product[]> listOfProducts = new List<Product[]>();

            using var httpClient = _httpClientFactory.CreateClient();

            for (int i = 1; i <= pageCount; i++)
            {
                var pageRequestUrl = string.Format("{0}?page={1}", requestUrl, i);
                var pageDoc = await httpClient.GetHtmlDocument(pageRequestUrl);

                var products = pageDoc.DocumentNode.ParseProducts();
                listOfProducts.Add(products);
            }

            return listOfProducts.SelectMany(x => x).ToArray();
        }
    }
}
