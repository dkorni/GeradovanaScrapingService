using Geradovana.ScrapingService.Application.Common.Providers;
using Geradovana.ScrapingService.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geradovana.ScrapingService.Application.Queries
{
    public class GetProductCategoriesQueryHandler(IProductCategoryProvider _categoryProvider) : IRequestHandler<GetProductCategoriesQuery, ProductCategory[]>
    {
        public Task<ProductCategory[]> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
        {
            return _categoryProvider.GetAll();
        }
    }
}