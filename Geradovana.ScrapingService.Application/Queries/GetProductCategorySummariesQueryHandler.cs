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
    public class GetProductCategorySummariesQueryHandler(
        IProductCategoryProvider categoryProvider)
        : IRequestHandler<GetProductCategorySummariesQuery, ProductCategorySummary[]>
    {
        public Task<ProductCategorySummary[]> Handle(GetProductCategorySummariesQuery request, CancellationToken cancellationToken)
        {
            return categoryProvider.GetSummaries(request.CategoryName, request.SubCategoryName);
        }
    }
}