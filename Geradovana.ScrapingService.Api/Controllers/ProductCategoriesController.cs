using Geradovana.ScrapingService.Application.Queries;
using Geradovana.ScrapingService.Contracts;
using Geradovana.ScrapingService.Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Geradovana.ScrapingService.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductCategoriesController(ISender _mediator) : ControllerBase
    {
        public async Task<ProductCategoryDto[]> Get()
        {
            var query = new GetProductCategoriesQuery();

            var result = await _mediator.Send(query);

            return result.Select(x => ToDto(x)).ToArray();
        }

        private static ProductCategoryDto ToDto(ProductCategory productCategory) => new ProductCategoryDto(productCategory.name, productCategory.subcategories);
    }
}