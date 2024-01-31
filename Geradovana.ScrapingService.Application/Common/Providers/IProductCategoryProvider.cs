using Geradovana.ScrapingService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geradovana.ScrapingService.Application.Common.Providers
{
    public interface IProductCategoryProvider
    {
        Task<ProductCategory[]> GetAll();

        Task<ProductCategorySummary[]> GetSummaries(string categoryName, string? subCategoryName);
    }
}