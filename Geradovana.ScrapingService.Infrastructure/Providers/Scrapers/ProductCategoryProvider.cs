using Geradovana.ScrapingService.Application.Common.Providers;
using Geradovana.ScrapingService.Application.Common.Utils;
using Geradovana.ScrapingService.Domain;
using Geradovana.ScrapingService.Domain.Enums;
using HtmlAgilityPack;

namespace Geradovana.ScrapingService.Infrastructure.Providers.Scrapers
{
    public class ProductCategoryProvider : IProductCategoryProvider
    {
        private static readonly string Url = "https://www.geradovana.lt/";
        private static readonly string CategoryMenuItems = "//ul[@class='wrapper mega-menu__categories bg-grey']/li";
        private static readonly string CategoryNameXpath = "a";
        private static readonly string SubcategoryNameXpath = "ul/li/a[@class='list-item']";
        private static readonly string PageListXPath = "//select[@id='PageList']";
        private static readonly string PageItemXPath = "option";
        private static readonly string ProductXPath = "//div[@class=' load-more-gifts clearfix']/div[contains(@class, 'product ')]";
        private static readonly string ProductNameXPath = "a/div/span[@class='productname']";
        private static readonly string ProductPriceXPath = "a/div/span[@class='productprice']/span";
        private static readonly string ProductTypeXPath = "a/div/div[@class='badge-container']/span/span";

        public async Task<ProductCategory[]> GetAll()
        {
            var htmlDoc = await GetHtmlDocument(Url);

            var categoryMenuItemNodes = htmlDoc.DocumentNode.SelectNodes(CategoryMenuItems);

            var result = categoryMenuItemNodes.Select(ParseProductCategory).ToArray();
            return result;
        }

        public async Task<ProductCategorySummary[]> GetSummaries(string categoryName, string? subCategoryName)
        {
            #region format_category_names
            categoryName = ConvertCategoryToUrlFormat(categoryName);
            subCategoryName = string.IsNullOrEmpty(subCategoryName) ? null : ConvertCategoryToUrlFormat(subCategoryName);
            var requestUrl = subCategoryName is null ? Path.Combine(Url, categoryName) : Path.Combine(Url, categoryName, subCategoryName);
            #endregion

            var htmlDoc = await GetHtmlDocument(requestUrl);

            var pageList = htmlDoc.DocumentNode.SelectSingleNode(PageListXPath);
            var lastPage = ParseLastPageCount(pageList);

            Product[] products = null!;

            if(lastPage > 0)
            {
                // read all pages
                products = await ReadAllPages(requestUrl, lastPage);
            }
            else
            {
                // only one
                products = ParseProducts(htmlDoc.DocumentNode);
            }

            var summaries = CreateSummaries(categoryName, subCategoryName, products);

            return summaries;
        }

        private async Task<HtmlDocument> GetHtmlDocument(string url)
        {
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            return htmlDoc;
        }

        private async Task<Product[]> ReadAllPages(string requestUrl, int pageCount)
        {
            List<Product[]> listOfProducts = new List<Product[]>();

            for (int i = 1; i <= pageCount; i++)
            {
                var pageRequestUrl = string.Format("{0}?page={1}", requestUrl, i);
                var pageDoc = await GetHtmlDocument(pageRequestUrl);

                var products = ParseProducts(pageDoc.DocumentNode);
                listOfProducts.Add(products);
            }

            return listOfProducts.SelectMany(x=>x).ToArray();
        }

        private string ConvertCategoryToUrlFormat(string category)
        {
            category = category.ToLower().Replace(" ", "-");
            category = StringUtils.RemoveEmoji(category);
            category = StringUtils.ConvertLithuanianToAscii(category);
            return category;
        }

        private ProductCategory ParseProductCategory (HtmlNode categoryMenuItemNode)
        {
            var name = categoryMenuItemNode.SelectSingleNode(CategoryNameXpath).InnerText.Trim();
            var subCategories = categoryMenuItemNode
                .SelectNodes(SubcategoryNameXpath)?
                .Select(x => x.InnerText.Trim())?
                .ToArray();

            return new ProductCategory (name, subCategories);
        }

        private int ParseLastPageCount(HtmlNode pageList)
        {
            if(pageList == null)
                return 0;

            var pages = pageList.SelectNodes(PageItemXPath);
            int lastPage = int.Parse(pages[pages.Count - 1].InnerText);
            return lastPage;
        }

        private Product[] ParseProducts(HtmlNode documentNode)
        {
            var productNodes = documentNode.SelectNodes(ProductXPath);
            var products = productNodes.Select(ParseProduct).ToArray();
            return products;
        }

        private Product ParseProduct(HtmlNode productNode)
        {
            var productName = productNode.SelectSingleNode(ProductNameXPath).InnerText.Trim();

            var productPriceText = productNode.SelectSingleNode(ProductPriceXPath).InnerText;
            productPriceText = StringUtils.ExtractDigits(productPriceText);
            var prdouctPrice = decimal.Parse(productPriceText);

            var productTypeText = productNode.SelectSingleNode(ProductTypeXPath)?.InnerText?
                .Replace(" ", string.Empty);

            var productType = productTypeText is null ? ProductType.Standard : Enum.Parse<ProductType>(productTypeText, ignoreCase:true);

            return new Product(productName, prdouctPrice, productType);
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