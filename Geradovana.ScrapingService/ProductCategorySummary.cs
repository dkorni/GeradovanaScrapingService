using Geradovana.ScrapingService.Domain.Enums;

namespace Geradovana.ScrapingService.Domain;

public record ProductCategorySummary(
    string CategoryName, 
    string? SubCategoryName, 
    ProductType Type, 
    long Amount, 
    decimal AveragePrice);