using Geradovana.ScrapingService.Application.Common.Utils;
using Geradovana.ScrapingService.Domain.Enums;
using Geradovana.ScrapingService.Domain;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geradovana.ScrapingService.Infrastructure.Constants;

namespace Geradovana.ScrapingService.Infrastructure.Extensions
{
    public static class HtmlNodeExtensions
    {
        public static ProductCategory ParseProductCategory(this HtmlNode categoryMenuItemNode)
        {
            var name = categoryMenuItemNode.SelectSingleNode(XPathes.CategoryNameXpath).InnerText.Trim();
            var subCategories = categoryMenuItemNode
                .SelectNodes(XPathes.SubcategoryNameXpath)?
                .Select(x => x.InnerText.Trim())?
                .ToArray();

            return new ProductCategory(name, subCategories);
        }

        public static int ParseLastPageCount(this HtmlNode pageList)
        {
            if (pageList == null)
                return 0;

            var pages = pageList.SelectNodes(XPathes.PageItemXPath);
            int lastPage = int.Parse(pages[pages.Count - 1].InnerText);
            return lastPage;
        }

        public static Product[] ParseProducts(this HtmlNode documentNode)
        {
            var productNodes = documentNode.SelectNodes(XPathes.ProductXPath);
            var products = productNodes.Select(ParseProduct).ToArray();
            return products;
        }

        public static Product ParseProduct(this HtmlNode productNode)
        {
            var productName = productNode.SelectSingleNode(XPathes.ProductNameXPath).InnerText.Trim();

            var productPriceText = productNode.SelectSingleNode(XPathes.ProductPriceXPath).InnerText;
            productPriceText = StringUtils.ExtractDigits(productPriceText);
            var prdouctPrice = decimal.Parse(productPriceText);

            var productTypeText = productNode.SelectSingleNode(XPathes.ProductTypeXPath)?.InnerText?
                .Replace(" ", string.Empty);

            var productType = productTypeText is null ? ProductType.Standard : Enum.Parse<ProductType>(productTypeText, ignoreCase: true);

            return new Product(productName, prdouctPrice, productType);
        }
    }
}