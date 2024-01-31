using Geradovana.ScrapingService.Application.Common.Providers;
using Geradovana.ScrapingService.Application.Common.Utils;
using Geradovana.ScrapingService.Domain;
using Geradovana.ScrapingService.Domain.Enums;
using Geradovana.ScrapingService.Infrastructure.Constants;
using Geradovana.ScrapingService.Infrastructure.Extensions;
using Geradovana.ScrapingService.Infrastructure.Interfaces;
using HtmlAgilityPack;

namespace Geradovana.ScrapingService.Infrastructure.Providers.Scrapers
{
    public class ProductCategoryProvider : IProductCategoryProvider
    {
        private static readonly string Url = "https://www.geradovana.lt/";
        
        private readonly IReadAllPagesStrategy _readAllPagesStrategy;

        internal ProductCategoryProvider(IReadAllPagesStrategy readAllPagesStrategy)
        {
            _readAllPagesStrategy = readAllPagesStrategy;
        }

        public async Task<ProductCategory[]> GetAll()
        {
            var htmlDoc = await HtmlDocumentProvider.GetHtmlDocument(Url);

            var categoryMenuItemNodes = htmlDoc.DocumentNode.SelectNodes(XPathes.CategoryMenuItems);

            var result = categoryMenuItemNodes.Select(x => x.ParseProductCategory()).ToArray();
            return result;
        }

        public async Task<ProductCategorySummary[]> GetSummaries(string categoryName, string? subCategoryName)
        {
            #region format_category_names
            categoryName = ConvertCategoryToUrlFormat(categoryName);
            subCategoryName = string.IsNullOrEmpty(subCategoryName) ? null : ConvertCategoryToUrlFormat(subCategoryName);
            var requestUrl = subCategoryName is null ? Path.Combine(Url, categoryName) : Path.Combine(Url, categoryName, subCategoryName);
            #endregion

            var htmlDoc = await HtmlDocumentProvider.GetHtmlDocument(requestUrl);

            var pageList = htmlDoc.DocumentNode.SelectSingleNode(XPathes.PageListXPath);
            var lastPage = pageList.ParseLastPageCount();

            Product[] products = null!;

            if(lastPage > 0)
            {
                // read all pages
                products = await _readAllPagesStrategy.ReadAllPages(requestUrl, lastPage);
            }
            else
            {
                // only one
                products = htmlDoc.DocumentNode.ParseProducts();
            }

            var summaries = CreateSummaries(categoryName, subCategoryName, products);

            return summaries;
        }

        private string ConvertCategoryToUrlFormat(string category)
        {
            category = category.ToLower().Replace(" ", "-");
            category = StringUtils.RemoveEmoji(category);
            category = StringUtils.ConvertLithuanianToAscii(category);
            return category;
        }     

        private ProductCategorySummary[] CreateSummaries(string categoryName, string? subCategoryName, Product[] products)
        {
            var dictionaryOfProducts = products
                .GroupBy(x=>x.ProductType)
                .ToDictionary(x=>x.Key, x=>x.ToArray());

            var summaries = dictionaryOfProducts.Select(x =>
            {
                var amount = x.Value.Length;
                var averagePrice = x.Value.Sum(y => y.Price) / amount;
                var type = x.Key;

                return new ProductCategorySummary(categoryName, subCategoryName, type, amount, averagePrice);
            }).ToArray();

            return summaries;
        }
    }
}