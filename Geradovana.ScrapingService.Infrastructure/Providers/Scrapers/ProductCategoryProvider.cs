using Geradovana.ScrapingService.Application.Common.Providers;
using Geradovana.ScrapingService.Domain;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geradovana.ScrapingService.Infrastructure.Providers.Scrapers
{
    public class ProductCategoryProvider : IProductCategoryProvider
    {
        private static readonly string _url = "https://www.geradovana.lt/";
        private static readonly string _categoryMenuItems = "//ul[@class='wrapper mega-menu__categories bg-grey']/li";
        private static readonly string _categoryNameXpath = "a";
        private static readonly string _subcategoryNameXpath = "ul/li/a[@class='list-item']";

        public async Task<ProductCategory[]> GetAll()
        {
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(_url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var categoryMenuItemNodes = htmlDoc.DocumentNode.SelectNodes(_categoryMenuItems);

            var result = categoryMenuItemNodes.Select(ConvertToProductCategory).ToArray();
            return result;
        }

        private static ProductCategory ConvertToProductCategory (HtmlNode categoryMenuItemNode)
        {
            var name = categoryMenuItemNode.SelectSingleNode(_categoryNameXpath).InnerText.Trim();
            var subCategories = categoryMenuItemNode
                .SelectNodes(_subcategoryNameXpath)?
                .Select(x => x.InnerText.Trim())?
                .ToArray();

            return new ProductCategory (name, subCategories);
        }
    }
}