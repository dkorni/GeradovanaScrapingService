using Geradovana.ScrapingService.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geradovana.ScrapingService.Application.Queries
{
    public record GetProductCategoriesQuery() : IRequest<ProductCategory[]>;
}