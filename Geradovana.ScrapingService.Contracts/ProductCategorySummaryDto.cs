namespace Geradovana.ScrapingService.Contracts;

public record ProductCategorySummaryDto(
    string CategoryName, 
    string? SubCategoryName, 
    string Type, 
    long Amount, 
    decimal AveragePrice);