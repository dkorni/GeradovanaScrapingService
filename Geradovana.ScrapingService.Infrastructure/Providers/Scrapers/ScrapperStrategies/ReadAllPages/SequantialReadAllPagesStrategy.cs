using Geradovana.ScrapingService.Infrastructure.Extensions;
using Geradovana.ScrapingService.Infrastructure.Interfaces;

namespace Geradovana.ScrapingService.Infrastructure.Providers.Scrapers.ScrapperStrategies.ReadAllPages
{
    internal class SequantialReadAllPagesStrategy : IReadAllPagesStrategy
    {
        public async Task<Product[]> ReadAllPages(string requestUrl, int pageCount)
        {
            List<Product[]> listOfProducts = new List<Product[]>();

            for (int i = 1; i <= pageCount; i++)
            {
                var pageRequestUrl = string.Format("{0}?page={1}", requestUrl, i);
                var pageDoc = await HtmlDocumentProvider.GetHtmlDocument(pageRequestUrl);

                var products = pageDoc.DocumentNode.ParseProducts();
                listOfProducts.Add(products);
            }

            return listOfProducts.SelectMany(x => x).ToArray();
        }
    }
}
