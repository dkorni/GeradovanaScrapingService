using Geradovana.ScrapingService.Application.Queries;
using Geradovana.ScrapingService.Contracts;
using Geradovana.ScrapingService.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Geradovana.ScrapingService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductCategoriesController(ISender mediator) : ControllerBase
    {
        public async Task<ProductCategoryDto[]> Get()
        {
            var query = new GetProductCategoriesQuery();

            var result = await mediator.Send(query);

            return result.Select(x => ToDto(x)).ToArray();
        }

        [Route("summaries")]
        public async Task<ProductCategorySummaryDto[]> GetSummaries(string category, string? subcategory)
        {
            var query = new GetProductCategorySummariesQuery(category, subcategory);

            var result = await mediator.Send(query);
            
            return result.Select(x => ToDto(x)).ToArray();
        }

        private static ProductCategoryDto ToDto(ProductCategory productCategory) =>
            new ProductCategoryDto(productCategory.name,
                productCategory.subcategories);
        
        private static ProductCategorySummaryDto ToDto(ProductCategorySummary summary) =>
            new ProductCategorySummaryDto(summary.CategoryName,
                summary.SubCategoryName,
                summary.Type.ToString(),
                summary.Amount,
                summary.AveragePrice);

    }
}