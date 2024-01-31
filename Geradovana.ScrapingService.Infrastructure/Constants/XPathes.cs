using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geradovana.ScrapingService.Infrastructure.Constants
{
    public static class XPathes
    {
        public static readonly string CategoryMenuItems = "//ul[@class='wrapper mega-menu__categories bg-grey']/li";
        public static readonly string CategoryNameXpath = "a";
        public static readonly string SubcategoryNameXpath = "ul/li/a[@class='list-item']";
        public static readonly string PageListXPath = "//select[@id='PageList']";
        public static readonly string PageItemXPath = "option";
        public static readonly string ProductXPath = "//div[@class=' load-more-gifts clearfix']/div[contains(@class, 'product ')]";
        public static readonly string ProductNameXPath = "a/div/span[@class='productname']";
        public static readonly string ProductPriceXPath = "a/div/span[@class='productprice']/span";
        public static readonly string ProductTypeXPath = "a/div/div[@class='badge-container']/span/span";
    }
}
