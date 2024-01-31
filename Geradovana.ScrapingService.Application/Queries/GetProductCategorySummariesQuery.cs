using Geradovana.ScrapingService.Domain;
using MediatR;

namespace Geradovana.ScrapingService.Application.Queries;

public record GetProductCategorySummariesQuery(
    string CategoryName,
    string? SubCategoryName) : IRequest<ProductCategorySummary[]>;